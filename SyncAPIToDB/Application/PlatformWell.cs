using Newtonsoft.Json;
using SyncAPIToDB.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SyncAPIToDB.Application
{
    public class PlatformWell
    {
        public void InsertPlatform(List<PlatformWell> lsPlatformWell)
        {

        }

        public List<PlatformWell> GetPlatformWell(string token)
        {
            HttpWebResponse response = null;
            List<PlatformWell> lsPlatformWell = new();
            string ResponseString = "";
            var request = (HttpWebRequest)WebRequest.Create(AppSettings.Instance._apiURL + "/api/PlatformWell/GetPlatformWellActual");
            request.Accept = "application/json";
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + token;

            try
            {
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    ResponseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }

                lsPlatformWell = JsonConvert.DeserializeObject<List<PlatformWell>>(ResponseString);
                
            }
            catch (Exception ex)
            {
                //Catch exeption here
            }
            return lsPlatformWell;
        }
    }
}
