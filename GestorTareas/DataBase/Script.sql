IF NOT EXISTS (
    SELECT name FROM sys.databases
    WHERE name = 'GestorTareas'
)
BEGIN
    CREATE DATABASE GestorTareas;
END
GO

USEL GestorTareas;
GO
