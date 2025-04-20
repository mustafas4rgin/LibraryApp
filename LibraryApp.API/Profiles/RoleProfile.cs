using AutoMapper;
using LibraryApp.Domain.DTOs.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;

namespace LibraryApp.API.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDTO>()
    .ForMember(dest => dest.CreatedAt,
        opt => opt.MapFrom(src => src.CreatedAt.ToString("dd.MM.yyyy HH:mm")))
    .ForMember(dest => dest.UpdatedAt,
        opt => opt.MapFrom(src => src.UpdatedAt.ToString("dd.MM.yyyy HH:mm")));

        CreateMap<RoleDTO, Role>()
            .ForMember(dest => dest.CreatedAt,
                opt => opt.Ignore()) 
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.Ignore());

        CreateMap<CreateRoleDTO, Role>().ReverseMap();
        CreateMap<UpdateRoleDTO, Role>().ReverseMap();

    }
}