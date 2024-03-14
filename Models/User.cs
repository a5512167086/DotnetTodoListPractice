using Microsoft.AspNetCore.Identity;

namespace PracticeProject.Models
{
    public class User : IdentityUser
    {
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}