using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dtos.Comment;
using PracticeProject.Helpers;
using PracticeProject.Interfaces;
using PracticeProject.Models;

namespace PracticeProject.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApiDbContext _context;
        public CommentRepository(ApiDbContext apiDbContext)
        {
            _context = apiDbContext;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject query)
        {
            var comments = _context.Comments.AsQueryable();

            if (query.TodoItemId != null)
            {
                comments = comments.Where(item => item.TodoItemId.Equals(query.TodoItemId));
            }

            if (!string.IsNullOrEmpty(query.SortBy) && query.SortBy.Equals("CreatedAt"))
            {
                comments = query.IsDecesending ? comments.OrderByDescending(item => item.CreatedAt) : comments.OrderBy(item => item.CreatedAt);
            }

            var offset = (query.Offset - 1) * query.Limit;
            var commentsResult = await comments.Skip(offset).Take(query.Limit).ToListAsync();

            return commentsResult;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return null;
            }

            return comment;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto commentDto)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(item => item.Id == id);

            if (comment == null)
            {
                return null;
            }

            comment.Title = commentDto.Title;
            comment.Content = commentDto.Content;
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(item => item.Id == id);

            if (comment == null)
            {
                return null;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<bool?> IsCommentMatchUser(int id, string userId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(item => item.Id == id);
            if (comment == null)
            {
                return null;
            }

            var isCommentMatchUser = comment.UserId!.Equals(userId);
            return isCommentMatchUser;
        }
    }
}