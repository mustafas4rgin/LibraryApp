using System.IO.Compression;
using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.Results;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Services;

public class BookService : GenericService<Book>, IBookService
{
    private readonly IGenericRepository _genericRepository;
    public BookService(IGenericRepository genericRepository, IValidator<Book> validator) : base(genericRepository, validator)
    {
        _genericRepository = genericRepository;
    }
    public async Task<IServiceResult<Book>> GetBookWithIncludesAsync(string? include, int id)
    {
        try
        {
            var query = _genericRepository.GetAll<Book>();

            if (!string.IsNullOrWhiteSpace(include))
            {
                var includes = include.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var inc in includes.Select(x => x.Trim().ToLower()))
                    {
                        if (inc == "rented-users")
                            query = query.Include(b => b.RentedUsers)
                                    .ThenInclude(br => br.User);
                    }
            }
            var book = await query.FirstOrDefaultAsync(b => b.Id == id);
            
            if (book is null)
                return new ErrorDataResult<Book>("There is no book with this id.");

            return new SuccessDataResult<Book>("Book found.",book);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<Book>(ex.Message);
        }
    }
    public async Task<IServiceResult<IEnumerable<Book>>> GetBooksWithIncludesAsync(string? include)
    {
        try
        {
            var query = _genericRepository.GetAll<Book>();

        if (!string.IsNullOrWhiteSpace(include))
        {
            var includes = include.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var inc in includes.Select(x => x.Trim().ToLower()))
                {
                    if (inc == "rented-users")
                        query = query.Include(b => b.RentedUsers)
                            .ThenInclude(br => br.User);
                }
        }

        var books = await query.ToListAsync();

        if (!books.Any())
            return new ErrorDataResult<IEnumerable<Book>>("There is no book.");

        return new SuccessDataResult<IEnumerable<Book>>("Books found.",books);

        }
        catch (Exception ex)
        {
            return new ErrorDataResult<IEnumerable<Book>>(ex.Message);
        }
        
    }
}