using System;
using System.Security.Claims;
using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using GestorTareas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareas.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfilesController : ControllerBase
{
    private readonly UserManagerService _userManagerService;
    public ProfilesController(UserManagerService userManagerService) => _userManagerService = userManagerService;

    [HttpGet("{userId}")]
    public IActionResult GetProfileDto()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString is null) return Unauthorized();
        var userId = int.Parse(userIdString);
        return Ok(_userManagerService.GetUserById(userId));
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateProfile([FromBody] UpdateUserDto userDto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString is null) return Unauthorized();
        var userId = int.Parse(userIdString);
        _userManagerService.UpdateUser(
            userId,
            userDto.UserName,
            userDto.UserLastName,
            userDto.UserEmail
        );
        return NoContent();
    }

}