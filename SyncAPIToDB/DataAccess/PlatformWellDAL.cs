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
    public class PlatformWellDAL
    {
        public bool Insert(List<PlatformDto> lsPlatform)
        {
            using (SqlConnection cn = new SqlConnection(AppSettings.Instance.ConnectionString))
            {
                bool success = false;
                cn.Open();
                using (SqlTransaction trans = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (var platform in lsPlatform)
                        {
                            var result = 0;
                            string query = "";
                            string selectQuery = "SELECT count(*) from Platform where id=@Id";
                            using (SqlCommand cmdSelect = new(selectQuery, cn))
                            {
                                cmdSelect.Parameters.AddWithValue("@Id", platform.Id);
                                cmdSelect.Transaction = trans;
                                SqlDataReader reader = cmdSelect.ExecuteReader();
                                while (reader.Read())
                                {
                                    result = (int)reader[0];
                                }
                                reader.Close();
                            }
                            if (result == 0)
                            {
                                query = "INSERT INTO dbo.Platform (Id, UniqueName, Latitude, Longitude, CreatedAt, UpdatedAt) VALUES (@Id, @UniqueName, @Latitude, @Longitude, @CreatedAt, @UpdatedAt) ";
                            }
                            else
                            {
                                query = "UPDATE dbo.Platform SET UniqueName=@UniqueName, Latitude=@Latitude, Longitude=@Longitude, CreatedAt=@CreatedAt, UpdatedAt=@UpdatedAt";
                            }
                            using (SqlCommand cmd = new(query, cn))
                            {
                                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = platform.Id;
                                cmd.Parameters.Add("@UniqueName", SqlDbType.VarChar, 50).Value = platform.UniqueName;
                                cmd.Parameters.Add("@Latitude", SqlDbType.Float).Value = platform.Latitude;
                                cmd.Parameters.Add("@Longitude", SqlDbType.Float).Value = platform.Longitude;
                                cmd.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = platform.CreatedAt;
                                cmd.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = platform.UpdatedAt;

                                cmd.Transaction = trans;
                                cmd.ExecuteNonQuery();
                            }

                            foreach (var well in platform.Well)
                            {
                                var resultWell = 0;
                                string queryWell = "";
                                string selectQueryWell = "SELECT count(*) from Well where id=@Id";
                                using (SqlCommand cmdSelectWell = new SqlCommand(selectQueryWell, cn))
                                {
                                    cmdSelectWell.Parameters.Add("@Id", SqlDbType.Int).Value = well.Id;
                                    cmdSelectWell.Transaction = trans;
                                    SqlDataReader readerWell = cmdSelectWell.ExecuteReader();
                                    while (readerWell.Read())
                                    {
                                        resultWell = (int)readerWell[0];
                                    }
                                    readerWell.Close();
                                }
                                if (resultWell == 0)
                                {
                                    queryWell = "INSERT INTO dbo.Well (Id,PlatformId ,UniqueName, Latitude, Longitude, CreatedAt, UpdatedAt) VALUES (@Id,@PlatformId, @UniqueName, @Latitude, @Longitude, @CreatedAt, @UpdatedAt) ";
                                }
                                else
                                {
                                    queryWell = "UPDATE dbo.Well SET Id=@Id,PlatformId=@PlatformId, UniqueName=@UniqueName, Latitude=@Latitude, Longitude=@Longitude, CreatedAt=@CreatedAt, UpdatedAt=@UpdatedAt where Id = @Id";
                                }
                                using (SqlCommand cmdWell = new(queryWell, cn))
                                {
                                    cmdWell.Parameters.Add("@Id", SqlDbType.Int).Value = well.Id;
                                    cmdWell.Parameters.Add("@PlatformId", SqlDbType.Int).Value = well.PlatformId;
                                    cmdWell.Parameters.Add("@UniqueName", SqlDbType.VarChar, 50).Value = well.UniqueName;
                                    cmdWell.Parameters.Add("@Latitude", SqlDbType.Float).Value = well.Latitude;
                                    cmdWell.Parameters.Add("@Longitude", SqlDbType.Float).Value = well.Longitude;
                                    cmdWell.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = well.CreatedAt;
                                    cmdWell.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = well.UpdatedAt;
                                    cmdWell.Transaction = trans;
                                    cmdWell.ExecuteNonQuery();
                                }
                            }
                        }
                        trans.Commit();
                        success = true;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                cn.Close();
                return success;
            }            
        }
    }
}
