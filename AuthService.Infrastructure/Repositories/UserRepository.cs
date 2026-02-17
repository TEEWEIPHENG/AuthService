using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AuthService.Application.Interfaces.Repositories;

namespace AuthService.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _db;

        public UserRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email.Value == email);
        }

        public async Task<User?> GetByMobileNoAsync(string mobileNo)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Value == mobileNo);
        }
        public async Task<User?> ValidateCredentialAsync(string credential, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.PasswordHash.Value == password && 
                (x.Email.Value.Equals(credential, StringComparison.CurrentCultureIgnoreCase) || x.Username.Equals(credential)));
        }

        public async Task CreateAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
