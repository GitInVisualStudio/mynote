using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyNote.Utils.Math;
using MyNoteServer.Utils.DB;
using MySql.Data.MySqlClient;

namespace MyNoteServer.Utils
{
    public class ServerManager
    {
        private DBConnection connection;
        private CRandom rnd;

        public ServerManager(string host, string user, string password)
        {
            rnd = new CRandom();
            connection = new DBConnection(host, user, password);
            InitDB();
            CreateUser("test@test.com", "test1", "");
            CreateUser("test2@test.com", "test2", "");
        }

        private void InitDB()
        {
            string cmdString = GetInitDBCommand();
            connection.ExecuteNonQuery(cmdString);
        }
        
        private string GetInitDBCommand()
        {
            string cmd;
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            using (Stream s = thisAssembly.GetManifestResourceStream("MyNoteServer.db_init.sql"))
                using (StreamReader sr = new StreamReader(s))
                    cmd = sr.ReadToEnd();

            cmd = cmd.Replace("\r\n", "\n");
            cmd = cmd.Replace("\t", "");
            return cmd;
        }

        public int CreateUser(string email, string username, string password)
        {
            int salt = rnd.Next(100000, 999999);
            password = HashAndSalt(password, salt);
            RowCollection rows = new RowCollection(new Row[]
            {
                new Row(new Cell[]
                {
                    new Cell(new Column("email", typeof(string)), email),
                    new Cell(new Column("password", typeof(string)), password),
                    new Cell(new Column("salt", typeof(int)), salt),
                    new Cell(new Column("username", typeof(string)), username)
                })
            });
            connection.Insert("user", rows);
            return 0;
        }

        private string HashAndSalt(string password, int salt)
        {
            password += salt;
            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
