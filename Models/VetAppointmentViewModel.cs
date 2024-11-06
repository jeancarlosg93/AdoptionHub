using System.ComponentModel.DataAnnotations;

namespace AdoptionHub.Models
{
    public class VetAppointmentViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Pet selection is required")]
        public int? PetId { get; set; }
        
        [Required(ErrorMessage = "Veterinarian selection is required")]
        public int? VetId { get; set; }
        
        [Required(ErrorMessage = "Appointment date is required")]
        public DateTime? ApptDate { get; set; }
        
        [Required(ErrorMessage = "Appointment reason is required")]
        [StringLength(200)]
        public string? ApptReason { get; set; }
        
        public List<Pet>? AvailablePets { get; set; }
        public List<Veterinarian>? AvailableVets { get; set; }
        
        // Navigation properties for display
        public Pet? Pet { get; set; }
        public Veterinarian? Vet { get; set; }
    }
}