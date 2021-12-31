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
            Console.WriteLine("Getting platform data from API..");
            var platform = new PlatformWell();
            var lsPlatform = platform.GetPlatformWell(token);
            Console.WriteLine("Inserting platform data to database..");
            var result = platform.InsertPlatform(lsPlatform);
            if (result)
            {
                Console.WriteLine("Data successfully inserted into database.");
            }
            else
            {
                Console.WriteLine("Something went wrong during inserting the data.Please relaunch the program.");
            }
        }

    }
}
