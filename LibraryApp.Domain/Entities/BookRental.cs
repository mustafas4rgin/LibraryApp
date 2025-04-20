namespace LibraryApp.Domain.Entities;

public class BookRental : EntityBase
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public DateTime RentalDate { get; set; }
}