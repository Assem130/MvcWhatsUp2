namespace MvcWhatsUP.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public User() { }

        public User(int userId, string userName, string mobileNumber, string email, string password)
        {
            UserId = userId;
            UserName = userName;
            MobileNumber = mobileNumber;
            Email = email;
            Password = password;
        }
    }
}
