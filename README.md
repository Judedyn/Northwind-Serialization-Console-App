# Northwind-Serialization-Console-App
Assignment 3 Question 1  C#

![.NET](https://img.shields.io/badge/.NET-6.0-blue)
![C#](https://img.shields.io/badge/C%23-Programming-green)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red)
![Status](https://img.shields.io/badge/Status-Completed-brightgreen)

---

##  Description

This console application connects to a SQL Server database and retrieves data from the **Categories** and **Products** tables. The data is then serialized into three different formats: **JSON**, **XML**, and **Binary**.

After serialization, the application calculates the size (in bytes) of each format and displays a ranking from smallest to largest. This helps compare the efficiency of different serialization methods in .NET.

---

##  Features

* Connects to SQL Server (Northwind database)
* Retrieves Categories and Products
* Serializes data into:

  * JSON
  * XML
  * Binary (or DataContract)
* Saves output files
* Displays size comparison

---

##  Database Setup

This project uses a **simplified version of the Northwind database** for demonstration purposes.

**Note:**
The Categories and Products data used in this project are **sample/example data created manually**, not the full official Northwind database. This was done to simplify setup and focus on serialization functionality.

The project includes only two tables:

* Categories
* Products

Run this SQL script in SSMS:

```sql
USE Northwind;

CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY,
    CategoryName NVARCHAR(100)
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    CategoryID INT,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Example Categories
INSERT INTO Categories VALUES
(1, 'Beverages'),
(2, 'Condiments'),
(3, 'Confections');

-- Example Products (sample data)
INSERT INTO Products VALUES
(1, 'Chai', 1),
(2, 'Chang', 1),
(3, 'Aniseed Syrup', 2),
(4, 'Chef Anton''s Cajun Seasoning', 2),
(5, 'Chocolate', 3);
```

---

##  Connection String

Update the connection string in `Program.cs`:

```csharp
string connectionString = @"Server=YOUR_SERVER_NAME;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;";
```

Example:

```csharp
string connectionString = @"Server=LAPTOP-DU6SRIJM;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;";
```

---

## How to Run

1. Open terminal in project folder
2. Build the project:

```
dotnet build
```

3. Run the application:

```
dotnet run
```

---

## Sample Output

```
=== Serialization Size Ranking ===

JSON: 701 bytes
Binary: 892 bytes
XML: 1182 bytes
```

---

## Output Files

After running the program, the following files are created:

* data.json
* data.xml
* data.bin

---

## Concepts Used

* ADO.NET (SQL Server connection)
* Object-Oriented Programming (Classes)
* Serialization in .NET
* File handling
* LINQ (sorting results)

---

## Notes

* BinaryFormatter is obsolete in modern .NET, but used here for learning purposes.
* JSON is typically the most efficient format for storage.
* XML produces larger file sizes due to its structure.

---

## Author

Frans Jude Del Castillo

---

## Purpose

This project was created as part of a .NET course assignment to demonstrate understanding of database access and serialization techniques.

