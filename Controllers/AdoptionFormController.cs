using AdoptionHub.Contexts;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

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

        [Route("Adoption/Form")]
        public IActionResult Form(int petId)
        {
            var pet = _context.Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null)
            {
                return NotFound();
            }

            var model = new AdoptionFormViewModel
            {
                PetId = petId
            };
            return View(model);
        }

        
        [HttpPost]
        public IActionResult SubmitApplication(AdoptionFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adoptionApplication = new Adoptionapplication
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Address = model.Address,
                    City = model.City,
                    Province = model.Province,
                    Country = model.Country,
                    PhoneNumber = model.PhoneNumber,
                    Comments = model.Comments,
                    PetId = model.PetId,
                    ApplicationDateTime = DateTime.Now,
                    ApplicationStatus = "Pending"  // default status
                };

                _context.Adoptionapplications.Add(adoptionApplication);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Thanks for your submission!";

                return View("Form", model);
            }

            return View("Form", model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
