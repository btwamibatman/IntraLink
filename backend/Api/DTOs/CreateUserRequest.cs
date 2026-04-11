using System.ComponentModel.DataAnnotations;
//DTO for creating a new user, with validation attributes to ensure data integrity.
namespace Api.DTOs;

public class CreateUserRequest
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string? Email { get; set; }
}
