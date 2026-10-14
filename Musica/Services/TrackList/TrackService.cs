using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Components.Forms;

namespace Musica;

public class TrackService : ITrackService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public TrackService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    private IDbConnection Connection => new SqlConnection(_configuration.GetConnectionString("SQLConnection"));

    public async Task<List<TrackModel>> GetAllRecords()
    {
        List<TrackModel> Items = new List<TrackModel>();

        using (IDbConnection connection = Connection)
        {
            connection.Open();

            string sql = "SELECT t.id, t.title, t.release_date, t.artist_profile_id, t.genre_id, t.file_url, t.cover_url,ap.stage_name FROM TRACK t LEFT JOIN ARTIST_PROFILE ap ON t.artist_profile_id = ap.id;";
            using var command = new SqlCommand(sql, (SqlConnection)connection);
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                Items.Add(new TrackModel
                {
                    ID = reader.GetInt32(0),
                    title = reader.GetString(1),
                    release_dateP = reader.GetDateTime(2),
                    artist_profile_id = reader.GetInt32(3),
                    genre_id = reader.GetInt32(4),
                    file_url = reader.GetString(5),
                    cover_url = reader.GetString(6),
                    artist_name = reader.GetString(7)
                });
            }
        }

        return Items;
    }

    public async Task<bool> UploadRecord(TrackModel model, IBrowserFile audioFile, IBrowserFile coverFile, string userId)
    {
        int artist_profile_id = await FindArtistByUserID(Int32.Parse(userId));

        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        if (audioFile != null)
        {
            long maxAudioSize = 50 * 1024 * 1024;
            string audioExt = Path.GetExtension(audioFile.Name);
            string audioFileName = Guid.NewGuid().ToString() + audioExt;
            string audioFilePath = Path.Combine(uploadsFolder, audioFileName);

            using (var stream = new FileStream(audioFilePath, FileMode.Create))
            {
                await audioFile.OpenReadStream(maxAudioSize).CopyToAsync(stream);
            }
            model.file_url = "/uploads/" + audioFileName;
        }

        if (coverFile != null)
        {
            long maxImageSize = 5 * 1024 * 1024;
            string coverExt = Path.GetExtension(coverFile.Name);
            string coverFileName = "cover_" + Guid.NewGuid().ToString() + coverExt;
            string coverFilePath = Path.Combine(uploadsFolder, coverFileName);

            using (var stream = new FileStream(coverFilePath, FileMode.Create))
            {
                await coverFile.OpenReadStream(maxImageSize).CopyToAsync(stream);
            }
            model.cover_url = "/uploads/" + coverFileName;
        }

        using (IDbConnection connection = Connection)
        {
            connection.Open();

            string sql = @"
                INSERT INTO TRACK (title, file_url, cover_url, artist_profile_id, genre_id) 
                VALUES (@title, @file_url, @cover_url, @artist_profile_id, @genre_id);";

            using var command = new SqlCommand(sql, (SqlConnection)connection);
            
            command.Parameters.Add(new SqlParameter("@title", SqlDbType.NVarChar, 255) { Value = model.title ?? (object)DBNull.Value });
            command.Parameters.Add(new SqlParameter("@file_url", SqlDbType.NVarChar, 255) { Value = model.file_url ?? (object)DBNull.Value });
            command.Parameters.Add(new SqlParameter("@cover_url", SqlDbType.NVarChar, 255) { Value = model.cover_url ?? (object)DBNull.Value });
            command.Parameters.Add(new SqlParameter("@artist_profile_id", SqlDbType.Int) { Value = artist_profile_id });
            command.Parameters.Add(new SqlParameter("@genre_id", SqlDbType.Int) { Value = 1 });

            await command.ExecuteNonQueryAsync();
        }

        return true;
    }

    public async Task<int> FindArtistByUserID(int userId)
    {
        using (IDbConnection connection = Connection)
        {
            connection.Open();
            
            using (SqlCommand cmd = new SqlCommand("SELECT id FROM ARTIST_PROFILE WHERE user_id = @id", (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = userId });

                object? result = await cmd.ExecuteScalarAsync();

                if (result == null)
                {
                    return 0;
                }

                return Convert.ToInt32(result);
            }
        }
    }
}