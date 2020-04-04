using MyNoteBase.Canvasses;
using MyNoteBase.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNoteBase.Classes
{
    [Serializable]
    public class Course
    {
        private string name;
        private Color color;
        private List<Canvas> canvasses;
        private Utils.Graphic.Icon icon;
        [NonSerialized]
        private Semester semester;
        private int onlineID;
        private int localID;
        private int semesterOnlineID;
        private int semesterLocalID;
        private List<Test> tests;

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

        public Utils.Graphic.Icon Icon
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

        public Semester Semester { get => semester; set => semester = value; }
        public List<Test> Tests { get => tests; set => tests = value; }
        public int SemesterOnlineID { get => semesterOnlineID; set => semesterOnlineID = value; }
        public int SemesterLocalID { get => semesterLocalID; set => semesterLocalID = value; }
        public int LocalID { get => localID; }
        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Course(string name, Color color, Utils.Graphic.Icon icon, Semester s)
        {
            this.name = name;
            this.color = color;
            this.icon = icon;
            this.canvasses = new List<Canvas>();
            this.semester = s;
            this.localID = Globals.GetAndIncrementLocalID();
            this.onlineID = 0;
            this.semesterLocalID = semester.LocalID;
            this.SemesterOnlineID = semester.OnlineID;
        }

        public void InitAfterDeserialization()
        {
            canvasses = new List<Canvas>();
        }
    }
}
