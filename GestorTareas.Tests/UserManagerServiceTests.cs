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
        new User{UserName = "user1",UserLastName="lastname1",UserEmail="user1@hotmail.com"},
        new User{UserName = "user2",UserLastName="lastname2",UserEmail="user2@hotmail.com"},
        // new User{UserName = "user3",UserLastName="lastname3",UserEmail="user1@hotmail.com"},
        // new User{UserName = "user4",UserLastName="lastname4",UserEmail="ushotmail.com"}
    };
    private List<User> _userListToTest;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userManagerService = new UserManagerService(_mockRepository.Object);
        _userListToTest = new List<User>(_usersList);
    }

    [Test]
    public void AddUser_MailPassOK_AddTheUser()
    {
        var newUser = _userListToTest[0];

        // _mockRepository.Setup(r => r.AddUser(It.IsAny<User>()));
        _mockRepository.Setup(r => r.AddUser(newUser));

        var createdUser = _userManagerService.AddUser(newUser.UserName,newUser.UserLastName, newUser.UserEmail,true,false);

        _mockRepository.Verify(r => r.AddUser(newUser),Times.Once());
        Assert.That(createdUser.UserEmail, Is.EqualTo(newUser.UserEmail));
    }
}
