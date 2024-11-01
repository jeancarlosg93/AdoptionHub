using AdoptionHub.Contexts;
using AdoptionHub.Filters;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AdoptionHub.Controllers;

[RoleAuthorize("foster")]
public class FosterDashboardController : Controller
{
    private readonly ILogger<FosterDashboardController> _logger;
    private readonly ApplicationDbContext _context;

    public FosterDashboardController(ILogger<FosterDashboardController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index(List<FosterDashboardViewModel> model)
    {

        // Get the current username from the session
        var username = HttpContext.Session.GetString("userName");

        // Query the database to get the list of pets fostered by the current user
        var fosteredPets = (from p in _context.Pets.Include(p => p.Details)
                                //join d in _context.PetDetails on p.Id equals d.Id
                            join fa in _context.Fosterassignments on p.Id equals fa.PetId
                            join u in _context.Users on fa.FosterId equals u.Id
                            where u.Username == username
                            select new FosterDashboardViewModel
                            {
                                Id = p.Id,
                                Name = p.Details.Name,
                                Species = p.Details.Species,
                                Images = _context.Petimages
                                                .Where(pi => pi.PetId == p.Id)
                                                .Select(pi => pi.ImageUrl).ToList()
                            }).ToList();

        model = fosteredPets;

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}