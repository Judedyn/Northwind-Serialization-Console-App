USE Northwind;

-- Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY,
    CategoryName NVARCHAR(100)
);

-- Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    CategoryID INT,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Insert Categories
INSERT INTO Categories VALUES
(1, 'Beverages'),
(2, 'Condiments'),
(3, 'Confections');

-- Insert Products
INSERT INTO Products VALUES
(1, 'Chai', 1),
(2, 'Chang', 1),
(3, 'Aniseed Syrup', 2),
(4, 'Chef Anton''s Cajun Seasoning', 2),
(5, 'Chocolate', 3);