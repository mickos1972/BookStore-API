using System;
using AutoMapper;
using BookStore_API.Data.DTOs;

namespace BookStore_API.Data.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
