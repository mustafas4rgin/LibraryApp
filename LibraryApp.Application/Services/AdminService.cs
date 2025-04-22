using LibraryApp.Application.Concrete;
using LibraryApp.Application.Results;
using LibraryApp.Domain.DTOs.Update;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Services;

public class AdminService : IAdminService
{
    private readonly IGenericRepository _genericRepository;
    public AdminService(IGenericRepository genericRepository)
    {
        _genericRepository = genericRepository;
    }
    public async Task<IServiceResult> ChangeRoleAsync(UpdateUsersRoleDTO dto)
    {
        try
        {
            var user = await _genericRepository.GetByIdAsync<User>(dto.UserId);

            if (user is null)
                return new ErrorResult("No user found.");

            var selectedRole = await _genericRepository.GetByIdAsync<Role>(dto.RoleId);

            if (selectedRole is null)
                return new ErrorResult("No role found.");

            user.RoleId = dto.RoleId;

            await _genericRepository.UpdateAsync(user);
            await _genericRepository.SaveChangesAsync();
            
            return new SuccessResult("Role successfully updated.");
        }

        catch (Exception ex)
        {
            return new ErrorResult(ex.Message);
        }

    }
}