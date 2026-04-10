using System;

namespace GestorTareas.Interfaces;

public interface IRecurring
{
     Task GenerateNewInstance();

     bool IsRecurrenceActive();
}
ç