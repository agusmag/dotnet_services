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
			CreateMap<AuthorDTO, Author>();
		}
	}
}

