using trial.Contracts.Users;
using trial.Models;

namespace trial.Services
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetUserByIdAsync(int id);

        Task<IReadOnlyCollection<UserSummaryDto>> GetAllUsersAsync();

        Task<User?> CreateUserAsync(CreateUserRequestDto request);

        Task<User?> AuthenticateUserAsync(string username, string password);

        Task<bool> UpdateUserAsync(int id, UpdateUserRequestDto request, int actorUserId);

        Task<bool> DeleteUserAsync(int id, int actorUserId);
    }
}
