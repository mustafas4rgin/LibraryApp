using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.Results;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Services;

public class UserService : GenericService<User>, IUserService
{
    private readonly IGenericRepository _genericRepository;

    public UserService(IGenericRepository genericRepository, IValidator<User> validator) : base(genericRepository, validator)
    {
        _genericRepository = genericRepository;
    }
    public async Task<IServiceResult<User>> GetUserWithIncludeAsync(string? include, int id)
    {
        try
        {
            var query = _genericRepository.GetAll<User>();

            if (!string.IsNullOrEmpty(include))
            {
                var includes = include.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var inc in includes.Select(x => x.Trim().ToLower()))
                {
                    if (inc == "role")
                        query = query.Include(u => u.Role);
                    else if(inc == "books")
                        query = query.Include(u => u.Books)
                            .ThenInclude(br => br.Book);
                }
            }

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return new ErrorDataResult<User>("User cannot be found.");

            return new SuccessDataResult<User>("User found.",user);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<User>(ex.Message);
        }
    }
    public async Task<IServiceResult<IEnumerable<User>>> GetUsersWithIncludeAsync(string? include)
    {
        try
        {
            var query = _genericRepository.GetAll<User>();

            if (!string.IsNullOrWhiteSpace(include))
            {
                var includes = include.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var inc in includes.Select(x => x.Trim().ToLower()))
                {
                    if (inc == "role")
                        query = query.Include(u => u.Role);
                    else if (inc == "books")
                        query = query.Include(u => u.Books)
                            .ThenInclude(br => br.Book);
                }
            }

            var users = await query.ToListAsync();

            if (!users.Any())
                return new ErrorDataResult<IEnumerable<User>>("There is no user.");

            return new SuccessDataResult<IEnumerable<User>>("Users found.", users);

        }
        catch (Exception ex)
        {
            return new ErrorDataResult<IEnumerable<User>>(ex.Message);
        }
    }
}