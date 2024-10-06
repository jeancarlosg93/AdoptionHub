using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? FosterUserId { get; set; }

    public int? PetId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public virtual User? CreatedByUser { get; set; }

    public virtual User? FosterUser { get; set; }

    public virtual Pet? Pet { get; set; }
}
