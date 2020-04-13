using MyNoteBase.Canvasses;
using MyNoteBase.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNoteBase.Classes
{
    [JsonObject(MemberSerialization.Fields)]
    public class Course
    {
        private string name;
        private DateTime created;
        private Color color;
        [JsonIgnore]
        private List<Canvas> canvasses;
        [JsonIgnore]
        private Utils.Graphic.Icon icon;
        private int iconID;
        [JsonIgnore]
        private Semester semester;
        private int onlineID;
        private string localID;
        private int semesterOnlineID;
        private string semesterLocalID;
        [JsonIgnore]
        private List<Test> tests;
        private List<string> testLocalIDs;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public List<Canvas> Canvasses
        {
            get
            {
                return canvasses;
            }

            set
            {
                canvasses = value;
            }
        }

        public Utils.Graphic.Icon Icon
        {
            get
            {
                return icon;
            }

            set
            {
                icon = value;
                iconID = icon.Id;
            }
        }

        public Semester Semester
        {
            get => semester;
            set
            {
                semester = value;
                this.semesterLocalID = semester.LocalID;
                this.semesterOnlineID = semester.OnlineID;
                semester.Courses.Add(this);
            }
        }

        public List<Test> Tests { get => tests; set => tests = value; }
        public int SemesterOnlineID { get => semesterOnlineID; set => semesterOnlineID = value; }
        public string SemesterLocalID { get => semesterLocalID; set => semesterLocalID = value; }
        public string LocalID 
        { 
            get => localID; 
            private set
            {
                localID = value;
                foreach (Test t in tests)
                    t.CourseLocalID = localID;
                foreach (Canvas c in canvasses)
                    c.CourseLocalID = localID;
            }
        }
        public int OnlineID 
        { 
            get => onlineID; 
            set
            {
                onlineID = value;
                foreach (Test t in tests)
                    t.CourseOnlineID = onlineID;
                foreach (Canvas c in canvasses)
                    c.CourseOnlineID = onlineID;
            } 
        }

        public DateTime Created { get => created; set => created = value; }
        public List<string> TestLocalIDs { get => testLocalIDs; set => testLocalIDs = value; }

        public Course(JObject json)
        {
            this.name = json["name"].ToObject<string>();
            this.color = json["color"].ToObject<Color>();
            this.iconID = json["iconID"].ToObject<int>();
            this.onlineID = json["onlineID"].ToObject<int>();
            this.semesterOnlineID = json["semesterOnlineID"].ToObject<int>();
            this.created = json["created"].ToObject<DateTime>();
            if (!(json.ContainsKey("localID") && json.ContainsKey("semesterLocalID")))
            {
                this.localID = GetLocalID();
                this.semesterLocalID = "";
            }
            else
            {
                this.localID = json["localID"].ToObject<string>();
                this.semesterLocalID = json["semesterLocalID"].ToObject<string>();
            }

            this.testLocalIDs = json["testLocalIDs"].ToObject<List<string>>();
            this.canvasses = new List<Canvas>();
            this.tests = new List<Test>();
        }

        public Course(string name, DateTime created, Color color, Utils.Graphic.Icon icon, Semester s)
        {
            this.name = name;
            this.created = created;
            this.color = color;
            this.icon = icon;
            this.canvasses = new List<Canvas>();
            this.Semester = s;
            this.localID = GetLocalID();
            this.onlineID = 0;
            this.semesterLocalID = semester.LocalID;
            this.SemesterOnlineID = semester.OnlineID;
            this.tests = new List<Test>();
        }

        private string GetLocalID()
        {
            return Globals.GetLocalID("k_" + name, created);
        }

        public void PrepareForSerialization()
        {
            TestLocalIDs = new List<string>();
            foreach (Test t in tests)
                TestLocalIDs.Add(t.LocalID);
        }
    }
}
