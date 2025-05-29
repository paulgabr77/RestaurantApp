CREATE DATABASE RestaurantDB;
GO

USE RestaurantDB;
GO

CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

CREATE TABLE Allergens (
    AllergenId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

CREATE TABLE Dishes (
    DishId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(10,2) NOT NULL,
    PortionQuantity DECIMAL(10,2) NOT NULL,
    TotalQuantity DECIMAL(10,2) NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    IsAvailable BIT DEFAULT 1
);

CREATE TABLE DishAllergens (
    DishId INT FOREIGN KEY REFERENCES Dishes(DishId),
    AllergenId INT FOREIGN KEY REFERENCES Allergens(AllergenId),
    PRIMARY KEY (DishId, AllergenId)
);

CREATE TABLE Menus (
    MenuId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    DiscountPercentage DECIMAL(5,2) DEFAULT 0
);

CREATE TABLE MenuDishes (
    MenuId INT FOREIGN KEY REFERENCES Menus(MenuId),
    DishId INT FOREIGN KEY REFERENCES Dishes(DishId),
    Quantity DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (MenuId, DishId)
);

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    DeliveryAddress NVARCHAR(200),
    Password NVARCHAR(100) NOT NULL,
    IsEmployee BIT DEFAULT 0
);

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    OrderDate DATETIME DEFAULT GETDATE(),
    OrderCode NVARCHAR(20) UNIQUE NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    DeliveryFee DECIMAL(10,2) DEFAULT 0,
    DiscountAmount DECIMAL(10,2) DEFAULT 0,
    EstimatedDeliveryTime DATETIME
);

CREATE TABLE OrderDetails (
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    DishId INT FOREIGN KEY REFERENCES Dishes(DishId),
    MenuId INT FOREIGN KEY REFERENCES Menus(MenuId),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL
);

CREATE TABLE DishImages (
    ImageId INT IDENTITY(1,1) PRIMARY KEY,
    DishId INT FOREIGN KEY REFERENCES Dishes(DishId),
    ImagePath NVARCHAR(200) NOT NULL,
    IsMain BIT DEFAULT 0
); 