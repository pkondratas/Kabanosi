using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.Auth;

public record RegisterRequestDto
{
    [Required]
    public required string UserName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public required string Email { get; init; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; init; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public required string ConfirmPassword { get; init; } = string.Empty;
}