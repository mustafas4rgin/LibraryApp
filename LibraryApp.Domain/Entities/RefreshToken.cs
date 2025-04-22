namespace LibraryApp.Domain.Entities;

public class RefreshToken : EntityBase
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public bool IsUsed { get; set; } = false;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}