using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteServer.Utils.DB
{
    public class ColumnCollection
    {
        private Column[] cols;

        public Column[] Cols { get => cols; set => cols = value; }

        public ColumnCollection(Column[] cols = null)
        {
            this.cols = cols ?? new Column[0];
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < cols.Length; i++)
            {
                res += "`" + cols[i] + "`";
                if (i < cols.Length - 1)
                    res += ",";
            }
            return res;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ColumnCollection))
                return false;
            ColumnCollection c = (ColumnCollection)obj;
            return cols.SequenceEqual(c.Cols);
        }

        public static bool operator ==(ColumnCollection a, ColumnCollection b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ColumnCollection a, ColumnCollection b)
        {
            return !(a == b);
        }
    }
}
