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
        var username = HttpContext.Session.GetString("userName");
        var fosterUser = _context.Users.FirstOrDefault(u => u.Username == username);

        if (fosterUser == null)
        {
            return NotFound("Foster user not found.");
        }

        //fosterassignments associated with fosterUser.Id
        var fosterAssignments = _context.Fosterassignments
            .Include(fa => fa.Pet).ThenInclude(p => p.Details)
            .Include(fa => fa.Pet).ThenInclude(p => p.Petimages)
            .Where(fa => fa.FosterId == fosterUser.Id)
            .ToList();

        //current date for comparison
        var today = DateOnly.FromDateTime(DateTime.Now);

        foreach (var assignment in fosterAssignments)
        {
            var pet = assignment.Pet;
            var imageUrl = pet.Petimages != null && pet.Petimages.Any()
                ? pet.Petimages.First().ImageUrl
                : "/images/exampleImg/noimage.jpg";

            model.Add(new FosterDashboardViewModel
            {
                Id = pet.Id,
                Name = pet.Details.Name,
                Species = pet.Details.Species,
                Images = new List<string> { imageUrl },
                IsCurrentFoster = assignment.EndDate == null || assignment.EndDate > today
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