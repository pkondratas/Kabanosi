namespace Kabanosi.Dtos.Auth;

public record LoginResponseDto
{
    public string UserId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public string Message { get; init; } = "Login successful";
}