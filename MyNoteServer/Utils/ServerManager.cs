using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyNoteServer.Utils.DB;
using MySql.Data.MySqlClient;

namespace MyNoteServer.Utils
{
    public class ServerManager
    {
        private DBConnection connection;

        public ServerManager(string host, string user, string password)
        {
            connection = new DBConnection(host, user, password);
            InitDB();
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
    }
}
