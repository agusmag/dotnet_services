using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Filters;
using WebApiAuthors.Models;
using WebApiAuthors.Services;

namespace WebApiAuthors.Controllers
{
	[ApiController]
	[Route("api/authors")]
    // [Route("api/[controller]")] Placeholder to use the controller name on the endpoint
    public class AuthorsController : ControllerBase
	{
		private readonly ApplicationDbContext context;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
		private readonly ServiceScoped serviceScoped;
		private readonly ServiceSingleton serviceSingleton;

        public ILogger<AuthorsController> Logger { get; }

        public AuthorsController(ApplicationDbContext context, IService service, ServiceTransient serviceTransient,
			ServiceScoped serviceScoped, ServiceSingleton serviceSingleton, ILogger<AuthorsController> logger)
		{
			this.context = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
			this.serviceScoped = serviceScoped;
			this.serviceSingleton = serviceSingleton;
            Logger = logger;
        }

		[HttpGet]
		[Authorize] // Can be at Controller Level
		// [HttpGet("/authorslist")] Ignore the base route api/authors and respond against /authorslist
		public async Task<ActionResult<List<Author>>> Get()
		{
			Logger.LogInformation("TRACE - Getting the authors");
			Logger.LogWarning("WARN - Getting the authors");

			return await context.Authors.Include(x => x.Books).ToListAsync();
		}

        [HttpGet("first")] // api/authors/first?name=Agustin&surname=Magliano
        public async Task<ActionResult<Author>> GetFirst([FromHeader] int myValue, [FromQuery] string name)
        {
			return await context.Authors.Include(x => x.Books).FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")] // If the :int restriction is not matched, returns 404
							  // There is no :string restriction
							  // each variable between {var} is another parameter in the method
							  // {var?} set the var parameter as optional (null if it's not provided)
							  // {var=foo} set the default value of var to "foo"
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await context.Authors.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);

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
		public async Task<ActionResult> Post([FromBody] Author author)
		{
			var authorExists = await context.Authors.AnyAsync(x => x.Name == author.Name);

			if (authorExists)
			{
				return BadRequest($"The author: {author.Name} already exists");
			}

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

		[HttpGet("guid")]
		[ResponseCache(Duration = 10)] // Response Caching Middleware in seconds
		[ServiceFilter(typeof(MyActionFilter))] // This is going to call the MyActionFilter methods before and after the Action (request)
		public ActionResult GetGuids()
		{
			return Ok(new {
				AuthorsController_Transient = serviceTransient.Guid,
				ServiceA_Transient = service.GetTransient(),
				AuthorsController_Scoped = serviceScoped.Guid,
				ServiceA_Scoped = service.GetScoped(),
				AuthorsController_Singleton = serviceSingleton.Guid,
				ServiceA_Singleton = service.GetSingleton()
            });
		}
	}
}

