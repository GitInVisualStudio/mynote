using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyNoteBase.Utils.API
{
    public class APIManager
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://127.0.0.1:80/api/")
        };

        private async Task<string> Post(string uri, Dictionary<string, string> values)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync(uri, content).ConfigureAwait(false);
            string responseStr = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseStr;
        }

        public async Task<string> GetAuthToken(string username, string password)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("password", password);
            string resultStr = await Post("getAuthToken.php", dict).ConfigureAwait(false);
            JObject json = JObject.Parse(resultStr);
            if (json["result"].ToString() != "ok")
                throw APIExceptionManager.FromID(json["error_code"].ToObject<int>());
            return json["token"].ToString();
        }

        public async Task<string> Test()
        {
            string token = await GetAuthToken("yamimiriam", "Maxstrasse90").ConfigureAwait(false);
            return token;
        }
    }
}
