using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace KA.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KADbContext _context;
        public UserRepository(KADbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User if exist</returns>
        public async Task<User?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

    }
}
