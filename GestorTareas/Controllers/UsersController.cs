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
   public IActionResult Update(int id)
   {
      throw new NotImplementedException();
   }
   [HttpDelete("{id}")]
   public IActionResult Delete(int id)
   {
      throw new NotImplementedException();
   }
}
