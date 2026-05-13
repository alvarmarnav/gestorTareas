using GestorTareas.Application.Services;
using GestorTareas.Models;
using Moq;
using NUnit.Framework;

namespace GestorTareas.Tests;

[TestFixture]
public class AuthServiceTests
{



    [SetUp]
    public void Setup()
    {

    }
    //TODO: Camino Feliz: Login_ValidCredentials_ReturnsTokenDto: Verificar que al introducir email y contraseña correctos, el mock del repositorio es consultado y el servicio devuelve un objeto con el JWT y su fecha de expiración

    // TODO:Camino Triste:
    // Login_InvalidPassword_ReturnsNull: Simular un usuario existente pero con contraseña incorrecta (usando BCrypt.Verify fallido) y asegurar que el servicio no devuelve un token
    // .
    // Login_NonExistentUser_ReturnsNull: Probar el escenario donde el repositorio devuelve null al buscar el email
    // .
    // Register_DuplicateEmail_ReturnsNullOrThrows: Verificar que el sistema impide registros duplicados lanzando un conflicto o excepción
    [Test]
    public void GenerateNewUser_WithMailPassOK_ReturnsResponseDto()
    {
        var email = "user@gmail.com";
        var pass = "Password123";
    }
}
