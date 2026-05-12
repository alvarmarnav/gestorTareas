using GestorTareas.Application.Services;
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
        _mockRepository.Setup(r => r.GetAllTasks())
        .Returns(new List<Models.Task>
        {
            new SimpleTask{Id=1,Title="Titulo prueba 1", UserId=2},
            new SimpleTask{Id=2,Title="Titulo prueba 2",UserId=2}
        });

        var result = _taskService.GetAllTasks();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Title=="Titulo prueba 1");
    }

    [Test]
    public void GetAllTasksById_ReturnsAlLUserTasks_WhenHeHasTasks()
    {
        // _mockRepository.Setup();
    }
}
