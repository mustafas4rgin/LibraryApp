namespace LibraryApp.Domain.DTos.Create;

public class CreateBookDTO
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Page { get; set; } = null!;
    public int Stock { get; set; } 
}