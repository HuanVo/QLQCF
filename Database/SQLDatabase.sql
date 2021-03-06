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

INSERT INTO dbo.FoodCategory ( name ) VALUES  ( N'Cà Phê' )
INSERT INTO dbo.FoodCategory ( name ) VALUES  ( N'Sinh Tố' )
INSERT INTO dbo.FoodCategory ( name ) VALUES  ( N'Nước Ngọt')
INSERT INTO dbo.FoodCategory ( name ) VALUES  ( N'Thuốc Lá')
INSERT INTO dbo.FoodCategory ( name ) VALUES  ( N'Danh Mục Món Phụ')

CREATE TABLE Food
(
	idFood INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idFoodCategory INT NOT NULL,
	unit NVARCHAR(50),
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

INSERT INTO dbo.Employee
        ( fullName ,
          sex ,
          addres ,
          phone ,
          dayStart ,
          salaryLevel ,
          indicator ,
          advance
        )
VALUES  ( N'Hoàng Thị Thiên' , -- fullName - nvarchar(100)
          0 , -- sex - bit
          N'Nghệ An' , -- addres - nvarchar(255)
          '09766662' , -- phone - varchar(15)
          GETDATE() , -- dayStart - date
          1200000 , -- salaryLevel - money
          GETDATE() , -- indicator - date
          0  -- advance - money
        )
		GO
        
INSERT INTO dbo.Employee
        ( fullName ,
          sex ,
          addres ,
          phone ,
          dayStart ,
          salaryLevel ,
          indicator ,
          advance
        )
VALUES  ( N'Lê Đức Huy' , -- fullName - nvarchar(100)
          1 , -- sex - bit
          N'Quảng Trị' , -- addres - nvarchar(255)
          '0978747474' , -- phone - varchar(15)
          GETDATE() , -- dayStart - date
          1500000 , -- salaryLevel - money
          GETDATE() , -- indicator - date
          0  -- advance - money
        )
		GO
        
INSERT INTO dbo.Employee
        ( fullName ,
          sex ,
          addres ,
          phone ,
          dayStart ,
          salaryLevel ,
          indicator ,
          advance
        )
VALUES  ( N'Hồ Xuân Thạch' , -- fullName - nvarchar(100)
          1 , -- sex - bit
          N'Gia Lai' , -- addres - nvarchar(255)
          '0978520202' , -- phone - varchar(15)
          GETDATE() , -- dayStart - date
          1300000 , -- salaryLevel - money
          GETDATE() , -- indicator - date
          0  -- advance - money
        )
		GO


CREATE TABLE Bill
(
	idBill INT IDENTITY PRIMARY KEY,
	DateCheckIn DATETIME NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATETIME,
	idTableFood INT NOT NULL,
	saleoff INT DEFAULT 0,
	vat INT DEFAULT 0,
	idEmployee INT NOT NULL, -- Ma nhan vien thanh toan
	stats BIT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán
	
	FOREIGN KEY (idTableFood) REFERENCES dbo.TableFood(idTableFood),
	FOREIGN KEY (idEmployee) REFERENCES dbo.Employee(idEmployee)
)
GO

INSERT INTO dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTableFood ,
          idEmployee ,
          stats
        )
		
VALUES  ( GETDATE() , -- DateCheckIn - datetime
          GETDATE() , -- DateCheckOut - datetime
          3 , -- idTableFood - int
          1 , -- idEmployee - int
          0  -- stats - bit
        )
		GO
 INSERT INTO dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTableFood ,
          idEmployee ,
          stats
        )
VALUES  ( GETDATE() , -- DateCheckIn - datetime
          GETDATE() , -- DateCheckOut - datetime
          4 , -- idTableFood - int
          1 , -- idEmployee - int
          0  -- stats - bit
        )

		GO

INSERT INTO dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTableFood ,
          idEmployee ,
          stats
        )
VALUES  ( GETDATE() , -- DateCheckIn - datetime
          GETDATE() , -- DateCheckOut - datetime
          2 , -- idTableFood - int
          2 , -- idEmployee - int
          0  -- stats - bit
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

/*
	Thêm mới nhân viên
	EXEC dbo.addEmployee N'Võ Văn Huấn', True, N'bvvvvvvvvvvvvv', '123585', '10/31/2017 12:00:00 AM', '1500000', '10/31/2017 12:00:00 AM', 0
*/
CREATE PROC addEmployee( @fullname NVARCHAR(100), @sex BIT,@address NVARCHAR(255), @phone VARCHAR(20), @daystart DATE,@salarylevel MONEY, @indicator DATE, @advance MONEY)
AS
BEGIN
	INSERT INTO Employee(fullName, sex, addres, phone, dayStart, salaryLevel, indicator, advance)
	VALUES(@fullname, @sex, @address, @phone, @daystart, @salarylevel, @indicator, @advance)
END
GO

/*
	sửa mới nhân viên
	
*/
CREATE PROC editEmployee( @fullname NVARCHAR(100), @sex BIT,@address NVARCHAR(255), @phone VARCHAR(20), @daystart DATE,@salarylevel MONEY, @indicator DATE, @advance MONEY, @id INT)
AS
BEGIN
	UPDATE Employee
	SET fullName = @fullname, sex = @sex, addres = @address, phone = @phone, dayStart = @daystart, salaryLevel = @salarylevel, indicator = @indicator,advance = @advance
	WHERE dbo.Employee.idEmployee = @id
END
GO
/*
	Lấy danh nhân viên lên datagridview
	EXEC dbo.getEmployees
*/
CREATE PROC getEmployees
AS
BEGIN
	SELECT dbo.Employee.idEmployee AS [Mã Nhân Viên], dbo.Employee.fullName AS [Họ Và Tên],
	  [Giới Tính]= CASE dbo.Employee.sex when 'true' then N'Nam'
		when 'false' then N'Nữ' END,
	dbo.Employee.addres AS [Địa Chỉ], dbo.Employee.phone AS [Số Điện Thoại] FROM dbo.Employee
END
GO

CREATE PROC getEmployeesByID(@id INT)
AS
BEGIN
	SELECT *
	 FROM dbo.Employee
	 WHERE idEmployee = @id
END
GO

/*
Xoa nhan vien
*/
CREATE PROC DeleteEmployee(@id INT)
AS
BEGIN
	DELETE 
	 FROM dbo.Employee
	 WHERE idEmployee = @id
END
GO

/*
 - Load danh sach sanr pham.
 - EXEC getTableProduct
*/
CREATE PROC getTableProduct
AS
BEGIN
	SELECT idFood AS [Mã Sản Phẩm], name AS [Tên Phẩn Phẩm], unit AS [Đơn Vị], price AS [Giá] FROM dbo.Food
END
GO

/*
 - Thêm mới Sản phẩm
*/
CREATE PROC addProduct(@name NVARCHAR(100), @id INT, @unit NVARCHAR(50), @price MONEY)
AS
BEGIN
	INSERT INTO dbo.Food
	        ( name, idFoodCategory, unit, price )
	VALUES  ( @name, -- name - nvarchar(100)
	          @id, -- idFoodCategory - int
	          @unit, -- unit - nvarchar(50)
	          @price  -- price - money
	          )
END
    
GO

/*
 - Lấy sản phẩm bởi mã sản phẩm
*/
CREATE PROC getProductByID(@id VARCHAR(50))
AS
BEGIN
	SELECT * FROM dbo.Food
	WHERE idFood = @id
END

GO

/*
 - Sửa sản phẩm
*/

CREATE PROC editProduct(@id INT, @name NVARCHAR(100), @unit NVARCHAR(50), @price MONEY, @idcatalog INT)
AS
BEGIN
	UPDATE dbo.Food
	SET name=@name, idFoodCategory = @idcatalog, unit = @unit, price = @price
	WHERE idFood = @id
END
GO

/*
 - Xóa một sản phẩm
*/
CREATE PROC DeleteProduct(@id INT)
AS
BEGIN
	DELETE FROM dbo.Food
	WHERE idFood = @id
END
GO

/*
 - Thêm Một Catalog sản phẩm
*/

CREATE PROC AddCatalog(@name NVARCHAR(100))
AS
BEGIN
	INSERT INTO dbo.FoodCategory
	        (name)
	VALUES  (@name)
END
 EXEC dbo.AddCatalog @name = N'Thực phẩm xanh' -- nvarchar(100)
 GO
 
 /*
  - Load danh sách catalog
 */

 CREATE PROC getTableCatalog
 AS
 BEGIN
 	SELECT dbo.FoodCategory.idFoodCategory AS [Mã Danh Mục], dbo.FoodCategory.name AS [Tên Danh Mục] FROM dbo.FoodCategory
 END
 GO
 
 /*
  - Load danh sach catalog boi ma catalog
 */
  CREATE PROC getTableCatalogByID(@id INT)
 AS
 BEGIN
 	SELECT dbo.FoodCategory.idFoodCategory AS [Mã Danh Mục], dbo.FoodCategory.name AS [Tên Danh Mục] FROM dbo.FoodCategory
	WHERE idFoodCategory = @id
 END
 GO
 
/*
 - Sửa catalog
*/
CREATE PROC editCatalog(@id INT, @name NVARCHAR(100))
AS
BEGIN
	UPDATE dbo.FoodCategory
	SET name = @name
	WHERE idFoodCategory = @id
    
END
GO

/*
 - Xóa catalogs
*/
CREATE PROC DeleteCatalog(@id INT)
AS
BEGIN
	DELETE FROM dbo.FoodCategory
	WHERE idFoodCategory = @id
END
GO

/*
Lấy danh sách bàn uống
*/

CREATE PROC getTableTableFood
AS
BEGIN
	SELECT dbo.TableFood.idTableFood AS [Mã Bàn], dbo.TableFood.name AS [Tên Bàn],[Tình Trạng] = CASE dbo.TableFood.stats WHEN 'true' THEN N'Có người' WHEN 'false' THEN N'Trống' END FROM dbo.TableFood
END
GO


/*
 - Thêm Bàn
*/
CREATE PROC AddTableFood(@Name NVARCHAR(50), @status BIT)
AS
BEGIN
	INSERT INTO dbo.TableFood
	        ( name, stats )
	VALUES  ( @Name, -- name - nvarchar(100)
	          @status  -- stats - bit
	          )
END
GO

/*
	- lấy dữ liệu bản ghi bàn bởi id bàn
*/
CREATE PROC getTableTableFoodByID(@id INT)
AS
BEGIN
	SELECT dbo.TableFood.idTableFood, dbo.TableFood.name, stats FROM dbo.TableFood
	WHERE idTableFood = @id
END
GO

/*
 - sửa bàn uống
*/
CREATE PROC editTableFood(@id INT, @name NVARCHAR(50), @sts BIT)
AS
BEGIN
	UPDATE dbo.TableFood
	SET name = @name, stats = @sts
	WHERE idTableFood = @id
    
END
GO

/*
 - Xóa bàn
*/

CREATE PROC deleteTableFood(@id INT)
AS
BEGIN
	DELETE FROM dbo.TableFood
	WHERE idTableFood = @id
END
GO

/*
 - Lấy danh sách bàn.
*/
 CREATE PROC loadAllTableFood
 AS
 BEGIN
 	SELECT * FROM dbo.TableFood ORDER BY stats
 END

 GO
 

 CREATE PROC UpdateBillInfo(@count INT, @idbill INT, @idFood INT)
AS
BEGIN
	UPDATE dbo.BillInfo SET dbo.BillInfo.count = @count +(SELECT count FROM dbo.BillInfo WHERE dbo.BillInfo.idFood = @idFood AND dbo.BillInfo.idBill = @idbill) WHERE dbo.BillInfo.idFood = @idFood AND dbo.BillInfo.idBill = @idbill
END
GO

/*
	Load dữ liệu báo cáo thống kê.
*/
 CREATE PROC LoadTKBC
AS
BEGIN
	SELECT dbo.BillInfo.idBill, dbo.Employee.fullName, dbo.Bill.DateCheckOut, dbo.Bill.saleoff, dbo.Bill.vat, SUM(dbo.BillInfo.count*dbo.Food.price) [sum]
	 FROM dbo.BillInfo
	  INNER JOIN dbo.Food ON dbo.BillInfo.idFood = dbo.Food.idFood
	  INNER JOIN dbo.Bill ON Bill.idBill = BillInfo.idBill
	  INNER JOIN dbo.Employee ON Employee.idEmployee = Bill.idEmployee
	  

	 GROUP BY dbo.BillInfo.idBill, saleoff, vat, DateCheckOut, dbo.Employee.fullName
END
GO
/*
	Load dữ liệu báo cáo thống kê chi tiet bill voi id bill
*/
 CREATE PROC LoadTKBCDetailBillById(@idBill INT)
AS
BEGIN
	SELECT dbo.BillInfo.idBillInfo, dbo.Food.name, count, Bill.idBill
	 FROM dbo.BillInfo
	  INNER JOIN dbo.Food ON dbo.BillInfo.idFood = dbo.Food.idFood
	  INNER JOIN dbo.Bill ON Bill.idBill = BillInfo.idBill
	 WHERE Bill.idBill = @idBill
END
GO

