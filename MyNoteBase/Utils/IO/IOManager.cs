using MyNoteBase.Canvasses;
using MyNoteBase.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils.IO
{
    public class IOManager
    {
        private static string userSavePath;
        private string appSavePath;

        private ISaveLoader sl;
        private Dictionary<int, Semester> loadedSemesters;
        private Dictionary<int, Course> loadedCourses;

        public IOManager(ISaveLoader sl)
        {
            this.sl = sl;
            loadedSemesters = new Dictionary<int, Semester>();
            loadedCourses = new Dictionary<int, Course>();
            userSavePath = "saves" + Path.DirectorySeparatorChar.ToString();
            appSavePath = "userSaves" + Path.DirectorySeparatorChar.ToString();
        }
        
        public Canvas LoadCanvas(string path, IManager manager)
        {
            Canvas c;
            try
            {
                c = (Canvas)sl.Load(path);
                c.InitAfterDeserialization(manager);
            }
            catch
            {
                return null;
            }

            if (loadedCourses.ContainsKey(c.CourseLocalID))
                c.Course = loadedCourses[c.CourseLocalID];
            else
                c.Course = LoadCourse(c.CourseLocalID);

            c.Course.Canvasses.Add(c);
            return c;
        }

        public Course LoadCourse(int localID)
        {
            return LoadCourse(appSavePath + localID + ".myk");
        }

        public Course LoadCourse(string path)
        {
            Course c;
            try
            {
                c = (Course)sl.Load(path);
                c.InitAfterDeserialization();
            }
            catch
            {
                return null;
            }

            if (loadedSemesters.ContainsKey(c.SemesterLocalID))
                c.Semester = loadedSemesters[c.SemesterLocalID];
            else
                c.Semester = LoadSemester(c.SemesterLocalID);

            c.Semester.Courses.Add(c);
            return c;
        }

        public Semester LoadSemester(int localID)
        {
            return LoadSemester(appSavePath + localID + ".mys");
        }

        public Semester LoadSemester(string path)
        {
            try
            {
                Semester s = (Semester)sl.Load(path);
                s.InitAfterDeserialization();
                return s;
            }
            catch
            {
                return null;
            }
        }

        public string SaveCanvas(Canvas c)
        {
            SaveCourse(c.Course);
            string fileExtension = ".myn";
            string path = userSavePath + c.Course.Semester.Name + Path.DirectorySeparatorChar + c.Course.Name + Path.DirectorySeparatorChar + c.Name + fileExtension;
            sl.Save(path, c);
            return path;
        }

        public string SaveCourse(Course c)
        {
            SaveSemester(c.Semester);
            string path = appSavePath + c.LocalID + ".myk";
            sl.Save(path, c);
            return path;
        }

        public string SaveSemester(Semester s)
        {
            string path = appSavePath + s.LocalID + ".mys";
            sl.Save(path, s);
            return path;
        }

        public void SaveGlobals()
        {
            GlobalsInstancing instance = new GlobalsInstancing();
            sl.Save(appSavePath + "globals.cfg", instance);
        }

        public void LoadGlobals()
        {
            GlobalsInstancing instance = (GlobalsInstancing)sl.Load(appSavePath + "globals.cfg");
            Globals.InitFromInstance(instance);
        }
    }
}
