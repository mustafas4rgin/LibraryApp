using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Concrete;

public interface IBookService : IGenericService<Book>
{
    Task<IServiceResult<IEnumerable<Book>>> GetBooksWithIncludesAsync(string? include);
    Task<IServiceResult<Book>> GetBookWithIncludesAsync(string? include, int id);
}