using AdoptionHub.Contexts;
using AdoptionHub.Filters;
using AdoptionHub.Models;
using AdoptionHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;


namespace AdoptionHub.Controllers;

[RoleAuthorize("admin")]
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
        model = await _context.Pets.Include(p => p.CurrentFosterAssignment).ThenInclude(fa => fa.Foster).Include(p => p.Details).ToListAsync();
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
        model.Pet = await _context.Pets.Include(p => p.CurrentFosterAssignment).Include(p => p.Details).Where(p => p.Id == id).FirstOrDefaultAsync();
        if (model.Pet == null)
        {
            model.Pet = new Pet();
        }

        model.Users = await _context.Users.ToListAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPet(PetEditViewModel model)
    {
        //if no foster was selected in form, replace empty CurrentFosterAssignment object with null
        if (model.Pet?.CurrentFosterAssignment?.FosterId == null)
        {
            model.Pet.CurrentFosterAssignment = null;
        }
        var pet = await _context.Pets.Include(p => p.Details).Where(p => p.Id == model.Pet.Id).FirstOrDefaultAsync();

        //update pet if pet exists
        if (pet != null)
        {
            var petType = typeof(Pet);
            var modelPet = model.Pet;

            foreach (var property in petType.GetProperties())
            {
                if (property.Name == "Id" || property.Name == "FosterParent" || property.Name == "CurrentFosterAssignment")
                    continue;

                var newValue = property.GetValue(modelPet);
                var currentValue = property.GetValue(pet);


                if (newValue != null && !newValue.Equals(currentValue))
                {
                    property.SetValue(pet, newValue);
                }
            }

            //create new foster assignment if foster changed
            var newFoster = await _context.Users.FindAsync(model.Pet?.CurrentFosterAssignment?.FosterId);
            if (newFoster?.Id != pet.CurrentFosterAssignment?.FosterId)
            {
                var newFosterAssignment = new Fosterassignment
                {
                    Foster = newFoster,
                    Pet = pet,
                    StartDate = DateOnly.FromDateTime(DateTime.Now)
                };
                pet.CurrentFosterAssignment = newFosterAssignment;
                await _context.Fosterassignments.AddAsync(newFosterAssignment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //create pet if pet doesn't exist
        else if (model.Pet != null)
        {
            var foster = await _context.Users.FindAsync(model.Pet?.FosterParent?.Id);
            if (foster != null)
            {
                var newFosterAssignment = new Fosterassignment
                {
                    Foster = foster,
                    Pet = model.Pet,
                    StartDate = DateOnly.FromDateTime(DateTime.Now)
                };
                model.Pet.CurrentFosterAssignment = newFosterAssignment;
                await _context.Fosterassignments.AddAsync(newFosterAssignment);
            }
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

    public async Task<IActionResult> DownloadReport(List<Pet> model)
    {
        model = await _context.Pets.Include(p => p.CurrentFosterAssignment).ThenInclude(fa => fa.Foster).Include(p => p.Details).ToListAsync();
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("ID,Name,FosterParent,Species,Breed,DateOfBirth,Gender,Weight,Color,Temperament,DateArrived,Bio,Status,AdoptionFee");
        foreach (var pet in model)
        {
            String FosterParent;
            if (pet.CurrentFosterAssignment?.Foster == null)
            {
                FosterParent = "None";
            }
            else
            {
                FosterParent = pet.CurrentFosterAssignment.Foster.FullName;
            }

            csv.AppendLine($"{pet.Id},{pet.Details.Name},{FosterParent},{pet.Details.Species},{pet.Details.Breed},{pet.Details.DateOfBirth},{pet
                .Details.Gender},{pet.Details.Weight},{pet.Details.Color},{pet.Details.Temperament},{pet.Details.DateArrived},{pet.Details.Bio},{pet.Status},{pet.Details.AdoptionFee}");
        }
        String date = DateTime.Now.ToString("yyyyMMddHHmm");

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"ListOfPets{date}.csv");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}