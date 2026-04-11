using Api.DTOs;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<UsersController> _logger;

    public UsersController(AppDbContext db, ILogger<UsersController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name!.Trim(),
            Email = request.Email!.Trim().ToLowerInvariant()
        };

        _db.Users.Add(user);

        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} created with email {Email}", user.Id, user.Email);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            _logger.LogWarning("Email conflict during create for {Email}", user.Email);
            return Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "A user with this email already exists.",
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users
            .AsNoTracking()
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user is null)
        {
            _logger.LogWarning("User {UserId} not found", id);
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
        {
            _logger.LogWarning("Cannot update user {UserId}: not found", id);
            return NotFound();
        }

        user.Name = request.Name!.Trim();
        user.Email = request.Email!.Trim().ToLowerInvariant();

        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("User {UserId} updated", id);
            return Ok(user);
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            _logger.LogWarning("Email conflict during update for user {UserId} and email {Email}", id, user.Email);
            return Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "A user with this email already exists.",
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
        {
            _logger.LogWarning("Cannot delete user {UserId}: not found", id);
            return NotFound();
        }

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        _logger.LogInformation("User {UserId} deleted", id);

        return NoContent();
    }

    private static bool IsUniqueEmailViolation(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && postgresException.ConstraintName == "IX_users_Email";
    }
}
