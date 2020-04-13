using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyNoteBase.Classes;
using MyNoteBase.Utils.IO;
using System.Drawing;
using MyNoteBase.Canvasses;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace MyNoteBase.Utils.API
{
    public class APIManager
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://127.0.0.1:80/api/")
        };

        private string username;
        private string password;
        private string auth;
        private DateTime authSince;

        public APIManager(string username, string password)
        {
            this.username = username;
            this.password = password;

            Init().Wait();
        }

        private async Task Init()
        {
            auth = await GetAuthToken().ConfigureAwait(false);
            authSince = DateTime.Now;
        }

        private async Task<string> Post(string uri, JObject json)
        {
            StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, uri);
            message.Content = content;
            HttpResponseMessage response = await client.SendAsync(message).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<JObject> PostReturnJson(string uri, JObject json)
        {
            if (uri != "getAuthToken.php" && DateTime.Now - authSince >= new TimeSpan(24, 0, 0))
                Init().Wait();
            string resultStr = await Post(uri, json).ConfigureAwait(false);
            Console.WriteLine(resultStr);
            JObject resultJson = JObject.Parse(resultStr);
            if (resultJson["result"].ToString() != "ok")
                throw APIExceptionManager.FromID(resultJson["error_code"].ToObject<int>());
            return resultJson;
        }

        private async Task<string> GetAuthToken()
        {
            JObject json = new JObject();
            json["username"] = username;
            json["password"] = password;
            JObject resultJson = await PostReturnJson("getAuthToken.php", json).ConfigureAwait(false);
            return resultJson["token"].ToString();
        }

        public async Task<int> UploadSemester(Semester s)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["semester"] = Serializer.SerializeToJson(s);
            JObject resultJson = await PostReturnJson("uploadSemester.php", json).ConfigureAwait(false);
            int semesterID = resultJson["semesterID"].ToObject<int>();
            s.OnlineID = semesterID;
            return semesterID;
        }

        public async Task<int> UploadCourse(Course c)
        {
            c.PrepareForSerialization();
            if (c.Semester.OnlineID == 0)
                await UploadSemester(c.Semester).ConfigureAwait(false);
            JObject json = new JObject();
            json["auth"] = auth;
            json["course"] = Serializer.SerializeToJson(c);
            JObject resultJson = await PostReturnJson("uploadCourse.php", json).ConfigureAwait(false);
            c.OnlineID = resultJson["courseID"].ToObject<int>();
            foreach (Test t in c.Tests)
                await UploadTest(t);
            return c.OnlineID;
        }

        public async Task<int> UploadCanvas(Canvas c)
        {
            c.PrepareForSerialization();
            if (c.CourseOnlineID == 0)
                await UploadCourse(c.Course).ConfigureAwait(false);
            JObject json = new JObject();
            json["auth"] = auth;
            json["canvas"] = Serializer.SerializeToJson(c);
            JObject resultJson = await PostReturnJson("uploadCanvas.php", json).ConfigureAwait(false);
            c.OnlineID = resultJson["canvasID"].ToObject<int>();
            return c.OnlineID;
        }

        public async Task DeleteCanvas(Canvas c)
        {
            await DeleteCanvas(c.OnlineID).ConfigureAwait(false);
            c.OnlineID = 0;
        }

        private async Task DeleteCanvas(int canvasID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["canvasID"] = canvasID;
            await PostReturnJson("deleteCanvas.php", json);
        }

        public async Task SetCoursePassword(Course c, string password)
        {
            if (c.OnlineID == 0)
                await UploadCourse(c);
            await SetCoursePassword(c.OnlineID, password);
        }

        private async Task SetCoursePassword(int courseID, string password)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            json["password"] = password;
            await PostReturnJson("setCoursePassword.php", json);
        }

        public async Task<bool> AccessCourse(int courseID, string password)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            json["password"] = password;
            try
            {
                await PostReturnJson("accessCourse.php", json);
            } 
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<string> GetUsername(int userID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["userID"] = userID;
            JObject resultJson = await PostReturnJson("getUsername.php", json);
            return resultJson["username"].ToString();
        }

        public async Task<bool> UserIsAdmin(int courseID, int userID)
        {
            if (courseID == 0)
                return false;
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            json["userID"] = userID;
            JObject resultJson = await PostReturnJson("userIsAdmin.php", json);
            return resultJson["isAdmin"].ToObject<bool>();
        }

        public async Task SetUserAdmin(int courseID, int userID, bool admin)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            json["userID"] = userID;
            json["admin"] = admin;
            await PostReturnJson("setUserAdmin.php", json);
        }

        public async Task RemoveUserFromCourse(int courseID, int userID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            json["userID"] = userID;
            await PostReturnJson("removeUserFromCourse.php", json);
        }

        public async Task MoveCourseToSemester(Course c, int semesterID)
        {
            await MoveCourseToSemester(c.OnlineID, semesterID);
            c.SemesterOnlineID = semesterID;
        }

        private async Task MoveCourseToSemester(int courseID, int semesterID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            json["semesterID"] = semesterID;
            await PostReturnJson("moveCourseToSemester.php", json);
        }

        public async Task SetCanvasPublic(int canvasID, bool makePublic)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["canvasID"] = canvasID;
            json["public"] = makePublic;
            await PostReturnJson("setCanvasPublic.php", json);
        }

        public async Task<int[]> GetCourseCanvasIDs(int courseID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            JObject resultJson = await PostReturnJson("getCourseCanvasIDs.php", json);
            int[] ids = new int[resultJson["ids"].Count()];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = resultJson["ids"][i].ToObject<int>();
            return ids;
        }

        public async Task<int[]> GetOwnCanvasIDs()
        {
            JObject json = new JObject();
            json["auth"] = auth;
            JObject resultJson = await PostReturnJson("getOwnCanvasIDs.php", json);
            int[] ids = new int[resultJson["ids"].Count()];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = resultJson["ids"][i].ToObject<int>();
            return ids;
        }

        public async Task<Canvas> GetCanvas(int canvasID, IManager manager)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["canvasID"] = canvasID;
            JObject resultJson = await PostReturnJson("getCanvas.php", json);
            string type = resultJson["canvas"]["type"].ToString();
            Type t = Globals.StringToType(type);
            return (Canvas)t.GetConstructor(new Type[] { typeof(JObject), typeof(IManager) }).Invoke(new object[] { resultJson["canvas"].ToObject<JObject>(), manager });
        }

        public async Task<int> UploadTest(Test t)
        {
            if (t.CourseOnlineID == 0)
                await UploadCourse(t.Course);
            JObject json = new JObject();
            json["auth"] = auth;
            json["test"] = Serializer.SerializeToJson(t);
            JObject resultJson = await PostReturnJson("uploadTest.php", json).ConfigureAwait(false);
            t.OnlineID = resultJson["testID"].ToObject<int>();
            return t.OnlineID;
        }

        public async Task DeleteTest(int testID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["testID"] = testID;
            await PostReturnJson("deleteTest.php", json);
        }

        public async Task<Graphic.Icon> GetIcon(int iconID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["iconID"] = iconID;
            JObject resultJson = await PostReturnJson("getIcon.php", json).ConfigureAwait(false);
            Graphic.Icon icon = new Utils.Graphic.Icon(resultJson["icon"].ToObject<JObject>());
            return icon;
        }

        public async Task<Graphic.Icon[]> GetAllIcons()
        {
            int[] ids = await GetIconIDs().ConfigureAwait(false);
            Graphic.Icon[] icons = new Graphic.Icon[ids.Length];
            for (int i = 0; i < icons.Length; i++)
                icons[i] = await GetIcon(ids[i]).ConfigureAwait(false);
            return icons;
        }

        public async Task<int[]> GetCourseIDs()
        {
            JObject json = new JObject();
            json["auth"] = auth;
            JObject resultJson = await PostReturnJson("getCourseIDs.php", json).ConfigureAwait(false);
            int[] ids = new int[resultJson["ids"].Count()];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = resultJson["ids"][i].ToObject<int>();
            return ids;
        }

        public async Task<Course> GetCourse(int courseID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            JObject resultJson = await PostReturnJson("getCourse.php", json).ConfigureAwait(false);
            Course c = new Course(resultJson["course"].ToObject<JObject>());
            return c;
        }

        public async Task<Course[]> GetCourses()
        {
            int[] ids = await GetCourseIDs().ConfigureAwait(false);
            Course[] courses = new Course[ids.Length];
            for (int i = 0; i < courses.Length; i++)
                courses[i] = await GetCourse(ids[i]).ConfigureAwait(false);
            return courses;
        }

        public async Task<Semester> GetSemester(int semesterID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["semesterID"] = semesterID;
            JObject resultJson = await PostReturnJson("getSemester.php", json).ConfigureAwait(false);
            Semester s = new Semester(resultJson["semester"].ToObject<JObject>());
            return s;
        }

        public async Task UninterpretCourse(Course c)
        {
            await UninterpretCourse(c.OnlineID).ConfigureAwait(false);
        }

        public async Task UninterpretCourse(int courseID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            await PostReturnJson("uninterpretCourse.php", json);
        }

        public async Task<int[]> GetIconIDs()
        {
            JObject json = new JObject();
            json["auth"] = auth;
            JObject resultJson = await PostReturnJson("getIconIDs.php", json);
            int[] ids = new int[resultJson["ids"].Count()];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = resultJson["ids"][i].ToObject<int>();
            return ids;
        }

        public async Task<int[]> GetCourseTestIDs(Course c)
        {
            return await GetCourseTestIDs(c.OnlineID);
        }

        public async Task<int[]> GetCourseTestIDs(int courseID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["courseID"] = courseID;
            JObject resultJson = await PostReturnJson("getCourseTestIDs.php", json).ConfigureAwait(false);
            int[] ids = new int[resultJson["ids"].Count()];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = resultJson["ids"][i].ToObject<int>();
            return ids;
        }

        public async Task<Test[]> GetCourseTests(Course c)
        {
            Test[] tests = await GetCourseTests(c.OnlineID).ConfigureAwait(false);
            foreach (Test t in tests)
                t.Course = c;
            return tests;
        }

        public async Task<Test[]> GetCourseTests(int courseID)
        {
            int[] ids = await GetCourseTestIDs(courseID).ConfigureAwait(false);
            Test[] tests = new Test[ids.Length];
            for (int i = 0; i < tests.Length; i++)
                tests[i] = await GetTest(ids[i]);
            return tests;
        }

        public async Task<Test> GetTest(int testID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["testID"] = testID;
            JObject resultJson = await PostReturnJson("getTest.php", json).ConfigureAwait(false);
            Test t = new Test(resultJson["test"].ToObject<JObject>());
            return t;
        }
    }
}
