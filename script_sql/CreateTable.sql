CREATE DATABASE CarRentalManagementSystem;
GO

USE CarRentalManagementSystem;
GO

-- Bảng Staff
CREATE TABLE Staff (
    StaffId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(50) NOT NULL UNIQUE,
    StaffName NVARCHAR(255) NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Salary DECIMAL(10, 2),
    isDeleted BIT,
);
GO
--Bảng StaffProfit
CREATE TABLE StaffProfit (
    StaffId INT,
    StaffName NVARCHAR(255) NOT NULL,
    Salary DECIMAL(18, 2) NOT NULL,
    RentalCount INT NOT NULL,
    Commission DECIMAL(18, 2) NOT NULL,
    TotalSalary DECIMAL(18, 2)
	FOREIGN KEY (StaffId) REFERENCES Staff(StaffId),
);
GO
-- Bảng RankLevelCustomer
CREATE TABLE RankLevelCustomer (
    RankLevelId INT PRIMARY KEY IDENTITY(1,1),
	RankLevelName NVARCHAR(50),
	Discount INT
);
GO
-- Bảng Customer
CREATE TABLE Customer (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    CustomerName NVARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
	Point INT,
	RankLevel INT,
	FOREIGN KEY (RankLevel) REFERENCES RankLevelCustomer(RankLevelId),
    isDeleted BIT
);
GO

-- Bảng CarStatus
CREATE TABLE CarStatus (
    CarStatusId INT PRIMARY KEY IDENTITY(1,1),
    CarStatusName NVARCHAR(50) NOT NULL
);
GO

-- Bảng Car
CREATE TABLE Car (
    LicensePlates NVARCHAR(50) PRIMARY KEY,
    CarName NVARCHAR(80) NOT NULL,
    [Type] NVARCHAR(50) NOT NULL,
    DateCar DATE,
    Color NVARCHAR(50) NOT NULL,
    Brand NVARCHAR(50) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    NumberOfSeats INT NOT NULL,
    CarStatusId INT,
    Fuel NVARCHAR(50) NOT NULL,
    RentalPrice DECIMAL(10, 2) NOT NULL,
    isDeleted BIT,
    FOREIGN KEY (CarStatusId) REFERENCES CarStatus(CarStatusId)
);
GO

-- Bảng CarRental
CREATE TABLE CarRental (
    RentalId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    LicensePlates NVARCHAR(50),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    StaffId INT NOT NULL,
	Total Decimal NOT NULL,
    isDeleted BIT,
	FOREIGN KEY (StaffId) REFERENCES Staff(StaffId),
    FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId),
    FOREIGN KEY (LicensePlates) REFERENCES Car(LicensePlates)
);
GO

-- Bảng HistoryCarRental
CREATE TABLE HistoryCarRental (
    HistoryCarRentalId INT PRIMARY KEY IDENTITY(1,1),
    RentalId INT,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    ActualReturnTime DATETIME2 NOT NULL,
    TotalPrice DECIMAL(10, 2) NOT NULL,
	FOREIGN KEY (RentalId) REFERENCES CarRental(RentalId)
);
GO
