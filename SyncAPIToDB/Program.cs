using SyncAPIToDB.Application;
using System;

namespace SyncAPIToDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Login to Web API..");
            var user = new User();
            var token = user.GetToken();
            if (token.Length == 0)
            {
                Console.WriteLine("Login failed.Please check user credential or API setting.");
            }
            var platform = new PlatformWell();
            var lsPlatform = platform.GetPlatformWell(token);
            platform.InsertPlatform(lsPlatform);
            Console.WriteLine(lsPlatform.ToString());
        }

    }
}
