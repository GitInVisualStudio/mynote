using MyNoteBase.Canvasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNoteBase.Classes
{
    public class Course
    {
        private string name;
        private Color color;
        private List<Canvas> canvasses;
        private Icon icon;
        private Semester semester;
        private string semesterFilePath;

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

        public Icon Icon
        {
            get
            {
                return icon;
            }

            set
            {
                icon = value;
            }
        }

        [XmlIgnore]
        public Semester Semester { get => semester; set => semester = value; }
        public string SemesterFilePath { get => semesterFilePath; set => semesterFilePath = value; }

        /// <summary>
        /// Has to exist because of how the XMLSerializer works
        /// </summary>
        public Course()
        {

        }

        public Course(string name, Color color, Icon icon)
        {
            this.name = name;
            this.color = color;
            this.icon = icon;
            this.canvasses = new List<Canvas>();
        }
    }
}
