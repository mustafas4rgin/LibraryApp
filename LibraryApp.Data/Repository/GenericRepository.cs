using LibraryApp.Application.Concrete;
using LibraryApp.Data.Context;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repository;

internal class GenericRepository : IGenericRepository
{
    private readonly AppDbContext _context;
    public GenericRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context)); // null check for safety

    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public IQueryable<T> GetAll<T>() where T : EntityBase
    {
        return _context.Set<T>();
    }
    public async Task<T?> GetByIdAsync<T>(int id) where T : EntityBase
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public async Task<T?> AddAsync<T>(T entity) where T : EntityBase
    {
        entity.Id = default;
        entity.CreatedAt = DateTime.UtcNow;

        await _context.Set<T>().AddAsync(entity);

        return entity;
    }
    public async Task<T?> UpdateAsync<T>(T entity) where T : EntityBase
    {
        if (entity.Id == default)
           return null;

        var dbEntity = await GetByIdAsync<T>(entity.Id);

        if (dbEntity == null)
            return null;

        entity.CreatedAt = dbEntity.CreatedAt;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Update(entity);

        return entity;
    }
    public async Task DeleteByIdAsync<T>(int id) where T : EntityBase
    {
        var entity = await GetByIdAsync<T>(id);

        if (entity is null)
            return;

        _context.Set<T>().Remove(entity);
    }
}