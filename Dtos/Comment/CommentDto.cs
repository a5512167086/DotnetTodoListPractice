namespace PracticeProject.Dtos.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? TodoItemId { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}