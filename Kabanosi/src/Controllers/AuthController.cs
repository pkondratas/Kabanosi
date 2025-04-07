using System.Net;
using Kabanosi.Entities;
using Kabanosi.Entities.Dtos.Auth;
using Kabanosi.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _db;
    private readonly UserManager<User> _userManager;
    private readonly string _secretKey;

    public AuthController(DatabaseContext db,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _db = db;
        _userManager = userManager;
        _secretKey = configuration.GetValue<string>("JwtSettings:Secret");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);
        if (existingUser != null)
            return BadRequest("Email already exists");

        try
        {
            var newUser = new User
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
            };

            var result = await _userManager.CreateAsync(newUser, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            return Ok(new
            {
                Message = "User registered successfully",
                UserId = newUser.Id,
            });
        }
        catch (Exception ex)
        {
            // Add logging later
            return StatusCode(500, new
            {
                Message = "An unexpected error occurred while registering the user."
            });
        }
    }
}