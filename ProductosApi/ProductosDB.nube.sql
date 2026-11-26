IF OBJECT_ID('Productos', 'U') IS NOT NULL DROP TABLE Productos;
IF OBJECT_ID('Categorias', 'U') IS NOT NULL DROP TABLE Categorias;
IF OBJECT_ID('Usuarios', 'U') IS NOT NULL DROP TABLE Usuarios;
GO

CREATE TABLE Categorias (
    Id     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Productos (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(100) NOT NULL,
    Precio      DECIMAL(10,2) NOT NULL,
    Stock       INT NOT NULL,
    CategoriaId INT NOT NULL,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias (Id)
);

CREATE TABLE Usuarios (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Correo    NVARCHAR(100) NOT NULL,
    ClaveHash NVARCHAR(100) NOT NULL,
    Rol       NVARCHAR(20)  NOT NULL,
    CONSTRAINT UQ_Usuarios_Correo UNIQUE (Correo)
);
GO

INSERT INTO Categorias (Nombre) VALUES ('Laptops'), ('Accesorios'), ('Monitores');

INSERT INTO Productos (Nombre, Precio, Stock, CategoriaId) VALUES
    ('Laptop Lenovo IdeaPad 15', 2599.90, 12, 1),
    ('Mouse Logitech M170',        49.90, 40, 2),
    ('Teclado Redragon Kumara',   189.00, 25, 2);

INSERT INTO Usuarios (Correo, ClaveHash, Rol)
VALUES ('admin@productos.com', '$2a$11$YZ7MxkTbnkbYQyCm16jPZOPqzU1/pw3BaElZ1FVsae1DOnuVjPB0q', 'Admin');
GO

SELECT * FROM Categorias;
SELECT * FROM Productos;
SELECT Id, Correo, Rol FROM Usuarios;
