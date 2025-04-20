namespace LibraryApp.Domain.DTOs.Update;

public class UpdateBookDTO
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Page { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public int Stock { get; set; } 
}