using MvcWhatsUP.Models;

namespace MvcWhatsUP.Repositories
{
    public interface IUsersRepository
    {
        List<User> GetAllUsers();
        User GetByName(string name);
        void Add(User user);
        User GetById(int id);
        void Edit(User user);
        void SoftDelete(int id);

        User GetByLoginCredentials(string userName, string password);
    }
}
