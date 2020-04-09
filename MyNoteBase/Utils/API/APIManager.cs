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
            auth = await GetAuthToken(username, password).ConfigureAwait(false);
            authSince = DateTime.Now;
        }

        private async Task<string> Post(string uri, Dictionary<string, string> values)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(uri, content).ConfigureAwait(false);
            string responseStr = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseStr;
        }

        private async Task<JObject> PostReturnJson(string uri, Dictionary<string, string> values)
        {
            if (uri != "getAuthToken.php" && DateTime.Now - authSince >= new TimeSpan(24, 0, 0))
                Init().Wait();
            string resultStr = await Post(uri, values).ConfigureAwait(false);
            Console.WriteLine(resultStr);
            JObject json = JObject.Parse(resultStr);
            if (json["result"].ToString() != "ok")
                throw APIExceptionManager.FromID(json["error_code"].ToObject<int>());
            return json;
        }

        public async Task<string> GetAuthToken(string username, string password)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("password", password);
            JObject json = await PostReturnJson("getAuthToken.php", dict).ConfigureAwait(false);
            return json["token"].ToString();
        }

        public async Task<int> UploadSemester(Semester s)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("auth", auth);
            dict.Add("semester", Serializer.Serialize(s));
            JObject json = await PostReturnJson("uploadSemester.php", dict).ConfigureAwait(false);
            return json["semesterID"].ToObject<int>();
        }

        public async Task<string> Test()
        {
            int id = await UploadSemester(new Semester("Q2", DateTime.Now)).ConfigureAwait(false);
            return id.ToString();
        }
    }
}
