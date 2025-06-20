using Microsoft.AspNetCore.Identity;

namespace UserService.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }
        [PersonalData]
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
} 