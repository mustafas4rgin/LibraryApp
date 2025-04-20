using AutoMapper;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.Helpers;
using LibraryApp.Domain.DTOs.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;

namespace LibraryApp.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDTO, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 2)) 
            .AfterMap((src, dest) =>
            {
                HashingHelper.CreatePasswordHash(src.Password, out var hash, out var salt);
                dest.PasswordHash = hash;
                dest.PasswordSalt = salt;
            });

        CreateMap<UpdateUserDTO, User>().ReverseMap();

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt.ToString("dd.MM.yyyy HH:mm")))
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedAt.ToString("dd.MM.yyyy HH:mm")));

        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<BaseUserDTO, User>().ReverseMap();
        CreateMap<User,BaseUserDTO>().ReverseMap();
    }
}
