using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Concrete;

public interface IBookRentalService : IGenericService<BookRental>
{
    Task<IServiceResult> RentBookAsync(BookRental bookRental);
}