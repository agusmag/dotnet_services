using System;
namespace WebApiAuthors.Models
{
	public class AuthorBook
	{
		public int AuthorId { get; set; }

		public int BookId { get; set; }

		public int  Priority { get; set; }

		public Author Author { get; set; }

		public Book Book { get; set; }
	}
}

