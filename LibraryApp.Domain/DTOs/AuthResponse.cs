namespace LibraryApp.Domain.DTOs;

public class AuthResponseDTO
{
    public string RefreshToken { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
}