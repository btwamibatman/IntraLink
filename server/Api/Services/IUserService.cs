using Api.DTOs;

namespace Api.Services;

public interface IUserService
{
    Task<(UserResponse? user, string? error)> CreateAsync(CreateUserRequest request);
    Task<(UserResponse? user, string? error)> AuthenticateAsync(LoginRequest request);
    Task<List<UserResponse>> GetAllAsync();
    Task<UserResponse?> GetByIdAsync(int id);
    Task<(UserResponse? user, string? error)> UpdateAsync(int id, UpdateUserRequest request);
    Task<bool> DeleteAsync(int id);
}
