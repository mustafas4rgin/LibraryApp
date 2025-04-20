namespace LibraryApp.Domain.DTOs.List;

public class BookDTO
{
    public int Id { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Page { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public int Stock { get; set; } 
    public List<BookRentalWithUserDTO> RentedUsers { get; set; } = null!;
}