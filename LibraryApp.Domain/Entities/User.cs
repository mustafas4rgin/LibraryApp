namespace LibraryApp.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int RoleId { get; set; }
    //navigation properties
    public ICollection<BookRental> Books { get; set; } = null!;
    public Role Role { get; set; } = null!;
}