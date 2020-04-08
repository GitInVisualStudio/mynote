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
        private DateTime created;
        [JsonIgnore]
        private List<Course> courses;
        private string localID;
        private int onlineID;

        public string Name { get => name; set => name = value; }
        public List<Course> Courses { get => courses; set => courses = value; }
        public string LocalID 
        { 
            get => localID; 
            private set
            {
                localID = value;
                foreach (Course c in courses)
                    c.SemesterLocalID = localID;
            }
        }
        public int OnlineID
        {
            get => onlineID;
            set
            {
                onlineID = value;
                foreach (Course c in courses)
                    c.SemesterOnlineID = OnlineID;
            }
        }

        public DateTime Created { get => created; set => created = value; }

        public Semester(JObject json)
        {
            this.name = json["name"].ToString();
            this.localID = json["localID"].ToObject<string>();
            this.onlineID = json["onlineID"].ToObject<int>();
            this.courses = new List<Course>();
        }

        public Semester(string name, DateTime created)
        {
            this.name = name;
            this.created = created;
            this.courses = new List<Course>();
            localID = Globals.GetLocalID("s_" + name, created);
            onlineID = 0;
        }
    }
}