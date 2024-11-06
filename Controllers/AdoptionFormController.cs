using AdoptionHub.Contexts;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdoptionHub.Controllers
{
    public class AdoptionFormController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdoptionFormController> _logger;

        public AdoptionFormController(ILogger<AdoptionFormController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Form(int petId)
        //{
        //    var model = new AdoptionFormViewModel();
        //    ViewBag.PetId = petId;
        //    return View(model);
        //}
        public IActionResult Form(int petId)
        {
            var model = new AdoptionFormViewModel { PetId = petId };
            return View(model);
        }


        [HttpPost]
        public IActionResult SubmitApplication(AdoptionFormViewModel model)
        {
            var pet = _context.Adoptionapplications.FirstOrDefault(p => p.Id == model.PetId);

            if (pet == null)
            {
                ModelState.AddModelError("", "Pet not found.");
                return View("Form", model);
            }

            var adoptionApplication = new Adoptionapplication
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Comments = model.Comments,
                PetId = model.PetId,
                ApplicationDateTime = DateTime.Now,
                ApplicationStatus = "Pending"  //default status
            };

            _context.Adoptionapplications.Add(adoptionApplication);
            _context.SaveChanges();

            return RedirectToAction("ApplicationConfirmation", new { id = adoptionApplication.Id });
        }
    }
}
