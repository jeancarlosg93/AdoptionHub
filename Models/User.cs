using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string UserRole { get; set; } = null!;

    public virtual ICollection<Adoptionapplication> Adoptionapplications { get; set; } = new List<Adoptionapplication>();

    public virtual ICollection<Appointment> AppointmentCreatedByUsers { get; set; } = new List<Appointment>();

    public virtual ICollection<Appointment> AppointmentFosterUsers { get; set; } = new List<Appointment>();

    public virtual ICollection<Fosterassignment> Fosterassignments { get; set; } = new List<Fosterassignment>();
}
