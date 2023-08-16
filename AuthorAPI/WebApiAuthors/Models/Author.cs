using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Models
{
	public class Author
	{
		public int Id { get; set; }

		public string Name { get; set; }
    }
}
