using LibraryApp.Application.Concrete;
using LibraryApp.Application.Helpers;

namespace LibraryApp.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    public (byte[] passwordHash, byte[] passwordSalt) CreateHash(string password)
    {
        HashingHelper.CreatePasswordHash(password, out var hash, out var salt);
        return (hash, salt);
    }
}
