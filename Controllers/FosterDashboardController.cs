using System.Diagnostics;
using AdoptionHub.Contexts;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace AdoptionHub.Controllers;
public class FosterDashboardController : Controller
{
    public readonly ILogger<LoginController> _logger;
    public readonly string connectionString;

    public FosterDashboardController(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IActionResult Index(List<FosterDashboardViewModel> model)
    {
        if (!HttpContext.Session.GetString("userRole").Equals("foster")) {
            return RedirectToAction("Index", "Home");
        }

        //obtain animal list that the user fosters

        model = new List<FosterDashboardViewModel>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = "select p.* from Pets p join FosterAssignments fa on p.id = fa.petId join Users u on fa.fosterId = u.id where Username = '" + HttpContext.Session.GetString("Username") + "';";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        string name = reader["name"].ToString();
                        string species = reader["species"].ToString();

                        var pet = new FosterDashboardViewModel
                        {
                            Id = id,
                            Name = name,
                            Species = species,
                            Images = new List<string>()
                        };
                        
                        model.Add(pet);
                    }
                }
            }
        }
        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}