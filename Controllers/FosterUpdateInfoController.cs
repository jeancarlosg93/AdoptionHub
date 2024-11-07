using AdoptionHub.Contexts;
using AdoptionHub.Filters;
using AdoptionHub.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AdoptionHub.Controllers
{
    [RoleAuthorize("foster")]
    public class FosterUpdateInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

        public FosterUpdateInfoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public IActionResult Index(int petId)
        {
            FosterUpdateInfoViewModel model = new FosterUpdateInfoViewModel();
            model.ImageUrl = new List<string>();
            model.ApptDate = new List<DateTime?>();
            model.ApptReason = new List<string>();

            var pet = _context.Pets.Include(pet => pet.Details)
                .Where(p => p.Id == petId)
                .Include(p => p.Vetappointments)
                .ThenInclude(v =>v.Vet)
                .Include(p => p.Petimages)
                .FirstOrDefault();

            if (pet != null)
            {
                model.Id = pet.Id;
                model.Name = pet.Details.Name;
                model.Bio = pet.Details.Bio;
                model.Breed = pet.Details.Breed;
                model.Gender = pet.Details.Gender;
                model.Temperament = pet.Details.Temperament;

                foreach (var appointment in pet.Vetappointments.OrderByDescending(v => v.ApptDate))
                {
                    string vetName = "N/A";
                    string vetPhone = "N/A";
                    string vetEmail = "N/A";

                    if (appointment.Vet != null)
                    {
                        vetName = $"Dr. {appointment.Vet.FirstName} {appointment.Vet.LastName}".Trim();
                        vetPhone = appointment.Vet.PhoneNumber ?? "N/A";
                        vetEmail = appointment.Vet.Email ?? "N/A";
                    }

                    model.AddAppointment(
                        appointment.ApptDate,
                        appointment.ApptReason ?? "Not specified",
                        vetName,
                        vetPhone,
                        vetEmail
                    );
                }
                foreach (var image in pet.Petimages)
                {
                    model.ImageUrl.Add(image.ImageUrl);
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePetInfo(FosterUpdateInfoViewModel model, List<string> cloudinaryUrl)
        {
            var pet = _context.Pets.Include(pet => pet.Details).First(pet => pet.Id == model.Id);

            // Update pet bio
            pet.Details!.Bio = model.Bio;
            _context.Pets.Update(pet);

            foreach (var image in cloudinaryUrl)
            {
                var petImage = new Petimage
                {
                    PetId = pet.Id,
                    ImageUrl = image
                };
                _context.Petimages.Add(petImage);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "FosterDashboard");
        }

        [HttpPost]
        public IActionResult DeleteImage(string imageUrl)
        {
            try
            {
                var image = _context.Petimages.FirstOrDefault(image => image.ImageUrl.Contains(imageUrl));
                if (image == null)
                {
                    return Json(new { success = false, message = "Image not found" });
                }
                
                string imageToDelete = ExtractImageId(image.ImageUrl);
                
                var deleteParams = new DelResParams(){
                    PublicIds = new List<string>{imageToDelete},
                    Type = "upload",
                    ResourceType = ResourceType.Image};
                
                var result = _cloudinary.DeleteResources(deleteParams);
                Console.WriteLine(result.JsonObj);

                _context.Petimages.Remove(image);
                _context.SaveChanges();
        
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { success = false, message = e.Message });
            }
        }

        public string ExtractImageId(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return string.Empty;
            string[] parts = imageUrl.Split('/');
            string lastPart = parts[parts.Length - 1];
            return Path.GetFileNameWithoutExtension(lastPart);
        }
    }
}