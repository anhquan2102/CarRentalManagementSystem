USE CarRentalManagementSystem;
GO

-- Insert data into the Staff table
INSERT INTO Staff (Email, StaffName, [Password], Address, PhoneNumber, Salary, isDeleted)
VALUES
    ('staff1@example.com', 'John Doe', 'password123', '123 Main St, Anytown USA', '123-456-7890', 50000.00, 0),
    ('staff2@example.com', 'Jane Smith', 'password456', '456 Oak Rd, Othertown USA', '987-654-3210', 55000.00, 0);

-- Insert data into the RankLevelCustomer table
INSERT INTO RankLevelCustomer (RankLevelName, Discount)
VALUES
    ('Bronze', 5),
    ('Silver', 10),
    ('Gold', 15),
    ('Platinum', 20);

-- Insert data into the Customer table
INSERT INTO Customer (CustomerName, PhoneNumber, Address, Point, RankLevel, isDeleted)
VALUES
    ('Alice Johnson', '555-1234', '789 Elm St, Somecity USA', 100, 1, 0),
    ('Bob Williams', '555-5678', '321 Oak Ln, Otherplace USA', 200, 2, 0),
    ('Charlie Davis', '555-9012', '654 Maple Ave, Somewhere USA', 300, 3, 0),
    ('Diana Wilson', '555-3456', '987 Pine Rd, Elsewhereville USA', 400, 4, 0);

-- Insert data into the CarStatus table
INSERT INTO CarStatus (CarStatusName)
VALUES
    ('Available'),
    ('Rented'),
    ('Maintenance');

-- Insert data into the Car table
INSERT INTO Car (LicensePlates, CarName, [Type], DateCar, Color, Brand, Price, NumberOfSeats, CarStatusId, Fuel, RentalPrice, isDeleted)
VALUES
    ('ABC123', 'Toyota Camry', 'Sedan', '2022-01-01', 'Silver', 'Toyota', 25000.00, 5, 1, 'Gasoline', 50.00, 0),
    ('XYZ456', 'Honda Civic', 'Sedan', '2021-06-15', 'Blue', 'Honda', 22000.00, 5, 1, 'Gasoline', 45.00, 0),
    ('PQR789', 'Ford F-150', 'Truck', '2020-09-01', 'Red', 'Ford', 35000.00, 5, 1, 'Gasoline', 65.00, 0),
    ('LMN012', 'Nissan Rogue', 'SUV', '2019-03-10', 'White', 'Nissan', 28000.00, 7, 1, 'Gasoline', 55.00, 0);

-- Insert data into the CarRental table
INSERT INTO CarRental (CustomerId, LicensePlates, StartDate, EndDate, StaffId, Total, isDeleted)
VALUES
    (1, 'ABC123', '2023-06-01 08:00:00', '2023-06-05 18:00:00', 1, 200.00, 0),
    (2, 'XYZ456', '2023-05-15 10:00:00', '2023-05-20 16:00:00', 2, 225.00, 0),
    (3, 'PQR789', '2023-04-01 12:00:00', '2023-04-08 20:00:00', 1, 455.00, 0),
    (4, 'LMN012', '2023-03-20 14:00:00', '2023-03-25 22:00:00', 2, 275.00, 0);

-- Insert data into the HistoryCarRental table
INSERT INTO HistoryCarRental (RentalId, StartDate, EndDate, ActualReturnTime, TotalPrice)
VALUES
    (1, '2023-06-01 08:00:00', '2023-06-05 18:00:00', '2023-06-05 17:30:00', 200.00),
    (2, '2023-05-15 10:00:00', '2023-05-20 16:00:00', '2023-05-20 15:45:00', 225.00),
    (3, '2023-04-01 12:00:00', '2023-04-08 20:00:00', '2023-04-08 19:30:00', 455.00),
    (4, '2023-03-20 14:00:00', '2023-03-25 22:00:00', '2023-03-25 21:45:00', 275.00);