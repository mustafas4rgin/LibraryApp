using AutoMapper;
using FluentValidation;
using LibraryApp.Application.Concrete;
using LibraryApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T,TCreateDto,TUpdateDto,TListDto> : 
    ControllerBase
    where T : EntityBase
    where TCreateDto : class
    where TUpdateDto : class
    where TListDto : class
    {
        private readonly IGenericService<T> _genericService;
        private readonly IMapper _mapper;
        private readonly IValidator<TCreateDto> _createValidator;
        private readonly IValidator<TUpdateDto> _updateValidator;
        public GenericController(IGenericService<T> genericService, IMapper mapper, IValidator<TCreateDto> createValidator, IValidator<TUpdateDto> updateValidator)
        {
            _genericService = genericService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet("GetById/{id}")]
        public async virtual Task<IActionResult> GetById([FromRoute]int id, [FromQuery]string? include)
        {
            var result = await _genericService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            var entity = result.Data;

            var dto = _mapper.Map<TListDto>(entity);
            
            return Ok(dto);
        }
        [HttpGet("GetAll")]
        public async virtual Task<IActionResult> GetAll([FromQuery]string? include)
        {
            var result = await _genericService.GetAllAsync();

            if (!result.Success)
                return NotFound(result.Message);

            var data = result.Data;

            var dto = _mapper.Map<List<TListDto>>(data);

            return Ok(dto);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]TCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var newEntity = _mapper.Map<T>(dto);

            var creatingResult = await _genericService.AddAsync(newEntity);

            if (!creatingResult.Success)
                return BadRequest(creatingResult.Message);

            return Ok(creatingResult);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody]TUpdateDto dto, [FromRoute]int id)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var existingResult = await _genericService.GetByIdAsync(id);

            if (!existingResult.Success)
                return NotFound(existingResult.Message);

            var existingEntity = existingResult.Data;

            _mapper.Map(dto,existingEntity);
            var updateResult = await _genericService.UpdateAsync(existingEntity);

            if (!updateResult.Success)
                return BadRequest(updateResult.Message);

            return Ok(updateResult);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _genericService.DeleteAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
    }
}
