using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils
{
    public class Globals
    {
        private static int currentLocalID = 0;
        private static Encoding encoding = Encoding.Unicode;

        public static int CurrentLocalID { get => currentLocalID; }
        public static Encoding Encoding { get => encoding; set => encoding = value; }

        public static int GetAndIncrementLocalID()
        {
            currentLocalID++;
            return currentLocalID;
        }

        public static void InitFromInstance(GlobalsInstancing instance)
        {
            currentLocalID = instance.CurrentLocalID;
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
