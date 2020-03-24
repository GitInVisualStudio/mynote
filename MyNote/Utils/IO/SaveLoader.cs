using MyNoteBase.Utils.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNote.Utils.IO
{
    public class SaveLoader : ISaveLoader
    {
        public object Load(string path, Type t)
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            XmlSerializer xml = new XmlSerializer(t);
            return xml.Deserialize(stream);
        }

        public void Save(string path, object obj)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            XmlSerializer xml = new XmlSerializer(obj.GetType());
            xml.Serialize(stream, obj);
        }
    }
}
