using AutoMapper;
using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Domain.DTOs;
using LibraryApp.Domain.DTOs.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin ,Librarian")]
    [ApiController]
    public class BookRentalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookRental> _createValidator;
        private readonly IValidator<UpdateBookRental> _updateValidator;
        private readonly IBookRentalService _bookRentalService;
        public BookRentalController(IMapper mapper, IBookRentalService bookRentalService, IValidator<CreateBookRental> createValidator, IValidator<UpdateBookRental> updateValidator)
        {
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _bookRentalService = bookRentalService;
        }
        [HttpPost("rent-book")]
        public async Task<IActionResult> RentBook(CreateBookRental dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var bookRental = _mapper.Map<BookRental>(dto);

            var result = await _bookRentalService.RentBookAsync(bookRental);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpGet("get-book-rentals")]
        public async Task<IActionResult> GetAll(string? include)
        {
            var result = await _bookRentalService.ListBookRentalsAsync(include);

            if (!result.Success)
                return NotFound(result.Message);

            var bookRentals = result.Data;

            var dto = _mapper.Map<List<BookRentalDTO>>(bookRentals);

            return Ok(dto);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery]int userId, [FromQuery]int bookId)
        {
            var result = await _bookRentalService.DeleteBookRentalAsync(userId,bookId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
