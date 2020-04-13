using MyNoteBase.Utils;
using MyNoteBase.Utils.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNote.Utils.IO
{
    public class SaveLoader : ISaveLoader
    {
        public string Load(string path)
        {
            return File.ReadAllText(path);
        }

        public void Save(string path, string obj)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            byte[] bytes = Globals.Decode(obj);
            using (FileStream stream = new FileStream(path, FileMode.Create))
                stream.Write(bytes, 0, bytes.Length);
        }
    }
}
