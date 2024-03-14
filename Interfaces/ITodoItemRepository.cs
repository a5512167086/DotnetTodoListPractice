using PracticeProject.Dtos.TodoItem;
using PracticeProject.Helpers;
using PracticeProject.Models;

namespace PracticeProject.Interfaces
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetAllAsync(TodoItemQueryObject query);
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task<TodoItem?> UpdateAsync(int id, UpdateTodoItemDto todoItemDto);
        Task<TodoItem?> DeleteAsync(int id);
        Task<Boolean> IsTodoItemExists(int id);
        Task<Boolean?> IsTodoItemMatchUser(int id, string userId);
    }
}