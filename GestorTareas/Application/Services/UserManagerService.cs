using System;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using User = GestorTareas.Models.User;
using GestorTareas.Application.DTOs;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace GestorTareas.Application.Services;

public class UserManagerService
{
    private readonly IUserRepository _userRepository;
    public UserManagerService(IUserRepository userRepository) => _userRepository = userRepository;
    public List<User> GetAllUsers()
    {
        return (List<User>)_userRepository.GetAllUsers()
        .Select(u => new UserResponseDto
        {
            Id = u.Id,
            UserName = u.UserName,
            UserLastName = u.UserLastName,
            UserEmail = u.UserEmail,
            IsActive = (bool)u.IsActive,
            IsAdmin = (bool)u.IsAdmin
        });
    }
    public User AddUser(
        string userName,
        string userLastName,
        string userEmail,
        bool? isActive,
        bool? isAdmin,
        int userActiveId)
    {
        //TODO:AÑADIDO PARA QUE NO PUEDAN CREARSE USUARIOS ADMIN, SOLO EL ADMIN
        var userActive = _userRepository.GetUserById(userActiveId) ?? throw new KeyNotFoundException($"No existe usuario con el ID: {userActiveId}");
        if (!(bool)userActive.IsAdmin)
            isAdmin = false;

        var newUser = new User
        {
            UserName = userName,
            UserLastName = userLastName,
            UserEmail = userEmail,
            IsActive = isActive,
            IsAdmin = isAdmin ?? false
        };
        _userRepository.AddUser(newUser);
        return newUser;
    }

    public UserResponseDto? GetUserById(int id)
    {
        var userSelected = _userRepository.GetUserById(id) ?? throw new KeyNotFoundException($"No se ha encontrado el usuario con Id{id}");

        return new UserResponseDto
        {
            Id = userSelected.Id,
            UserName = userSelected.UserName,
            UserLastName = userSelected.UserLastName,
            UserEmail = userSelected.UserEmail,
            IsActive = (bool)userSelected.IsActive,
            IsAdmin = (bool)userSelected.IsAdmin
        };
    }

    public void UpdateUser(int id, string userName, string userLastName, string userEmail)
    {
        var selectedUser = _userRepository.GetUserById(id);
        if (selectedUser is null)
            throw new Exception("El usuario NO existe.");
        selectedUser.UserName = userName;
        selectedUser.UserLastName = userLastName;
        selectedUser.UserEmail = userEmail;

        _userRepository.UpdateUser(selectedUser);
    }
    public void DeleteUser(int id)
    {
        var selectedUser = _userRepository.GetUserById(id);
        if (selectedUser is null)
            throw new Exception($"No existe el usuario con ID: {id}");
        _userRepository.DeleteUser(selectedUser);
    }
}
