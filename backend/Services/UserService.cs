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

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

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

        public async Task<User?> CreateUserAsync(CreateUserRequestDto request)
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
            return user;
        }


        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserRequestDto request, Guid actorUserId)
        {
            if (id != actorUserId)
                return false;

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;
            
            if (!string.IsNullOrEmpty(request.Username) && request.Username != user.Username)
            {
                if (await _userRepository.UsernameExistsAsync(request.Username))
                    return false;
                user.Username = request.Username;
            }

            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                if (await _userRepository.EmailExistsAsync(request.Email))
                    return false;
                user.Email = request.Email;
            }

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id, Guid actorUserId)
        {
            if (id != actorUserId)
                return false;
                
            var user = await _userRepository.GetByIdAsync(id);
            if(user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}