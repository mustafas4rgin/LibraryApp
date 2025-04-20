using System.Security.Cryptography;
using System.Text;

namespace LibraryApp.Application.Helpers;

public static class HashingHelper
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
    public static byte[] PasswordHash(string password)
    {
        CreatePasswordHash(password, out var passwordHash, out _); 
        return passwordHash;
    }


    public static byte[] PasswordSalt(string password)
    {
        CreatePasswordHash(password, out _, out var passwordSalt);
        return passwordSalt;
    }
}