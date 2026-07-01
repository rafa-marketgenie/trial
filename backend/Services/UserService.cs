using trial.Contracts.Users;
using trial.Models;
using trial.Repositories;

namespace trial.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<IReadOnlyCollection<UserSummaryDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserSummaryDto{
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToList();
        }

        public async Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request)
        {
            if (await _userRepository.UsernameExistsAsync(request.Username))
                return null;

            if (await _userRepository.EmailExistsAsync(request.Email))
                return null;
            
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        // public async Task<bool> UpdateUserAsync(int id, UpdateUserRequestDto request)
        // {
            
        // }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if(user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}