using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.DTOs;
using WebApiAuthors.Models;

namespace WebApiAuthors.Controllers
{
	[ApiController]
	[Route("api/authors")]
    public class AuthorsController : ControllerBase
	{
		private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
		{
			this.context = context;
            this.mapper = mapper;
        }

		[HttpGet]
		public async Task<ActionResult<List<AuthorDTO>>> Get()
		{
			var authors = await context.Authors.ToListAsync();
			return mapper.Map<List<AuthorDTO>>(authors);
		}

        [HttpGet("{id:int}", Name = "getAuthor")] // If the :int restriction is not matched, returns 404
							  // There is no :string restriction
							  // each variable between {var} is another parameter in the method
							  // {var?} set the var parameter as optional (null if it's not provided)
							  // {var=foo} set the default value of var to "foo"
        public async Task<ActionResult<AuthorDTOWithBooks>> Get(int id)
        {
            var author = await context.Authors
				.Include(x => x.AuthorsBooks)
				.ThenInclude(x => x.Book)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (author == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<AuthorDTOWithBooks>(author));
		}

		[HttpGet("{name}")]
        public async Task<ActionResult<AuthorDTO>> Get([FromRoute] string name)
        {
            var authors = await context.Authors.Where(x => x.Name.Contains(name)).ToListAsync();

            return Ok(mapper.Map<List<AuthorDTO>>(authors));
        }

        [HttpPost]
		public async Task<ActionResult> Post([FromBody] AuthorCreateDTO authorDTO)
		{
			var authorExists = await context.Authors.AnyAsync(x => x.Name == authorDTO.Name);

			if (authorExists)
			{
				return BadRequest($"The author: {authorDTO.Name} already exists");
			}

			var author = mapper.Map<Author>(authorDTO);

			context.Add(author);
			await context.SaveChangesAsync();

			var returnAuthorDTO = mapper.Map<AuthorDTO>(author);

			return CreatedAtRoute("getAuthor", new { id = author.Id }, returnAuthorDTO);
		}

		[HttpPut("{id:int}")] //api/authors/{id}
		public async Task<ActionResult> Put(AuthorCreateDTO authorDTO, int id)
		{
            var exists = await context.Authors.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }

			var author = mapper.Map<Author>(authorDTO);
			author.Id = id;

            context.Update(author);
			await context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> Delete(int id)
		{
			var exists = await context.Authors.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Author() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
	}
}

