using System.Diagnostics;
using AdoptionHub.Contexts;
using AdoptionHub.Models;
using AdoptionHub.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace AdoptionHub.Controllers;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly LogInLogService _logInLogService;

    public LoginController(ApplicationDbContext context ,LogInLogService logInLogService)
    {
        _logInLogService = logInLogService;
        _context = context;
    }

    public readonly ILogger<LoginController> _logger;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult LoginMethod(LoginViewModel model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        if (user != null)
        {
            HttpContext.Session.SetString("userName", user.Username);
            HttpContext.Session.SetString("userRole", user.UserRole);
            HttpContext.Session.SetString("IsAuthenticated", "Y");

            if (user.UserRole == "admin")
            {
                _logInLogService.UpdateLogRegistry("userName: " + model.Username + ", result: Successful login");
                return RedirectToAction("Index", "AdminDashboard");
            }

            if (user.UserRole == "foster")
            {
                _logInLogService.UpdateLogRegistry("userName: " + model.Username + ", result: Successful login");
                return RedirectToAction("Index", "FosterDashboard");
            }
        }
        else
        {
                _logInLogService.UpdateLogRegistry($"userName: {model.Username}, result: Unsuccessful login");
            model.ErrorMessage = "Your username or password is incorrect";
            return View("Index",model);
        }

        return View("Index",model);
    }

    public IActionResult Register(RegisterViewModel model)
    {
        var code = _context.SignupCodes.FirstOrDefault(a => a.Code == model.Code);

        var newuser = new User();

        if (model != null)
        {
            newuser.Username = model.User.Username;
            newuser.Password = model.User.Password;
            newuser.Email = model.User.Email;
            newuser.LastName = model.User.LastName;
            newuser.FirstName = model.User.FirstName;
            newuser.Address = model.User.Address;
            newuser.PhoneNumber =  model.User.PhoneNumber;
        }

        if (code != null)
        {
            _context.Users.Add(newuser);
        }

        return View("Register",model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}