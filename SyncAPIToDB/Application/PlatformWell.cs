using Newtonsoft.Json;
using SyncAPIToDB.DataAccess;
using SyncAPIToDB.Entities;
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
        public bool InsertPlatform(List<PlatformDto> lsPlatformWell)
        {
            var platform = new PlatformWellDAL();
            return platform.Insert(lsPlatformWell);
        }

        public List<PlatformDto> GetPlatformWell(string token)
        {
            HttpWebResponse response = null;
            List<PlatformDto> lsPlatformWell = new();
            string ResponseString = "";
            var request = (HttpWebRequest)WebRequest.Create(AppSettings.Instance.ApiUrl + "/api/PlatformWell/GetPlatformWellActual");
            request.Accept = "application/json";
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + token;

            try
            {
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    ResponseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }

                lsPlatformWell = JsonConvert.DeserializeObject<List<PlatformDto>>(ResponseString);
                
            }
            catch (Exception ex)
            {
                //Catch exeption here
            }
            return lsPlatformWell;
        }
    }
}
