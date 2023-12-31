﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Http;
using Words.Server.Data;
using Words.Shared;

namespace Words.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {

        public readonly DataContext _context;

        private readonly IHttpClientFactory _httpClientFactory;



        public WordController(DataContext context, IHttpClientFactory httpClientFactory)
        { // inject database context

            _context = context;
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<List<UserWord>>> GetAllWords(int userId)
        {

            var userWithWords = await _context.Users.FindAsync(userId);


            if (userWithWords != null)
            {
                var userWords =  _context.UserWords.Where(w => w.UserId == userId).ToList();
                if (userWords != null)
                {
                    return Ok(userWords);
                }
                else
                { return BadRequest("NOTHING"); }


            }

            return BadRequest("User not found");

        }

        [HttpGet("{userId}/{wordId}")]
        [Authorize]
        public async Task<ActionResult<UserWord>> GetWord(int userId, int wordId)
        {

            var userWithWords = await _context.Users.FindAsync(userId);


            if (userWithWords != null)
            {
                var userWord = await _context.UserWords.FindAsync(wordId);

                if (userWord != null)
                {
                    return Ok(userWord);
                }
                else
                { return BadRequest("Word not found haha"); }


            }

            return BadRequest("User not found");

        }

        [HttpGet("getrandomadvice")]
        public async Task<ActionResult<AdviceSlip>> GetRandomAdvice()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var adviceSlip = await httpClient.GetFromJsonAsync<AdviceSlip>("https://api.adviceslip.com/advice");
                return adviceSlip;
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Failed to get random advice");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<string>> AddWord(UserWord word)
        {
            _context.UserWords.Add(word);
            await _context.SaveChangesAsync();

            return Ok(word);
        }


        [HttpPut("{wordId}")]
        [Authorize]
        public async Task<ActionResult<UserWord>> UpdateWord(int wordId, UserWord wordUpdate)
        {
            var result = await _context.UserWords.FindAsync(wordId);

            if (result == null)
            {
                return NotFound("Word not found");
            }

            result.Term = wordUpdate.Term;
            result.Definition = wordUpdate.Definition;

            await _context.SaveChangesAsync();

            return Ok(result);
        }

        
        [HttpDelete("{wordId}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteWord(int wordId)
        {
            var result = await _context.UserWords.FindAsync(wordId);

            if (result == null)
            {
                return NotFound("Word not found");
            }

            _context.UserWords.Remove(result);
            await _context.SaveChangesAsync();

            return Ok(true);
        }


    }
}
