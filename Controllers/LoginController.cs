using AdoptionHub.Contexts;
using AdoptionHub.Models;
using AdoptionHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace AdoptionHub.Controllers;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly LogInLogService _logInLogService;

    public LoginController(ApplicationDbContext context, LogInLogService logInLogService)
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

    public async Task<IActionResult> LoginMethod(LoginViewModel model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

        if (user == null)
        {
            await _logInLogService.UpdateLogRegistry($"userName: {model.Username}, result: Unsuccessful login");
            model.ErrorMessage = "The provided username does not exist";
            return View("Index", model);
        }
        else
        {
            var hashedPassword = HashPassword(model.Password, user.Salt);
            if (user.Password != hashedPassword)
            {
                await _logInLogService.UpdateLogRegistry($"userName: {model.Username}, result: Unsuccessful login");
                model.ErrorMessage = "The provided password is incorrect";
                return View("Index", model);
            }
            HttpContext.Session.SetString("userName", user.Username);
            HttpContext.Session.SetString("userRole", user.UserRole);
            HttpContext.Session.SetString("IsAuthenticated", "Y");
            if (user.UserRole == "admin")
            {
                await _logInLogService.UpdateLogRegistry("userName: " + model.Username + ", result: Successful login");
                HttpContext.Session.SetString("IsAdmin", "Y");
                return RedirectToAction("Index", "AdminDashboard");
            }

            if (user.UserRole == "foster")
            {
                await _logInLogService.UpdateLogRegistry("userName: " + model.Username + ", result: Successful login");
                HttpContext.Session.SetString("IsFoster", "Y");
                return RedirectToAction("Index", "FosterDashboard");
            }
        }
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel { User = new User() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ModelState.Remove("User.Salt");
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var code = await _context.SignupCodes.FirstOrDefaultAsync(a =>
                a.Code == model.Code &&
                a.ExpiresAt > DateTime.Now);

            if (code == null)
            {
                ModelState.AddModelError("Code", "Invalid or expired registration code");
                return View(model);
            }


            if (await _context.Users.AnyAsync(u => u.Username == model.User.Username))
            {
                ModelState.AddModelError("User.Username", "Username already taken");
                return View(model);
            }

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(model.User.Password, salt);



            var newUser = new User
            {
                Username = model.User.Username,
                Password = hashedPassword,
                Salt = salt,
                Email = model.User.Email,
                LastName = model.User.LastName,
                FirstName = model.User.FirstName,
                Address = model.User.Address,
                PhoneNumber = model.User.PhoneNumber,
                UserRole = "foster"
            };

            await _context.Users.AddAsync(newUser);

            _context.SignupCodes.Remove(code);

            await _context.SaveChangesAsync();

            _logInLogService.UpdateLogRegistry($"New user registered: {newUser.Username}");

            TempData["SuccessMessage"] = "Registration successful! Please log in.";
            return RedirectToAction("Index", "Login");
        }
        catch (Exception ex)
        {
            _logInLogService.UpdateLogRegistry($"Registration failed: {ex.Message}");
            ModelState.AddModelError("", "An error occurred during registration. Please try again.");
            return View(model);
        }
    }

    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        RandomNumberGenerator.Fill(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    private string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + salt;
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashBytes);
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}