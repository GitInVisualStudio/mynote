using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteServer.Utils.DB
{
    public class Cell
    {
        private Column col;
        private object data;

        public Column Col { get => col; set => col = value; }
        public object Data { get => data; set => data = value; }

        public Cell(Column col, object data)
        {
            this.col = col;
            this.data = data;
        }

        public override string ToString()
        {
            return data.ToString();
        }
    }
}
