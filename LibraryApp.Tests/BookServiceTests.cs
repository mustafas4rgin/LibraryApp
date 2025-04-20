using System.ComponentModel.DataAnnotations;
using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using MockQueryable;
using Moq;
using FluentValidation.Results;
using System.Threading.Tasks;
using FluentAssertions;

namespace LibraryApp.Tests;

public class BookServiceTests
{
    [Fact]
    public async Task GetBooksWithInclude_NullError_ReturnsFalseResult()
    {
        string? include = "";
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();
        var books = new List<Book>().AsQueryable().BuildMock();
        mockRepo.Setup(x => x.GetAll<Book>()).Returns(books);

        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Book>(), default))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var service = new BookService(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.GetBooksWithIncludesAsync(include);

        //assert
        result.Success.Should().Be(false);
        result.Message.Should().Be("There is no book.");
    }
    [Fact]
    public async Task GetBooksWithInclude_ReturnsTrueResult()
    {
        //arrange
        string include = "";
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Sample"}
        }
        .AsQueryable().BuildMock();
        mockRepo.Setup(x => x.GetAll<Book>()).Returns(books);

        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Book>(), default))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var service = new BookService(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.GetBooksWithIncludesAsync(include);
        //assert

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Books found.");
    }
    [Fact]
    public async Task GetBooksWithIncludes_RentedUsers_ReturnsBooksWithUsers()
    {
        //arrange
        string? include = "rented-users";
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();
        var books = new List<Book>
        {
            new Book
            {
                Id = 1,
                RentedUsers = new List<BookRental>
                {
                    new BookRental {User = new User {Name = "Sample"}}
                }

            }
        }.AsQueryable().BuildMock();

        mockRepo.Setup(x => x.GetAll<Book>()).Returns(books);
        mockValidator.Setup(x => x.ValidateAsync(It.IsAny<Book>(), default))
                        .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var service = new BookService(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.GetBooksWithIncludesAsync(include);

        //assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Books found.");
        result.Data.Should().NotBeNull();
        result.Data.First().RentedUsers.Should().NotBeNull();
        result.Data.First().RentedUsers.First().User.Name.Should().Be("Sample");
    }
    [Fact]
    public async Task GetBookWithIncludes_RentedUsers_ReturnsBooksWithUsers()
    {
        string? include = "rented-users";
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();
        var book = new Book
        {
            Id = 1,
            RentedUsers = new List<BookRental>
            {
                new BookRental{Id = 1, User = new User {Id = 1, Name = "Sample"}}
            }
        };

        var bookList = new List<Book> { book }.AsQueryable().BuildMock();

        mockRepo.Setup(x => x.GetAll<Book>())
                .Returns(bookList);

        mockValidator.Setup(x => x.ValidateAsync(It.IsAny<Book>(), default))
                        .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var service = new BookService(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.GetBookWithIncludesAsync(include, 1);

        //assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Book found.");
        result.Data.Should().NotBeNull();
        result.Data.RentedUsers.Should().NotBeNull();
        result.Data.RentedUsers.First().User.Name.Should().Be("Sample");
    }
    [Fact]
    public async Task GetBooksWithIncludes_WhenExceptionThrown_ReturnsError()
    {
        // arrange
        var include = "";
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();

        mockRepo.Setup(x => x.GetAll<Book>())
                .Throws(new Exception("Veritabanı hatası"));

        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Book>(), default))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var service = new BookService(mockRepo.Object, mockValidator.Object);

        // act
        var result = await service.GetBooksWithIncludesAsync(include);

        // assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Veritabanı hatası");
    }
    [Fact]
    public async Task GetBooksWithIncludes_WhenIncludeIsInvalid_ReturnsBooksWithoutInclude()
    {
        var include = "invalid-include";
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();

        var books = new List<Book>
    {
        new Book { Id = 1, Title = "Test Book" }
    }.AsQueryable().BuildMock();

        mockRepo.Setup(x => x.GetAll<Book>()).Returns(books);
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Book>(), default))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var service = new BookService(mockRepo.Object, mockValidator.Object);

        var result = await service.GetBooksWithIncludesAsync(include);

        result.Success.Should().BeTrue();
        result.Data.Count().Should().Be(1);
    }


}