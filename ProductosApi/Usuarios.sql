USE ProductosDB;
GO

IF OBJECT_ID('Usuarios', 'U') IS NOT NULL DROP TABLE Usuarios;
GO

CREATE TABLE Usuarios (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Correo    NVARCHAR(100) NOT NULL,
    ClaveHash NVARCHAR(100) NOT NULL,
    Rol       NVARCHAR(20)  NOT NULL,
    CONSTRAINT UQ_Usuarios_Correo UNIQUE (Correo)
);
GO

INSERT INTO Usuarios (Correo, ClaveHash, Rol)
VALUES ('admin@productos.com', '$2a$11$YZ7MxkTbnkbYQyCm16jPZOPqzU1/pw3BaElZ1FVsae1DOnuVjPB0q', 'Admin');
GO

SELECT Id, Correo, Rol FROM Usuarios;
