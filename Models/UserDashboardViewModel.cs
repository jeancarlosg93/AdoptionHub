using Mysqlx.Datatypes;

namespace AdoptionHub.Models;

public class UserDashboardViewModel
{
    public int Id { get; set; }
    public String Name { get; set; }
    public String Species { get; set; }
    public String Breed { get; set; }

    public String Gender { get; set; }

    public String Size{ get; set; }

    public String Age { get; set; }
    public String Temperament { get; set; }

    public String ImageUrl { get; set; }
}