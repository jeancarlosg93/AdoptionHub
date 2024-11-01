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

        var pets =  _context.Pets
            .Include(p => p.Details)
            .Include(p => p.Fosterassignments)
            .ThenInclude(fa => fa.Foster)
            .Include(p => p.Petimages)
            .Where(p => p.Fosterassignments.Any(fa => fa.Foster.Username == username))
            .ToList();

        foreach (var pet in pets)
        {
            var imageUrl = pet.Petimages != null && pet.Petimages.Any()
                ? pet.Petimages.First().ImageUrl
                : "/images/exampleImg/noimage.jpg";

            model.Add(new FosterDashboardViewModel
            {
                Id = pet.Id,
                Name = pet.Details.Name,
                Species = pet.Details.Species,
                Images = new List<string> { imageUrl }
            });
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}