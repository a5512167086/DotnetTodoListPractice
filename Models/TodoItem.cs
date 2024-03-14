using System.ComponentModel.DataAnnotations.Schema;

namespace PracticeProject.Models
{
    [Table("TodoItems")]
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        public string? UserId { get; set; }
        // Navigation Property
        public User? User { get; set; } = null!;
        // 一對多關係
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}