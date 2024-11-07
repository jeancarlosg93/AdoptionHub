using System.ComponentModel.DataAnnotations;

namespace AdoptionHub.Models;

public class AdoptionFormViewModel
{
    public int PetId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(30)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(30)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(50)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(50)] public string Address { get; set; }

    [Required(ErrorMessage = "City is required")]
    [StringLength(50)] public string City { get; set; }

    [Required(ErrorMessage = "Province is required")]
    [StringLength(50)] public string Province { get; set; }

    [Required(ErrorMessage = "Country is required")]
    [StringLength(50)] public string Country { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Comments")]
    [StringLength(100)] 
    public string? Comments { get; set; }

}