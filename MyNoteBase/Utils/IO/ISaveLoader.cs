using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils.IO
{
    /// <summary>
    /// Interface for saving/loading a serialized object to a file
    /// </summary>
    public interface ISaveLoader
    {
        void Save(string path, string obj);
        string Load(string path);
    }
}
