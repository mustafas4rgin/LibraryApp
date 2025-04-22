using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Concrete;

public interface IBookRentalService : IGenericService<BookRental>
{
    Task<IServiceResult> DeleteBookRentalAsync(int userId, int bookId);
    Task<IServiceResult> RentBookAsync(BookRental bookRental);
    Task<IServiceResult<IEnumerable<BookRental>>> ListBookRentalsAsync(string? include);
}