using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Dtos.User
{
    public class UserLoginDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}