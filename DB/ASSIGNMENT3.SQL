-- Create query for Department table
CREATE TABLE Department
(
	dept_id SERIAL PRIMARY KEY,
	dept_name VARCHAR(40) NOT NULL
);

-- Create query for Employee table
CREATE TABLE Employee
(
	emp_id SERIAL PRIMARY KEY,
	dept_id INT,
	mngr_id INT,
	emp_name VARCHAR(40) NOT NULL,
	salary DECIMAL(18,2) NOT NULL,
	FOREIGN KEY(dept_id) REFERENCES Department(dept_id),
	FOREIGN KEY(mngr_id) REFERENCES Employee(emp_id)
);

-- INSERT QUERY
INSERT INTO Department (dept_name) VALUES
('Sales'),
('Finance'),
('Human Resources'),
('IT'),
('Marketing'),
('Engineering'),
('Customer Service'),
('Research and Development'),
('Legal'),
('Administration');

INSERT INTO Employee (dept_id, mngr_id, emp_name, salary) VALUES
(1, NULL, 'John Doe', 60000.00),
(1, 1, 'Jane Smith', 55000.00),
(2, 1, 'Bob Johnson', 70000.00),
(2, 3, 'Alice Brown', 60000.00),
(3, 3, 'Charlie Davis', 65000.00),
(4, NULL, 'David Miller', 75000.00),
(5, 5, 'Eva Wilson', 62000.00),
(5, 6, 'Frank White', 68000.00),
(6, 6, 'Grace Jones', 80000.00),
(7, NULL, 'Harry Taylor', 58000.00);

SELECT

-- Q-1 : write a SQL query to find Employees who have the biggest salary in their Department
	
	SELECT e.emp_name, e.DEPT_ID, e.salary FROM employee AS e
	WHERE e.salary IN (SELECT max(E.salary)
     FROM employee AS E
     GROUP BY E.DEPT_ID);
	 
	 
-- Q-2 : write a SQL query to find Departments that have less than 3 people in it
	 SELECT COUNT(employee.emp_id) AS COUNTS, department.dept_name
     FROM employee
	 JOIN department ON department.dept_id = employee.dept_id
     GROUP BY employee.DEPT_ID,department.dept_name
	 HAVING COUNT(employee.emp_id) < 3
	 ORDER BY COUNT(employee.emp_id);
	 
-- Q-3 : write a SQL query to find All Department along with the number of people there
	 SELECT COUNT(employee.emp_id) AS NO_OF_PEOPLE, department.dept_name
     FROM employee
	 JOIN department ON department.dept_id = employee.dept_id
     GROUP BY employee.DEPT_ID,department.dept_name
	 
-- Q-4 : write a SQL query to find All Department along with the total salary there
	 SELECT SUM(employee.salary) AS Total_salary, department.dept_name
     FROM employee
	 JOIN department ON department.dept_id = employee.dept_id
     GROUP BY employee.DEPT_ID,department.dept_name
