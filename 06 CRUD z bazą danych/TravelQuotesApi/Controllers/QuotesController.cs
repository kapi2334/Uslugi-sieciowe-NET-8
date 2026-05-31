using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelQuotesApi.Interfaces; // Dodano przestrzeń nazw dla IRepository
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IRepository<Quote> _quoteRepository;

        public QuotesController(IRepository<Quote> quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        // GET: api/Quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            var quotes = await _quoteRepository.GetAllAsync();
            return Ok(quotes);
        }

        // GET: api/Quotes/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _quoteRepository.GetByIdAsync(id);

            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote);
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            var createdQuote = await _quoteRepository.CreateAsync(quote);

            return CreatedAtAction(nameof(GetQuote), new { id = createdQuote.Id }, createdQuote);
        }

    }
}