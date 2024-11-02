using System.Runtime.CompilerServices;

namespace AdoptionHub.Models
{
    public class FosterUpdateInfoViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Bio { get; set; }
        
        public String Breed { get; set; }

        public String Gender { get; set; }

        public String Age { get; set; }
        
        public String Temperament { get; set; }
        public List<string> ImageUrl { get; set; }
        public List<DateTime?> ApptDate { get; set; }
        public List<string> ApptReason { get; set; }

        public void AddAppointment(DateTime? date, string reason)
        {
            ApptDate.Add(date);
            ApptReason.Add(reason);
        }

        public List<string> GetAppointments()
        {
            List<string> result = new List<string>();

            for (int i = 0; i < ApptDate.Count; i++)
            {
                if (ApptDate[i].HasValue && !string.IsNullOrEmpty(ApptReason[i]))
                {
                    result.Add($"{ApptReason[i]}, {ApptDate[i]:yyyy MMMM dd hh:mm tt}");
                }
            }

            return result;
        }
    }
}