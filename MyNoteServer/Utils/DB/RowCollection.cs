using MyNoteServer.Utils.Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteServer.Utils.DB
{
    public class RowCollection
    {
        private Row[] rows;

        public Row[] Rows { get => rows; set => rows = value; }

        public RowCollection(Row[] rows)
        {
            this.rows = rows ?? new Row[0];
            if (this.rows.Length > 0)
            {
                ColumnCollection cols = this.rows[0].GetColumns();
                foreach (Row r in this.rows)
                    if (r.GetColumns() != cols)
                        throw new RowColumnsNotEqualException();
            }
        }

        public ColumnCollection GetColumns()
        {
            if (rows.Length > 0)
                return rows[0].GetColumns();
            else
                return new ColumnCollection();
        }

        public static RowCollection FromDataReader(MySqlDataReader reader)
        {
            List<Row> rows = new List<Row>();
            while (reader.Read())
            {
                List<Cell> cells = new List<Cell>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Column col = new Column(reader.GetName(i), reader.GetFieldType(i));
                    Cell c = new Cell(col, reader[i]);
                    cells.Add(c);
                }
                rows.Add(new Row(cells.ToArray()));
            }
            return new RowCollection(rows.ToArray());
        }

        public override string ToString()
        {
            string res = "";
            foreach (Row row in rows)
                res += row;
            return res;
        }
    }
}
