using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Moq;
using NUnit.Framework;
using User = GestorTareas.Models.User;

namespace GestorTareas.Tests;

[TestFixture]
public class UserManagerServiceTests
{
    private Mock<IUserRepository> _mockRepository;
    private UserManagerService _userManagerService;

    private readonly List<User> _usersList = new()
    {
        new User{UserName = "ADMIN",UserLastName="AdminSystem",UserEmail="admin@hotmail.com",IsActive=true,IsAdmin=true,Id=1},
        new User{UserName = "user1",UserLastName="lastname2",UserEmail="user1@hotmail.com",IsActive=true,IsAdmin=false,Id=2},
        new User{UserName = "user2",UserLastName="lastname3",UserEmail="user2@hotmail.com",IsActive=true,IsAdmin=false,Id=3},
        new User{UserName = "user3",UserLastName="lastname4",UserEmail="us@hotmail.com",IsActive=true,IsAdmin=false,Id=4}
    };
    private List<User> _userListToTest = [];

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userManagerService = new UserManagerService(_mockRepository.Object);
        _userListToTest = [.. _usersList];
    }

    [Test]
    public void AddUser_MailPassNotOK_AddTheUser()
    {
        var activeUserIsAdmin = ActiveUser(_userListToTest[0]);
        var userDto = NewUserDto(_userListToTest[1]);
        userDto.UserEmail="mailMalo";
        
         _mockRepository
            .Setup(r => r.GetUserById(activeUserIsAdmin.Id))
            .Returns(activeUserIsAdmin);

        // Act
        Assert.Throws<ArgumentException>(()=>
        _userManagerService.AddUser(userDto,activeUserIsAdmin.Id));

        _mockRepository.Verify(r => r.AddUser(It.IsAny<User>()),Times.Never);
        
    }

    [Test]
    public void AddUser_WhenActiveUserIsAdmin_AddsUserWithDtoValues()
    {
        // Arrange
        var activeUserIsAdmin = ActiveUser(_usersList[0]);
        var userDto = NewUserDto(_userListToTest[1]);

        _mockRepository
            .Setup(r => r.GetUserById(activeUserIsAdmin.Id))
            .Returns(activeUserIsAdmin);

        // Act
        var result = _userManagerService.AddUser(userDto, activeUserIsAdmin.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.UserName, Is.EqualTo(userDto.UserName));
            Assert.That(result.UserLastName, Is.EqualTo(userDto.UserLastName));
            Assert.That(result.UserEmail, Is.EqualTo(userDto.UserEmail));
            Assert.That(result.IsActive, Is.True);
            Assert.That(result.IsAdmin, Is.False);
        });

        _mockRepository.Verify(r => r.AddUser(It.Is<User>(u =>
            u.UserName == userDto.UserName &&
            u.UserLastName == userDto.UserLastName &&
            u.UserEmail == userDto.UserEmail &&
            u.IsActive == userDto.IsActive &&
            u.IsAdmin == userDto.IsAdmin
        )), Times.Once);
    }

    [Test]
    public void AddUser_WhenActiveUserIsNotAdmin_ForcesNewUserAsNotAdmin()
    {
        // Arrange
        var activeUser = ActiveUser(_userListToTest[3]);
        var userDto = NewUserDto(_userListToTest[2]);

        _mockRepository
            .Setup(r => r.GetUserById(activeUser.Id))
            .Returns(activeUser);

        // Act
        var result = _userManagerService.AddUser(userDto, activeUser.Id);

        // Assert
        Assert.That(result.IsAdmin, Is.False);

        _mockRepository.Verify(r => r.AddUser(It.Is<User>(u =>
            u.UserName == userDto.UserName &&
            u.UserEmail == userDto.UserEmail &&
            u.IsAdmin == false
        )), Times.Once);
    }

    [Test]
    public void AddUser_WhenActiveUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userDto = NewUserDto(_userListToTest[2]);

        _mockRepository
            .Setup(r => r.GetUserById(666))
            .Returns((User?)null);

        // Act + Assert
        Assert.Throws<KeyNotFoundException>(() =>
            _userManagerService.AddUser(userDto, 666));

        _mockRepository.Verify(r =>
            r.AddUser(It.IsAny<User>()),
            Times.Never);
    }

    [Test]
    public void GetAllUsers_WhenUsersExist_ReturnsUserDtos()
    {
        // Arrange
        var users = new List<User>
        {
            ActiveUser(_userListToTest[0]),
            ActiveUser(_userListToTest[3])
        };

        _mockRepository
            .Setup(r => r.GetAllUsers())
            .Returns(users);

        // Act
        var result = _userManagerService.GetAllUsers();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].UserName, Is.EqualTo("ADMIN"));
            Assert.That(result[1].UserEmail, Is.EqualTo("us@hotmail.com"));
        });
    }

    [Test]
    public void GetUserById_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var user = ActiveUser(_userListToTest[1]);

        _mockRepository
            .Setup(r => r.GetUserById(user.Id))
            .Returns(user);

        // Act
        var result = _userManagerService.GetUserById(user.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.UserName, Is.EqualTo("user1"));
            Assert.That(result.UserEmail, Is.EqualTo("user1@hotmail.com"));
        });
    }

    [Test]
    public void GetUserById_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetUserById(999))
            .Returns((User?)null);

        // Act + Assert
        Assert.Throws<KeyNotFoundException>(() =>
            _userManagerService.GetUserById(999));
    }

    [Test]
    public void UpdateUser_WhenUserExists_UpdatesUser()
    {
        // Arrange
        var user = ActiveUser(_userListToTest[2]);

        _mockRepository
            .Setup(r => r.GetUserById(user.Id))
            .Returns(user);

        // Act
        _userManagerService.UpdateUser(
            user.Id,
            "Nombre actualizado",
            "Apellido actualizado",
            "actualizado@test.com"
        );

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(user.UserName, Is.EqualTo("Nombre actualizado"));
            Assert.That(user.UserLastName, Is.EqualTo("Apellido actualizado"));
            Assert.That(user.UserEmail, Is.EqualTo("actualizado@test.com"));
        });

        _mockRepository.Verify(r =>
            r.UpdateUser(user),
            Times.Once);
    }

    [Test]
    public void DeleteUser_WhenUserExists_DeletesUser()
    {
        // Arrange
        var user = ActiveUser(_userListToTest[3]);

        _mockRepository
            .Setup(r => r.GetUserById(user.Id))
            .Returns(user);

        // Act
        _userManagerService.DeleteUser(user.Id);

        // Assert
        _mockRepository.Verify(r =>
            r.DeleteUser(user),
            Times.Once);
    }

    // -------------------------
    // Helpers
    // -------------------------

    private static User ActiveUser(User user)
    {
        return new User
        {
            Id = user.Id,
            UserName = user.UserName,
            UserLastName = user.UserLastName,
            UserEmail = user.UserEmail,
            IsActive = user.IsActive,
            IsAdmin = user.IsAdmin,
        };
    }

    private static CreateUserDto NewUserDto(User user)
    {
        return new CreateUserDto
        {
            UserName = user.UserName,
            UserLastName = user.UserLastName,
            UserEmail = user.UserEmail,
            IsActive = (bool)user.IsActive,
            IsAdmin = (bool)user.IsAdmin,
            UserActiveId = user.Id
        };
    }
}