using MyNoteBase.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyNoteBase.Classes
{
    [JsonObject(MemberSerialization.Fields)]
    public class Semester
    {
        private string name;
        [JsonIgnore]
        private List<Course> courses;
        private int localID;
        private int onlineID;

        public string Name { get => name; set => name = value; }
        public List<Course> Courses { get => courses; set => courses = value; }
        public int LocalID { get => localID; }
        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Semester(JObject json)
        {
            this.name = json["name"].ToString();
            this.localID = json["localID"].ToObject<int>();
            this.onlineID = json["onlineID"].ToObject<int>();
            this.courses = new List<Course>();
        }

        public Semester(string name)
        {
            this.name = name;
            this.courses = new List<Course>();
            localID = Globals.GetAndIncrementLocalID();
            onlineID = 0;
        }
    }
}