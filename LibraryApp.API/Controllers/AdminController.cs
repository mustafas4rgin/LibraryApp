using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.DTOValidators.Update;
using LibraryApp.Domain.DTOs.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IValidator<UpdateUsersRoleDTO> _updateRoleValidator;
        private readonly IAdminService _adminService;
        public AdminController(IValidator<UpdateUsersRoleDTO> updateRoleValidator, IAdminService adminService)
        {
            _adminService = adminService;
            _updateRoleValidator = updateRoleValidator;
        }
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(UpdateUsersRoleDTO dto)
        {
            var validationResult = await _updateRoleValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _adminService.ChangeRoleAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
