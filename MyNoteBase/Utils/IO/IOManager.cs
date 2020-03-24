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
        private string savePath;

        private ISaveLoader sl;
        private Dictionary<string, Semester> loadedSemesters;
        private Dictionary<string, Course> loadedCourses;

        public IOManager(ISaveLoader sl)
        {
            this.sl = sl;
            loadedSemesters = new Dictionary<string, Semester>();
            loadedCourses = new Dictionary<string, Course>();
            savePath = "saves" + Path.DirectorySeparatorChar.ToString();
        }
        
        public Canvas LoadCanvas(string path)
        {
            Canvas c;
            try
            {
                c = (Canvas)sl.Load(path, typeof(Canvas));
            }
            catch
            {
                return null;
            }

            if (loadedCourses.ContainsKey(c.CourseFilePath))
                c.Course = loadedCourses[c.CourseFilePath];
            else
                c.Course = LoadCourse(c.CourseFilePath);

            c.Course.Canvasses.Add(c);
            return c;
        }

        public Course LoadCourse(string path)
        {
            Course c;
            try
            {
                c = (Course)sl.Load(path, typeof(Course));
            }
            catch
            {
                return null;
            }

            if (loadedSemesters.ContainsKey(c.SemesterFilePath))
                c.Semester = loadedSemesters[c.SemesterFilePath];
            else
                c.Semester = LoadSemester(c.SemesterFilePath);

            c.Semester.Courses.Add(c);
            return c;
        }

        public Semester LoadSemester(string path)
        {
            try
            {
                return (Semester)sl.Load(path, typeof(Semester));
            }
            catch
            {
                return null;
            }
        }

        public string SaveCanvas(Canvas c)
        {
            string coursePath = SaveCourse(c.Course);
            c.CourseFilePath = coursePath;
            string path = savePath + c.Course.Semester.Name + Path.DirectorySeparatorChar + c.Course.Name + Path.DirectorySeparatorChar + c.Name + ".myc";
            sl.Save(path, c);
            return path;
        }

        public string SaveCourse(Course c)
        {
            string semesterPath = SaveSemester(c.Semester);
            c.SemesterFilePath = semesterPath;
            string path = savePath + c.Semester.Name + Path.DirectorySeparatorChar + c.Name + Path.DirectorySeparatorChar + "course.myk";
            sl.Save(path, c);
            return path;
        }

        public string SaveSemester(Semester s)
        {
            string path = savePath + s.Name + Path.DirectorySeparatorChar + "semester.mys";
            sl.Save(path, s);
            return path;
        }
    }
}
