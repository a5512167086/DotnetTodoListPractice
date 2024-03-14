using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeProject.Dtos.User
{
    public class TokenDto
    {
        public string UserId { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}