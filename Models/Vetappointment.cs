using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Vetappointment
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public int? VetId { get; set; }

    public bool? IsFostered { get; set; }

    public int? FosterId { get; set; }

    public DateTime? ApptDate { get; set; }

    public string? ApptReason { get; set; }

    public virtual User? Foster { get; set; }

    public virtual Pet? Pet { get; set; }

    public virtual Veterinarian? Vet { get; set; }
}
