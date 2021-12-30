using SyncAPIToDB.Entities;
using SyncAPIToDB.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncAPIToDB.DataAccess
{
    public class PlatformWell
    {
        public void Insert(PlatformDto platform)
        {
            string query = "INSERT INTO dbo.Platform (Id, UniqueName, Latitude, Longitude, CreatedAt, UpdatedAt) " +
                   "VALUES (@Id, @UniqueName, @Latitude, @Longitude, @CreatedAt, @UpdatedAt) ";

            using (SqlConnection cn = new SqlConnection(AppSettings.Instance.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                // define parameters and their values
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = platform.Id;
                cmd.Parameters.Add("@UniqueName", SqlDbType.VarChar, 50).Value = platform.UniqueName;
                cmd.Parameters.Add("@Latitude", SqlDbType.Float).Value = platform.Latitude;
                cmd.Parameters.Add("@Longitude", SqlDbType.Float).Value = platform.Longitude;
                cmd.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = platform.CreatedAt;
                cmd.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = platform.UpdatedAt;

                // open connection, execute INSERT, close connection
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
