using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteServer.Utils.Exceptions
{
    public class RowColumnsNotEqualException : Exception
    {
        public RowColumnsNotEqualException() : base("The columns of the specified rows were not all equal.")
        {

        }
    }
}
