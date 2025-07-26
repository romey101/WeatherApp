using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;
using WeatherApp.Helpers;



namespace WeatherAppMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }

            var user = await _userService.GetUserByUsernameAsync(username);

            if (user != null)
            {
                bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.Salt);
                if (isValid)
                {
                    HttpContext.Session.SetInt32("userId", user.Id);
                    return RedirectToAction("Index", "Weather");
                }
            }

            ViewBag.Error = "Invalid credentials.";
            return View();
        }
    }
}
