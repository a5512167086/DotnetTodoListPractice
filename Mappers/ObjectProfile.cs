using AutoMapper;
using PracticeProject.Dtos.Comment;
using PracticeProject.Dtos.TodoItem;
using PracticeProject.Models;

namespace PracticeProject.Mappers
{
    public class ObjectProfile : Profile
    {
        public ObjectProfile()
        {
            CreateMap<TodoItem, TodoItemDto>();
            CreateMap<CreateTodoItemDto, TodoItem>();
            CreateMap<Comment, CommentDto>();
            CreateMap<CreateCommentDto, Comment>();
        }
    }
}