using System;
using GestorTareas.Models;
namespace GestorTareas.Interfaces;

public interface IRecurring
{
     void GenerateNewInstance();

     bool IsRecurrenceActive();
}
