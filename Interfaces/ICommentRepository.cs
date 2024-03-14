using PracticeProject.Dtos.Comment;
using PracticeProject.Helpers;
using PracticeProject.Models;

namespace PracticeProject.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(CommentQueryObject query);
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(int id, UpdateCommentDto commentDto);
        Task<Comment?> DeleteAsync(int id);
        Task<bool?> IsCommentMatchUser(int id, string userId);
    }
}