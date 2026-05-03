using Application.Users;

namespace Application.Interfaces;

public interface IUserService
{
    Task<(UserResult? user, string? error)> CreateAsync(CreateUserCommand command);
    Task<(UserResult? user, string? error)> AuthenticateAsync(LoginCommand command);
    Task<List<UserResult>> GetAllAsync();
    Task<UserResult?> GetByIdAsync(int id);
    Task<(UserResult? user, string? error)> UpdateAsync(int id, UpdateUserCommand command);
    Task<bool> DeleteAsync(int id);
}
