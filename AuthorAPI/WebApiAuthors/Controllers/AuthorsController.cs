using Microsoft.AspNetCore.Mvc;
using WebApiAuthors.Models;

namespace WebApiAuthors.Controllers
{
    [ApiController]
	[Route("api/authors")]
	public class AuthorsController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<Author>> Get()
		{
			return new List<Author> {
				new Author() { Id = 1, Name = "Felipe" },
				new Author() { Id = 2, Name = "Carla" }
			};
		}
	}
}

