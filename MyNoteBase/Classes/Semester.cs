using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyNoteBase.Classes
{
    public class Semester
    {
        private string name;
        private List<Course> courses;

        public string Name { get => name; set => name = value; }
        [XmlIgnore]
        public List<Course> Courses { get => courses; set => courses = value; }

        /// <summary>
        /// Has to exist because of how the XMLSerializer works
        /// </summary>
        public Semester()
        {

        }

        public Semester(string name)
        {
            this.name = name;
            this.courses = new List<Course>();
        }
    }
}