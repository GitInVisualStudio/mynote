using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Inserts values into a specified table and returns the id of the first item
        /// </summary>
        /// <returns>The id of the first inserted item</returns>
        public int Insert(string table, RowCollection rows)
        {
            return Insert(table, rows.GetColumns().ToString(), rows.ToString());
        }

        /// <summary>
        /// Inserts values into a specified table and returns the id of the first item
        /// </summary>
        /// <returns>The id of the first inserted item</returns>
        public int Insert(string table, string columns, string rows)
        {
            if (database != "")
                Query(string.Format("INSERT INTO {0} ({1}) VALUES {2}", table, columns, rows));
            MySqlDataReader reader = Query("SELECT LAST_INSERT_ID() AS id");
            int id = 0;
            if (reader == null)
                return 0;
            while (reader.Read())
                id = (int)reader["id"];
            return id;
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

        public int ExecuteNonQuery(string text)
        {
            try
            {
                MySqlCommand c = new MySqlCommand(text, connection);
                return c.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
        }

        public static string MySQLEscape(string str)
        {
            return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
                delegate (Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                    return "\\0";
                        case "\b":              // BACKSPACE character
                    return "\\b";
                        case "\n":              // NEWLINE (linefeed) character
                    return "\\n";
                        case "\r":              // CARRIAGE RETURN character
                    return "\\r";
                        case "\t":              // TAB
                    return "\\t";
                        case "\u001A":          // Ctrl-Z
                    return "\\Z";
                        default:
                            return "\\" + v;
                    }
                });
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