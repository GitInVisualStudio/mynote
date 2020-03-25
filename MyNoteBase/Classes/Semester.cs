using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyNoteBase.Classes
{
    [Serializable]
    public class Semester
    {
        private string name;
        [NonSerialized]
        private List<Course> courses;

        public string Name { get => name; set => name = value; }
        public List<Course> Courses { get => courses; set => courses = value; }

        public Semester(string name)
        {
            this.name = name;
            this.courses = new List<Course>();
        }

        public void InitAfterDeserialization()
        {
            this.courses = new List<Course>();
        }
    }
}