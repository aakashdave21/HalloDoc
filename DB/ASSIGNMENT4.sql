
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE freight_average
	-- Add the parameters for the stored procedure here
	@Customer_ID nvarchar(5), 
	@Average_Freight money output
AS
BEGIN
	SELECT @Average_Freight = AVG(Freight)
	FROM Orders
	WHERE CustomerID = @Customer_ID
END
GO

CREATE TRIGGER CheckFreight
ON Orders
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE  @CustomerIdInput NVARCHAR(5);
    DECLARE @Freight MONEY;
    DECLARE @AverageFreight MONEY;
	DECLARE @AverageFreightOutput money

    -- Assuming you have a CustomerID and Freight column in the Orders table
    SELECT  @CustomerIdInput = CustomerID, @Freight = Freight
    FROM inserted;

    -- Call the stored procedure to get the average freight for the customer
     EXEC freight_Average @Customer_ID =  @CustomerIdInput,@Average_Freight = @AverageFreightOutput OUTPUT

    -- Check if the Freight exceeds the average freight
    IF @Freight > @AverageFreightOutput 
    BEGIN
       print 'Freight,The freight exceeds the average' + ' of $' + CONVERT(varchar(12), @AverageFreightOutput, 1)  + ' of previous orders.'
       ROLLBACK; -- Cancel the command
    END;
END;

insert into Orders values(N'VINET',5,'7/4/1996','8/1/1996','7/16/1996',3,7,N'Vins et alcools Chevalier',N'59 rue de l''Abbaye',N'Reims',NULL,N'51100',N'France');




-- Query 2 : Employee Sales By Country
USE [aakashdave_db]
GO

CREATE PROCEDURE [dbo].[Employee Sales by Country] 
    @Beginning_Date DateTime,
    @Ending_Date DateTime
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Employees.Country,
        Employees.LastName,
        Employees.FirstName,
        Orders.ShippedDate,
        Orders.OrderID,
        "Order Subtotals".Subtotal AS SaleAmount
    FROM
        Employees
        INNER JOIN (
            Orders
            INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
        ) ON Employees.EmployeeID = Orders.EmployeeID
    WHERE
        Orders.ShippedDate BETWEEN @Beginning_Date AND @Ending_Date;
END
GO


--Query 3 : Sales By Year
USE [aakashdave_db]
GO

CREATE PROCEDURE [dbo].[Sales by Year] 
    @Beginning_Date DateTime,
    @Ending_Date DateTime
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Orders.ShippedDate,
        Orders.OrderID,
        "Order Subtotals".Subtotal,
        DATENAME(yy, ShippedDate) AS Year
    FROM
        Orders
        INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
    WHERE
        Orders.ShippedDate BETWEEN @Beginning_Date AND @Ending_Date;
END
GO


--Query 4 : Sales By Category
USE [aakashdave_db]
GO

-- Create the procedure
CREATE PROCEDURE [dbo].[SalesByCategory]
    @CategoryName nvarchar(15),
    @OrdYear nvarchar(4) = '1998'
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if @OrdYear is not valid, set it to default value '1998'
    IF @OrdYear NOT IN ('1996', '1997', '1998')
    BEGIN
        SET @OrdYear = '1998'
    END

    SELECT
        ProductName,
        TotalPurchase = ROUND(SUM(CONVERT(decimal(14, 2), OD.Quantity * (1 - OD.Discount) * OD.UnitPrice)), 0) AS TotalPurchase
    FROM
        [Order Details] OD
        INNER JOIN Orders O ON OD.OrderID = O.OrderID
        INNER JOIN Products P ON OD.ProductID = P.ProductID
        INNER JOIN Categories C ON P.CategoryID = C.CategoryID
    WHERE
        C.CategoryName = @CategoryName
        AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear
    GROUP BY
        ProductName
    ORDER BY
        ProductName;
END
GO

-- Query 5 : Ten Most Expensive Products
USE [aakashdave_db]
GO

-- Create the procedure
CREATE PROCEDURE [dbo].[Ten Most Expensive Products]
AS
BEGIN
    SET NOCOUNT ON;

    SET ROWCOUNT 10; -- Limit the result to 10 rows

    SELECT
        Products.ProductName AS TenMostExpensiveProducts,
        Products.UnitPrice
    FROM
        Products
    ORDER BY
        Products.UnitPrice DESC;
END
GO

-- Query 6 :  Create Stored procedure in the Northwind database to insert 
--            Customer Order Details 

USE Northwind;
GO

CREATE PROCEDURE InsertOrderDetails
    @CustomerID nchar(5),
    @EmployeeID int,
    @OrderDate datetime,
    @ProductID int,
    @Quantity int,
    @UnitPrice money
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OrderID int;

    -- Insert into Orders table
    INSERT INTO Orders (CustomerID, EmployeeID, OrderDate)
    VALUES (@CustomerID, @EmployeeID, @OrderDate);

    -- Get the OrderID of the newly inserted order
    SET @OrderID = SCOPE_IDENTITY();

    -- Insert into OrderDetails table
    INSERT INTO [Order Details] (OrderID, ProductID, Quantity, UnitPrice)
    VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice);
END;


-- Query 7 : write a SQL query to Create Stored procedure in the Northwind database to update 
--           Customer Order Details
USE Northwind;

GO

CREATE PROCEDURE UpdateOrderDetails
    @OrderID int,
    @ProductID int,
    @Quantity int,
    @UnitPrice money
AS
BEGIN
    SET NOCOUNT ON;

    -- Update OrderDetails table
    UPDATE [Order Details]
    SET ProductID = @ProductID,
        Quantity = @Quantity,
        UnitPrice = @UnitPrice
    WHERE OrderID = @OrderID;
END;
