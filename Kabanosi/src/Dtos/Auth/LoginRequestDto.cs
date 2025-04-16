using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.Auth;

public record LoginRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; init; } = string.Empty;
}