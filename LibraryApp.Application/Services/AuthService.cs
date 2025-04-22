using LibraryApp.Application.Concrete;
using LibraryApp.Application.Helpers;
using LibraryApp.Application.Results;
using LibraryApp.Domain.DTOs;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository _repository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(IGenericRepository repository, JwtHelper jwtHelper)
    {
        _repository = repository;
        _jwtHelper = jwtHelper;
    }

    public async Task<IServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO dto)
    {
        var user = await _repository
            .GetAll<User>()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);



        if (user == null || !HashingHelper.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            return new ErrorDataResult<AuthResponseDTO>("Auth error.");

        var accessToken = _jwtHelper.GenerateAccessToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        var tokenEntity = new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _repository.AddAsync(tokenEntity);
        await _repository.SaveChangesAsync();

        var oldTokens = await _repository
                .GetAll<RefreshToken>()
                .Where(t => t.UserId == user.Id && (t.IsUsed || t.IsRevoked || t.ExpiresAt < DateTime.UtcNow))
                .ToListAsync();

        foreach (var token in oldTokens)
            await _repository.DeleteAsync(token);

        var tokenDto = new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return new SuccessDataResult<AuthResponseDTO>("Login success.", tokenDto);
    }

    public async Task<IServiceResult<AuthResponseDTO>> RefreshTokenAsync(string refreshToken)
    {
        var tokenInDb = await _repository
            .GetAll<RefreshToken>()
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (tokenInDb == null ||
            tokenInDb.ExpiresAt < DateTime.UtcNow ||
            tokenInDb.IsRevoked ||
            tokenInDb.IsUsed)
        {
            return new ErrorDataResult<AuthResponseDTO>("Token error.");
        }

        tokenInDb.IsUsed = true;
        tokenInDb.IsRevoked = true;

        var newAccessToken = _jwtHelper.GenerateAccessToken(tokenInDb.User);
        var newRefreshToken = _jwtHelper.GenerateRefreshToken();

        var newTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = tokenInDb.UserId
        };

        await _repository.AddAsync(newTokenEntity);
        await _repository.SaveChangesAsync();

        var tokenDto = new AuthResponseDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
        return new SuccessDataResult<AuthResponseDTO>("Token refreshed.", tokenDto);
    }

    public async Task<IServiceResult> LogoutAsync(string refreshToken)
    {
        var tokenInDb = await _repository
            .GetAll<RefreshToken>()
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (tokenInDb == null)
            return new ErrorResult("Logout error.");

        tokenInDb.IsRevoked = true;
        await _repository.SaveChangesAsync();

        return new SuccessResult("Logged out successfully.");
    }

}
