using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using FluentValidation;
using LibraryApp.API.Controllers;
using LibraryApp.Application.Concrete;
using LibraryApp.Domain.DTOs.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : GenericController<User, CreateUserDTO, UpdateUserDTO, UserDTO>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper, IValidator<CreateUserDTO> createValidator, IValidator<UpdateUserDTO> updateValidator)
        : base(service, mapper, createValidator, updateValidator)
        {
            _mapper = mapper;
            _userService = service;
        }
        public async override Task<IActionResult> GetAll(string? include)
        {
            var result = await _userService.GetUsersWithIncludeAsync(include);

            if (!result.Success)
                return BadRequest(result.Message);

            var users = result.Data;

            var dto = _mapper.Map<List<UserDTO>>(users);

            return Ok(dto);
        }
        [HttpGet("GetById/{id}")]
        public async override Task<IActionResult> GetById([FromRoute]int id,[FromQuery]string? include)
        {
            var result = await _userService.GetUserWithIncludeAsync(include,id);

            if (!result.Success)
                return NotFound(result.Message);

            var user = result.Data;

            var dto = _mapper.Map<UserDTO>(user);

            return Ok(dto);
        }
    }
}
