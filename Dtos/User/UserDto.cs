using PracticeProject.Dtos.Comment;
using PracticeProject.Dtos.TodoItem;

namespace PracticeProject.Dtos.User
{
    public class UserDto
    {
        public ICollection<TodoItemDto> TodoItems { get; set; } = new List<TodoItemDto>();
        public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}