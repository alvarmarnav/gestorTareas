using System;
using User = GestorTareas.Models.User;

namespace GestorTareas.Interfaces;

public interface IUserRepository
{
    public List<User> GetAllUsers();
    public User? GetUserById(int id);
    public void AddUser(User user);
    public void UpdateUser(User user);
    public void DeleteUser(User user);
    public User? GetUserByEmail(string userEmail);
}
