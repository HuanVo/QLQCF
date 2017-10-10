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
-- Shift
-- timekeepDetail


CREATE TABLE TableFood
(
	idTableFood INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa có tên',
	stats BIT -- 0:trong, 1: co nguoi
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR(100) NOT NULL PRIMARY KEY,	
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'ADMIM',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type BIT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
)
GO

CREATE TABLE FoodCategory
(
	idFoodCategory INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	idFood INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idFoodCategory INT NOT NULL,
	price MONEY NOT NULL DEFAULT 0
	
	FOREIGN KEY (idFoodCategory) REFERENCES dbo.FoodCategory(idFoodCategory)
)
GO
CREATE TABLE Employee
(
	idEmployee INT IDENTITY PRIMARY KEY,
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
CREATE TABLE Bill
(
	idBill INT IDENTITY PRIMARY KEY,
	DateCheckIn DATETIME NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATETIME,
	idTableFood INT NOT NULL,
	idEmployee INT NOT NULL, -- Ma nhan vien thanh toan
	stats BIT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán
	
	FOREIGN KEY (idTableFood) REFERENCES dbo.TableFood(idTableFood),
	FOREIGN KEY (idEmployee) REFERENCES dbo.Employee(idEmployee)
)
GO

CREATE TABLE BillInfo
(
	idBillInfo INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(idBill),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(idFood)
)
GO

CREATE TABLE Shifts -- ca lam viec
(
	idShifts INT IDENTITY PRIMARY KEY,
	name NVARCHAR(20),
	hours FLOAT -- so tieng tren mot ca
)
GO

CREATE TABLE TimekeepDetail
(
	idTimekeepDetail INT IDENTITY PRIMARY KEY,
	days DATE, -- ngay di lam
	idShifts INT, -- ca di lam
	idEmployee INT, -- Ma nhan vien

	FOREIGN KEY(idShifts) REFERENCES dbo.shifts(idShifts),
	FOREIGN KEY(idEmployee) REFERENCES dbo.Employee(idEmployee)

)
GO
INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'admin' , -- UserName - nvarchar(100)
          N'Quản trị 1' , -- DisplayName - nvarchar(100)
          N'21232f297a57a5a743894a0e4a801fc3' , -- PassWord - nvarchar(1000)
          1  -- Type - bit
        )
GO

/*
	Kiểm tra đăng nhập.
	EXEC dbo.CheckLogin @UserName = 'admin', -- varchar(100)
    @Password = '21232f297a57a5a743894a0e4a801fc3' -- varchar(1000)
    
*/
CREATE PROC CheckLogin(@UserName varchar(100), @Password VARCHAR(1000))
AS
BEGIN
	SELECT COUNT(*) FROM
	dbo.Account
	WHERE UserName = @UserName AND PassWord = @Password
END
GO
/*
	Lấy thông tin tài khoản
	EXEC dbo.getAccount @UserName = 'admin' -- varchar(100)
*/
CREATE PROC getAccount(@UserName VARCHAR(100))
AS
BEGIN
SELECT UserName, DisplayName,Type FROM dbo.Account
WHERE UserName = @UserName
END
GO




