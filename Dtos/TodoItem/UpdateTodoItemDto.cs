using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Dtos.TodoItem
{
    public class UpdateTodoItemDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
        [MaxLength(100, ErrorMessage = "Title cannot be over 100 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters")]
        [MaxLength(500, ErrorMessage = "Content cannot be over 100 characters")]
        public string Content { get; set; } = string.Empty;
        [Required]
        public bool IsComplete { get; set; } = false;
    }
}