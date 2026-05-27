using System;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareas.Infraestructure.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // pasar al siguiente middleware
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso no encontrado");
            await EscribirError(context, 404, "Recurso no encontrado", ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argumento inválido");
            await EscribirError(context, 400, "Datos incorrectos", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acceso denegado");
            await EscribirError(context, 403, "Acceso denegado", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflicto de negocio");
            await EscribirError(context, 409, "Conflicto de negocio", ex.Message);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
            await EscribirError(context, 500, "Error interno del servidor",
            "Se ha producido un error inesperado");
        }
    }
    private static async Task EscribirError(
    HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails { Status = status, Title = title, Detail = detail };
        await context.Response.WriteAsJsonAsync(problem);
    }
}