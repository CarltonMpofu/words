using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Words.Server.Data;
using Words.Shared;

namespace Words.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Get access to the database via dependency injection
        private readonly DataContext _context;
        public UserController(DataContext context) 
        { 

            _context = context;
        }

        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            //var list = new List<User>()
            //{
            //    new User { Id = 1, Username = "Carlton", Password = "123456"},
            //    new User { Id = 1, Username = "Thembi", Password = "12345678"}
            //};

            var list = await _context.Users.ToListAsync();

            return Ok(list);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> CreateUser(RegisterFormModel request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Set user 
            //Console.WriteLine(passwordHash.ToString(), passwordSalt.ToString());
            User user = new User { };

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        { 
            using( var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
