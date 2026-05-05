using System;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Interfaces;
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
}
