using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Medicalrecord
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public int? VetId { get; set; }

    public bool? IsVaccinated { get; set; }

    public bool? IsNeutered { get; set; }

    public DateOnly? VisitDate { get; set; }

    public string? VisitDescription { get; set; }

    public string? HealthStatus { get; set; }

    public string? SpecialNeeds { get; set; }

    public virtual Pet? Pet { get; set; }

    public virtual Veterinarian? Vet { get; set; }
}
