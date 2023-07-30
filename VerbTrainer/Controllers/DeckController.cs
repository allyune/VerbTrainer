using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Data;
using VerbTrainer.Models;
using VerbTrainer.DTOs;
using VerbTrainer.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace VerbTrainer.Controllers
{
    [Route("api/[controller]")]
    public class DeckController : Controller
    {
        private readonly ILogger<DeckController> _logger;
        private readonly VerbTrainerDbContext _dbContext;

        public DeckController(ILogger<DeckController> logger, VerbTrainerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }

        [HttpGet("{deckid}")]
        public IActionResult GetDeckById(int deckid)
        {
            Deck? deck = _dbContext.Decks.FirstOrDefault(d => d.Id == deckid);
            if (deck == null)
            {
                return NotFound();
            }
            DeckDto deckDto = new DeckDto
            {
                Name = deck.Name,
                Verbs = _dbContext.Verbs
                        .Where(v => _dbContext.DeckVerbs.Any(dw => dw.DeckId == deck.Id && dw.VerbId == v.Id))
                        .Include(verb => verb.Conjugations)
                            .ThenInclude(conjugation => conjugation.Tense)
                        .Select(verb => new VerbDto
                        {
                            Id = verb.Id,
                            Name = verb.Name,
                            Meaning = verb.Meaning,
                            Binyan = verb.Binyan,
                            Root = verb.Root,
                            Conjugations = verb.Conjugations.ToList()
                        })
                        .ToList()
            };

                
            return Json(deckDto);
        }

        [HttpPost]
        public IActionResult AddDeck([FromBody] AddDeckDto data)
        {
            _dbContext.Decks.Add(new Deck { UserId = data.UserId, Name = data.DeckName });
            _dbContext.SaveChanges();

            return Ok(200);
        }

        [HttpDelete("{deckid}")]
        public IActionResult DeleteDeck(int deckid)
        {
            Deck deckToDelete = _dbContext.Decks.First(d => d.Id == deckid);
            if (deckToDelete != null)
            {
                _dbContext.Decks.Remove(deckToDelete);
                _dbContext.SaveChanges();
                return Ok(200);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public IActionResult GetUserDecks(int id)
        {
            User? user = _dbContext.Users.Include(u => u.Decks).FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userDTOs = user.Decks.Select(deck => new DeckDto
            {
                Name = deck.Name,
                Verbs = _dbContext.Verbs
                        .Where(v => _dbContext.DeckVerbs.Any(dw => dw.DeckId == deck.Id && dw.VerbId == v.Id))
                        .Include(verb => verb.Conjugations)
                            .ThenInclude(conjugation => conjugation.Tense)
                        .Select(verb => new VerbDto
                        {
                            Id = verb.Id,
                            Name = verb.Name,
                            Meaning = verb.Meaning,
                            Binyan = verb.Binyan,
                            Root = verb.Root,
                            Conjugations = verb.Conjugations.ToList()
                        })
                        .ToList()
            });
            return Json(userDTOs);
        }


        [HttpPost("verb")]
        public IActionResult AddVerbToDeck([FromBody] AddVerbDto data)
        {
            Deck? deck = _dbContext.Decks.FirstOrDefault(d => d.Id == data.DeckId);
            Verb? verb = _dbContext.Verbs.FirstOrDefault(v => v.Id == data.VerbId);

            if (deck == null || verb == null)
            {
                return NotFound("Deck or Verb not found");
            }

            if (!_dbContext.DeckVerbs.Any(dv => dv.DeckId == deck.Id && dv.VerbId == verb.Id))
            {
                DeckVerb deckVerb = new DeckVerb
                {
                    DeckId = deck.Id,
                    VerbId = verb.Id
                };

                _dbContext.DeckVerbs.Add(deckVerb);
                _dbContext.SaveChanges();
            }
            return Ok(200);
        }

    }
}