using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Fosterassignment
{
    public int Id { get; set; }

    public int? FosterId { get; set; }

    public int? PetId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual User? Foster { get; set; }

    public virtual Pet? Pet { get; set; }
}
