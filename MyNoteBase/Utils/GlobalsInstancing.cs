using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils
{
    [Serializable]
    public class GlobalsInstancing
    {
        private int currentLocalID;

        public int CurrentLocalID { get => currentLocalID; }

        /// <summary>
        /// This object is supposed to be used only for serialization
        /// </summary>
        public GlobalsInstancing()
        {
            this.currentLocalID = Globals.CurrentLocalID;
        }
    }
}
