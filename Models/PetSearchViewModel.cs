
namespace AdoptionHub.Models
{
    public class PetSearchViewModel
    {
        public string Name { get; set; }
        public string? Species { get; set; } 
        public string? Breed { get; set; } 
        public string? Age { get; set; } 
        public string? Size { get; set; } 
        public string? Gender { get; set; }
        public string? Temperament { get; set; }
        public string ImageUrl { get; set; }

        public List<PetDetail> Pets { get; set; }
    }
}
