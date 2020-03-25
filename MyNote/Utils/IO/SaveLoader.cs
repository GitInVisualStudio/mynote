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
        public object Load(string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
                return bf.Deserialize(stream);
        }

        public void Save(string path, object obj)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create))
                bf.Serialize(stream, obj);
        }
    }
}
