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

        public IActionResult OurMission()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Dashboard(UserDashboardViewModel viewModel)
        {
            List<UserDashboardViewModel> model = new List<UserDashboardViewModel>();

            var petsQuery = _context.Pets.Include(pet => pet.Details)
                            .Include(pet => pet.Petimages).AsQueryable();

            DateTime now = DateTime.Now;
            DateTime sixMonthsAgo = now.AddMonths(-6);
            DateTime threeYearsAgo = now.AddYears(-3);
            DateTime sevenYearsAgo = now.AddYears(-7);


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
                switch (viewModel.Age)
                {
                    case "puppy":
                        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth >= sixMonthsAgo);
                        break;
                    case "young":
                        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth < sixMonthsAgo && pet.Details.DateOfBirth >= threeYearsAgo);
                        break;
                    case "adult":
                        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth < threeYearsAgo && pet.Details.DateOfBirth >= sevenYearsAgo);
                        break;
                    case "senior":
                        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth < sevenYearsAgo);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Size))
            {
                switch (viewModel.Size)
                {
                    case "small":
                        petsQuery = petsQuery.Where(pet => pet.Details.Weight.HasValue && pet.Details.Weight.Value <= 25);
                        break;
                    case "medium":
                        petsQuery = petsQuery.Where(pet => pet.Details.Weight.HasValue && pet.Details.Weight.Value > 25 && pet.Details.Weight.Value <= 50);
                        break;
                    case "large":
                        petsQuery = petsQuery.Where(pet => pet.Details.Weight.HasValue && pet.Details.Weight.Value > 50 && pet.Details.Weight.Value <= 90);
                        break;
                    case "extra large":
                        petsQuery = petsQuery.Where(pet => pet.Details.Weight.HasValue && pet.Details.Weight.Value > 90);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Gender))
            {
                petsQuery = petsQuery.Where(pet => pet.Details.Gender == viewModel.Gender);
            }

            var pets = petsQuery.ToList();

            if (pets == null || pets.Count == 0)
            {
                ViewBag.NoPetsFoundMessage = "No pets found matching your criteria.";
                return View(model);
            }

            foreach (var pet in pets)
            {
                var imageUrl = pet.Petimages != null && pet.Petimages.Any()
                    ? pet.Petimages.First().ImageUrl
                    : "/images/exampleImg/noimage.jpg";
                
                model.Add(new UserDashboardViewModel
                {
                    
                    Id = pet.Id,
                    Name = pet.Details.Name,
                    Breed = pet.Details.Breed,
                    Gender = pet.Details.Gender == "F" ? "Female" : "Male",
                    AgeCategory = GetAgeCategory((DateTime)pet.Details.DateOfBirth),
                    Temperament = pet.Details.Temperament,
                    ImageUrl = imageUrl
                });
            }
  
            return View(model);
        }

        private string GetAgeCategory(DateTime dateOfBirth)
        {
            int daysSinceBirth = (DateTime.Now - dateOfBirth).Days;

            if (daysSinceBirth < 183)
            {
                return "Puppy";
            }
            else if (daysSinceBirth < 1095)
            {
                return "Young";
            }
            else if (daysSinceBirth < 2922)
            {
                return "Adult";
            }
            else
            {
                return "Senior";
            }
        }

        private string CalculateAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Now;
            int years = today.Year - dateOfBirth.Year;
            int months = today.Month - dateOfBirth.Month;

            if (today < dateOfBirth.AddYears(years)) //if bday didn't happen yet this year
            {
                years--;
                months += 12;
            }

            if (years > 0)
                return $"{years} year{(years > 1 ? "s" : "")} old";
            else
                return $"{months} month{(months > 1 ? "s" : "")} old";
        }

        public IActionResult Details(int id)
        {
            Pet pet = _context.Pets.Include(p => p.Details)
                    .Include(p => p.Petimages)
                    .FirstOrDefault(p => p.Id == id);
            pet.Details.Gender = pet.Details.Gender == "F" ? "Female" : "Male";
            ViewData["ExactAge"] = CalculateAge((DateTime)pet.Details.DateOfBirth);

            if (pet.Petimages.Count == 0)
            {
                var imageUrl = pet.Petimages != null && pet.Petimages.Any()
                    ? pet.Petimages.First().ImageUrl
                    : "/images/exampleImg/noimage.jpg";
                
                pet.Petimages.Add(new Petimage() { ImageUrl = imageUrl });
            }


            return View(pet);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}