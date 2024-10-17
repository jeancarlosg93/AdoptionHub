namespace AdoptionHub.Models
{
    public class FosterUpdateInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public List<string> ImageUrl { get; set; }
        public List<DateTime?> ApptDate { get; set; }
        public List<string> ApptReason { get; set; }
    }
}
