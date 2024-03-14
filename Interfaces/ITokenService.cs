using PracticeProject.Dtos.User;
using PracticeProject.Models;

namespace PracticeProject.Service
{
    public interface ITokenService
    {
        string CreateToken(User user);
        TokenDto EncodeToken(string token);
    }
}