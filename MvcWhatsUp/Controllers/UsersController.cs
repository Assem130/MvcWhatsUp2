using Microsoft.AspNetCore.Mvc;
using MvcWhatsUP.Models;
using MvcWhatsUP.Repositories;

namespace MvcWhatsUP.Controllers
{
    public class UsersController : Controller
    {
        private IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IActionResult Index()
        {
            List<User> users = _usersRepository.GetAllUsers();

            string userIdString = Request.Cookies["UserId"];

            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int loggedInUserId))
            {
                ViewData["LoggedInUserId"] = loggedInUserId;
            }

            return View(users);
        }
        public IActionResult Search(string name)
        {
            User user = _usersRepository.GetByName(name);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                _usersRepository.Add(user);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(user);
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("User not found.");
            }
            User? user = _usersRepository.GetById(id.Value);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _usersRepository.Edit(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception
                    ModelState.AddModelError("", $"Error updating user: {ex.Message}");
                }
            }
            return View(user);

        }
        [HttpGet]
        public ActionResult SoftDelete(int? id)
        {
            var user = _usersRepository.GetById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                _usersRepository.SoftDelete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            User user = _usersRepository.GetByLoginCredentials(loginModel.UserName, loginModel.Password);

            if (user != null)
            {
                Response.Cookies.Append("UserId", user.UserId.ToString());
                return RedirectToAction("Index", "Users");
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password";
                return View(loginModel);
            }


        }
    }


}
