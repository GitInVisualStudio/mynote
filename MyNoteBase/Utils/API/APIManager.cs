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
            HttpResponseMessage response = await client.SendAsync(message);
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

        public async Task DeleteCanvas(int canvasID)
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

        public async Task SetCoursePassword(int courseID, string password)
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

        public async Task<bool> UserIsAdmin(Course c, int userID)
        {
            return await UserIsAdmin(c.OnlineID, userID);
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

        public async Task MoveCourseToSemester(int courseID, int semesterID)
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

        public async Task Test()
        {
            await SetCanvasPublic(25, true);
        }
    }
}
