using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Concrete;

public interface IGenericService<T> where T : EntityBase
{
    Task<IServiceResult<IEnumerable<T>>> GetAllAsync();
    Task<IServiceResult<T>> GetByIdAsync(int id);
    Task<IServiceResult> UpdateAsync(T entity);
    Task<IServiceResult> AddAsync(T entity);
    Task<IServiceResult> DeleteAsync(int id);
}