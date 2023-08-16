using System;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.DTOs;
using WebApiAuthors.Models;

namespace WebApiAuthors.Controllers
{
	[ApiController]
	[Route("api/books")]
	public class BooksController : Controller
	{
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
		{
            this.context = context;
            this.mapper = mapper;
        }

		[HttpGet("{id:int}", Name = "getBook")]
		public async Task<ActionResult<BookDTOWithAuthors>> Get(int id)
		{
			var book = await context.Books
				.Include(x => x.AuthorsBooks)
				.ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);
			//.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id); //Not recommended for Lazy Loading

			if (book == null) { return NotFound(); }

			book.AuthorsBooks = book.AuthorsBooks.OrderBy(x => x.Priority).ToList();

            return Ok(mapper.Map<BookDTOWithAuthors>(book));
		}

		[HttpPost]
		public async Task<ActionResult> Post(BookCreateDTO bookDTO)
		{
			if (bookDTO.AuthorIds == null || bookDTO.AuthorIds.Count == 0 )
			{
				return BadRequest("A book requires at least 1 author");
			}

			var authorsIds = await context.Authors
				.Where(x => bookDTO.AuthorIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();

			if (bookDTO.AuthorIds.Count != authorsIds.Count)
			{
				return BadRequest("One or more authors don't exist");
			}

			var book = mapper.Map<Book>(bookDTO);

			SortAuthorsByPriority(book);

            context.Add(book);
			await context.SaveChangesAsync();

			var returnBookDTO = mapper.Map<BookDTO>(book);

            return CreatedAtRoute("getBook", new { id = book.Id }, returnBookDTO);
        }

        [HttpPut("{id:int}")]
		public async Task<ActionResult> Put(int id, BookCreateDTO bookDTO)
		{
			// Get existing book with relations from the DB
			var bookDB = await context.Books
				.Include(x => x.AuthorsBooks)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (bookDB == null) { return NotFound(); }

			// Is mapping the existing entity on the DB with the DTO without modify any other relation associated with this entity
			bookDB = mapper.Map(bookDTO, bookDB);

			SortAuthorsByPriority(bookDB);

            await context.SaveChangesAsync();
			return NoContent();
		}

		private void SortAuthorsByPriority(Book book)
		{
            if (book.AuthorsBooks != null)
            {
                for (int i = 0; i < book.AuthorsBooks.Count; i++)
                {
                    book.AuthorsBooks[i].Priority = i;
                }
            }
        }

		[HttpPatch("{id:int}")]
		public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> patchDocument)
		{
			if (patchDocument == null) { return BadRequest(); }

			var bookDB = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

			if (bookDB == null)
			{
				return NotFound();
			}

			var bookDTO = mapper.Map<BookPatchDTO>(bookDB);

			patchDocument.ApplyTo(bookDTO, ModelState);

			var isValid = TryValidateModel(bookDTO);

			if (!isValid) { return BadRequest(ModelState); }

			mapper.Map(bookDTO, bookDB);

			await context.SaveChangesAsync();

			return NoContent();
		}

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Books.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }

			// This is going to remove in cascade all the other entries in the DB
            context.Remove(new Book() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
