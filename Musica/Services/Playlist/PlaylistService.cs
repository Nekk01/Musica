using System.Data;
using Microsoft.Data.SqlClient;


namespace Musica;

public class PlaylistService : IPlaylistService
{
    private readonly IConfiguration _configuration;
    public PlaylistService(IConfiguration configuration)
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
    
    public async Task<List<PlaylistModel>> GetAllRecords(int user_id)
    {
        List<PlaylistModel> Items = new List<PlaylistModel>();

        using (IDbConnection connection = Connection)
        {
            connection.Open();

            string sql = "SELECT id, name, owner_user_id, created_at FROM PLAYLIST WHERE owner_user_id = @Id;";


            using var command = new SqlCommand(sql, (SqlConnection)connection);
            
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = user_id });
            
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                Items.Add(new PlaylistModel
                {
                    id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    owner_user_id = reader.GetInt32(2),
                    created_at = reader.GetDateTime(3)
                });
            }
        }
        return Items;
    }
    public async Task<bool> CreatePlaylist(string name, int owner_id)
    {
        using(IDbConnection connection = Connection)
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand("INSERT INTO PLAYLIST (name, owner_user_id) VALUES (@Name, @Owner_id)", (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50) { Value = name });
                cmd.Parameters.Add(new SqlParameter("@Owner_id", SqlDbType.Int) { Value = owner_id });
                
                await cmd.ExecuteReaderAsync();

                return true;
            }
        }
    }
    public async Task<bool> RemovePlaylist(int id)
    {
        return false;
    }
    public async Task<bool> AddTrackToPlaylist(int playlistId, int trackId)
    {
        using (IDbConnection connection = Connection)
        {
            connection.Open();

            string sql = @"
                IF NOT EXISTS (SELECT 1 FROM PLAYLIST_TRACK WHERE playlist_id = @PlaylistId AND track_id = @TrackId)
                BEGIN
                    INSERT INTO PLAYLIST_TRACK (playlist_id, track_id, added_at)
                    VALUES (@PlaylistId, @TrackId, GETDATE());
                END;";

            using (var cmd = new SqlCommand(sql, (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@PlaylistId", SqlDbType.Int) { Value = playlistId });
                cmd.Parameters.Add(new SqlParameter("@TrackId", SqlDbType.Int) { Value = trackId });
                
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }
    }
    public async Task<bool> RemoveTrackFromPlaylist(int playlistId, int trackId)
    {
        using (IDbConnection connection = Connection)
        {
            connection.Open();

            string sql = "DELETE FROM PLAYLIST_TRACK WHERE playlist_id = @PlaylistId AND track_id = @TrackId;";

            using (var cmd = new SqlCommand(sql, (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@PlaylistId", SqlDbType.Int) { Value = playlistId });
                cmd.Parameters.Add(new SqlParameter("@TrackId", SqlDbType.Int) { Value = trackId });
                
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
