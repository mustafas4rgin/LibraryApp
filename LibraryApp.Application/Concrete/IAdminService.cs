using LibraryApp.Domain.DTOs.Update;

namespace LibraryApp.Application.Concrete
{
    public interface IAdminService
    {
        Task<IServiceResult> ChangeRoleAsync(UpdateUsersRoleDTO dto);
    }
    
}