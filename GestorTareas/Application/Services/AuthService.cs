using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestorTareas.Application.DTOs;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Microsoft.IdentityModel.Tokens;

namespace GestorTareas.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }
    public async Task<TokenResponseDto?> Registrar(RegistrationDto dto)
    {
        // Verificar que el email no está en uso
        if (_userRepository.GetUserByEmail(dto.UserEmail) != null)
            return null; // email ya registrado
                         // Crear el usuario con la contraseña hasheada
        var user = new User
        {
            UserName = dto.UserName,
            UserEmail = dto.UserEmail,
            IsAdmin = false,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword)
        };
        _userRepository.AddUser(user);
        return GenerarToken(user);
    }
    public TokenResponseDto? Login(LoginDto dto)
    {
        var user = _userRepository.GetUserByEmail(dto.UserEmail);
        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.PasswordHash))
            return null;
        return GenerarToken(user);
    }
    private TokenResponseDto GenerarToken(User user)
    {
        var expiracion = DateTime.UtcNow.AddMinutes(
        int.Parse(_config["Jwt:ExpiracionMinutos"]!));
        var claims = new[]
        {
new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
new Claim(ClaimTypes.Name, user.UserName),
new Claim(ClaimTypes.Email, user.UserEmail),
new Claim(ClaimTypes.Role, (bool)user.IsAdmin ? "Admin" : "User")
};
        var clave = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config["Jwt:ClaveSecreta"]!));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
        issuer: _config["Jwt:Emisor"],
        audience: _config["Jwt:Audiencia"],
        claims: claims,
        expires: expiracion,
        signingCredentials: credenciales);
        return new TokenResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = expiracion
        };
    }
}

