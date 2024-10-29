
namespace AdoptionHub.Models
{
    public class PetSearchViewModel
    {
        public string? Species { get; set; } 
        public string? Breed { get; set; } 
        public string? Age { get; set; } 
        public string? Size { get; set; } 
        public string? Gender { get; set; }

        public List<PetDetail> Pets { get; set; }
    }
}
