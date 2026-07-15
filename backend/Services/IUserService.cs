using trial.Contracts.Users;
using trial.Models;

namespace trial.Services
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetUserByIdAsync(Guid id);

        Task<IReadOnlyCollection<UserSummaryDto>> GetAllUsersAsync();

        Task<User?> CreateUserAsync(CreateUserRequestDto request);

        Task<User?> AuthenticateUserAsync(string username, string password);

        Task<bool> UpdateUserAsync(Guid id, UpdateUserRequestDto request, Guid actorUserId);

        Task<bool> DeleteUserAsync(Guid id, Guid actorUserId);
    }
}
