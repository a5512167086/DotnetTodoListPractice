using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dtos.TodoItem;
using PracticeProject.Helpers;
using PracticeProject.Interfaces;
using PracticeProject.Models;

namespace PracticeProject.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApiDbContext _context;

        public TodoItemRepository(ApiDbContext apiDbContext)
        {
            _context = apiDbContext;
        }

        public async Task<List<TodoItem>> GetAllAsync(TodoItemQueryObject query)
        {
            var todoItems = _context.TodoItems.Include(item => item.Comments).AsQueryable();
            // LINQ
            if (!string.IsNullOrEmpty(query.Title))
            {
                todoItems = todoItems.Where(item => item.Title.Contains(query.Title));
            }
            if (!string.IsNullOrEmpty(query.Content))
            {
                todoItems = todoItems.Where(item => item.Content.Contains(query.Content));
            }

            if (!string.IsNullOrEmpty(query.SortBy) && query.SortBy.Equals("Title"))
            {
                todoItems = query.IsDecesending ? todoItems.OrderByDescending(item => item.Title) : todoItems.OrderBy(item => item.Title);
            }
            if (!string.IsNullOrEmpty(query.SortBy) && query.SortBy.Equals("Content"))
            {
                todoItems = query.IsDecesending ? todoItems.OrderByDescending(item => item.Content) : todoItems.OrderBy(item => item.Content);
            }

            var offset = (query.Offset - 1) * query.Limit;
            var todoItemsResult = await todoItems.Skip(offset).Take(query.Limit).ToListAsync();

            return todoItemsResult;
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            var todoItem = await _context.TodoItems.Include(item => item.Comments).FirstOrDefaultAsync(item => item.Id == id);

            return todoItem;
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem?> UpdateAsync(int id, UpdateTodoItemDto todoItemDto)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(item => item.Id == id);

            if (todoItem == null)
            {
                return null;
            }

            todoItem.Title = todoItemDto.Title;
            todoItem.Content = todoItemDto.Content;
            todoItem.IsComplete = todoItemDto.IsComplete;

            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<TodoItem?> DeleteAsync(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(item => item.Id == id);

            if (todoItem == null)
            {
                return null;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<bool> IsTodoItemExists(int id)
        {
            var isExists = await _context.TodoItems.AnyAsync(item => item.Id == id);
            return isExists;
        }

        public async Task<bool?> IsTodoItemMatchUser(int id, string userId)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(item => item.Id == id);
            if (todoItem == null)
            {
                return null;
            }

            var isTodoItemMatchUser = todoItem.UserId!.Equals(userId);
            return isTodoItemMatchUser;
        }
    }
}