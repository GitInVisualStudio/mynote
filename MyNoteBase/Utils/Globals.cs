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

        public static int CurrentLocalID { get => currentLocalID; }

        public static int GetAndIncrementLocalID()
        {
            currentLocalID++;
            return currentLocalID;
        }

        public static void InitFromInstance(GlobalsInstancing instance)
        {
            currentLocalID = instance.CurrentLocalID;
        }
    }
}
