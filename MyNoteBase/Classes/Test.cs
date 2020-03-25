using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Classes
{
    [Serializable]
    public class Test
    {
        private DateTime date;
        private string topic;
        private Course course;
        private TestType type;

        public DateTime Date { get => date; set => date = value; }
        public string Topic { get => topic; set => topic = value; }
        public Course Course { get => course; set => course = value; }
        public TestType Type { get => type; set => type = value; }

        public Test(DateTime date, Course course, string topic, TestType type)
        {
            this.date = date;
            this.course = course;
            this.topic = topic;
            this.type = type;
            course.Tests.Add(this);
        }
    }
}
