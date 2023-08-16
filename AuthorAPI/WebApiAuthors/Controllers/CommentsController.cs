using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.DTOs;
using WebApiAuthors.Models;

namespace WebApiAuthors.Controllers
{
	[ApiController]
	[Route("api/books/{bookId:int}/comments")]
	public class CommentsController : ControllerBase
	{
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
		{
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
        {
            var bookExists = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!bookExists)
            {
                return NotFound();
            }

            var comments = await context.Comments.Where(x => x.BookId == bookId).ToListAsync();

            return Ok(mapper.Map<List<CommentDTO>>(comments));
        }

        [HttpGet("{id:int}", Name = "getComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return mapper.Map<CommentDTO>(comment);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int bookId, CommentCreateDTO commentDTO)
        {
            var bookExists = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!bookExists)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentDTO);
            comment.BookId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();

            var returnCommentDTO = mapper.Map<CommentDTO>(comment);

            return CreatedAtRoute("getComment", new { id = comment.Id, bookId = bookId }, returnCommentDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int bookId, int id, CommentCreateDTO commentDTO)
        {
            var bookExists = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!bookExists)
            {
                return NotFound();
            }

            var commentExists = await context.Comments.AnyAsync(x => x.Id == id);

            if (!commentExists)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentDTO);
            comment.Id = id;
            comment.BookId = bookId;

            context.Update(comment);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
