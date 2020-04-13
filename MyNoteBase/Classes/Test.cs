using MyNoteBase.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Classes
{
    [JsonObject(MemberSerialization.Fields)]
    public class Test
    {
        private DateTime date;
        private string topic;
        [JsonIgnore]
        private Course course;
        private int onlineID;
        private string localID;
        private string courseLocalID;
        private int courseOnlineID;
        private TestType type;

        public DateTime Date { get => date; set => date = value; }
        public string Topic { get => topic; set => topic = value; }
        public Course Course
        {
            get => course;
            set
            {
                course = value;
                this.courseLocalID = course.LocalID;
                this.courseOnlineID = course.OnlineID;
                course.Tests.Add(this);
            }
        }
        public TestType Type { get => type; set => type = value; }
        public int CourseOnlineID { get => courseOnlineID; set => courseOnlineID = value; }
        public string CourseLocalID { get => courseLocalID; set => courseLocalID = value; }
        public string LocalID { get => localID; }
        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Test(JObject json)
        {
            this.date = json["date"].ToObject<DateTime>();
            this.topic = json["topic"].ToObject<string>();
            this.onlineID = json["onlineID"].ToObject<int>();
            if (!(json.ContainsKey("localID") && json.ContainsKey("courseLocalID")))
            {
                this.localID = GetLocalID();
                this.CourseLocalID = "";
            }
            else
            {
                this.localID = json["localID"].ToObject<string>();
                this.courseLocalID = json["courseLocalID"].ToObject<string>();
            }
            this.courseOnlineID = json["courseOnlineID"].ToObject<int>();
            this.type = json["type"].ToObject<TestType>();
        }

        public Test(DateTime date, Course course, string topic, TestType type)
        {
            this.date = date;
            this.course = course;
            this.topic = topic;
            this.type = type;
            this.onlineID = 0;
            this.localID = GetLocalID();
            course.Tests.Add(this);
            this.courseLocalID = course.LocalID;
            this.courseOnlineID = course.OnlineID;
        }

        private string GetLocalID()
        {
            return Globals.GetLocalID("t_" + topic, date);
        }
    }
}
