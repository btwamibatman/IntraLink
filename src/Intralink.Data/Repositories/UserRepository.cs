using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
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

    public async Task AddAsync(User user)
    {
        _db.Users.Add(user);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            throw new EmailAlreadyExistsException(user.Email);
        }
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _db.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateAsync(User user)
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

    private static bool IsUniqueEmailViolation(DbUpdateException ex)
    {
        return ex.InnerException is PostgresException pg
               && pg.SqlState == PostgresErrorCodes.UniqueViolation
               && pg.ConstraintName == "IX_users_Email";
    }
}
