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
		public async Task<ActionResult<List<Author>>> Get()
		{
			return await context.Authors.ToListAsync();
		}

        [HttpGet("{id:int}")] // If the :int restriction is not matched, returns 404
							  // There is no :string restriction
							  // each variable between {var} is another parameter in the method
							  // {var?} set the var parameter as optional (null if it's not provided)
							  // {var=foo} set the default value of var to "foo"
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);

			if (author == null)
			{
				return NotFound();
			}

			return Ok(author);
		}

		[HttpGet("{name}")]
        public async Task<ActionResult<Author>> Get([FromRoute] string name)
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Name == name);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
		public async Task<ActionResult> Post([FromBody] AuthorDTO authorDTO)
		{
			var authorExists = await context.Authors.AnyAsync(x => x.Name == authorDTO.Name);

			if (authorExists)
			{
				return BadRequest($"The author: {authorDTO.Name} already exists");
			}

			var author = mapper.Map<Author>(authorDTO);

			context.Add(author);
			await context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut("{id:int}")] //api/authors/{id}
		public async Task<ActionResult> Put(Author author, int id)
		{
			if (author.Id != id)
			{
				return BadRequest($"The author with id: {id} doesn't match");
			}

            var exists = await context.Authors.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Update(author);
			await context.SaveChangesAsync();
			return Ok();
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
            return Ok();
        }
	}
}

