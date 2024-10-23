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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}