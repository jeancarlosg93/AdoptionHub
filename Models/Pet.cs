using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Pet
{
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

    public string Status { get; set; } = null!;

    public decimal? AdoptionFee { get; set; }

    public int? FosterParentId { get; set; }

    public virtual ICollection<Adoptionapplication> Adoptionapplications { get; set; } = new List<Adoptionapplication>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User? FosterParent { get; set; }

    public virtual ICollection<Fosterassignment> Fosterassignments { get; set; } = new List<Fosterassignment>();

    public virtual ICollection<Medicalrecord> Medicalrecords { get; set; } = new List<Medicalrecord>();

    public virtual ICollection<Petimage> Petimages { get; set; } = new List<Petimage>();

    public virtual ICollection<Vetappointment> Vetappointments { get; set; } = new List<Vetappointment>();
}
