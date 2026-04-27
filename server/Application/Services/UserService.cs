using Application.Interfaces;
using Application.Users;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(
        AppDbContext db,
        ILogger<UserService> logger,
        IPasswordHasher<User> passwordHasher)
    {
        _db = db;
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

        _db.Users.Add(user);

        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} created with email {Email}", user.Id, user.Email);
            return (MapToResponse(user), null);
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            _logger.LogWarning("Email conflict during create for {Email}", user.Email);
            return (null, "Email already exists");
        }
    }

    public async Task<(UserResult? user, string? error)> AuthenticateAsync(LoginCommand command)
    {
        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

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
        return await _db.Users
            .AsNoTracking()
            .Select(u => MapToResponse(u))
            .ToListAsync();
    }

    public async Task<UserResult?> GetByIdAsync(int id)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user is null ? null : MapToResponse(user);
    }

    public async Task<(UserResult? user, string? error)> UpdateAsync(int id, UpdateUserCommand command)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
            return (null, "not_found");

        user.Name = command.Name.Trim();
        user.Email = command.Email.Trim().ToLowerInvariant();

        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} updated", id);
            return (MapToResponse(user), null);
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            _logger.LogWarning("Email conflict during update for user {UserId}", id);
            return (null, "Email already exists");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
            return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        _logger.LogInformation("User {UserId} deleted", id);
        return true;
    }

    private static UserResult MapToResponse(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };

    private static bool IsUniqueEmailViolation(DbUpdateException ex)
    {
        return ex.InnerException is PostgresException pg
               && pg.SqlState == PostgresErrorCodes.UniqueViolation
               && pg.ConstraintName == "IX_users_Email";
    }
}
