using Microsoft.AspNetCore.Mvc;
using MvcWhatsUP.Models;
using MvcWhatsUP.Repositories;
using MvcWhatsUp2.Repositories;

namespace MvcWhatsUp.Controllers
{
    public class ChatsController : Controller
    {
        private readonly IChatsRepository _chatsRepository;
        private readonly IUsersRepository _usersRepository;

        // Constructor with dependency injection
        public ChatsController(IChatsRepository chatsRepository, IUsersRepository usersRepository)
        {
            _chatsRepository = chatsRepository;
            _usersRepository = usersRepository;
        }

        // GET: /Chats/AddMessage/{id}
        public IActionResult AddMessage(int? id)
        {
            // Receiver user id (parameter) must be available
            if (id == null)
                return RedirectToAction("Index", "Users");

            // User needs to be logged in
            // (for now, id of logged in user is stored in a cookie)
            string? loggedInUserId = Request.Cookies["UserId"];
            if (loggedInUserId == null)
                return RedirectToAction("Index", "Users");

            // Get the receiving User so we can show the name in the View
            User? receiverUser = _usersRepository.GetById((int)id);
            ViewData["ReceiverUser"] = receiverUser;

            // Create message object and set sender/receiver
            Message message = new Message();
            message.SenderUserId = int.Parse(loggedInUserId);
            message.ReceiverUserId = (int)id;

            return View(message);
        }

        // POST: /Chats/AddMessage
        [HttpPost]
        public IActionResult AddMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                // Set the message date/time to now
                message.SendDateTime = DateTime.Now;

                // Add the message to the database
                _chatsRepository.AddMessage(message);

                // Redirect to the DisplayChat action to show all messages
                return RedirectToAction("DisplayChat", new { id = message.ReceiverUserId });
            }

            // If there's an error, get the receiver user again to display the name
            User? receiverUser = _usersRepository.GetById(message.ReceiverUserId);
            ViewData["ReceiverUser"] = receiverUser;

            return View(message);
        }

        // GET: /Chats/DisplayChat/{id}
        public IActionResult DisplayChat(int? id)
        {
            // Receiver user id (parameter) must be available
            if (id == null)
                return RedirectToAction("Index", "Users");

            // User needs to be logged in
            string? loggedInUserId = Request.Cookies["UserId"];
            if (loggedInUserId == null)
                return RedirectToAction("Index", "Users");

            int loggedInUserIdInt = int.Parse(loggedInUserId);

            // Get both users to display their names
            User? loggedInUser = _usersRepository.GetById(loggedInUserIdInt);
            User? otherUser = _usersRepository.GetById((int)id);

            if (loggedInUser == null || otherUser == null)
                return RedirectToAction("Index", "Users");

            // Send both users to the view (for displaying their names)
            ViewData["LoggedInUser"] = loggedInUser;
            ViewData["OtherUser"] = otherUser;

            // Get all messages between these two users
            var messages = _chatsRepository.GetMessages(loggedInUserIdInt, (int)id);

            return View(messages);
        }
    }
}