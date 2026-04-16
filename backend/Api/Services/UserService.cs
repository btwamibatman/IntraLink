using Api.DTOs;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Services;

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

    public async Task<(UserResponse? user, string? error)> CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name!.Trim(),
            Email = request.Email!.Trim().ToLowerInvariant()
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password!);

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

    public async Task<List<UserResponse>> GetAllAsync()
    {
        return await _db.Users
            .AsNoTracking()
            .Select(u => MapToResponse(u))
            .ToListAsync();
    }

    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user is null ? null : MapToResponse(user);
    }

    public async Task<(UserResponse? user, string? error)> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
            return (null, "not_found");

        user.Name = request.Name!.Trim();
        user.Email = request.Email!.Trim().ToLowerInvariant();

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

    private static UserResponse MapToResponse(User user) => new()
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
