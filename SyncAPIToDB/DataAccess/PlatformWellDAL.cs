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
                                cmd.Parameters.AddWithValue("@Id", platform.Id);
                                cmd.Parameters.AddWithValue("@UniqueName", platform.UniqueName);
                                cmd.Parameters.AddWithValue("@Latitude", platform.Latitude);
                                cmd.Parameters.AddWithValue("@Longitude", platform.Longitude);
                                cmd.Parameters.AddWithValue("@CreatedAt", platform.CreatedAt.HasValue? platform.CreatedAt: DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdatedAt", platform.UpdatedAt.HasValue ? platform.UpdatedAt : DBNull.Value);

                                cmd.Transaction = trans;
                                cmd.ExecuteNonQuery();
                            }

                            foreach (var well in platform.Well)
                            {
                                var resultWell = 0;
                                string queryWell = "";
                                string selectQueryWell = "SELECT count(*) from Well where id=@Id";
                                using (SqlCommand cmdSelectWell = new(selectQueryWell, cn))
                                {
                                    cmdSelectWell.Parameters.AddWithValue("@Id", well.Id);
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
                                    cmdWell.Parameters.AddWithValue("@Id", well.Id);
                                    cmdWell.Parameters.AddWithValue("@PlatformId", well.PlatformId);
                                    cmdWell.Parameters.AddWithValue("@UniqueName", well.UniqueName);
                                    cmdWell.Parameters.AddWithValue("@Latitude", well.Latitude);
                                    cmdWell.Parameters.AddWithValue("@Longitude", well.Longitude);
                                    cmdWell.Parameters.AddWithValue("@CreatedAt", well.CreatedAt.HasValue ? well.CreatedAt : DBNull.Value);
                                    cmdWell.Parameters.AddWithValue("@UpdatedAt", well.UpdatedAt.HasValue ? well.UpdatedAt : DBNull.Value);
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
