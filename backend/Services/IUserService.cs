using trial.Contracts.Users;

namespace trial.Services
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetUserByIdAsync(int id);

        Task<IReadOnlyCollection<UserSummaryDto>> GetAllUsersAsync();

        Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request);

        Task<bool> UpdateUserAsync(int id, UpdateUserRequestDto request);

        Task<bool> DeleteUserAsync(int id);
    }
}
