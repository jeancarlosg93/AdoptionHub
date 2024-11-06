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
        public List<string> VetNames { get; set; }
        public List<string> VetPhones { get; set; }
        public List<string> VetEmails { get; set; }

        public FosterUpdateInfoViewModel()
        {
            ImageUrl = new List<string>();
            ApptDate = new List<DateTime?>();
            ApptReason = new List<string>();
            VetNames = new List<string>();
            VetPhones = new List<string>();
            VetEmails = new List<string>();
        }

        public void AddAppointment(DateTime? date, string reason, string vetName, string vetPhone, string vetEmail)
        {
            ApptDate.Add(date);
            ApptReason.Add(reason);
            VetNames.Add(vetName);
            VetPhones.Add(vetPhone);
            VetEmails.Add(vetEmail);
        }

        public List<AppointmentInfo> GetAppointments()
        {
            var appointments = new List<AppointmentInfo>();
            for (int i = 0; i < ApptDate.Count; i++)
            {
                if (ApptDate[i].HasValue && !string.IsNullOrEmpty(ApptReason[i]))
                {
                    appointments.Add(new AppointmentInfo
                    {
                        Date = ApptDate[i].Value,
                        Reason = ApptReason[i],
                        VetName = i < VetNames.Count ? VetNames[i] : "N/A",
                        VetPhone = i < VetPhones.Count ? VetPhones[i] : "N/A",
                        VetEmail = i < VetEmails.Count ? VetEmails[i] : "N/A"
                    });
                }
            }

            return appointments.OrderByDescending(a => a.Date).ToList();
        }
    }

    public class AppointmentInfo
    {
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string VetName { get; set; }
        public string VetPhone { get; set; }
        public string VetEmail { get; set; }

        public bool IsUpcoming => Date > DateTime.Now;
    }
}