using System;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;
using User = GestorTareas.Models.User;

namespace GestorTareas.Infraestructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GestorTareasContext _gestorTareasContext;
    public UserRepository(GestorTareasContext gestorTareasContext) => _gestorTareasContext = gestorTareasContext;
    public List<User> GetAllUsers()
    {
        return _gestorTareasContext.Users.ToList();
    }
    User? IUserRepository.GetUserById(Guid id)
    {
        return _gestorTareasContext.Users.Include(u => u.Id.Equals(id)).FirstOrDefault();
    }
    void IUserRepository.AddUser(User user)
    {
        _gestorTareasContext.Add(user);
        _gestorTareasContext.SaveChanges();
    }
    void IUserRepository.UpdateUser(User user)
    {
        _gestorTareasContext.Users.Update(user);
    }
    void IUserRepository.DeleteUser(User user)
    {
        _gestorTareasContext.Remove(user);
        _gestorTareasContext.SaveChanges();
    }

}