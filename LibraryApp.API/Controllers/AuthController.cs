using LibraryApp.Application.Concrete;
using LibraryApp.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);
    
            return Ok(result.Data);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody]string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody]string refreshToken)
        {
            var result = await _authService.LogoutAsync(refreshToken);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
