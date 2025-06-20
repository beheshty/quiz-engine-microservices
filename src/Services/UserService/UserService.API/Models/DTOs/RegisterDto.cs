namespace UserService.API.Models.DTOs
{
    public class RegisterDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}