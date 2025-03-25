namespace MvcWhatsUp2.Repositories;

using MvcWhatsUP.Models;


public interface IChatsRepository
{
    void AddMessage(Message message);
    List<Message> GetMessages(int userId1, int userId2);
}
