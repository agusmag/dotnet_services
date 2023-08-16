using System;
using System.ComponentModel.DataAnnotations;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
	public class BookCreateDTO
	{
        [StringLength(maximumLength: 120, ErrorMessage = "The field {0} must not be greater than 120 chars")]
        [FirstLetterUpper]
        public string Title { get; set; }

        public List<int> AuthorIds { get; set; }
	}
}
