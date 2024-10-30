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

            var petsQuery = _context.Pets.Include(pet => pet.Details)
                            .Include(pet => pet.Petimages).AsQueryable();

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
                petsQuery = petsQuery.Where(pet => GetAgeCategory((DateTime)pet.Details.DateOfBirth) == viewModel.Age);
            }
            //if (!string.IsNullOrWhiteSpace(viewModel.Age))
            //{
            //    if (viewModel.Age.Equals("Puppy"))
            //    {
            //        DateTime sixMonthsAgo = DateTime.Now.AddMonths(-6);
            //        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth > sixMonthsAgo);
            //    }
            //    else if (viewModel.Age.Equals("Young"))
            //    {
            //        DateTime sixMonthsAgo = DateTime.Now.AddMonths(-6);
            //        DateTime twoYearsAgo = DateTime.Now.AddYears(-2);
            //        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth >= sixMonthsAgo && pet.Details.DateOfBirth < twoYearsAgo);
            //    }

            //    else if (viewModel.Age.Equals("Adult"))
            //    {
            //        DateTime twoYearsAgo = DateTime.Now.AddYears(-2);
            //        DateTime eightYearsAgo = DateTime.Now.AddYears(-8);
            //        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth >= twoYearsAgo && pet.Details.DateOfBirth < eightYearsAgo);
            //    }

            //    else if (viewModel.Age.Equals("Senior"))
            //    {
            //        DateTime eightYearsAgo = DateTime.Now.AddYears(-8);
            //        petsQuery = petsQuery.Where(pet => pet.Details.DateOfBirth >= eightYearsAgo);
            //    }
            //}

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

            if (pets.Any())
            {
                foreach (var pet in pets)
                {
                    model.Add(new UserDashboardViewModel
                    {
                        Id = pet.Id,
                        Name = pet.Details.Name,
                        Breed = pet.Details.Breed,
                        Gender = pet.Details.Gender == "F" ? "Female" : "Male",
                        AgeCategory = GetAgeCategory((DateTime)pet.Details.DateOfBirth),
                        Temperament = pet.Details.Temperament,
                        ImageUrl = pet.Petimages.FirstOrDefault()?.ImageUrl
                    });
                }
            } else
            {
                ViewBag.NoPetsFoundMessage = "No pets match your search criteria. Please try again with other filters.";
            }

            return View(model);
        }

        private string GetAgeCategory(DateTime dateOfBirth)
        {
            int ageInYears = DateTime.Now.Year - dateOfBirth.Year;

            if (dateOfBirth > DateTime.Now.AddYears(-ageInYears))
                ageInYears--;

            if (ageInYears < 1 || (ageInYears == 0 && (DateTime.Now - dateOfBirth).TotalDays < 183))
                return "Puppy"; 
            else if (ageInYears <= 2)
                return "Young"; 
            else if (ageInYears < 8)
                return "Adult"; 
            else
                return "Senior";
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

            return View(pet);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}