using Microsoft.AspNetCore.Identity;

namespace AlicisinaWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Phone { get; set; }
    }
}