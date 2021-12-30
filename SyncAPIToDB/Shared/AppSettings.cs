using System.IO;
using System.Configuration;
using SyncAPIToDB.Entities;

namespace SyncAPIToDB.Shared
{
    public sealed class AppSettings
    {
        private static AppSettings _instance = null;
        private static readonly object padlock = new object();
        public readonly string _connectionString = string.Empty;
        public readonly string _apiURL = string.Empty;
        public readonly LoginRequest _userCredential = new();
        private AppSettings()
        {
            _apiURL = ConfigurationManager.AppSettings["ApiUrl"];
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _userCredential.Username = ConfigurationManager.AppSettings["ApiUsername"];
            _userCredential.Password = ConfigurationManager.AppSettings["ApiPassword"];
        }

        public static AppSettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppSettings();
                    }
                    return _instance;
                }
            }
        }

        public string ConnectionString
        {
            get => _connectionString;
        }

        public string ApiUrl
        {
            get => _apiURL;
        }

        public LoginRequest UserCredential
        {
            get => _userCredential;
        }
    }
}
