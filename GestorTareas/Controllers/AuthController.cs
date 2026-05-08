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
    [HttpPost("registro")]
    [AllowAnonymous]
    public IActionResult Registro([FromBody] RegistrationDto dto)
    {
        var resultado = _authService.Registrar(dto);
        if (resultado == null)
            return Conflict("El email ya está registrado");
        return Ok(resultado);
    }
    // POST /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var resultado = _authService.Login(dto);
        if (resultado == null)
            return Unauthorized("Credenciales incorrectas");
        return Ok(resultado);
    }
}