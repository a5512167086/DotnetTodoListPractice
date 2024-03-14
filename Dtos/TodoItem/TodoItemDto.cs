using PracticeProject.Dtos.Comment;

namespace PracticeProject.Dtos.TodoItem
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        public string? UserId { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}