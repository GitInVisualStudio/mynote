using MyNoteBase.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils.IO
{
    public class Serializer
    {
        private static JsonSerializer serializer = new JsonSerializer();

        public static string Serialize(object obj)
        {
            string res;
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter sr = new StreamWriter(stream, Globals.Encoding);
                serializer.Serialize(sr, obj);
                sr.Flush();
                byte[] bytes = stream.ToArray();
                res = Globals.Encode(bytes);
            }
            return res;
        }

        public static JObject Deserialize(string str)
        {
            object res;
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] bytes = Globals.Decode(str);
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                res = serializer.Deserialize(new JsonTextReader(new StreamReader(stream, Globals.Encoding)));
            }
            return (JObject)res;
        }
    }
}
