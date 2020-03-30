using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteServer.Utils.DB
{
    public class Row
    {
        private Cell[] cells;

        public Cell[] Cells { get => cells; set => cells = value; }
        public int Length { get => cells.Length; }

        public Row(Cell[] cells)
        {
            this.cells = cells ?? new Cell[0];
        }

        public ColumnCollection GetColumns()
        {
            List<Column> cols = new List<Column>();
            foreach (Cell c in cells)
                cols.Add(c.Col);
            return new ColumnCollection(cols.ToArray());
        }

        public override string ToString()
        {
            string res = "('";
            for (int i = 0; i < cells.Length; i++)
            {
                res += cells[i];
                if (i < cells.Length - 1)
                    res += "','";
            }
            res += "')";
            return res;
        }
    }
}
