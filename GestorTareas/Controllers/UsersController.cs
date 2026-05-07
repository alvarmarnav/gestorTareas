using System;
using Microsoft.AspNetCore.Mvc;
using GestorTareas.Application.Services;
using GestorTareas.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using User = GestorTareas.Models.User;

namespace GestorTareas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
   private readonly UserManagerService _userManagerService;

   public UsersController(UserManagerService userManagerService) => _userManagerService = userManagerService;

   [HttpGet]
   public IActionResult GetAll()
   {
      return Ok(_userManagerService.GetAllUsers());
   }

   [HttpGet("{id}")]
   public IActionResult GetById(int id)
   {
      return Ok(_userManagerService.GetUserById(id));
   }
   [HttpPost]
   public IActionResult Create([FromBody] CreateUserDto userDto)
   {
      var newUser = _userManagerService.AddUser(
          userDto.UserName,
          userDto.UserLastName,
          userDto.UserEmail,
          userDto.IsActive,
          userDto.IsAdmin
      );

      return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);

   }
   [HttpPut("{id}")]
   public IActionResult Update(int id, [FromBody] UpdateUserDto userDto )
   {
      if(_userManagerService.GetUserById(id) is null)
         return NotFound();

      _userManagerService.UpdateUser(
         id,
         userDto.UserName,
         userDto.UserLastName,
         userDto.UserEmail
      );

      return NoContent();
   }
   [HttpDelete("{id}")]
   public IActionResult Delete(int id)
   {
      try{
      _userManagerService.DeleteUser(id);
      return NoContent();
      }catch(Exception ex)
      {
         return Problem($"Error: {ex.Message}");
      }
   }
}
