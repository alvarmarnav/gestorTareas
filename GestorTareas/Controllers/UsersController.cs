using System;
using Microsoft.AspNetCore.Mvc;
using GestorTareas.Application.Services;
using GestorTareas.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using User = GestorTareas.Models.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GestorTareas.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
   private readonly UserManagerService _userManagerService;

   public UsersController(UserManagerService userManagerService) => _userManagerService = userManagerService;

   /// <summary>
   /// Obtiene todos los usuarios del sistema
   /// </summary>
   /// <returns></returns>
   [HttpGet]
   public IActionResult GetAll()
   {
      return Ok(_userManagerService.GetAllUsers());
   }
   /// <summary>
   /// Obtiene el usuario por su Id si existe
   /// </summary>
   /// <param name="id"></param>
   /// <returns></returns>
   [HttpGet("{id}")]
   public IActionResult GetById(int id)
   {
      return Ok(_userManagerService.GetUserById(id));
   }
   /// <summary>
   /// Crea  usuarios
   /// </summary>
   /// <param name="userDto"></param>
   /// <returns></returns>
   [HttpPost]
   public IActionResult Create([FromBody] CreateUserDto userDto)
   {
      var userActiveStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (userActiveStr is null) return Unauthorized();
      var userActiveId = int.Parse(userActiveStr);

      var newUser = _userManagerService.AddUser(
          userDto.UserName,
          userDto.UserLastName,
          userDto.UserEmail,
          userDto.IsActive,
          userDto.IsAdmin,//TODO:ATENTO A ESTE PUNTO, SI DEBE SER ACCESIBLE O LIMITAR
         userDto.UserActiveId
      );

      return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);

   }
   /// <summary>
   /// Actualiza el usuario a través de su Id
   /// </summary>
   /// <param name="id"></param>
   /// <param name="userDto"></param>
   /// <returns></returns>
   [HttpPut("{id}")]
   public IActionResult Update(int id, [FromBody] UpdateUserDto userDto)
   {
      if (_userManagerService.GetUserById(id) is null)
         return NotFound();

      _userManagerService.UpdateUser(
         id,
         userDto.UserName,
         userDto.UserLastName,
         userDto.UserEmail
      );

      return NoContent();
   }
   /// <summary>
   /// Elimina el usuario seleccionado por Id
   /// </summary>
   /// <param name="id"></param>
   /// <returns></returns>
   [HttpDelete("{id}")]
   public IActionResult Delete(int id)
   {
      try
      {
         _userManagerService.DeleteUser(id);
         return NoContent();
      }
      catch (Exception ex)
      {
         return Problem($"Error: {ex.Message}");
      }
   }
   [Authorize]
   [HttpGet("ownUser")]
   public IActionResult GetOwnUserInfo()
   {
      var userActiveStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (userActiveStr is null) return Unauthorized();
      var userActiveId = int.Parse(userActiveStr);
      return Ok(_userManagerService.GetUserById(userActiveId));
   }
}
