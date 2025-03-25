using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUP.Models;

namespace MvcWhatsUP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public string GetAllStudents()
        //{
        //    return "Diplaying all students";
        //}
        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }
        [HttpPost]
        public string SendMessage(Message message)
        {
            return $"Message {message.MessageText} has been sent by {message.SenderUserId}";
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
