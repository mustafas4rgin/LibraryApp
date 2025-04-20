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

        await _genericRepository.AddAsync(bookRental);
        await _genericRepository.SaveChangesAsync();

        return new SuccessResult("Book rented successfully.");

    }
}