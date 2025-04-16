using AutoMapper;
using Kabanosi.Dtos.Auth;
using Kabanosi.Entities;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly  ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
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
            var newUser = _mapper.Map<User>(registerRequestDto);

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);
        if (existingUser == null || !await _userManager.CheckPasswordAsync(existingUser, loginRequestDto.Password))
        {
            return Unauthorized(new { Message = "Username or password is incorrect" });
        }

        var token = _tokenService.GenerateToken(existingUser);
        
        var loginResponse = _mapper.Map<LoginResponseDto>(existingUser);
        loginResponse = loginResponse with { Token = token };

        return Ok(loginResponse);
    }
}