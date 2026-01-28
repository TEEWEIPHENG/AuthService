namespace AuthService.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<Domain.Entities.User?> GetByUsernameAsync(string username);
        public Task<Domain.Entities.User?> GetByIdAsync(string userId);
        public Task<Domain.Entities.User?> GetByEmailAsync(string email);
        public Task CreateAsync(Domain.Entities.User user);
        public Task UpdateAsync(Domain.Entities.User user);
    }
}
