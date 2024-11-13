using KA.Domain.Entities;

namespace KA.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username);
    }
}
