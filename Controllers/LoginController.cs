using System.Diagnostics;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace AdoptionHub.Controllers;

public class LoginController : Controller
{
    private readonly string connectionString;

    public LoginController(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public readonly ILogger<LoginController> _logger;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return View("Index");
    }

    public IActionResult LoginMethod(LoginViewModel model)
    {
        model.Username = "fosterPro";
        model.Password = "fosterPass456";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            String query = "SELECT * FROM Users WHERE Username = @Username and Password =@Password";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", model.Username);
                command.Parameters.AddWithValue("@Password", model.Password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        String userRole = reader["userRole"].ToString();
                        HttpContext.Session.SetString("userRole", userRole);
                        HttpContext.Session.SetString("Username", model.Username);
                        HttpContext.Session.SetString("IsAuthenticated", "Y");

                        if (userRole.Equals("admin"))
                        {
                            return RedirectToAction("Index", "AdminDashboard");
                        }
                        else if (userRole.Equals("foster"))
                        {
                            return RedirectToAction("Index", "FosterDashboard");
                        }

                        else
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        model.ErrorMessage = "Your username/password is incorrect";
                        return View("Index", model);
                    }
                }
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}