using Application.Interfaces;
using Application.Users;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<(UserResult? user, string? error)> CreateAsync(CreateUserCommand command)
    {
        var user = new User
        {
            Name = command.Name.Trim(),
            Email = command.Email.Trim().ToLowerInvariant()
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, command.Password);

        try
        {
            await _userRepository.AddAsync(user);
            _logger.LogInformation("User {UserId} created with email {Email}", user.Id, user.Email);
            return (MapToResponse(user), null);
        }
        catch (EmailAlreadyExistsException)
        {
            _logger.LogWarning("Email conflict during create for {Email}", user.Email);
            return (null, "Email already exists");
        }
    }

    public async Task<(UserResult? user, string? error)> AuthenticateAsync(LoginCommand command)
    {
        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail);

        if (user is null)
        {
            _logger.LogWarning("Login failed for non-existing email {Email}", normalizedEmail);
            return (null, "invalid_credentials");
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Login failed due to wrong password for user {UserId}", user.Id);
            return (null, "invalid_credentials");
        }

        _logger.LogInformation("User {UserId} logged in", user.Id);
        return (MapToResponse(user), null);
    }

    public async Task<List<UserResult>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToResponse).ToList();
    }

    public async Task<UserResult?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return user is null ? null : MapToResponse(user);
    }

    public async Task<(UserResult? user, string? error)> UpdateAsync(int id, UpdateUserCommand command)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return (null, "not_found");

        user.Name = command.Name.Trim();
        user.Email = command.Email.Trim().ToLowerInvariant();

        try
        {
            var updated = await _userRepository.UpdateAsync(user);

            if (!updated)
                return (null, "not_found");

            _logger.LogInformation("User {UserId} updated", id);
            return (MapToResponse(user), null);
        }
        catch (EmailAlreadyExistsException)
        {
            _logger.LogWarning("Email conflict during update for user {UserId}", id);
            return (null, "Email already exists");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await _userRepository.DeleteAsync(id);

        if (!deleted)
            return false;

        _logger.LogInformation("User {UserId} deleted", id);
        return true;
    }

    private static UserResult MapToResponse(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };
}
