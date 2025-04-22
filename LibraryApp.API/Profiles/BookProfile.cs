using AutoMapper;
using LibraryApp.Application.Helpers;
using LibraryApp.Domain.DTos.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;

namespace LibraryApp.API.Profiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<CreateBookDTO, Book>()
            .AfterMap((src, dest) =>
        {
        if (string.IsNullOrWhiteSpace(dest.ISBN))
        {
            dest.ISBN = IsbnHelper.GenerateIsbn13();
        }
        });
        
        CreateMap<UpdateBookDTO, Book>().ReverseMap();

        CreateMap<Book, BookDTO>()
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt.ToString("dd.MM.yyyy HH:mm")))
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedAt.ToString("dd.MM.yyyy HH:mm")));

        CreateMap<BookDTO, Book>()
            .ForMember(dest => dest.CreatedAt,
                opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                if (string.IsNullOrWhiteSpace(src.ISBN))
                {
                    dest.ISBN = IsbnHelper.GenerateIsbn13();
                }
                else
                {
                    dest.ISBN = src.ISBN;
                }
            });
    }
}
