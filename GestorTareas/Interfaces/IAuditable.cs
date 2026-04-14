using System;

namespace GestorTareas.Interfaces;

public interface IAuditable
{
    public DateTime CreationDate();

    public string CreatorUser();
}
