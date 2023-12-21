using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        
    }
}
