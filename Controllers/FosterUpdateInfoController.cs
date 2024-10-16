using AdoptionHub.Contexts;
using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace AdoptionHub.Controllers
{
    public class FosterUpdateInfoController : Controller
    {

        private readonly ApplicationDbContext _context;

        public FosterUpdateInfoController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int petId)
        {
            FosterUpdateInfoViewModel model = new FosterUpdateInfoViewModel();
            model.ImageUrl = new List<string>();
            model.ApptDate = new List<DateTime>();
            model.ApptReason = new List<string>();

            var pet = _context.Pets
                .Where(p => p.Id == petId)
                .Include(p => p.Vetappointments)
                .Include(p => p.Petimages)
                .FirstOrDefault();

            if (pet != null)
            {
                model.Id = pet.Id;
                model.Name = pet.Name;
                model.Bio = pet.Bio;

                foreach (var appointment in pet.Vetappointments)
                {
                    model.ApptDate.Add(appointment.ApptDate);
                    model.ApptReason.Add(appointment.ApptReason);
                }

                foreach (var image in pet.Petimages)
                {
                    model.ImageUrl.Add(image.ImageUrl);
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePetInfo(FosterUpdateInfoViewModel model, IFormFile newImage)
        {
            var pet = _context.Pets.Find(model.Id);

            if (pet != null)
            {
                // Update pet bio
                pet.Bio = model.Bio;
                _context.Pets.Update(pet);
                _context.SaveChanges();

                // Handle new image upload
                if (newImage != null && newImage.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images", newImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        newImage.CopyTo(stream); // Save uploaded file
                    }

                    // Save image info to database
                    var petImage = new Petimage
                    {
                        PetId = pet.Id,
                        ImageUrl = $"/images/{newImage.FileName}"
                    };
                    _context.Petimages.Add(petImage);
                    _context.SaveChanges();
                }
            }

            // Redirect back to foster dashboard
            return RedirectToAction("Index", "FosterDashboard");
        }




        //public IActionResult Index(int petId)
        //{
        //    FosterUpdateInfoViewModel model = new FosterUpdateInfoViewModel();
        //    model.ImageUrl = new List<string>();
        //    model.ApptDate = new List<DateTime>();
        //    model.ApptReason = new List<string>();

        //using (MySqlConnection connection = new MySqlConnection(connectionString))
        //{
        //    connection.Open();
        //    string sql = "SELECT p.*, va.apptDate, va.apptReason, pi.imageUrl FROM Pets p LEFT JOIN vetAppointments va ON p.id = va.petId LEFT JOIN PetImages pi ON p.id = pi.petId WHERE p.Id = @petId;";

        //    using (MySqlCommand command = new MySqlCommand(sql, connection))
        //    {
        //        command.Parameters.AddWithValue("@petId", petId);

        //        using (MySqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                model.Id = (int)reader["id"];
        //                model.Name = reader["name"].ToString();
        //                model.Bio = reader["bio"].ToString();

        //                do
        //                {
        //                    if (reader["imageUrl"] != DBNull.Value)
        //                    {
        //                        model.ImageUrl.Add(reader["imageUrl"].ToString());
        //                    }
        //                    if (reader["apptDate"] != DBNull.Value)
        //                    {
        //                        model.ApptDate.Add(reader.GetDateTime("apptDate"));
        //                    }
        //                    if (reader["apptReason"] != DBNull.Value)
        //                    {
        //                        model.ApptReason.Add(reader["apptReason"].ToString());
        //                    }  
        //                } 
        //                while (reader.Read());
        //            }
        //        }
        //    }
        //}
        //return View(model);
        // }

        //[HttpPost]
        //public IActionResult UpdatePetInfo(FosterUpdateInfoViewModel model, IFormFile newImage)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        //update pet bio in the database
        //        string updateSql = "UPDATE Pets SET bio = @bio WHERE id = @petId";
        //        using (MySqlCommand command = new MySqlCommand(updateSql, connection))
        //        {
        //            command.Parameters.AddWithValue("@bio", model.Bio);
        //            command.Parameters.AddWithValue("@petId", model.Id);
        //            command.ExecuteNonQuery();
        //        }

        //        //image
        //        if (newImage != null && newImage.Length > 0)
        //        {
        //            var filePath = Path.Combine("wwwroot/images", newImage.FileName);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                newImage.CopyTo(stream); //save uploaded file
        //            }

        //            //insert new image into the database
        //            string insertImageSql = "INSERT INTO PetImages (PetId, ImageUrl) VALUES (@petId, @imageUrl)";
        //            using (MySqlCommand command = new MySqlCommand(insertImageSql, connection))
        //            {
        //                command.Parameters.AddWithValue("@petId", model.Id);
        //                command.Parameters.AddWithValue("@imageUrl", $"/images/{newImage.FileName}");
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    //redirect back to foster dashboard
        //    return RedirectToAction("Index", "FosterDashboard");

    }
}