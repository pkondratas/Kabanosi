namespace Kabanosi.Dtos.Project;

public record ProjectResponseDto(
    Guid Id,
    string Name,
    string Description
);