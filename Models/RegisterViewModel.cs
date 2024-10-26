using System.ComponentModel.DataAnnotations;

namespace AdoptionHub.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Registration code is required")]
    [Display(Name = "Registration Code")]
    public string Code { get; set; }

    public User User { get; set; }
}

public class UserRegistrationInfo
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(25, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 25 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(50)]
    public string Email { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(30)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(30)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [StringLength(50)] public string Address { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
}