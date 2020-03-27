using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace MyNoteServer.Utils.DB
{
    public class DBConnection
    {
        private string database;

        private MySqlConnection connection;
        private string host;
        private string user;
        private string password;

        public MySqlConnection Connection { get => connection; }
        public string Database
        {
            get => database;
            set
            {
                try
                {
                    InitConnection(host, user, password, value);
                    database = value;
                }
                catch
                {
                    return;
                }
            }
        }

        public DBConnection(string host, string user, string password, string database="")
        {
            InitConnection(host, user, password, database);
            this.host = host;
            this.user = user;
            this.password = password;
        }

        private void InitConnection(string host, string user, string password, string database)
        {
            if (connection != null)
                connection.Close();
            string connectionString = string.Format("SERVER={0}; UID={1}; PASSWORD={2};", host, user, password);
            if (database != "")
                connectionString += " DATABASE=" + database;
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public RowCollection Select(string table, ColumnCollection cols, string where = "", string orderBy = "", string limit = "")
        {
            string query = "SELECT ";
            if (cols.ToString() == "")
                query += "* ";
            else
                query += cols.ToString() + " ";
            query += "FROM " + table;
            if (where != "")
                query += "WHERE " + where + " ";
            if (orderBy != "")
                query += "ORDER BY " + orderBy + " ";
            if (limit != "")
                query += "LIMIT " + limit;
            return RowCollection.FromDataReader(Query(query));
        }

        public void Insert(string table, RowCollection rows)
        {
            Insert(table, rows.GetColumns().ToString(), rows.ToString());
        }

        public void Insert(string table, string columns, string rows)
        {
            if (database != "")
                Query(string.Format("INSERT INTO {0} ({1}) VALUES {2}", table, columns, rows));
        }

        public MySqlDataReader Query(string query)
        {
            try
            {
                MySqlCommand c = new MySqlCommand(query, connection);
                return c.ExecuteReader();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Deconstructor that closes the open connection
        /// </summary>
        ~DBConnection()
        {
            connection.Close();
        }
    }
}