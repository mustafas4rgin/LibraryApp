using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Concrete;

public interface IGenericRepository
{
    Task SaveChangesAsync();
    IQueryable<T> GetAll<T>() where T : EntityBase;
    Task<T?> GetByIdAsync<T>(int id) where T : EntityBase;
    Task<T?> AddAsync<T>(T entity) where T : EntityBase;
    Task DeleteByIdAsync<T>(int id) where T : EntityBase;
    Task<T?> UpdateAsync<T>(T entity) where T : EntityBase;
}