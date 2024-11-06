namespace AdoptionHub.Models;

public partial class Adoptionapplication
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public string ApplicationStatus { get; set; } = null!;

    public DateTime? ApplicationDateTime { get; set; }

    public virtual Pet? Pet { get; set; }

    public string? Email { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? Country { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Comments { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();

}
