using NUnit.Framework;
using GestorTareas.Models;
using GestorTareas.Enums;

namespace GestorTareas.Tests
{
    [TestFixture]
    public class RecurringTaskTests
    {
        [Test]
        public void GenerateNewInstance_DebeSumarDiasCorrectamente()
        {
            // Arrange (Preparar)
            var fechaInicial = new DateTime(2026, 5, 5);
            int diasRecurrencia = 7;
            var tareaOriginal = new RecurringTask(
                title: "Reunión Semanal",
                userId: 1,
                dueTime: fechaInicial.AddDays(5),
                recurrenceRule: diasRecurrencia
            );

            // Act (Actuar)
            var nuevaInstancia = tareaOriginal.GenerateNewInstance(fechaInicial);

            // Assert (Afirmar)
            // La nueva fecha debe ser 01/01/2024 + 7 días = 08/01/2024
            Assert.That(nuevaInstancia.DueTime, Is.EqualTo(new DateTime(2026, 5, 12)));
            Assert.That(nuevaInstancia.Title, Is.EqualTo(tareaOriginal.Title));
        }
        //TODO:Camino Triste: Constructor_InvalidArgument_ThrowsArgumentException: Probar recurringtask validar que un valor de recurrencia ≤0 lanza  error 
        //Cancel_NonPendingTask_ThrowsInvalidOperationException: Probar la regla de negocio que impide cancelar tareas que ya están completadas o en curso
        [Test]
        public void GenerateNewInstance_DebeIncrementarContadorInstancias()
        {
            // Arrange
            var tarea = new RecurringTask("Test", 1, DateTime.Now, 1);

            // Act
            var instancia1 = tarea.GenerateNewInstance(DateTime.Now);
            var instancia2 = instancia1.GenerateNewInstance((DateTime)instancia1.DueTime);

            // Assert
            Assert.That(instancia1.RecurringTasksCount, Is.EqualTo(1));
            Assert.That(instancia2.RecurringTasksCount, Is.EqualTo(2));
        }

        [Test]
        public void GenerateNewInstance_CuandoSuperaLimite_LanzaExcepcion()
        {
            // Arrange
            // Creamos una tarea que ya está en el límite (15)
            var tareaLimite = new RecurringTask(
                title: "Limite",
                userId: 1,
                dueTime: DateTime.Now,
                recurrenceRule: 1
            // recurringTasksCount: 15
            );

            // Act & Assert
            // Verificamos que al intentar generar la 16 lance InvalidOperationException
            Assert.Throws<InvalidOperationException>(() =>
            {
                tareaLimite.GenerateNewInstance(DateTime.Now);
            });
        }
    }
}
