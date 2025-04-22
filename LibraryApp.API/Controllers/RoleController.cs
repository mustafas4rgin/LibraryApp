using AutoMapper;
using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Domain.DTOs.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class RoleController : GenericController<Role,CreateRoleDTO,UpdateRoleDTO,RoleDTO>
    {
        public RoleController(IGenericService<Role> service,IMapper mapper, IValidator<CreateRoleDTO> createValidator, IValidator<UpdateRoleDTO> updateValidator)
        : base(service,mapper,createValidator,updateValidator){}
    }
}
