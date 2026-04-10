using System;

namespace GestorTareas.Interfaces;

public interface ITimed
{
    TimeSpan StartTimer();
    TimeSpan StopTimer();
}
