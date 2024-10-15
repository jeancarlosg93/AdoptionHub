using AdoptionHub.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace AdoptionHub.Controllers
{
    public class FosterUpdateInfoController : Controller
    {
        public readonly ILogger<LoginController> _logger;
        public readonly string connectionString;
        public FosterUpdateInfoController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index(int petId)
        {

            FosterUpdateInfoViewModel model = new FosterUpdateInfoViewModel();
            model.ImageUrl = new List<string>();
            model.ApptDate = new List<DateTime>();
            model.ApptReason = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT p.*, va.apptDate, va.apptReason, pi.imageUrl FROM Pets p LEFT JOIN vetAppointments va ON p.id = va.petId LEFT JOIN PetImages pi ON p.id = pi.petId WHERE p.Id = @petId;";
                
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@petId", petId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.Id = (int)reader["id"];
                            model.Name = reader["name"].ToString();
                            model.Bio = reader["bio"].ToString();

                            do
                            {
                                if (reader["imageUrl"] != DBNull.Value)
                                {
                                    model.ImageUrl.Add(reader["imageUrl"].ToString());
                                }
                                if (reader["apptDate"] != DBNull.Value)
                                {
                                    model.ApptDate.Add(reader.GetDateTime("apptDate"));
                                }
                                if (reader["apptReason"] != DBNull.Value)
                                {
                                    model.ApptReason.Add(reader["apptReason"].ToString());
                                }  
                            } 
                            while (reader.Read());
                        }
                    }
                }
            }
            return View(model);
        }


        [HttpPost]
        public IActionResult UpdatePetInfo(FosterUpdateInfoViewModel model, IFormFile newImage)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Update pet bio in the database
                string updateSql = "UPDATE Pets SET bio = @bio WHERE id = @petId";
                using (MySqlCommand command = new MySqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@bio", model.Bio);
                    command.Parameters.AddWithValue("@petId", model.Id);
                    command.ExecuteNonQuery();
                }

                // Handle image upload
                if (newImage != null && newImage.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images", newImage.FileName); // Define the path for the image
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        newImage.CopyTo(stream); // Save the uploaded file
                    }

                    // Insert the new image into the database
                    string insertImageSql = "INSERT INTO PetImages (PetId, ImageUrl) VALUES (@petId, @imageUrl)";
                    using (MySqlCommand command = new MySqlCommand(insertImageSql, connection))
                    {
                        command.Parameters.AddWithValue("@petId", model.Id);
                        command.Parameters.AddWithValue("@imageUrl", $"/images/{newImage.FileName}"); // Store the path in the database
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Redirect back to the same page
            return RedirectToAction("Index", new { petId = model.Id });
        }
    }

}