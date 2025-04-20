namespace LibraryApp.Domain.DTOs.Update;

public class UpdateUserDTO
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
}