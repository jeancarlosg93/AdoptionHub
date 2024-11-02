using System;
using System.Collections.Generic;

namespace AdoptionHub.Models;

public partial class Petimage
{
    public int Id { get; set; }

    public int? PetId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Pet? Pet { get; set; }

}