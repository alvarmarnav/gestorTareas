using GestorTareas.Application.Services;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Moq;
using NUnit.Framework;
using Task = GestorTareas.Models.Task;

namespace GestorTareas.Tests;

[TestFixture]
public class TaskManagerServiceTests
{
    private Mock<ITaskRepository> _mockRepository;
    private TaskManagerService _taskService;

    // private static IEnumerable<TestCaseData> TestCaseDatas()
    // {
    //     yield return new TestCaseData("Titulo 1", null, null, null, null)
    //     .SetName("Constructor HappyWay sólo título");

    //     yield return new TestCaseData("Titulo 2", "Description", TaskPriority.High, TaskStatus.InProgress, DateTime.Now.AddDays(10))
    //     .SetName("Constructor HappyWay todos params.");
    // }
    private readonly List<Models.Task> _taskListBase = new(){
        new SimpleTask{Id=1,Title="Titulo prueba 1", UserId=2},
        new SimpleTask{Id=2,Title="Titulo prueba 2",UserId=2},
        new SimpleTask{Id=3,Title="Titulo prueba 3",UserId=1},
        // new SimpleTask{Title="",UserId=1}
        };
    private List<Models.Task> _taskToTest;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskRepository>();
        _taskService = new TaskManagerService(_mockRepository.Object);
        _taskToTest = new List<Models.Task>(_taskListBase);
    }
    [Test]
    public void AddTask_PassTheCorrectValueToRepo()
    {

        // _mockRepository.Setup(r => r.AddTask(_taskToTest.Find(t => t.Id == 1)));

        var result = _taskService.AddTask(_taskToTest[0].Title, _taskToTest[0].UserId, _taskToTest[0].TaskDescription, null, null, null, null, null, null, null, null);

        _mockRepository.Verify(r => r.AddTask(It.IsAny<Task>()), Times.Once());
        _mockRepository.Verify(r => r.AddTask(It.Is<Task>(t => t.Title == result.Title && t.UserId == result.UserId)));
    }
    [Test]
    public void AddTaskWithTitleEmpty_MustThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _taskService.AddTask("", 2, null, null, null, null, null, null, null, null, null));
    }
    [Test]
    public void AddTaskWithPastDateTime_MustThrowInvalidDate()
    {

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
        Assert.That(result[0].Title, Is.EqualTo("Titulo prueba 1"));
    }
    [Test]
    public void GetTaskById_ReturnsSelectedIdTasks_IfExists()
    {
        int taskId = 2;
        var selectedTask = _taskListBase.First(t => t.Id == taskId);

        // _taskToTest = new List<Models.Task>(_taskListBase);
        _mockRepository.Setup(r => r.GetTaskById(taskId))
        .Returns(selectedTask);

        var result = _taskService.GetTaskById(taskId);
        Assert.That(result.Title, Is.EqualTo("Titulo prueba 2"));
        Assert.That(result.Id, Is.EqualTo(2));

    }
    //TODO: COMPROBAR SI AQUÍ DEBE MANDAR UNA EXCEPCION O COMO DEBERÍA MANEJARLO
    [Test]
    public void GetTaskById_ReturnsNullIfNotExists()
    {
        var notExistsTaskId = 4;
        Task? resultNull = _taskToTest.FirstOrDefault(t => t.Id == notExistsTaskId);
        _mockRepository.Setup(r => r.GetTaskById(notExistsTaskId))
        .Returns(resultNull);

        var result = _taskService.GetTaskById(notExistsTaskId);

        Assert.That(result, Is.Null);
    }
    [Test]
    public void GetAllTasksByUserId_ReturnsAlLUserTasks_WhenHeHasTasks()
    {
        // var userTasks = _taskToTest
        // .Where(t => t.UserId == 2)
        // .ToList();
        var userId = 2;
        var userTasks = new List<Task>
        {
            new SimpleTask{Id=1,Title="Aviones de papel",UserId=1},
            new SimpleTask{Id=2,Title="Cambiar Aceite Coche",UserId=2},
            new SimpleTask{Id=3,Title="Limpiar",UserId=1},
            new SimpleTask{Id=4,Title="Cambiar Rueda",UserId=2},
            new SimpleTask{Id=5,Title="Pasear huron",UserId=1},
            new SimpleTask{Id=6,Title="Comprar lentejas",UserId=3}
        };
        var expectedTasks = (List<Task>)userTasks.Where(t => t.UserId == userId).ToList();

        _mockRepository.Setup(r => r.GetAllTasksByUser(userId))
        .Returns(expectedTasks);

        var result = _taskService.GetAllTasksByUser(userId);

        _mockRepository.Verify(r => r.GetAllTasksByUser(userId), Times.Once());

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result,Has.Some.Matches<Task>(t => t.Title == "Cambiar Rueda"));
        Assert.That(result.All(t => t.UserId == 2), Is.True);
    }
    [Test]
    public void DeleteTask_DeleteOnlyTheSelectedTask()
    {
        var taskIdToDelete = 2;
        var taskToDelete = _taskToTest.FirstOrDefault(t => t.Id == taskIdToDelete);

        var numberOfTasksBeforeDelete = _taskToTest.Count;


        _mockRepository.Setup(r => r.GetTaskById(taskIdToDelete))
        .Returns(taskToDelete);

        _mockRepository.Setup(r => r.DeleteTask(It.IsAny<Models.Task>()))
                   .Callback<Models.Task>(t => _taskToTest.Remove(t));

        // _mockRepository.Setup(r => r.DeleteTask(taskToDelete)).Verifiable();

        _taskService.DeleteTask(taskIdToDelete);

        _mockRepository.Verify(r => r.DeleteTask(taskToDelete), Times.Once());

        Assert.That(_taskToTest.Count, Is.EqualTo(numberOfTasksBeforeDelete - 1));
        Assert.IsFalse(_taskToTest.Any(t => t.Id == taskIdToDelete));
    }
    [Test]
    public void DeleteTask_ThrowsNotFoundException_IfTaskIdNotExists()
    {
        var taskIdNotExist = 200;
        // var taskToDelete = _taskToTest.Fir

        _mockRepository.Setup(r => r.GetTaskById(taskIdNotExist))
        .Returns((Task?)null);


        Assert.Throws<KeyNotFoundException>(() => _taskService.DeleteTask(taskIdNotExist));
        _mockRepository.Verify(r => r.DeleteTask(It.IsAny<Task>()), Times.Never);
    }
    [Test]
    public void UpdateTask_UpdatesOnlySelectedTask_UpdatesEachPropertyOfTaskType()
    {
        var selectedTaskId = 2;
        var selectedTask = _taskToTest.FirstOrDefault(t => t.Id == selectedTaskId);

        var newTaskDto = new Application.DTOs.UpdateTaskDto("New Title", "NEw taskdescription", Enums.TaskPriority.Critical, Enums.TaskStatus.InProgress, DateTime.Now.AddDays(30), null, null, null);
        // {
        //     Title = "New Title",
        //     TaskDescription = "NEw taskdescription",
        // };

        _mockRepository.Setup(r => r.GetTaskById(selectedTaskId))
        .Returns(selectedTask);

        _mockRepository.Setup(r => r.UpdateTask(selectedTask))
        .Callback<Task>(t => selectedTask.Title = t.Title);

        _taskService.UpdateTask(selectedTaskId, newTaskDto);

        Assert.That(selectedTask.Title, Is.EqualTo("New Title"));
        _mockRepository.Verify(r => r.UpdateTask(It.IsAny<Task>()), Times.Once);

    }
}
