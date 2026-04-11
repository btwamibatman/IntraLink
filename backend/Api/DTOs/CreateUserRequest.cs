using System.ComponentModel.DataAnnotations;
//DTO for creating a new user, with validation attributes to ensure data integrity.
namespace Api.DTOs;

public class CreateUserRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}
