using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNoteBase.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyNoteBase.Canvasses
{
    [JsonObject(MemberSerialization.Fields)]
    public class Note : Canvas
    {
        public Note(JObject json, IManager manager) : base(json, manager)
        {

        } 

        public Note(DateTime dt, string name, Course course, IManager manager) : base(dt, name, course, manager)
        {
        }
    }
}
