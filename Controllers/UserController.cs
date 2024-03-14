using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Dtos.User;
using PracticeProject.Models;
using PracticeProject.Service;

namespace PracticeProject.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    UserName = userDto.UserName,
                    Email = userDto.UserEmail
                };

                var createUser = await _userManager.CreateAsync(user, userDto.Password);

                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        var newUserDto = new NewUserDto
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user)
                        };
                        return Ok(newUserDto);
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(createUser.Errors);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(item => item.UserName == userDto.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);

            if (!loginResult.Succeeded)
            {
                return Unauthorized("Username not found or password incorrect");
            }

            var loginUserDto = new NewUserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(loginUserDto);
        }
    }
}