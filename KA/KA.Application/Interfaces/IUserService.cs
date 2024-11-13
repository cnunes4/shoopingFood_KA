using KA.Application.DTO;

namespace KA.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> ValidateUserAsync(string username, string password);
    }
}
