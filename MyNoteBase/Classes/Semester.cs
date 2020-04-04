using MyNoteBase.Utils;
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
        private int localID;
        private int onlineID;

        public string Name { get => name; set => name = value; }
        public List<Course> Courses { get => courses; set => courses = value; }
        public int LocalID { get => localID; }
        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Semester(string name)
        {
            this.name = name;
            this.courses = new List<Course>();
            localID = Globals.GetAndIncrementLocalID();
            onlineID = 0;
        }

        public void InitAfterDeserialization()
        {
            this.courses = new List<Course>();
        }
    }
}