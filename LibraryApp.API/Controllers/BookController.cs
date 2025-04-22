using AutoMapper;
using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Domain.DTos.Create;
using LibraryApp.Domain.DTOs.List;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace LibraryApp.API.Controllers
{
    [Authorize(Roles = "Admin ,Librarian")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : GenericController<Book,CreateBookDTO,UpdateBookDTO,BookDTO>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BookController(IBookService service, IMapper mapper, IValidator<CreateBookDTO> createValidator, IValidator<UpdateBookDTO> updateValidator)
        : base(service,mapper,createValidator,updateValidator) 
        {
            _mapper = mapper;
            _bookService = service;
        }
        [HttpGet("GetAll")]
        public async override Task<IActionResult> GetAll([FromQuery]string? include)
        {
            var result = await _bookService.GetBooksWithIncludesAsync(include);

            if (!result.Success)
                return NotFound(result.Message);

            var books = result.Data;

            var dto = _mapper.Map<List<BookDTO>>(books);

            return Ok(dto);
        }
        [HttpGet("GetById/{id}")]
        public async override Task<IActionResult> GetById([FromRoute]int id,[FromQuery]string? include)
        {
            var result = await _bookService.GetBookWithIncludesAsync(include,id);

            if (!result.Success)
                return NotFound(result.Message);

            var book = result.Data;

            var dto = _mapper.Map<BookDTO>(book);

            return Ok(dto);
        }
    }
}
