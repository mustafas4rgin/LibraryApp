namespace LibraryApp.Application.Concrete;

public interface IPasswordHasher
{
    (byte[] passwordHash, byte[] passwordSalt) CreateHash(string password);
}