-- 1. Database Creation
CREATE DATABASE NORTHWIND;

-- 2. Table Creation
CREATE TABLE PRODUCTS
(
	ProductId INT PRIMARY KEY,
	ProductName VARCHAR NOT NULL,
	SupplierId INT,
	CategoryId INT,
	QuantityPerUnit INT,
	UnitPrice DECIMAL(10,2) NOT NULL,
	UnitInStock INT,
	UnitsOnOrder INT,
	ReorderLevel INT,
	Discontinued BIT
);

-- 3. Query 1 : Write a query to get a Product list
--    (id, name, unit price) where current products cost less than $20

		SELECT productid,productname,unitprice fROM PRODUCTS
		WHERE unitprice < 20;
		
-- 4. QUERY 2 : Write a query trodu o get Product list (id, name, unit price)
--  			where products cost between $15 and $25

		SELECT productid,productname,unitprice FROM PRODUCTS
		WHERE unitprice BETWEEN 15 AND 25;
		
-- 5. QUERY 2.1 : Write a query to get Product list (name, unit price) of above average price.

		-- AVG PRICE QUERY 
		SELECT AVG(unitprice) FROM PRODUCTS;
		
		
		SELECT productid,productname,unitprice fROM PRODUCTS
		WHERE unitprice >= (SELECT AVG(unitprice) FROM PRODUCTS);

-- 6. QUERY 2.2 : Write a query to get Product list (name, unit price) of ten most expensive product.

		SELECT productid,productname,unitprice fROM PRODUCTS
		ORDER BY unitprice DESC
		LIMIT 10;
		
-- 7. Query 3.1 : Write a query to count current and discontinued products
		SELECT
		COUNT(CASE WHEN discontinued = B'0' THEN productid END) AS current_products,
		COUNT(CASE WHEN discontinued = B'1' THEN productid END) AS discountinued_products
		FROM PRODUCTS;
		
-- 8. QUERY 3.2 : Write a query to get Product list (name, units on order, units in stock)
-- 				  of stock is less than the quantity on order
		SELECT productname, unitsonorder, unitinstock FROM PRODUCTS
		WHERE unitinstock < unitsonorder;
		
		



















