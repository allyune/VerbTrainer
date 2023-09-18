using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Models;
using VerbTrainer.DTOs;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;
using VerbTrainer.Application.CreateDeck;
using VerbTrainer.Application.LoadDeck;
using VerbTrainer.Infrastructure.Data.Models;
using VerbTrainer.Application.DeleteDeck;
using VerbTrainer.Application.AddVerbToDeck;

namespace VerbTrainer.Controllers
{
    [Route("api/[controller]")]
    public class DeckController : Controller
    {
        private readonly ILogger<DeckController> _logger;
        private readonly ICreateDeckHandler _createDeckHandler;
        private readonly ILoadDeckHandler _loadDeckHandler;
        private readonly IDeleteDeckHandler _deleteDeckHandler;
        private readonly IAddVerbToDeckHandler _addVerbToDeckHandler;

        public DeckController(
            ILogger<DeckController> logger,
            ICreateDeckHandler createDeckHandler,
            ILoadDeckHandler loadDeckHandler,
            IDeleteDeckHandler deleteDeckHandler,
            IAddVerbToDeckHandler addVerbToDeckHandler)
        {
            _logger = logger;
            _createDeckHandler = createDeckHandler;
            _loadDeckHandler = loadDeckHandler;
            _deleteDeckHandler = deleteDeckHandler;
            _addVerbToDeckHandler = addVerbToDeckHandler;
        }

        [HttpGet("{deckid}")]
        public async Task<IActionResult> GetDeckById(int deckId)
        {
            Deck deck;
            try
            {
                deck = await _loadDeckHandler.LoadDeckById(deckId);
            }

            catch (DeckNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Deck with ID {deckId} not found.");
            }

            // TODO: Use mapper for conversions
            ICollection<DeckVerb>? deckVerbs = deck.DeckVerbs;
            List<Verb>? verbs = deckVerbs?.Select(v => v.Verb).ToList();
            List<VerbDto>? verbDtos = verbs?.Select(
                v => VerbDto.CreateNew(
                    v.Id,
                    v.Name,
                    v.Root,
                    v.Meaning,
                    v.Binyan,
                    v.Conjugations.ToList())).ToList();

            DeckDto deckDto = DeckDto.CreateNew(deck.Name, verbDtos);
            return Json(deckDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddDeck([FromBody] AddDeckDto data)
        {
            // TODO: validate dto with fluent validator
            int newDeckId = await _createDeckHandler.CreateDeckForUser(
                data.UserId, data.DeckName);
            return Ok(newDeckId);
        }

        [HttpDelete("{deckid}")]
        public async Task<IActionResult> DeleteDeck(int deckId)
        {
            try
            {
                await _deleteDeckHandler.DeleteDeckById(deckId);
            }
            catch (DeckNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Deck with ID {deckId} not found.");
            }
            catch (CouldNotDeleteDeckException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(
                    500,
                    $"Error occured while deleting Deck {deckId}. Try again");
            }

            return Ok();

        }


        [HttpPost("verb")]
        public async Task<IActionResult> AddVerbToDeck([FromBody] AddVerbDto data)
        {
            try
            {
                await _addVerbToDeckHandler.AddVerbToDeck(data.VerbId, data.DeckId);
            }
            catch (RecordDoesNotExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (DeckVerbAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(AddVerbToDeckException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
            return Ok(200);
        }

    }
}