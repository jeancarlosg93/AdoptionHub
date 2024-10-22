using System.Diagnostics;
using AdoptionHub.Contexts;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace AdoptionHub.Controllers;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;

    public LoginController(ApplicationDbContext context)
    {
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
                return RedirectToAction("Index", "AdminDashboard");
            }

            if (user.UserRole == "foster")
            {
                return RedirectToAction("Index", "FosterDashboard");
            }
        }
        else
        {
            model.ErrorMessage = "Your username/password is incorrect";
            return View("Index",model);
        }

        return View("Index",model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}