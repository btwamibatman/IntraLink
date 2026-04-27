using Api.DTOs;
using Application.Interfaces;
using Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
  
    [HttpPost("register")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var (user, error) = await _userService.CreateAsync(new CreateUserCommand
        {
            Name = request.Name!,
            Email = request.Email!,
            Password = request.Password!
        });

        if (error is not null)
            return Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "A user with this email already exists.",
                Status = StatusCodes.Status409Conflict
            });

        return CreatedAtAction(nameof(GetById), new { id = user!.Id }, MapToResponse(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (user, error) = await _userService.AuthenticateAsync(new LoginCommand
        {
            Email = request.Email!,
            Password = request.Password!
        });
        if (error is not null)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid credentials",
                Detail = "Email or password is incorrect.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        return Ok(MapToResponse(user!));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var users = await _userService.GetAllAsync();
        return Ok(users.Select(MapToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        return user is null ? NotFound() : Ok(MapToResponse(user));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var (user, error) = await _userService.UpdateAsync(id, new UpdateUserCommand
        {
            Name = request.Name!,
            Email = request.Email!
        });

        if (error == "not_found") return NotFound();
        if (error is not null)
            return Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "A user with this email already exists.",
                Status = StatusCodes.Status409Conflict
            });

        return Ok(MapToResponse(user!));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private static UserResponse MapToResponse(UserResult user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };
}
