-- 1. USERS (KULLANICI) PROSEDÜRLERİ

CREATE PROC UserEY
    @Id INT,
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256),
    @Role NVARCHAR(20)
AS
BEGIN
    IF @Id = 0
        INSERT INTO Users (FullName, Email, PasswordHash, Role) 
        VALUES (@FullName, @Email, @PasswordHash, @Role)
    ELSE
        UPDATE Users 
        SET FullName = @FullName, Email = @Email, PasswordHash = @PasswordHash, Role = @Role
        WHERE Id = @Id
END
GO

CREATE PROC UserViewAll
AS
BEGIN
    SELECT * FROM Users
END
GO

CREATE PROC UserViewById
    @Id INT
AS
BEGIN
    SELECT * FROM Users WHERE Id = @Id
END
GO

CREATE PROC UserSil
    @Id INT
AS
BEGIN
    DELETE FROM Users WHERE Id = @Id
END
GO

CREATE PROC UserLoginKontrol
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash
END
GO


-- 2. WALLETS (CÜZDAN) PROSEDÜRLERİ

CREATE PROC WalletEY
    @Id INT,
    @UserId INT,
    @WalletNumber NVARCHAR(16),
    @Balance DECIMAL(18,2),
    @Currency NVARCHAR(3)
AS
BEGIN
    IF @Id = 0
        INSERT INTO Wallets (UserId, WalletNumber, Balance, Currency, UpdatedAt)
        VALUES (@UserId, @WalletNumber, @Balance, @Currency, GETDATE())
    ELSE
        UPDATE Wallets
        SET UserId = @UserId, WalletNumber = @WalletNumber, Balance = @Balance, Currency = @Currency, UpdatedAt = GETDATE()
        WHERE Id = @Id
END
GO

CREATE PROC WalletViewAll
AS
BEGIN
    SELECT * FROM Wallets
END
GO

CREATE PROC WalletViewById
    @Id INT
AS
BEGIN
    SELECT * FROM Wallets WHERE Id = @Id
END
GO

CREATE PROC WalletSil
    @Id INT
AS
BEGIN
    DELETE FROM Wallets WHERE Id = @Id
END
GO


-- 3. PRODUCTS (ÜRÜN) PROSEDÜRLERİ

CREATE PROC ProductEY
    @Id INT,
    @ProductName NVARCHAR(150),
    @Price DECIMAL(18,2),
    @Stock INT,
    @IsActive BIT
AS
BEGIN
    IF @Id = 0
        INSERT INTO Products (ProductName, Price, Stock, IsActive)
        VALUES (@ProductName, @Price, @Stock, @IsActive)
    ELSE
        UPDATE Products
        SET ProductName = @ProductName, Price = @Price, Stock = @Stock, IsActive = @IsActive
        WHERE Id = @Id
END
GO

CREATE PROC ProductViewAll
AS
BEGIN
    SELECT * FROM Products
END
GO

CREATE PROC ProductViewById
    @Id INT
AS
BEGIN
    SELECT * FROM Products WHERE Id = @Id
END
GO

CREATE PROC ProductSil
    @Id INT
AS
BEGIN
    DELETE FROM Products WHERE Id = @Id
END
GO


-- 4. ORDERS (SİPARİŞ) PROSEDÜRLERİ

CREATE PROC OrderEY
    @Id INT,
    @UserId INT,
    @ProductId INT,
    @WalletId INT,
    @OrderNumber NVARCHAR(50),
    @Quantity INT,
    @TotalPrice DECIMAL(18,2)
AS
BEGIN
    IF @Id = 0
        INSERT INTO Orders (UserId, ProductId, WalletId, OrderNumber, Quantity, TotalPrice, OrderDate)
        VALUES (@UserId, @ProductId, @WalletId, @OrderNumber, @Quantity, @TotalPrice, GETDATE())
    ELSE
        UPDATE Orders
        SET UserId = @UserId, ProductId = @ProductId, WalletId = @WalletId, OrderNumber = @OrderNumber, Quantity = @Quantity, TotalPrice = @TotalPrice
        WHERE Id = @Id
END
GO

CREATE PROC OrderViewAll
AS
BEGIN
    SELECT 
        O.Id,
        O.OrderNumber,
        O.Quantity,
        O.TotalPrice,
        O.OrderDate,
        U.FullName AS CustomerName,
        P.ProductName AS ProductName,
        W.WalletNumber AS WalletNumber
    FROM Orders O
    INNER JOIN Users U ON O.UserId = U.Id
    INNER JOIN Products P ON O.ProductId = P.Id
    INNER JOIN Wallets W ON O.WalletId = W.Id
END
GO

CREATE PROC OrderViewById
    @Id INT
AS
BEGIN
    SELECT * FROM Orders WHERE Id = @Id
END
GO

CREATE PROC OrderSil
    @Id INT
AS
BEGIN
    DELETE FROM Orders WHERE Id = @Id
END
GO