namespace AdoptionHub.Models
{
    public class SignupCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
