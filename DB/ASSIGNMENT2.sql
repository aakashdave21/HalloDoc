-- 1. Table Creat
CREATE TABLE SALESMAN
(
	SALESMAN_ID SERIAL PRIMARY KEY,
	NAME VARCHAR(40),
	CITY VARCHAR(40),
	COMMISSION DECIMAL(10,2)
);

CREATE TABLE CUSTOMER
(
	CUSTOMER_ID SERIAL PRIMARY KEY,
	NAME VARCHAR(40) NOT NULL,
	CITY VARCHAR(40),
	GRADE VARCHAR(40),
	SALESMAN_ID INT,
	FOREIGN KEY(SALESMAN_ID) REFERENCES SALESMAN(SALESMAN_ID)
);

CREATE TABLE ORDERS
(
	ORD_NO SERIAL PRIMARY KEY,
	PURCH_AMT DECIMAL(18,2) NOT NULL,
	ORD_DATE DATE DEFAULT CURRENT_DATE,
	CUSTOMER_ID INT,
	SALESMAN_ID INT,
	FOREIGN KEY(CUSTOMER_ID) REFERENCES CUSTOMER(CUSTOMER_ID),
	FOREIGN KEY(SALESMAN_ID) REFERENCES SALESMAN(SALESMAN_ID)
);

CREATE TABLE SALESMAN
(
	SALESMAN_ID SERIAL PRIMARY KEY,
	NAME VARCHAR(40),
	CITY VARCHAR(40),
	COMMISSION DECIMAL(10,2)
);

CREATE TABLE CUSTOMER
(
	CUSTOMER_ID SERIAL PRIMARY KEY,
	NAME VARCHAR(40) NOT NULL,
	CITY VARCHAR(40),
	GRADE VARCHAR(40),
	SALESMAN_ID INT,
	FOREIGN KEY(SALESMAN_ID) REFERENCES SALESMAN(SALESMAN_ID)
);

-- INSERT QUERY..
INSERT INTO SALESMAN (NAME, CITY, COMMISSION) VALUES
('Alice', 'New York', 0.10),
('Bob', 'London', 0.15),
('Charlie', 'Paris', 0.12),
('David', 'Tokyo', 0.08),
('Eve', 'Sydney', 0.11),
('Frank', 'Berlin', 0.09),
('Grace', 'Rome', 0.13),
('Harry', 'Beijing', 0.07),
('Irene', 'Delhi', 0.14),
('Jack', 'Moscow', 0.06);

INSERT INTO CUSTOMER (NAME, CITY, GRADE, SALESMAN_ID) VALUES
('Anna', 'New York', 'A', 1),
('Ben', 'London', 'B', 2),
('Cathy', 'Paris', 'C', 3),
('Dan', 'Tokyo', 'D', 4),
('Emma', 'Sydney', 'E', 5),
('Fred', 'Berlin', 'F', 6),
('Gina', 'Rome', 'G', 7),
('Henry', 'Beijing', 'H', 8),
('Isabel', 'Delhi', 'I', 9),
('James', 'Moscow', 'J', 10);

INSERT INTO ORDERS (PURCH_AMT, CUSTOMER_ID, SALESMAN_ID) VALUES
(1000.00, 1, 1),
(2000.00, 2, 2),
(1500.00, 3, 3),
(2500.00, 4, 4),
(3000.00, 5, 5),
(3500.00, 6, 6),
(4000.00, 7, 7),
(4500.00, 8, 8),
(5000.00, 9, 9),
(5500.00, 10, 10);

-- QUERY 1 : write a SQL query to find the salesperson and customer who reside in the same city. 
--           Return Salesman, cust_name and city

			SELECT SALESMAN.NAME AS SALESMAN, CUSTOMER.name AS CUSTOMER, CUSTOMER.CITY FROM CUSTOMER 
			JOIN SALESMAN ON SALESMAN.salesman_id = CUSTOMER.salesman_id
			WHERE SALESMAN.CITY = CUSTOMER.CITY;
			
-- QUERY 2 : write a SQL query to find those orders where the order amount exists between 500 
--			and 2000. Return ord_no, purch_amt, cust_name, city 

			SELECT ORDERS.ord_no, ORDERS.purch_amt, CUSTOMER.name AS CUSTOMER, CUSTOMER.city FROM
			ORDERS JOIN CUSTOMER ON ORDERS.customer_id = CUSTOMER.customer_id
			WHERE ORDERS.purch_amt BETWEEN 500 AND 2000;
			
-- QUERY 3 : Write a SQL query to find the salesperson(s) and the customer(s) he represents. 
--			 Return Customer Name, city, Salesman, commission


			SELECT SALESMAN.NAME AS SALESMAN, CUSTOMER.NAME AS CUSTOMER, CUSTOMER.CITY, SALESMAN.COMMISSION
			FROM SALESMAN LEFT JOIN CUSTOMER ON SALESMAN.salesman_id = CUSTOMER.salesman_id;
		
-- QUERY 4 : write a SQL query to find salespeople who received commissions of more than 12 
--           percent from the company. Return Customer Name, customer city, Salesman, 
--           commission

			SELECT SALESMAN.NAME AS SALESMAN, CUSTOMER.NAME AS CUSTOMER, CUSTOMER.CITY, SALESMAN.COMMISSION 
			FROM SALESMAN JOIN CUSTOMER ON SALESMAN.salesman_id = CUSTOMER.salesman_id
			WHERE SALESMAN.COMMISSION > 0.12;
			
-- QUERY 5: write a SQL query to locate those salespeople who do not live in the same city where 
-- 			their customers live and have received a commission of more than 12% from the 
--			company. Return Customer Name, customer city, Salesman, salesman city, 
-- 			commission

			SELECT SALESMAN.NAME AS SALESMAN, CUSTOMER.NAME AS CUSTOMER, CUSTOMER.CITY AS CUSTOMER_CITY,
			SALESMAN.CITY AS SALESMAN_CITY, SALESMAN.COMMISSION 
			FROM SALESMAN JOIN CUSTOMER ON SALESMAN.salesman_id = CUSTOMER.salesman_id
			WHERE SALESMAN.CITY <> CUSTOMER.CITY AND SALESMAN.COMMISSION > 0.12;
			
-- QUERY 6: write a SQL query to find the details of an order. Return ord_no, ord_date, 
--			purch_amt, Customer Name, grade, Salesman, commission

			SELECT ORDERS.ord_no, ORDERS.purch_amt, ORDERS.ord_date, CUSTOMER.name AS CUSTOMER, CUSTOMER.grade, CUSTOMER.city,
			SALESMAN.NAME AS SALESMAN,SALESMAN.COMMISSION
			FROM ORDERS
			JOIN CUSTOMER ON ORDERS.customer_id = CUSTOMER.customer_id
			JOIN SALESMAN ON CUSTOMER.salesman_id = SALESMAN.salesman_id;
			
-- Query 7 : Write a SQL statement to join the tables salesman, customer and orders so that the 
-- 			 same column of each table appears once and only the relational rows are returned. 

			SELECT CUSTOMER.customer_id, CUSTOMER.name AS CUSTOMER_NAME, CUSTOMER.city AS CUSTOMER_CITY,
			CUSTOMER.grade, SALESMAN.salesman_id, SALESMAN.name AS SALESMAN_NAME, SALESMAN.city AS SALESMAN_CITY,
			SALESMAN.commission, ORDERS.ord_no, ORDERS.purch_amt, ORDERS.ord_date FROM orders
			JOIN customer ON orders.customer_id = customer.customer_id
			JOIN salesman ON orders.salesman_id = salesman.salesman_id;



-- Query 8 : write a SQL query to display the customer name, customer city, grade, salesman, 
--			 salesman city. The results should be sorted by ascending customer_id. 
			SELECT customer.customer_id, CUSTOMER.name AS CUSTOMER_NAME, CUSTOMER.city AS CUSTOMER_CITY, CUSTOMER.grade,
			SALESMAN.name AS SALESMAN_NAME FROM customer
			JOIN salesman ON customer.salesman_id = salesman.salesman_id
			ORDER BY customer.customer_id;
			
-- QUERY 9 : write a SQL query to find those customers with a grade less than 300. Return 
--			cust_name, customer city, grade, Salesman, salesmancity. The result should be 
--			ordered by ascending customer_id.

-- 			changed the GRADE column datatypes VARCHAR -> INT
			ALTER TABLE customer
			ALTER COLUMN grade TYPE INT
			USING grade::integer;
			
			
			SELECT customer.customer_id, CUSTOMER.name AS CUSTOMER_NAME, CUSTOMER.city AS CUSTOMER_CITY, CUSTOMER.grade,
			SALESMAN.name AS SALESMAN_NAME , SALESMAN.city AS SALESMAN_CITY FROM customer
			JOIN salesman ON customer.salesman_id = salesman.salesman_id
			WHERE customer.grade <= 300
			ORDER BY customer.customer_id;
			
-- QUERY 10 : Write a SQL statement to make a report with customer name, city, order number, 
--				order date, and order amount in ascending order according to the order date to 
-- 				determine whether any of the existing customers have placed an order or not.

			SELECT CUSTOMER.name AS CUSTOMER_NAME, CUSTOMER.city AS CUSTOMER_CITY,
			ORDERS.ord_no, ORDERS.purch_amt, ORDERS.ord_date FROM customer
			LEFT JOIN orders ON orders.customer_id = customer.customer_id
			ORDER BY orders.ord_date;
			
-- QUERY 11 : Write a SQL statement to generate a report with customer name, city, order number, 
-- 		order date, order amount, salesperson name, and commission to determine if any of 
-- 		the existing customers have not placed orders or if they have placed orders through 
-- 		their salesman or by themselves.

			
			SELECT CUSTOMER.customer_id, CUSTOMER.name AS CUSTOMER_NAME, CUSTOMER.city AS CUSTOMER_CITY,
			CUSTOMER.grade, SALESMAN.salesman_id, SALESMAN.name AS SALESMAN_NAME, SALESMAN.city AS SALESMAN_CITY,
			SALESMAN.commission, ORDERS.ord_no, ORDERS.purch_amt, ORDERS.ord_date FROM customer 
			LEFT JOIN orders ON customer.customer_id = orders.customer_id
			LEFT JOIN salesman ON orders.salesman_id = salesman.salesman_id;
			
			
-- QUERY 12 :  Write a SQL statement to generate a list in ascending order of salespersons who 
--            work either for one or more customers or have not yet joined any of the customers.

		SELECT salesman.name AS salesman, COUNT(customer.salesman_id) FROM salesman
		LEFT JOIN customer ON customer.salesman_id = salesman.salesman_id
		GROUP BY salesman.salesman_id,customer.salesman_id,salesman.name
		ORDER BY customer.salesman_id;
			
-- QUERY 13 :  write a SQL query to list all salespersons along with customer name, city, grade, 
--			   order number, date, and amount

		SELECT SALESMAN.salesman_id, SALESMAN.name AS SALESMAN_NAME,
		CUSTOMER.customer_id, CUSTOMER.name AS CUSTOMER_NAME, CUSTOMER.city AS CUSTOMER_CITY,
		CUSTOMER.grade, ORDERS.ord_no, ORDERS.purch_amt, ORDERS.ord_date FROM salesman
		LEFT JOIN customer ON salesman.salesman_id = customer.salesman_id
		LEFT JOIN orders ON orders.customer_id = customer.customer_id;
	
		
-- QUERY 14 :  Write a SQL statement to make a list for the salesmen who either work for one or 
-- 				more customers or yet to join any of the customers. The customer may have placed, 
-- 				either one or more orders on or above order amount 2000 and must have a grade, or 
-- 				he may not have placed any order to the associated supplier

		SELECT DISTINCT SALESMAN.salesman_id, SALESMAN.name
		FROM SALESMAN
		LEFT JOIN CUSTOMER ON SALESMAN.salesman_id = CUSTOMER.salesman_id
		LEFT JOIN ORDERS ON CUSTOMER.customer_id = ORDERS.customer_id
		WHERE
			(
				(CUSTOMER.customer_id IS NOT NULL AND ORDERS.purch_amt >= 2000 AND CUSTOMER.grade IS NOT NULL)
				OR orders.customer_id IS NULL
			);

-- QUERY 15 :   Write a SQL statement to generate a list of all the salesmen who either work for one 
-- 				or more customers or have yet to join any of them. The customer may have placed 
-- 				one or more orders at or above order amount 2000, and must have a grade, or he 
-- 				may not have placed any orders to the associated supplier.

		SELECT DISTINCT SALESMAN.salesman_id, SALESMAN.name
		FROM SALESMAN
		LEFT JOIN CUSTOMER ON SALESMAN.salesman_id = CUSTOMER.salesman_id
		LEFT JOIN ORDERS ON CUSTOMER.customer_id = ORDERS.customer_id
		WHERE
			(
				(CUSTOMER.customer_id IS NOT NULL AND ORDERS.purch_amt >= 2000 AND CUSTOMER.grade IS NOT NULL)
				OR orders.customer_id IS NULL
			);


-- QUERY 16 :   Write a SQL statement to generate a report with the customer name, city, order no. 
--  			order date, purchase amount for only those customers on the list who must have a 
--				grade and placed one or more orders or which order(s) have been placed by the 
--				customer who neither is on the list nor has a grade.

		SELECT CUSTOMER.customer_id, CUSTOMER.name AS CUSTOMER_NAME, CUSTOMER.city AS CUSTOMER_CITY,
		CUSTOMER.grade, ORDERS.ord_no, ORDERS.purch_amt, ORDERS.ord_date FROM customer
		JOIN orders ON customer.customer_id = orders.customer_id
		WHERE (
			customer.grade IS NOT NULL AND CUSTOMER.customer_id IN(
				SELECT ORDERS.customer_id FROM ORDERS
			) 
			OR (
				CUSTOMER.customer_id IS NULL OR CUSTOMER.grade IS NULL
			)
		);
		
-- 			SELECT a.name, a.city, b.ord_no,
-- 			b.ord_date, b.purch_amt AS "Order Amount" 
-- 			FROM customer AS a 
-- 			FULL OUTER JOIN orders AS b 
-- 			ON a.customer_id = b.customer_id 
-- 			WHERE a.grade IS NOT NULL;
		

-- QUERY 17 :   Write a SQL query to combine each row of the salesman table with each row of the 
-- 				customer table

		SELECT CUSTOMER.*, SALESMAN.* FROM SALESMAN
		FULL OUTER JOIN CUSTOMER ON SALESMAN.salesman_id = CUSTOMER.salesman_id;


-- QUERY 18 :   Write a SQL statement to create a Cartesian product between salesperson and 
--  			customer, i.e. each salesperson will appear for all customers and vice versa for that 
-- 				salesperson who belongs to that city


		SELECT CUSTOMER.*, SALESMAN.* FROM SALESMAN
		CROSS JOIN customer
		WHERE SALESMAN.city IS NOT NULL;



-- QUERY 19 :   Write a SQL statement to create a Cartesian product between salesperson and 
-- 				customer, i.e. each salesperson will appear for every customer and vice versa for 
-- 				those salesmen who belong to a city and customers who require a grade


		SELECT CUSTOMER.*, SALESMAN.* FROM SALESMAN 
		CROSS JOIN customer
		WHERE SALESMAN.city IS NOT NULL 
		AND CUSTOMER.grade IS NOT NULL;





-- QUERY 20 :   Write a SQL statement to make a Cartesian product between salesman and 
-- 				customer i.e. each salesman will appear for all customers and vice versa for those 
-- 				salesmen who must belong to a city which is not the same as his customer and the 
-- 				customers should have their own grade


		SELECT CUSTOMER.*, SALESMAN.* FROM SALESMAN 
		CROSS JOIN customer
		WHERE SALESMAN.city IS NOT NULL AND customer.city IS NOT NULL AND
		SALESMAN.city <> CUSTOMER.city
		AND CUSTOMER.grade IS NOT NULL;





















