using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.Results;
using LibraryApp.Domain.DTOs.Create;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Services;

public class BookRentalService : GenericService<BookRental>, IBookRentalService
{
    private readonly IValidator<BookRental> validator;
    private readonly IGenericRepository _genericRepository;
    public BookRentalService(IGenericRepository repository, IValidator<BookRental> validator) : base(repository, validator)
    {
        this.validator = validator;
        _genericRepository = repository;
    }
    public async Task<IServiceResult<IEnumerable<BookRental>>> ListBookRentalsAsync(string? include)
    {
        try
        {
            var query = _genericRepository.GetAll<BookRental>();

            if (!string.IsNullOrWhiteSpace(include))
            {
                var includes = include.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var inc in includes.Select(x => x.Trim().ToLower()))
                {
                    if (inc == "user")
                        query = query.Include(br => br.User);
                    else if (inc == "book")
                        query = query.Include(br => br.Book);
                    else if (inc == "book-users")
                        query = query.Include(br => br.Book)
                                     .Include(br => br.User);
                }
            }

            var bookRentals = await query.ToListAsync();

            if (!bookRentals.Any())
                return new ErrorDataResult<IEnumerable<BookRental>>("There is no rent.");

            return new SuccessDataResult<IEnumerable<BookRental>>("Book rentals: ",bookRentals);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<IEnumerable<BookRental>>(ex.Message);
        }
    }
    public async Task<IServiceResult> RentBookAsync(BookRental bookRental)
    {
        var book = await _genericRepository.GetByIdAsync<Book>(bookRental.BookId);
        var user = await _genericRepository.GetByIdAsync<User>(bookRental.UserId);

        if (book is null || user is null)
            return new ErrorResult("Book or user doesn't exist.");

        var validationResult = await validator.ValidateAsync(bookRental);

        if (!validationResult.IsValid)
            return new ErrorResult(string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existingBookRental = await _genericRepository.GetAll<BookRental>()
                                .FirstOrDefaultAsync(br => br.UserId == bookRental.UserId && br.BookId == bookRental.BookId);

        if (existingBookRental is not null)
            return new ErrorResult("This book is already rented by this user.");

        if (book.Stock <= 0)
            return new ErrorResult("Book is out of stock.");

        bookRental.RentalDate = DateTime.UtcNow;   
        await _genericRepository.AddAsync(bookRental);
        await _genericRepository.SaveChangesAsync();

        book.Stock--;
        await _genericRepository.UpdateAsync(book);
        return new SuccessResult("Book rented successfully.");

    }
    public async Task<IServiceResult> DeleteBookRentalAsync(int userId, int bookId)
    {
        var book = await _genericRepository.GetByIdAsync<Book>(bookId);

        if (book is null)
            return new ErrorResult("There is no book with that id.");
        
        var user = await _genericRepository.GetByIdAsync<User>(userId);

        if (user is null)
            return new ErrorResult("There is no user with that id.");

        var bookRental = await _genericRepository.GetAll<BookRental>()
                            .FirstOrDefaultAsync(br => br.BookId == bookId && br.UserId == userId);

        if (bookRental is null)
            return new ErrorResult("This book wasn't rented by this user.");

        await _genericRepository.DeleteAsync(bookRental);
        await _genericRepository.SaveChangesAsync();

        book.Stock++;
        await _genericRepository.UpdateAsync(book);
        return new SuccessResult("Book rental deleted successfully.");
    }
}