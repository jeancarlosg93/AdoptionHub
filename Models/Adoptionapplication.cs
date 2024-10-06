using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Adoptionapplication
{
    public int Id { get; set; }

    public int? AdopterId { get; set; }

    public int? PetId { get; set; }

    public string ApplicationStatus { get; set; } = null!;

    public DateOnly? ApplicationDate { get; set; }

    public virtual User? Adopter { get; set; }

    public virtual Pet? Pet { get; set; }
}
