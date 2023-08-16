using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Models;

namespace WebApiAuthors.Controllers
{
	[ApiController]
	[Route("api/books")]
	public class BooksController : Controller
	{
        private readonly ApplicationDbContext context;

        public BooksController(ApplicationDbContext context)
		{
            this.context = context;
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Book>> Get(int id)
		{
			return await context.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
		}

		[HttpPost]
		public async Task<ActionResult> Post(Book book)
		{
			var authorExists = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

			if (!authorExists)
			{
				return BadRequest($"The AuthorId: {book.AuthorId} of the book doesn't exist");
			}

			context.Add(book);
			await context.SaveChangesAsync();
			return Ok();


		}
	}
}

