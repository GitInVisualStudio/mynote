using MyNoteBase.Utils;
using Newtonsoft.Json;
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
        public Course Course { get => course; set => course = value; }
        public TestType Type { get => type; set => type = value; }
        public int CourseOnlineID { get => courseOnlineID; set => courseOnlineID = value; }
        public string CourseLocalID { get => courseLocalID; set => courseLocalID = value; }
        public string LocalID { get => localID; }

        public Test(DateTime date, Course course, string topic, TestType type)
        {
            this.date = date;
            this.course = course;
            this.topic = topic;
            this.type = type;
            this.localID = Globals.GetLocalID(course.Name + topic, date);
            course.Tests.Add(this);
            this.courseLocalID = course.LocalID;
            this.courseOnlineID = course.OnlineID;
        }
    }
}
