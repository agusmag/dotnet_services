using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Models
{
	public class Author
	{
		public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required (custom)")]
        [StringLength(maximumLength: 250, ErrorMessage = "The field {0} must not be greater than 10 chars")]
        [FirstLetterUpper]
        public string Name { get; set; }

        public List<AuthorBook> AuthorsBooks { get; set; }
    }
}
