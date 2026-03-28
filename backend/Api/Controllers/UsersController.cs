using Api.Models;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Created($"/users/{user.Id}", user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserRequest request)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        user.Name = request.Name;
        user.Email = request.Email;

        await _db.SaveChangesAsync();
        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
