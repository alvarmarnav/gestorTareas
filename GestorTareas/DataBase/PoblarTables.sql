USE GestorTareas;
-- Insert USER
INSERT INTO Users (UserName,UserLastName,Email, IsActive, IsAdmin)
VALUES ('Jordi','Puyol' ,'jordi@generalitat.cat', 1,0);
GO
-- Insertar una TareaSimple
INSERT INTO SimpleTasks(Title, TaskDescription, DueTime)
VALUES ('Bajar la basuras', 'Bajars a la calle a ultima hora.','2026-06-30');
GO
-- -- Insertar una TareaSimple
-- INSERT INTO COMPOSITETASKS (Title, Description, Priority,DueTime)
-- VALUES ('Bajar la basura', 'Bajar a la calle a ultima hora.', 'Normal','2026-06-30');
GO
-- Insertar una TareaSimple
INSERT INTO SubTask(Title, TaskDescription, Priority,DueTime,CompositeTaskType)
VALUES ('Bajar la basura', 'Bajar a la calle a ultima hora.', 'Normal','2026-06-30','SubTask');
GO
-- Insertar una TareaSimple
INSERT INTO LinkedTasks(Title, TaskDescription, Priority,DueTime,taskOrder,CompositeTaskType)
VALUES ('Bajar la basura', 'Bajar a la calle a ultima hora.', 'Normal','2026-06-30',1,'LinkedTask');
GO
-- Insertar una TareaSimple
INSERT INTO RecurringTasks(Title, TaskDescription, Priority,DueTime, RecurrenceRule,RecurringTasksCount)
VALUES ('Bajar la basura', 'Bajar a la calle a ultima hora.', 'Normal','2026-06-30',2,0);
GO
