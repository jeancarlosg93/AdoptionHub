using AdoptionHub.Contexts;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace AdoptionHub.Controllers;

public class AdminDashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminDashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public readonly ILogger<LoginController> _logger;

    public async Task<IActionResult> Index(List<Pet> model)
    {

        model = await _context.Pets.ToListAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPet(int id)
    {

        Pet model = await _context.Pets.FindAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditPet(Pet pet)
    {
        if (ModelState.IsValid)
        {
            _context.Update(pet);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(pet);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}