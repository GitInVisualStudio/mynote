using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteServer.Utils.DB
{
    public struct Column
    {
        private string name;
        private Type dataType;

        public string Name { get => name; set => name = value; }
        public Type DataType { get => dataType; set => dataType = value; }

        public Column(string name, Type dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
