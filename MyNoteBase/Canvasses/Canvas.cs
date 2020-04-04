using MyNoteBase.Classes;
using MyNoteBase.Utils;
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
        [NonSerialized]
        private IManager manager;
        private DateTime dt;
        private string name;
        [NonSerialized]
        private Course course;
        private int onlineID;
        private int localID;
        private int courseOnlineID;
        private int courseLocalID;
        private Image pixels;

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

        public Image Pixels { get => manager.GetImage(); set => manager.SetImage(value); }
        public int CourseOnlineID { get => courseOnlineID; set => courseOnlineID = value; }
        public int CourseLocalID { get => courseLocalID; set => courseLocalID = value; }
        public int LocalID { get => localID;  }
        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Canvas(DateTime dt, string name, Course course, IManager manager)
        {
            this.dt = dt;
            this.name = name;
            this.course = course;
            this.manager = manager;
            this.localID = Globals.GetAndIncrementLocalID();
            this.onlineID = 0;
            this.courseLocalID = course.LocalID;
            this.courseOnlineID = course.OnlineID;
        }

        public void PrepareForSerialization()
        {
            pixels = Pixels;
        }

        public void InitAfterDeserialization(IManager manager)
        {
            this.manager = manager;
            manager.SetImage(pixels);
        }
    }
}
