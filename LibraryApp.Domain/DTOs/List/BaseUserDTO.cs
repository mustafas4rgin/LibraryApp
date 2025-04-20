namespace LibraryApp.Domain.DTOs.List;

public class BaseUserDTO
{
    public int Id { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
}