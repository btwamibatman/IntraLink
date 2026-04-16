using Api.DTOs;
using Api.Services;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var (user, error) = await _userService.CreateAsync(request);

        if (error is not null)
            return Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "A user with this email already exists.",
                Status = StatusCodes.Status409Conflict
            });

        return CreatedAtAction(nameof(GetById), new { id = user!.Id }, user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var (user, error) = await _userService.UpdateAsync(id, request);

        if (error == "not_found") return NotFound();
        if (error is not null)
            return Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "A user with this email already exists.",
                Status = StatusCodes.Status409Conflict
            });

        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}