using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils
{
    [JsonObject(MemberSerialization.Fields)]
    /// <summary>
    /// This object is supposed to be used only for serialization
    /// </summary>
    public class GlobalsInstancing
    {
        private int currentLocalID;

        public int CurrentLocalID { get => currentLocalID; }

        public GlobalsInstancing(JObject json)
        {
            this.currentLocalID = json["currentLocalID"].ToObject<int>();
        }

        public GlobalsInstancing()
        {
            this.currentLocalID = Globals.CurrentLocalID;
        }
    }
}
