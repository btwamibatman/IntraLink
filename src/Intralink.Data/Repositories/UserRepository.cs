using Application.Interfaces;
using Application.Users;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(UserAccount user)
    {
        var entity = MapToEntity(user);
        _db.Users.Add(entity);

        try
        {
            await _db.SaveChangesAsync();
            user.Id = entity.Id;
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            throw new EmailAlreadyExistsException(user.Email);
        }
    }

    public async Task<UserAccount?> GetByEmailAsync(string email)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        return user is null ? null : MapToAccount(user);
    }

    public async Task<List<UserAccount>> GetAllAsync()
    {
        return await _db.Users
            .AsNoTracking()
            .Select(u => new UserAccount
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                PasswordHash = u.PasswordHash
            })
            .ToListAsync();
    }

    public async Task<UserAccount?> GetByIdAsync(int id)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user is null ? null : MapToAccount(user);
    }

    public async Task<bool> UpdateAsync(UserAccount user)
    {
        var entity = await _db.Users.FindAsync(user.Id);

        if (entity is null)
            return false;

        entity.Name = user.Name;
        entity.Email = user.Email;
        entity.PasswordHash = user.PasswordHash;

        try
        {
            await _db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            throw new EmailAlreadyExistsException(user.Email);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Users.FindAsync(id);

        if (entity is null)
            return false;

        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    private static UserAccount MapToAccount(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        PasswordHash = user.PasswordHash
    };

    private static User MapToEntity(UserAccount user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        PasswordHash = user.PasswordHash
    };

    private static bool IsUniqueEmailViolation(DbUpdateException ex)
    {
        return ex.InnerException is PostgresException pg
               && pg.SqlState == PostgresErrorCodes.UniqueViolation
               && pg.ConstraintName == "IX_users_Email";
    }
}
