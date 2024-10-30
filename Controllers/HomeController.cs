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

        public IActionResult Dashboard(UserDashboardViewModel viewModel)
        {
            List<UserDashboardViewModel> model = new List<UserDashboardViewModel>();

            var petsQuery = _context.Pets.Include(pet => pet.Details).AsQueryable();

            if (!string.IsNullOrWhiteSpace(viewModel.Species))
            {
                petsQuery = petsQuery.Where(pet => pet.Details.Species == viewModel.Species);
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Breed))
            {
                petsQuery = petsQuery.Where(pet => pet.Details.Breed == viewModel.Breed);
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Age))
            {
                if (viewModel.Age.Equals("Puppy"))
                {
                    DateTime sixMonthsAgo = DateTime.Now.AddMonths(-6);
                    petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth > sixMonthsAgo);
                }
                else if (viewModel.Age.Equals("Young"))
                {
                    DateTime sixMonthsAgo = DateTime.Now.AddMonths(-6);
                    DateTime twoYearsAgo = DateTime.Now.AddYears(-2);
                    petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth >= sixMonthsAgo && pet.Details.DateOfBirth < twoYearsAgo);
                }

                else if (viewModel.Age.Equals("Adult"))
                {
                    DateTime twoYearsAgo = DateTime.Now.AddYears(-2);
                    DateTime eightYearsAgo = DateTime.Now.AddYears(-8);
                    petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth >= twoYearsAgo && pet.Details.DateOfBirth < eightYearsAgo);
                }

                else if (viewModel.Age.Equals("Senior"))
                {
                    DateTime eightYearsAgo = DateTime.Now.AddYears(-8);
                    petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth >= eightYearsAgo);
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Size))
            {
                if (viewModel.Size.Equals("Small"))
                {
                    petsQuery = petsQuery.Where(pet => pet.Details.Weight >= 0 && pet.Details.Weight <= 25);
                }

                else if (viewModel.Size.Equals("Medium"))
                {
                    petsQuery = petsQuery.Where(pet => pet.Details.Weight >= 26 && pet.Details.Weight <= 50);
                }

                else if (viewModel.Size.Equals("Large"))
                {
                    petsQuery = petsQuery.Where(pet => pet.Details.Weight >= 51 && pet.Details.Weight <= 90);
                }

                else if (viewModel.Size.Equals("Extra Large"))
                {
                    petsQuery = petsQuery.Where(pet => pet.Details.Weight > 90);
                }
            }
           

            if (!string.IsNullOrWhiteSpace(viewModel.Gender))
            {
                petsQuery = petsQuery.Where(pet => pet.Details.Gender == viewModel.Gender);
            }

            var pets = petsQuery.ToList();

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