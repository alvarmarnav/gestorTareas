using System;
using System.Reflection.Metadata;

namespace GestorTareas.Interfaces;

public interface IPostponeable
{
    bool Postpone(TimeSpan timeAdded);
}
