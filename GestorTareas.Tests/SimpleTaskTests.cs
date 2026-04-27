using GestorTareas.Models;
using NUnit.Framework;

namespace GestorTareas.Tests;

[TestFixture]
public class SimpleTaskTests
{
    private static IEnumerable<TestCaseData> TestCaseDatas()
    {
        yield return new TestCaseData("Titulo 1", null, null, null, null)
        .SetName("Constructor HappyWay sólo título");

        yield return new TestCaseData("Titulo 2", "Description", TaskPriority.High, Models.Task.TaskStatus.InProgress, DateTime.Now.AddDays(10))
        .SetName("Constructor HappyWay todos params.");
    }

    [SetUp]
    public void Init()
    {
        //ARRANGE
        // _taskSimple = new SimpleTask("Test SimpleTask1");

    }

    [TearDown]
    public void Cleanup()
    {
        Console.WriteLine("TearDown Method Called after each test.");
    }


    [Test, TestCaseSource(nameof(TestCaseDatas))]
    public void Constructor_ValidParameters_MustCreateNewTask(
        string title,
        string? description = null,
        GestorTareas.Models.Task.TaskPriority? priority = TaskPriority.Normal,
        Models.Task.TaskStatus? status = Models.Task.TaskStatus.Pending,
        DateTime? dueTime = null)
    {
        //ACT
        var testTask = new SimpleTask(
            title,
            description,
            priority,
            status,
            dueTime);

        //AsseRT
        Assert.Multiple(
            () =>
            {
                Assert.That(testTask.Title, Is.EqualTo(title));
                Assert.That(testTask.DueTime, Is.EqualTo(dueTime));
                Assert.That(testTask.Description, Is.EqualTo(description));
                Assert.That(testTask.Priority, Is.EqualTo(priority));
                Assert.That(testTask.Status, Is.EqualTo(status));

            }
        );
    }
}
