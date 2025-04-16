using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.Project;

public record ProjectRequestDto
{
    [Required]
    public required string Name { get; init; } = string.Empty;

    [Required]
    public required string Description { get; init; } = string.Empty;
}