namespace LibraryApp.Domain.Entities;

public class Book : EntityBase
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Page { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public int Stock { get; set; } 
    public ICollection<BookRental> RentedUsers { get; set; } = null!;
}