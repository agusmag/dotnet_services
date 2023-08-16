using System;
using System.ComponentModel.DataAnnotations;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
	public class AuthorCreateDTO
	{
        [Required(ErrorMessage = "The field {0} is required (custom)")]
        [StringLength(maximumLength: 250, ErrorMessage = "The field {0} must not be greater than 250 chars")]
        [FirstLetterUpper]
        public string Name { get; set; }
    }
}

