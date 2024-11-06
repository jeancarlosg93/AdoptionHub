using AdoptionHub.Contexts;
using AdoptionHub.Filters;
using AdoptionHub.Models;
using AdoptionHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        model = await _context.Pets.Include(p => p.CurrentFosterAssignment).ThenInclude(fa => fa.Foster)
            .Include(p => p.Details).ToListAsync();
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
        //options for status dropdown
        var statusOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "Available", Text = "Available" },
            new SelectListItem { Value = "Fostered", Text = "Fostered" },
            new SelectListItem { Value = "Adopted", Text = "Adopted" }
        };
        PetEditViewModel model = new PetEditViewModel();
        model.Pet = await _context.Pets.Include(p => p.CurrentFosterAssignment).Include(p => p.Details)
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        if (model.Pet == null)
        {
            model.Pet = new Pet();
            model.Pet.Status = "Available";
        }
        else
        {
            statusOptions.FirstOrDefault(option => option.Value == model.Pet?.Status).Selected = true;
        }

        ViewBag.StatusOptions = statusOptions;
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

        var pet = await _context.Pets.Include(p => p.Details).Include(p => p.CurrentFosterAssignment)
            .Where(p => p.Id == model.Pet.Id).FirstOrDefaultAsync();


        //update pet if pet exists
        if (pet != null)
        {
            var petType = typeof(Pet);
            var modelPet = model.Pet;

            foreach (var property in petType.GetProperties())
            {
                if (property.Name == "Id" || property.Name == "FosterParent" || property.Name == "FosterParentId" ||
                    property.Name == "CurrentFosterAssignment" || property.Name == "CurrentFosterAssignmentId" ||
                    property.Name == "Fosterassignments")
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
                //if pet was previously fostered, set foster assignment end date to now
                if (pet.CurrentFosterAssignment != null)
                {
                    pet.CurrentFosterAssignment.EndDate = DateOnly.FromDateTime(DateTime.Now);
                }

                //add new foster assignment if foster was selected
                if (newFoster != null)
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
                //remove foster assignment if no foster selected
                else
                {
                    pet.CurrentFosterAssignment = null;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("EditPets");
        }
        //create pet if pet doesn't exist
        else if (model.Pet != null)
        {
            var foster = await _context.Users.FindAsync(model.Pet?.CurrentFosterAssignment?.FosterId);
            //after model.Pet.CurrentFosterAssignemnt is used to set foster, clear CurrentFosterAssignment and save pet to avoid creating a foster assignment
            //with no petId and so that pet will have an id to be used when creating new Fosterassignment
            model.Pet.CurrentFosterAssignment = null;
            await _context.Pets.AddAsync(model.Pet);
            await _context.SaveChangesAsync();
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

            await _context.SaveChangesAsync();
            return RedirectToAction("EditPets");
        }

        return View(model);
    }

    [HttpGet]

    public async Task<IActionResult> ManageApplications()
    {
        var model = await _context.Adoptionapplications.ToListAsync();
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
        model = await _context.Pets.Include(p => p.CurrentFosterAssignment).ThenInclude(fa => fa.Foster)
            .Include(p => p.Details).ToListAsync();
        StringBuilder csv = new StringBuilder();
        csv.AppendLine(
            "ID,Name,FosterParent,Species,Breed,DateOfBirth,Gender,Weight,Color,Temperament,DateArrived,Bio,Status,AdoptionFee");
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

            csv.AppendLine(
                $"{pet.Id},{pet.Details.Name},{FosterParent},{pet.Details.Species},{pet.Details.Breed},{pet.Details.DateOfBirth},{pet
                    .Details.Gender},{pet.Details.Weight},{pet.Details.Color},{pet.Details.Temperament},{pet.Details.DateArrived},{pet.Details.Bio},{pet.Status},{pet.Details.AdoptionFee}");
        }

        String date = DateTime.Now.ToString("yyyyMMddHHmm");

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"ListOfPets{date}.csv");
    }

    
public async Task<IActionResult> VetAppointments()
{
    var appointments = await _context.Vetappointments
        .Include(v => v.Pet)
            .ThenInclude(p => p.Details)
        .Include(v => v.Vet)
        .OrderByDescending(v => v.ApptDate)
        .ToListAsync();
    
    return View(appointments);
}

[HttpGet]
public async Task<IActionResult> EditVetAppointment(int? id)
{
    var viewModel = new VetAppointmentViewModel
    {
        AvailablePets = await _context.Pets.Include(p => p.Details).ToListAsync(),
        AvailableVets = await _context.Veterinarians.ToListAsync()
    };

    if (id.HasValue)
    {
        var appointment = await _context.Vetappointments
            .Include(v => v.Pet)
            .Include(v => v.Vet)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (appointment != null)
        {
            viewModel.Id = appointment.Id;
            viewModel.PetId = appointment.PetId;
            viewModel.VetId = appointment.VetId;
            viewModel.ApptDate = appointment.ApptDate;
            viewModel.ApptReason = appointment.ApptReason;
            viewModel.Pet = appointment.Pet;
            viewModel.Vet = appointment.Vet;
        }
    }

    return View(viewModel);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditVetAppointment(VetAppointmentViewModel model)
{
    if (ModelState.IsValid)
    {
        Vetappointment appointment;
        if (model.Id == 0)
        {
            appointment = new Vetappointment();
            await _context.Vetappointments.AddAsync(appointment);
        }
        else
        {
            appointment = await _context.Vetappointments.FindAsync(model.Id);
            if (appointment == null)
            {
                return NotFound();
            }
        }

        appointment.PetId = model.PetId;
        appointment.VetId = model.VetId;
        appointment.ApptDate = model.ApptDate;
        appointment.ApptReason = model.ApptReason;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(VetAppointments));
    }

    // If we got this far, something failed, redisplay form
    model.AvailablePets = await _context.Pets.Include(p => p.Details).ToListAsync();
    model.AvailableVets = await _context.Veterinarians.ToListAsync();
    return View(model);
}

[HttpPost]
public async Task<IActionResult> DeleteVetAppointment(int id)
{
    var appointment = await _context.Vetappointments.FindAsync(id);
    if (appointment != null)
    {
        _context.Vetappointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }
    return RedirectToAction(nameof(VetAppointments));
}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}