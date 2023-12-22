using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        
        public async Task<ActionResult<List<UserWord>>> GetAllWords(int userId) 
        {
            var userWithWords = _context.Users.Include(u => u.UserWords).FirstOrDefault(u => u.Id == userId);

            return Ok(userWithWords.UserWords);
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
