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

    public ActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> EditPets(List<Pet> model)
    {

        model = await _context.Pets.Include(p => p.FosterParent).ToListAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPet(int id)
    {

        PetEditViewModel model = new PetEditViewModel();
        model.Pet = await _context.Pets.FindAsync(id);
        model.Users = await _context.Users.ToListAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPet(PetEditViewModel model)
    {
        var pet = await _context.Pets.FindAsync(model.Pet?.Id);

        if (pet != null)
        {
            var petType = typeof(Pet);
            var modelPet = model.Pet;

            foreach (var property in petType.GetProperties())
            {

                if (property.Name == "Id" || property.Name == "FosterParent")
                    continue;

                var newValue = property.GetValue(modelPet);
                var currentValue = property.GetValue(pet);


                if (newValue != null && !newValue.Equals(currentValue))
                {
                    property.SetValue(pet, newValue);
                }
            }

            var fosterParent = await _context.Users.FindAsync(model.Pet?.FosterParent?.Id);


            pet.FosterParent = fosterParent;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}