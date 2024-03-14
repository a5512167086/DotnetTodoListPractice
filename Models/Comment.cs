using System.ComponentModel.DataAnnotations.Schema;

namespace PracticeProject.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        // TodoItem的Foreign key
        public int? TodoItemId { get; set; }
        // Navigation Property
        // 可以讓我們訪問關聯的模型，例如TodoItem.Id
        public TodoItem? TodoItem { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}