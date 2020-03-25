using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils.IO
{
    /// <summary>
    /// Interface for (de-)serializing as binary and saving/loading to a file
    /// </summary>
    public interface ISaveLoader
    {
        void Save(string path, object obj);
        object Load(string path);
    }
}
