using LibraryApp.Application.Concrete;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Results;
using FluentValidation;


namespace LibraryApp.Application.Services;

public class GenericService<T> : IGenericService<T> where T : EntityBase
{
    private readonly IGenericRepository _genericRepository;
    private readonly IValidator<T> _validator;
    public GenericService(IGenericRepository genericRepository, IValidator<T> validator)
    {
        _genericRepository = genericRepository;
        _validator = validator;
    }
    //TODO: TRY CATCH
    public async Task<IServiceResult<IEnumerable<T>>> GetAllAsync()
    {
        var entities = await _genericRepository.GetAll<T>().ToListAsync();

        if (!entities.Any())
            return new ErrorDataResult<IEnumerable<T>>("There is no data.");

        return new SuccessDataResult<IEnumerable<T>>("Data found.", entities);
    }
    public async Task<IServiceResult<T>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _genericRepository.GetByIdAsync<T>(id);

            if (entity is null)
                return new ErrorDataResult<T>("Data cannot be found.");

            return new SuccessDataResult<T>("Data found.", entity);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<T>(ex.Message);
        }
    }

    public async Task<IServiceResult> AddAsync(T entity)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(entity);

            if (!validationResult.IsValid)
                return new ErrorResult(string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)));

            await _genericRepository.AddAsync(entity);
            await _genericRepository.SaveChangesAsync();

            return new SuccessResult("Entity created.");
        }
        catch (Exception ex)
        {
            return new ErrorResult(ex.Message);
        }
    }
    public async Task<IServiceResult> UpdateAsync(T entity)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(entity);

            if (!validationResult.IsValid)
                return new ErrorResult(string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)));

            await _genericRepository.UpdateAsync(entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await _genericRepository.SaveChangesAsync();
            return new SuccessResult("Entity updated successfully.");
        }
        catch (Exception ex)
        {
            return new ErrorResult(ex.Message);
        }
    }
    public async Task<IServiceResult> DeleteAsync(int id)
    {
        try
        {
            await _genericRepository.DeleteByIdAsync<T>(id);
            await _genericRepository.SaveChangesAsync();

            return new SuccessResult("Entity deleted successfully.");
        }
        catch (Exception ex)
        {
            return new ErrorResult(ex.Message);
        }
    }
}