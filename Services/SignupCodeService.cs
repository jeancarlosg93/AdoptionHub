using AdoptionHub.Contexts;
using AdoptionHub.Models;

namespace AdoptionHub.Services
{
    public class SignupCodeService
    {
        private readonly ApplicationDbContext _context;

        public SignupCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GenerateSignupCode()
        {

            string code = Guid.NewGuid().ToString();


            var signupCode = new SignupCode
            {
                Code = code,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(7)
            };


            _context.SignupCodes.Add(signupCode);
            _context.SaveChanges();

            return code;
        }
    }

}
