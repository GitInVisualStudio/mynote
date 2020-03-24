using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils.IO
{
    public interface ISaveLoader
    {
        void Save(string path, object obj);
        object Load(string path, Type t);
    }
}
