using System;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using User = GestorTareas.Models.User;

namespace GestorTareas.Application.Services;

public class UserManagerService
{
    private readonly IUserRepository _userRepository;
    public UserManagerService(IUserRepository userRepository) => _userRepository = userRepository;
    public List<User> GetAllUsers() => _userRepository.GetAllUsers();
    public User AddUser(
        string userName,
        string userLastName,
        string userEmail,
        bool? isActive,
        bool? isAdmin)
    {
        var newUser = new User
        {
            UserName = userName,
            UserLastName = userLastName,
            UserEmail = userEmail,
            IsActive = isActive,
            IsAdmin = isAdmin
        };
        _userRepository.AddUser(newUser);
        return newUser;
    }

    public User? GetUserById(int id)
    {
        return _userRepository.GetUserById(id);
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
            if(selectedUser is null)
                throw new Exception($"No existe el usuario con ID: {id}");
        _userRepository.DeleteUser(selectedUser);
    }
}
