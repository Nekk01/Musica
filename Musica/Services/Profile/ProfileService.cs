using System.Data;
using Microsoft.Data.SqlClient;


namespace Musica;

public class ProfileService : IProfileService
{
    private readonly IConfiguration _configuration;
    public ProfileService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public IDbConnection Connection
    {
        get
        {
            return new SqlConnection(_configuration.GetConnectionString("SQLConnection"));
        }
    }
    public async Task<UserModel> FindByID(int id)
    {
        using (IDbConnection connection = Connection)
            {
                connection.Open();

                using(SqlCommand cmd = new SqlCommand("SELECT display_name, bio, avatar_url, created_at, role_id FROM PROFILE WHERE user_id = @ID", (SqlConnection)connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = id });
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserModel
                            {
                                display_name = reader.GetString(0),
                                bio = reader.GetString(1),
                                avatar_url = reader.GetString(2),
                                created_at = reader.GetDateTime(3),
                                role_id = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }

        return null;
    }

    public async Task<bool> UpdateProfile(int userId, UserModel model)
    {
        using (IDbConnection connection = Connection)
        {
            connection.Open();
            
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE PROFILE SET display_name = @DisplayName, bio = @Bio, avatar_url = @AvatarUrl WHERE user_id = @UserId", (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
                cmd.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar) { Value = model.display_name ?? (object)DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Bio", SqlDbType.NVarChar) { Value = model.bio ?? (object)DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@AvatarUrl", SqlDbType.NVarChar) { Value = model.avatar_url ?? (object)DBNull.Value });
                
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
