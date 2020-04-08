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
    public class Excercise : Canvas
    {
        private string excerciseReference;

        public string ExcerciseReference { get => excerciseReference; set => excerciseReference = value; }

        public Excercise(JObject json, IManager manager) : base(json, manager)
        {
            this.excerciseReference = json["excerciseReference"].ToObject<string>();
        }

        public Excercise(DateTime dt, string name, Course course, IManager manager, string excerciseReference) : base(dt, name, course, manager)
        {
            this.excerciseReference = excerciseReference;
        }
    }
}
