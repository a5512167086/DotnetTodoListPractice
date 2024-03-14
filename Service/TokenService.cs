
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;
using PracticeProject.Dtos.User;
using PracticeProject.Models;

namespace PracticeProject.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]!));
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id),
                new Claim("userEmail", user.Email!),
                new Claim("userName", user.UserName!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public TokenDto EncodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodeToken = tokenHandler.ReadJwtToken(token);
            var tokenDto = new TokenDto
            {
                UserId = encodeToken.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value ?? "",
                UserEmail = encodeToken.Claims.FirstOrDefault(claim => claim.Type == "userEmail")?.Value ?? "",
                UserName = encodeToken.Claims.FirstOrDefault(claim => claim.Type == "userName")?.Value ?? "",
            };

            return tokenDto;
        }
    }
}