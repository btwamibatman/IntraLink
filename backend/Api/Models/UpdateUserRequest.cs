using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class UpdateUserRequest
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string? Email { get; set; }
}
