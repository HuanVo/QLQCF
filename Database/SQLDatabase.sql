USE master
GO
IF EXISTS(SELECT * FROM sysdatabases WHERE name='CoffeeRes')
DROP DATABASE CoffeeRes
GO

CREATE DATABASE CoffeeRes
GO

USE CoffeeRes
GO

-- Account
-- Food
-- FoodCategory
-- Table
-- Bill
-- BillInfo
-- Employee
-- Ca
-- ChiTietChamCong


CREATE TABLE TableFood
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa có tên',
	stats BIT -- 0:trong, 1: co nguoi
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY,	
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'ADMIM',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type BIT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
)
GO

CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price MONEY NOT NULL DEFAULT 0
	
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	idTable INT NOT NULL,
	stats BIT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán
	
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO
CREATE TABLE Employee
(
	id INT IDENTITY PRIMARY KEY,
	fullName NVARCHAR(100) NOT NULL,
	sex BIT,
	addres NVARCHAR(255),
	phone VARCHAR(15),
	dayStart DATE, -- ngay bat dau vao lam
	salaryLevel MONEY NOT NULL, -- he so luong tinh theo ca lam viec
	indicator DATE NOT NULL, -- ngay cuoi cung nhan luong 
	advance MONEY DEFAULT 0, -- tien ung truoc
)
GO
CREATE TABLE shifts -- ca lam viec
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(20),
	hours FLOAT -- so tieng tren mot ca
)
GO

CREATE TABLE timekeepDetail
(
	id INT IDENTITY PRIMARY KEY,
	days DATE, -- ngay di lam
	ca INT, -- ca di lam
	EmpID INT, -- Ma nhan vien

	FOREIGN KEY(ca) REFERENCES dbo.shifts(id),
	FOREIGN KEY(EmpID) REFERENCES dbo.Employee(id)

)
GO

