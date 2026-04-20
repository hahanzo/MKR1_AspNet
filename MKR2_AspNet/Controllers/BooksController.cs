using Microsoft.AspNetCore.Mvc;
using MKR1_AspNet;
using MKR2_AspNet.Entities;
using MKR2_AspNet.NewFolder;

namespace MKR2_AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Книгу з ID {id} не знайдено в базі даних.");
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
    }
}
