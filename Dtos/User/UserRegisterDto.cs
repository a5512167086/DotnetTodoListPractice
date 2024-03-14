using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Dtos.User
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}