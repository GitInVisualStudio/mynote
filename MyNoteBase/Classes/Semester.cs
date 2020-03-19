using System.Collections.Generic;

namespace MyNoteBase.Classes
{
    public class Semester
    {
        private string name;
        private List<Course> courses;

        public string Name { get => name; set => name = value; }
        public List<Course> Courses { get => courses; set => courses = value; }

        public Semester(string name)
        {
            this.name = name;
            this.courses = new List<Course>();
        }
    }
}