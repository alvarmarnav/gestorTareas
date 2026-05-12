using GestorTareas.Application.Services;
using GestorTareas.Controllers;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Moq;
using NUnit.Framework;

namespace GestorTareas.Tests;

[TestFixture]
public class TaskManagerServiceTests
{
    private Mock<ITaskRepository> _mockRepository;
    private TaskManagerService _taskService;
    private readonly List<Models.Task> _taskListBase = new List<Models.Task>{
        new SimpleTask{Id=1,Title="Titulo prueba 1", UserId=2},
        new SimpleTask{Id=2,Title="Titulo prueba 2",UserId=2},
        new SimpleTask{Id=3,Title="Titulo prueba 3",UserId=1}
        };
    private List<Models.Task> _taskToTest = new();

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskRepository>();
        _taskService = new TaskManagerService(_mockRepository.Object);
    }

    [Test]
    public void GetAllTasks_ReturnsAll_WhenThereAreTasks()
    {
        //Patron Arrange-Act-Assert
        _taskToTest = new List<Models.Task>(_taskListBase);
        _mockRepository.Setup(r => r.GetAllTasks())
        .Returns(_taskToTest);

        var result = _taskService.GetAllTasks();

        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].Title == "Titulo prueba 1");
    }
    [Test]
    public void GetTaskById_ReturnsSelectedIdTasks_IfExists()
    {
        int taskId = 2;

        _taskToTest = new List<Models.Task>(_taskListBase);
        _mockRepository.Setup(r => r.GetTaskById((int)taskId))
        .Returns((int taskId) => _taskToTest.Where(t => t.Id == (int)taskId).ToList());

        var result = _taskService.GetTaskById(taskId);
        Assert.That(result.Title == "Titulo prueba 2");
        Assert.That(result.Id == 2);

    }
    [Test]
    public void GetAllTasksById_ReturnsAlLUserTasks_WhenHeHasTasks()
    {
        _taskToTest = new List<Models.Task>(_taskListBase);
        _mockRepository.Setup(r => r.GetAllTasksByUser(2))
        .Returns((int userId) => _taskToTest.Where(t => t.UserId == 2).ToList());

        var result = _taskService.GetAllTasksByUser(2);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[1].Title == "Titulo prueba 2");
    }
}
