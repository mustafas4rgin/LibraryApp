using System.IO.Compression;
using System.Runtime.Serialization;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using LibraryApp.Application.Concrete;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using Moq;

namespace LibraryApp.Tests;

public class GenericServiceTests
{
    public static IEnumerable<Object[]> validEntities => new List<Object[]>
    {
        new object[] { new Book {Id = 1, Title = "Book Sample"} },
        new object[] { new User {Id = 1, Name = "User Sample"}},
        new object[] { new Role {Id = 1, Name = "Role Sample"}}
    };
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task AddAsync_NonValidEntity_ReturnsError<T>(T entity) where T : EntityBase
    {
        //arrangement
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockValidator.Setup(x => x.ValidateAsync(entity, default))
             .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
             {
                 new ValidationFailure("Field", "Validation failed.")
             }));


        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.AddAsync(entity);

        //assert
        result.Success.Should().BeFalse();

    }
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task AddAsync_WhenExceptionThrown_ReturnsValidatioError<T>(T entity) where T : EntityBase
    {
        //arrangement
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockValidator.Setup(v => v.ValidateAsync(entity, default))
                            .ReturnsAsync(new ValidationResult());
        mockRepo.Setup(x => x.AddAsync(entity))
                            .ThrowsAsync(new Exception("Database error."));

        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.AddAsync(entity);

        //assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Database error.");
    }
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task AddAsync_WithValidEntity_ReturnsSuccess<T>(T entity) where T : EntityBase
    {
        //arrange
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockValidator.Setup(v => v.ValidateAsync(entity, default))
                     .ReturnsAsync(new ValidationResult());

        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.AddAsync(entity);

        //assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Entity created.");
        mockRepo.Verify(r => r.AddAsync(entity), Times.Once);
        mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task UpdateAsync_WithValidEntity_ReturnsSuccess<T>(T entity) where T : EntityBase
    {
        //arrange
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockValidator.Setup(v => v.ValidateAsync(entity, default))
                            .ReturnsAsync(new ValidationResult());

        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.UpdateAsync(entity);

        //assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Entity updated successfully.");
        mockRepo.Verify(r => r.UpdateAsync(entity), Times.Once);
        mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task UpdateAsync_ThrowsException_ReturnsError<T>(T entity) where T : EntityBase
    {
        //arrangement
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockValidator.Setup(x => x.ValidateAsync(entity, default))
                            .ReturnsAsync(new ValidationResult());
        mockRepo.Setup(x => x.UpdateAsync(entity))
                            .ThrowsAsync(new Exception("Database error."));

        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.UpdateAsync(entity);

        //assert
        result.Success.Should().Be(false);
    }
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task UpdateAsync_NonValidEntity_ReturnsValidationError<T>(T entity) where T : EntityBase
    {
        //arrangement
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockValidator.Setup(x => x.ValidateAsync(entity, default))
             .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
             {
                 new ValidationFailure("Field", "Validation failed.")
             }));


        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.UpdateAsync(entity);

        //assert
        result.Message.Should().Contain("Validation failed.");
        result.Success.Should().Be(false);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    public async Task DeleteAsync_ReturnsSuccess(int id)
    {
        //arrangement
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();

        var book = new Book { Id = 1, Title = "Sample" };
        mockValidator.Setup(x => x.ValidateAsync(book, default))
                        .ReturnsAsync(new ValidationResult());

        var service = new GenericService<Book>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.DeleteAsync(id);

        //assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Entity deleted successfully.");
    }
    [Theory]
    [InlineData(99)]
    [InlineData(999)]
    public async Task DeleteAsync_ThrowsException(int id)
    {
        //arrangement
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<Book>>();

        var testBook = new Book { Id = 1, Title = "Test book" };

        mockValidator.Setup(x => x.ValidateAsync(testBook, default))
                            .ReturnsAsync(new ValidationResult());
        mockRepo.Setup(x => x.DeleteByIdAsync<Book>(id))
                            .ThrowsAsync(new Exception("Database error."));

        var service = new GenericService<Book>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.DeleteAsync(id);

        //assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Database error.");
    }
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task GetByIdAsync_ReturnsSuccess<T>(T entity) where T : EntityBase
    {
        //arrangement
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockRepo.Setup(x => x.GetByIdAsync<T>(entity.Id))
                        .ReturnsAsync(entity);

        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        //act
        var result = await service.GetByIdAsync(entity.Id);

        //assert
        result.Success.Should().BeTrue();
        result.Data.Should().Be(entity);
    }
    [Theory]
    [MemberData(nameof(validEntities))]
    public async Task GetByIdAsync_ThrowsException_ReturnsError<T>(T entity) where T : EntityBase
    {
        // arrange
        var mockRepo = new Mock<IGenericRepository>();
        var mockValidator = new Mock<IValidator<T>>();

        mockRepo.Setup(x => x.GetByIdAsync<T>(entity.Id))
                .ThrowsAsync(new Exception("Database error."));

        var service = new GenericService<T>(mockRepo.Object, mockValidator.Object);

        // act
        var result = await service.GetByIdAsync(entity.Id);

        // assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Database error.");
    }

}