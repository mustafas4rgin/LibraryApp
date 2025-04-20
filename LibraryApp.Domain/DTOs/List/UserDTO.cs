using LibraryApp.Domain.Entities;

namespace LibraryApp.Domain.DTOs.List;

public class UserDTO : BaseUserDTO
{
    public List<BookRentalWithBookDTO> Books { get; set; } = null!;
    public RoleDTO Role { get; set; } = null!;
}