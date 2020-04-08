using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils
{
    public class Globals
    {
        private static Encoding encoding = Encoding.Unicode;

        public static Encoding Encoding { get => encoding; set => encoding = value; }

        public static string GetLocalID(string name, DateTime created)
        {
            SHA256 sha = SHA256.Create();
            string beforeHash = name + created;
            return Encode(sha.ComputeHash(Decode(beforeHash)));
        }

        public static void InitFromInstance(GlobalsInstancing instance)
        {

        }

        public static string Encode(byte[] bytes)
        {
            return Encoding.GetString(bytes);
        }

        public static byte[] Decode(string str)
        {
            return Encoding.GetBytes(str);
        }
    }
}
