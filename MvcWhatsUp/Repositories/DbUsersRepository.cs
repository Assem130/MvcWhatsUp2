using MvcWhatsUP.Models;
using Microsoft.Data.SqlClient;

namespace MvcWhatsUP.Repositories
{
    public class DbUsersRepository : IUsersRepository
    {
        private readonly string? _connectionString;
        private User ReadUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = (int)reader["UserId"],
                UserName = reader["Username"].ToString(),
                Password = reader["Password"].ToString(),
                Email = reader["Email"].ToString(),
                MobileNumber = reader["Mobile"].ToString()
            };
        }



        public DbUsersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WhatsUpDatabase");
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT UserId, UserName, MobileNumber, EmailAddress, Password FROM Users WHERE IsDeleted = 0";
            using var command = new SqlCommand(query, connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    MobileNumber = reader.GetString(2),
                    Email = reader.GetString(3),
                    Password = reader.GetString(4)
                };
                users.Add(user);
            }
            return users;
        }
        public User GetByName(string name)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT UserId, UserName, MobileNumber, EmailAddress, Password FROM Users WHERE IsDeleted = 0";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", name);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    MobileNumber = reader.GetString(2),
                    Email = reader.GetString(3),
                    Password = reader.GetString(4)
                };
            }
            return null;
        }
        public User GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT UserId, UserName, MobileNumber, EmailAddress, Password FROM Users WHERE UserId = @UserId AND IsDeleted = 0";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", id);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    MobileNumber = reader.GetString(2),
                    Email = reader.GetString(3),
                    Password = reader.GetString(4)
                };
            }
            return null;
        }
        public void Add(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (UserName, MobileNumber, EmailAddress, Password) VALUES (@UserName, @MobileNumber, @EmailAddress, @Password)" + "Select Scope_Identity();";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    connection.Open();
                    user.UserId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

        }

        public void Edit(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Add some logging to debug
                Console.WriteLine($"Updating user: {user.UserId}, {user.UserName}");

                string query = "UPDATE Users SET UserName = @UserName, " +
                              "MobileNumber = @MobileNumber, " +
                              "EmailAddress = @Email, " + // Changed to match property name
                              "Password = @Password " +
                              "WHERE UserId = @UserId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName ?? string.Empty);
                    command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber ?? string.Empty);
                    command.Parameters.AddWithValue("@Email", user.Email ?? string.Empty);
                    command.Parameters.AddWithValue("@Password", user.Password ?? string.Empty);
                    command.Parameters.AddWithValue("@UserId", user.UserId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"Rows affected: {rowsAffected}");

                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No user found with ID {user.UserId}");
                    }
                }
            }
        }

        public void SoftDelete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "Update Users SET IsDeleted = 1 WHERE UserId = @UserId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    connection.Open();
                    int nrOfRowsAffected = command.ExecuteNonQuery();
                    if (nrOfRowsAffected == 0)
                    {
                        throw new Exception("No fields deleted");
                    }
                }
            }
        }
        public User GetByLoginCredentials(string username, string password)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT UserId, UserName, MobileNumber, EmailAddress, Password " +
                    "FROM Users " +
                    "WHERE UserName = @username AND Password = @password", connection);

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = ReadUser(reader);
                    }
                }
            }

            return user;
        }
    }
}
