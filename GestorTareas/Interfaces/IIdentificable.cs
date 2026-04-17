using System;

namespace GestorTareas.Interfaces;

public interface IIdentificable
{
    public Guid Id{get;}
    public string Title {get;}
}
