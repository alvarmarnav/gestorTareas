using System;

namespace GestorTareas.Application.DTOs;

public class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
