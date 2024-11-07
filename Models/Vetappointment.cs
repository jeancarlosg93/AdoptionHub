using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdoptionHub.Models;

public partial class Vetappointment
{
    public int Id { get; set; }

    [Required]
    public int PetId { get; set; }

    [Required]
    public int VetId { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    [Display(Name = "Appointment Date and Time")]
    // Validate that appointment date is not in the past
    [FutureDate(ErrorMessage = "Appointment date must be in the future")]
    public DateTime ApptDate { get; set; }

    [MaxLength(200)]
    [Display(Name = "Appointment Reason")]
    public string? ApptReason { get; set; }

    [ForeignKey("PetId")]
    public virtual Pet Pet { get; set; } = null!;

    [ForeignKey("VetId")]
    public virtual Veterinarian Vet { get; set; } = null!;
}

// Custom validation attribute to ensure appointment dates are in the future
public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }
        return false;
    }
}