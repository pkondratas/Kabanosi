namespace Kabanosi.Settings;

public class JwtSettings
{
    public string Secret { get; set; } = null!;
    public int ExpiryMinutes { get; set; }
}