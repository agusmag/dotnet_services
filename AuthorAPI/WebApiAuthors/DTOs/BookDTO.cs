using System;
using System.ComponentModel.DataAnnotations;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
	public class BookDTO
	{
		public int Id { get; set; }

		[Required]
        public string Title { get; set; }

		public DateTime? PublishDate { get; set; }

		//public List<CommentDTO> Comments { get; set; } Not recommended for Lazy Loading
	}
}
