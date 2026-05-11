using System;
using System.Security.Claims;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
// using MimeKit;
// using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;

namespace Musica;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    public AuthService(IConfiguration configuration)
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
    public async Task<ClaimsPrincipal> AuthenticateUser(string email, string password)
    {
        using(IDbConnection connection = Connection)
        {
            connection.Open();

            string sql = "SELECT user_id, email, password FROM PROFILE WHERE email = @Email AND password = @Password;";
            using var command = new SqlCommand(sql, (SqlConnection)connection);
                
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) { Value = email });
            command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 20) { Value = password });
            
            object? result = await command.ExecuteScalarAsync();

            if (result == null)
            {
                return null;
            }

            var UserID = await command.ExecuteScalarAsync();
            string UserIDAsString = UserID.ToString();
            
            // Console.WriteLine(UserIDAsString);

            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, UserIDAsString)
            };

            var identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return new ClaimsPrincipal(identity);
        }
    }

    public async Task<bool> RegisterUser(string email, string password, string displayName)
    {
        using(IDbConnection connection = Connection)
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT email FROM PROFILE WHERE email = @Email", (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) { Value = email });

                object? checkIfExistMatch = await cmd.ExecuteScalarAsync();

                if (checkIfExistMatch != null)
                {
                    return false;
                }
            }



            string sql = "INSERT INTO PROFILE (email, password, display_name) VALUES (@Email, @Password, @Name)";
            using var command = new SqlCommand(sql, (SqlConnection)connection);
            
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) { Value = email });
            command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 50) { Value = password });
            command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50) { Value = displayName });

            int result = command.ExecuteNonQuery();
            return true;
        }
    }

    public async Task<bool> RegisterArtist(int id)
    {
        using(IDbConnection connection = Connection)
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT user_id FROM ARTIST_PROFILE WHERE user_id = @id", (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

                object? checkIfExistMatch = await cmd.ExecuteScalarAsync();

                if (checkIfExistMatch != null)
                {
                    return false;
                }
            }


            using (SqlCommand cmd = new SqlCommand("INSERT INTO ARTIST_PROFILE (user_id) VALUES (@id)", (SqlConnection)connection))
            {
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
                
                await cmd.ExecuteScalarAsync();

                return true;
            }
        }
    }
}
