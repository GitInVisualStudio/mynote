using MyNoteBase.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNoteBase.Canvasses
{
    [Serializable]
    public abstract class Canvas
    {
        private Image pixels => Manager.GetImage();
        private IManager manager;
        private DateTime dt;
        private string name;
        private Course course;
        private string courseFilePath;

        [XmlIgnore]
        public IManager Manager
        {
            get
            {
                return manager;
            }

            set
            {
                manager = value;
            }
        }

        [XmlIgnore]
        public Course Course { get => course; set => course = value; }

        public DateTime Dt
        {
            get
            {
                return dt;
            }

            set
            {
                dt = value;
            }
        }

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

        public string CourseFilePath { get => courseFilePath; set => courseFilePath = value; }

        /// <summary>
        /// Has to exist because of how the XMLSerializer works
        /// </summary>
        public Canvas()
        { 
        }

        public Canvas(DateTime dt, string name, Course course, IManager manager)
        {
            this.dt = dt;
            this.name = name;
            this.course = course;
            this.manager = manager;
        }
    }
}
