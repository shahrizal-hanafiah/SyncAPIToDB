using Newtonsoft.Json;
using SyncAPIToDB.Shared;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace SyncAPIToDB.Application
{
    public class User
    {
        public string GetToken()
        {
            string ResponseString = "";
            string token = "";
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(AppSettings.Instance.ApiUrl + "/api/Account/Login");
            request.Accept = "application/json";
            request.Method = "POST";
            var content = JsonConvert.SerializeObject(AppSettings.Instance._userCredential);
            var data = Encoding.ASCII.GetBytes(content);

            try
            {
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    ResponseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }

                token = JsonConvert.DeserializeObject<string>(ResponseString);

            }
            catch (Exception ex)
            {
                //Catch exeption here
            }
            return token;
        }
    }
}
