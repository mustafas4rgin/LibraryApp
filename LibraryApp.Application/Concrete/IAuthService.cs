using LibraryApp.Domain.DTOs;

namespace LibraryApp.Application.Concrete;

public interface IAuthService
{
    Task<IServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO dto);
    Task<IServiceResult> LogoutAsync(string refreshToken);
    Task<IServiceResult<AuthResponseDTO>> RefreshTokenAsync(string refreshToken);
}