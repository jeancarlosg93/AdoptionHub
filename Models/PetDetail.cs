using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class PetDetail
{
    public PetDetail() { }
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Species { get; set; }

    public string? Breed { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public float? Weight { get; set; }

    public string? Color { get; set; }

    public string? Temperament { get; set; }

    public DateOnly? DateArrived { get; set; }

    public string? Bio { get; set; }

    public decimal? AdoptionFee { get; set; }

    public virtual required Pet Pet { get; set; }
}
