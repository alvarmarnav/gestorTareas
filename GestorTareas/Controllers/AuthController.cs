using System;
using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    => _authService = authService;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegistrationDto dto)
    {
        var result = await _authService.Register(dto);
        if (result == null)
            return Conflict("El email ya está registrado");
        return Ok(result);
    }
    // POST /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var result = _authService.Login(dto);
        if (result == null)
            return Unauthorized("Credenciales incorrectas");
        return Ok(result);
    }
    [HttpGet("check-email")]
    public IActionResult CheckEmail([FromQuery] string email)
    {
        if(string.IsNullOrWhiteSpace(email))
        return BadRequest(false);

        var emailExists = _authService.CheckEmail(email);
        return Ok(!emailExists);
    }
}