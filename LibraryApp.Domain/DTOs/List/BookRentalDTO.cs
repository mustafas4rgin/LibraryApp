using LibraryApp.Domain.DTOs.List;

namespace LibraryApp.Domain.DTOs;

public class BookRentalDTO
{
    public BaseUserDTO User { get; set; } = null!;
    public BookDTO Book { get; set; } = null!;
    public DateTime RentalDate { get; set; }
}