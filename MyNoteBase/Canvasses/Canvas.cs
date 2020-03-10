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
    public class Canvas
    {
        private Image pixels => Manager.GetImage();
        private IManager manager;
        private DateTime dt;
        private string name;
        private Course course;

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

        public Canvas(DateTime dt, string name, Course course, IManager manager)
        {
            this.dt = dt;
            this.name = name;
            this.course = course;
            this.manager = manager;
        }
    }
}
