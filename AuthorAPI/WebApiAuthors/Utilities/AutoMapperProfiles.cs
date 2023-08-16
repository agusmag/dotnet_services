using System;
using AutoMapper;
using WebApiAuthors.DTOs;
using WebApiAuthors.Models;

namespace WebApiAuthors.Utilities
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<AuthorCreateDTO, Author>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<Author, AuthorDTOWithBooks>()
                .ForMember(authorDTO => authorDTO.Books, opts => opts.MapFrom(MapAuthorDTOBooks));

            CreateMap<BookCreateDTO, Book>()
				.ForMember(book => book.AuthorsBooks, opts => opts.MapFrom(MapAuthorsBooks));
            CreateMap<Book, BookDTO>();
            CreateMap<Book, BookDTOWithAuthors>()
                .ForMember(bookDTO => bookDTO.Authors, opts => opts.MapFrom(MapBookDTOAuthors));
            CreateMap<BookPatchDTO, Book>().ReverseMap(); // Do it in both ways

			CreateMap<CommentCreateDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }

        private List<BookDTO> MapAuthorDTOBooks(Author author, AuthorDTO authorDTO)
        {
            var result = new List<BookDTO>();

            if (author.AuthorsBooks == null) { return result; }

            foreach (var authorBook in author.AuthorsBooks)
            {
                result.Add(new BookDTO()
                {
                    Id = authorBook.AuthorId,
                    Title = authorBook.Book.Title
                });
            }

            return result;
        }

        private List<AuthorBook> MapAuthorsBooks(BookCreateDTO bookDTO, Book book)
        {
            var result = new List<AuthorBook>();

            if (bookDTO.AuthorIds == null) { return result; }

            foreach (var authorId in bookDTO.AuthorIds)
            {
                result.Add(new AuthorBook() { AuthorId = authorId });
            }

            return result;
        }

        private List<AuthorDTO> MapBookDTOAuthors(Book book, BookDTO bookDTO)
        {
            var result = new List<AuthorDTO>();

            if (book.AuthorsBooks == null) { return result; }

            foreach(var authorBook in book.AuthorsBooks)
            {
                result.Add(new AuthorDTO()
                {
                    Id = authorBook.AuthorId,
                    Name = authorBook.Author.Name
                });
            }

            return result;
        }
    }
}
