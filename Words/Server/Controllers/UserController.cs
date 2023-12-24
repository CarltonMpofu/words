using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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

        //public IConfiguration _configuration;

        public UserController(DataContext context)
        { // Inject the database context

            _context = context;
        }

       
        public async Task<ActionResult<List<ApplicationUser>>> GetAllUsers()
        {

            var list = await _context.Users.ToListAsync();

            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(int id)
        {

            var userWithWords = await _context.Users.FindAsync(id);

            ApplicationUser rez;
            if (userWithWords != null)
            {
                rez = userWithWords;
                return Ok(rez);

            }

            return NotFound("User not found");

        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> CreateUser(UserRegisterDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            ApplicationUser user = new ApplicationUser { };
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(UserLoginDto request)
        {
            var users = await _context.Users.ToListAsync();

            if (users is not null)
            {
                var userList = users.Where((c) => c.Username.Equals(request.UserName));
                bool userNameExists = userList.Count() != 0;

                if (userNameExists)
                {
                    ApplicationUser user = userList.First();
                    if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        return BadRequest("Wrong password");
                    }
                    else
                    {

                        string token = CreateToken(user);

                        var response = new LoginResponse
                        {
                            Token = token,
                            UserId = user.Id
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Invalid username");
        }

        /// <summary>
        /// Creates a unique token for identifying and authenticating the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateToken(ApplicationUser user) 
        {
            // Claims describe the user that has been authenticated
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Sub", user.Id.ToString())
            };

            string tokenSecret = "This is a token. What is a token. It is a secret I cannot tell you. Sorry.";

            // create token
            //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            //    _configuration.GetSection("AppSettings:Token").Value));
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenSecret));

            // signing credentials
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // The payload of the token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            // Get the string
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        /// <summary>
        /// Creates a hash and salt for the users password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        { 
            using( var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Checks if the users password is correct
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userPasswordHash"></param>
        /// <param name="userPasswordSalt"></param>
        /// <returns>True if correct</returns>
        private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new HMACSHA512(userPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(userPasswordHash);
            }
        }
    }
}
