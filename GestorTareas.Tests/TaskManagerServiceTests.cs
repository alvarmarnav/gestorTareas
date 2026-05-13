using GestorTareas.Application.Services;
using GestorTareas.Controllers;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using NUnit.Framework;
using Task = GestorTareas.Models.Task;

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
        _taskToTest = new List<Models.Task>(_taskListBase);
    }

    [Test]
    public void GetAllTasks_ReturnsAll_WhenThereAreTasks()
    {
        //Patron Arrange-Act-Assert
        // _taskToTest = new List<Models.Task>(_taskListBase);
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
        var selectedTask = _taskListBase.First(t => t.Id == taskId);

        // _taskToTest = new List<Models.Task>(_taskListBase);
        _mockRepository.Setup(r => r.GetTaskById(It.IsAny<int>()))
        .Returns(selectedTask);

        var result = _taskService.GetTaskById(taskId);
        Assert.That(result.Title == "Titulo prueba 2");
        Assert.That(result.Id == 2);

    }
    [Test]
    public void GetTaskById_ReturnsNullIfNotExists()
    {
        var notExistsTaskId = 4;
        Task? resultNull =_taskToTest.FirstOrDefault(t => t.Id == notExistsTaskId);
        _mockRepository.Setup(r => r.GetTaskById(notExistsTaskId))
        .Returns(resultNull);

        var result = _taskService.GetTaskById(notExistsTaskId);

        Assert.That(result is null);
    }
    [Test]
    public void GetAllTasksByUserId_ReturnsAlLUserTasks_WhenHeHasTasks()
    {
        // _taskToTest = new List<Models.Task>(_taskListBase);
        var userTasks = _taskToTest
        .Where(t => t.UserId == 2)
        .ToList();
        _mockRepository.Setup(r => r.GetAllTasksByUser(2))
        .Returns(userTasks);

        var result = _taskService.GetAllTasksByUser(2);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[1].Title == "Titulo prueba 2");
    }
    [Test]
    public void DeleteTask_DeleteOnlyTheSelectedTask()
    {
        var taskIdToDelete = 2;
        var taskToDelete = _taskToTest.FirstOrDefault(t=>t.Id == taskIdToDelete);
        // _mockRepository.Setup(r => r.DeleteTask(taskToDelete));
        var numberOfTasksBeforeDelete = _taskToTest.Count();

        // _taskToTest.Remove(taskToDelete);
        _mockRepository.Setup(r => r.GetTaskById(taskIdToDelete))
        .Returns(taskToDelete);

        _mockRepository.Setup(r => r.DeleteTask(It.IsAny<Models.Task>()))
                   .Callback<Models.Task>(t => _taskToTest.Remove(t));

        // _mockRepository.Setup(r => r.DeleteTask(taskToDelete)).Verifiable();

       _taskService.DeleteTask(taskIdToDelete);

        _mockRepository.Verify(r => r.DeleteTask(taskToDelete), Times.Once());
        var ntaskAfter =_taskToTest.Count();

        Assert.That(_taskToTest.Count() , Is.EqualTo(numberOfTasksBeforeDelete - 1));
        Assert.IsFalse(_taskToTest.Any(t => t.Id == taskIdToDelete));
    }
    [Test]
    public void DeleteTask_ThrowsNotFoundException_IfTaskIdNotExists()
    {
        var taskIdNotExist = 200;
        // var taskToDelete = _taskToTest.Fir

        _mockRepository.Setup(r => r.GetTaskById(taskIdNotExist))
        .Returns((Task)null);

        Assert.Throws<KeyNotFoundException>(() => _taskService.DeleteTask(taskIdNotExist));
    }
    [Test]
    public void UpdateTask_UpdatesOnlySelectedTask_UpdatesEachPropertyOfTaskType()
    {
        // var selectedTaskId = 2;

    }
}
