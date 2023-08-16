using System;
using System.ComponentModel.DataAnnotations;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Models
{
	public class Book
	{
		public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required (custom)")]
        [StringLength(maximumLength: 120, ErrorMessage = "The field {0} must not be greater than 120 chars")]
        [FirstLetterUpper]
        public string Title { get; set; }

        public DateTime? PublishDate { get; set; }

        public List<AuthorBook> AuthorsBooks { get; set; }

        public List<Comment> Comments { get; set; }
	}
}
