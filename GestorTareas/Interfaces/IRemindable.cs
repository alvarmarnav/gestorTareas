using System;

namespace GestorTareas.Interfaces;

public interface IRemindable
{
    void ScheduleReminder(DateTime date);
}
