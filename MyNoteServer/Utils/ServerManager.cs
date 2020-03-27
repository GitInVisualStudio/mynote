using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNoteServer.Utils.DB;

namespace MyNoteServer.Utils
{
    public class ServerManager
    {
        private DBConnection connection;

        public ServerManager(string host, string user, string password)
        {
            connection = new DBConnection(host, user, password, "130102_foreignkey-tests");
            RowCollection rows = connection.Select("kunden", new ColumnCollection());
        }
    }
}
