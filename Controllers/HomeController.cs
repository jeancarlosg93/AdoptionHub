using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AdoptionHub.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AdoptionHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            List<UserDashboardViewModel> model = new List<UserDashboardViewModel>();

            var pets = _context.Pets.Include(pet => pet.Details);

            foreach (var pet in pets)
            {
                model.Add(new UserDashboardViewModel
                {
                    Id = pet.Id,
                    Name = pet.Details.Name,
                    Breed = pet.Details.Breed,
                    Gender = pet.Details.Gender == "F" ? "Female" : "Male",
                    Age = pet.Details.DateOfBirth.HasValue
                        ? (DateTime.Now.Year - pet.Details.DateOfBirth.Value.Year).ToString()
                        : "Unknown",
                    Temperament = pet.Details.Temperament,
                });
            }

            return View(model);
        }

        public IActionResult Details(int id)
        {
            Pet pet = _context.Pets.Include(p => p.Details).FirstOrDefault(p => p.Id == id);
            
          return View(pet);
        }


        [HttpPost]
        public IActionResult Search(string species, string breed, string gender)
        {
            var filteredPets = GetFilteredPets(species, breed, gender);
            return PartialView("_PetCards", filteredPets);
        }

        private List<UserDashboardViewModel> GetFilteredPets(string species = null, string breed = null, string gender = null)
        {
            // Query the pets with optional filters
            var petsQuery = _context.Pets.Include(pet => pet.Details).AsQueryable();

            if (!string.IsNullOrWhiteSpace(species))
            {
                petsQuery = petsQuery.Where(p => p.Details.Species == species);
            }
            if (!string.IsNullOrWhiteSpace(breed))
            {
                petsQuery = petsQuery.Where(p => p.Details.Breed == breed);
            }
            if (!string.IsNullOrWhiteSpace(gender))
            {
                petsQuery = petsQuery.Where(p => p.Details.Gender == gender);
            }

            var filteredPets = petsQuery.Select(p => new UserDashboardViewModel
            {
                Id = p.Id,
                Name = p.Details.Name,
                Breed = p.Details.Breed,
                Gender = p.Details.Gender == "F" ? "Female" : "Male",
                Age = p.Details.DateOfBirth.HasValue
                    ? (DateTime.Now.Year - p.Details.DateOfBirth.Value.Year).ToString()
                    : "Unknown",
                Temperament = p.Details.Temperament,
            }).ToList();

            return filteredPets;
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}