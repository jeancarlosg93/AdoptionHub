using System.Globalization;

namespace AdoptionHub.Models
{
    public class FosterDashboardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public List<string> Images { get; set; }

        public bool IsCurrentFoster { get; set; }
        
        public int HasVetAppointments { get; set; }
    }
}