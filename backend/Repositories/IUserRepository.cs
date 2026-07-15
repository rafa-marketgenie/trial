using trial.Models;

namespace trial.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> UsernameExistsAsync(string username);

        Task<bool> EmailExistsAsync(string email);
    }
}