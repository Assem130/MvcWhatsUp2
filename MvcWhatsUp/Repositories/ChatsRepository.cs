namespace MvcWhatsUp2.Repositories;
using Microsoft.Data.SqlClient;

using MvcWhatsUP.Models;

public class ChatsRepository : IChatsRepository
{

    private readonly string _connectionString;
    public ChatsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public void AddMessage(Message message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "Insert into Messages(SenderUserId, ReceiverUserId, MessageText, SentAt) values(@SenderUserId, @ReceiverUserId, @MessageText, @SentAt);" + "select SCOPE_IDENTITY()";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SenderUserId", message.SenderUserId);
            command.Parameters.AddWithValue("@ReceiverUserId", message.ReceiverUserId);
            command.Parameters.AddWithValue("@MessageText", message.MessageText);
            command.Parameters.AddWithValue("@SentAt", message.SentAt);
            connection.Open();
            var newId = Convert.ToInt32(command.ExecuteScalar);
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Database error adding message: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding message: {ex.Message}");
            throw; 
        }
    }
    public List<Message> GetMessages(int userId1, int userId2)
    {
        List<Message> messages = new List<Message>();
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "Select MessageId, SenderUserId, ReceiverUserId, MessageText, SentAt from Messages where (SenderUserId = @UserId1 and ReceiverUserId = @UserId2) " +
                "Or (SenderUserId = @UserId2 and ReceiverUserId = @UserId1) order by SendAt ASC";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId1", userId1);
            command.Parameters.AddWithValue("@UserId2", userId2);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new Message(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetDateTime(4)));
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Database error getting messages: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting messages: {ex.Message}");
            throw;
        }
        return messages;
    }


}
