using MyNoteBase.Canvasses;
using MyNoteBase.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            userSavePath = "userSaves" + Path.DirectorySeparatorChar.ToString();
            appSavePath = "saves" + Path.DirectorySeparatorChar.ToString();
        }
        
        public Canvas LoadCanvas(string path, IManager manager)
        {
            Canvas c;
            try
            {
                JObject json = Serializer.Deserialize(sl.Load(path));
                Type t = json["type"].ToObject<Type>();
                c = (Canvas)t.GetConstructor(new Type[] { typeof(JObject), typeof(IManager) }).Invoke(new object[] { json, manager });
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
                c = new Course(Serializer.Deserialize(sl.Load(path)));
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
                Semester s = new Semester(Serializer.Deserialize(sl.Load(path)));
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
            c.PrepareForSerialization();
            string fileExtension = ".myn";
            string path = userSavePath + c.Course.Semester.Name + Path.DirectorySeparatorChar + c.Course.Name + Path.DirectorySeparatorChar + c.Name + fileExtension;
            sl.Save(path, Serializer.Serialize(c));
            return path;
        }

        public string SaveCourse(Course c)
        {
            SaveSemester(c.Semester);
            string path = appSavePath + c.LocalID + ".myk";
            sl.Save(path, Serializer.Serialize(c));
            return path;
        }

        public string SaveSemester(Semester s)
        {
            string path = appSavePath + s.LocalID + ".mys";
            sl.Save(path, Serializer.Serialize(s));
            return path;
        }

        public void SaveGlobals()
        {
            GlobalsInstancing instance = new GlobalsInstancing();
            sl.Save(appSavePath + "globals.cfg", Serializer.Serialize(instance));
        }

        public void LoadGlobals()
        {
            GlobalsInstancing instance = new GlobalsInstancing(Serializer.Deserialize(sl.Load(appSavePath + "globals.cfg")));
            Globals.InitFromInstance(instance);
        }
    }
}
