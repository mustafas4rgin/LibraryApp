using AutoMapper;
using LibraryApp.Domain.DTOs.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.Entities;

namespace LibraryApp.API.Profiles;

public class BookRentalProfile : Profile
{
    public BookRentalProfile()
    {
        CreateMap<CreateBookRental, BookRental>().ReverseMap();
        CreateMap<BookRentalWithUserDTO, BookRental>().ReverseMap();
        
        CreateMap<BookRental, BookRentalWithBookDTO>()
            .ForMember(dest => dest.Book, opt => opt.MapFrom(src => new BookDTO
            {
                Id = src.Book.Id,
                Title = src.Book.Title,
                Author = src.Book.Author,
                ISBN = src.Book.ISBN,
                Page = src.Book.Page
            }));

        CreateMap<BookRentalWithBookDTO, BookRental>().ReverseMap();

    }
}
