using System;
using System.ComponentModel.DataAnnotations;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
	public class AuthorDTO
	{
        public int Id { get; set; }

		[Required]
        public string Name { get; set; }
	}
}

