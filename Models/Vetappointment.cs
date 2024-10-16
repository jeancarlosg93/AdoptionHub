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

    public DateOnly? ApptDate { get; set; }

    public string? ApptReason { get; set; }
}
