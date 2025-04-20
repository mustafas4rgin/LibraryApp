using Xunit;
using Moq;
using FluentAssertions;
using LibraryApp.Application.Services;
using LibraryApp.Application.Concrete;
using LibraryApp.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable;

public class BookRentalServiceTests
{
    [Fact]
    public async Task RentBookAsync_Should_Return_Error_If_Stock_Is_Zero()
    {
        // Arrange
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<BookRental>>();

        var book = new Book { Id = 1, Stock = 0 };
        var user = new User { Id = 1 };

        mockRepo.Setup(r => r.GetByIdAsync<Book>(1)).ReturnsAsync(book);
        mockRepo.Setup(r => r.GetByIdAsync<User>(1)).ReturnsAsync(user);

        var rentals = new List<BookRental>().AsQueryable().BuildMock();
        mockRepo.Setup(r => r.GetAll<BookRental>()).Returns(rentals);

        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<BookRental>(), default))
                     .ReturnsAsync(new ValidationResult());

        var service = new BookRentalService(mockRepo.Object, mockValidator.Object);
        var rental = new BookRental { BookId = 1, UserId = 1 };

        // Act
        var result = await service.RentBookAsync(rental);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Book is out of stock.");
    }
    
}
