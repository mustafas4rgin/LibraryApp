using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Concrete;

public interface IUserService : IGenericService<User>
{
    Task<IServiceResult<IEnumerable<User>>> GetUsersWithIncludeAsync(string? include);
    Task<IServiceResult<User>> GetUserWithIncludeAsync(string? include, int id);
}