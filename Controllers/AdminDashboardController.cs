using AdoptionHub.Contexts;
using AdoptionHub.Models;
using AdoptionHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;


namespace AdoptionHub.Controllers;

public class AdminDashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly SignupCodeService _signupCodeService;

    public AdminDashboardController(ApplicationDbContext context, SignupCodeService signupCodeService)
    {
        _context = context;
        _signupCodeService = signupCodeService;
    }

    public readonly ILogger<HomeController> _logger;

    public ActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> EditPets(List<Pet> model)
    {
        model = await _context.Pets.Include(p => p.FosterParent).Include(p => p.Details).ToListAsync();
        return View(model);
    }

    public async Task<IActionResult> EditUsers(List<User> model)
    {
        model = await _context.Users.ToListAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPet(int id)
    {
        PetEditViewModel model = new PetEditViewModel();
        model.Pet = await _context.Pets.Include(p => p.Details).Where(p => p.Id == id).FirstAsync();
        if (model.Pet == null)
        {
            model.Pet = new Pet();
        }

        model.Users = await _context.Users.ToListAsync();
        return View(model);
    }

    public async Task<IActionResult> DownloadReport(List<Pet> model)
    {
        model = await _context.Pets.Include(p => p.FosterParent).ToListAsync();
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("ID,Name,FosterParent,Species,Breed,DateOfBirth,Gender,Weight,Color,Temperament,DateArrived,Bio,Status,AdoptionFee");
        foreach (var pet in model)
        { 
            String FosterParent;
            if (pet.FosterParent == null)
            {
                FosterParent = "None";
            }
            else
            {
                FosterParent = pet.FosterParent.FullName;
            }
                
            csv.AppendLine($"{pet.Id},{pet.Name},{FosterParent},{pet.Species},{pet.Breed},{pet.DateOfBirth},{pet
                .Gender},{pet.Weight},{pet.Color},{pet.Temperament},{pet.DateArrived},{pet.Bio},{pet.Status},{pet.AdoptionFee}");
        }
        String date = DateTime.Now.ToString("yyyyMMddHHmm");
      
        return File(Encoding.UTF8.GetBytes(csv.ToString()),"text/csv",$"ListOfPets{date}.csv");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPet(PetEditViewModel model)
    {
        var pet = await _context.Pets.Include(p => p.Details).Where(p => p.Id == model.Pet.Id).FirstAsync();

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
        else if (model.Pet != null)
        {
            var fosterParent = await _context.Users.FindAsync(model.Pet?.FosterParent?.Id);


            model.Pet.FosterParent = fosterParent;
            await _context.Pets.AddAsync(model.Pet);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult GenerateSignupCode()
    {
        string newCode = _signupCodeService.GenerateSignupCode();

        return Json(new { code = newCode });
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}