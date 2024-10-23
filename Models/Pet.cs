using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Pet
{
    public Pet() { }
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public int? FosterParentId { get; set; }


    public virtual User? FosterParent { get; set; }

    public virtual ICollection<Adoptionapplication> Adoptionapplications { get; set; } = new List<Adoptionapplication>();

    public virtual ICollection<Fosterassignment> Fosterassignments { get; set; } = new List<Fosterassignment>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Medicalrecord> Medicalrecords { get; set; } = new List<Medicalrecord>();

    public virtual ICollection<Petimage> Petimages { get; set; } = new List<Petimage>();

    public virtual ICollection<Vetappointment> Vetappointments { get; set; } = new List<Vetappointment>();


    public virtual PetDetail? Details { get; set; }
}
