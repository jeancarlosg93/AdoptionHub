using System.Diagnostics;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace AdoptionHub.Controllers;

public class FosterDashboardController : Controller
{
    public readonly ILogger<LoginController> _logger;

    public IActionResult Index()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}