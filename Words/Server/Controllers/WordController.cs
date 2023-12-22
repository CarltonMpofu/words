using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Words.Server.Data;
using Words.Shared;

namespace Words.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {

        public readonly DataContext _context;
        public WordController(DataContext context)
        { // inject database context

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> GetAllWords()
        {
            var userWithWords = await _context.Users.FindAsync(2);

            ApplicationUser rez;
            if (userWithWords != null)
            {
                rez = userWithWords;
                return Ok(rez);
            }
            //var userWords = _context.UserWords.Where(uw => uw.UserId == userId).ToList();
            //return Ok(userWithWords);

            //var result = await _context.Users.ToListAsync();

            //return Ok(result);

            return NotFound("User not found");

        }

        // Works
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ApplicationUser>> GetAllWords(int id)
        //{

        //    var userWithWords = await _context.Users.FindAsync(id);

        //    ApplicationUser rez;
        //    if (userWithWords != null)
        //    {
        //        rez = userWithWords;
        //        return Ok(rez);

        //    }

        //    return NotFound("User not found");

        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserWord>>> GetAllWords(int id)
        {

            var userWithWords = await _context.Users.FindAsync(id);


            if (userWithWords != null)
            {
                var userWords =  _context.UserWords.Where(w => w.UserId == id).ToList();
                if (userWords != null)
                {
                    return Ok(userWords);
                }
                else
                { return BadRequest("NOTHING"); }


            }

            return BadRequest("User not found");

        }

        [HttpPost]
        public async Task<ActionResult<string>> AddWord(UserWord word)
        {
            _context.UserWords.Add(word);
            await _context.SaveChangesAsync();

            return Ok(word);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserWord>> GetAllWords(int id)
        //{
        //    var result = await _context.Words.FindAsync(id);

        //    if (result == null) 
        //    {
        //        return NotFound("This video game is not found");
        //    }

        //    return Ok(result);
        //}


        //[HttpPost]
        //public async Task<ActionResult<UserWord>> AddWord(UserWord word)
        //{
        //    _context.Words.Add(word);
        //    await _context.SaveChangesAsync();

        //    return Ok(word);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<UserWord>> UpdateWord(int id, UserWord wordUpdate)
        //{
        //    var result = await _context.Words.FindAsync(id);

        //    if (result == null)
        //    {
        //        return NotFound("This video game is not found");
        //    }

        //    result.Term = wordUpdate.Term;
        //    result.Definition = wordUpdate.Definition;

        //    await _context.SaveChangesAsync();

        //    return Ok(result);
        //}
    }
}
