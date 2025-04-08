namespace Kabanosi.Entities.Dtos.Auth;

public class LoginResponseDto
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Token { get; set; }
    
    public string Message { get; set; } = "Login successful";
}