using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Veterinarian
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Medicalrecord> Medicalrecords { get; set; } = new List<Medicalrecord>();

    public virtual ICollection<Vetappointment> Vetappointments { get; set; } = new List<Vetappointment>();
}
