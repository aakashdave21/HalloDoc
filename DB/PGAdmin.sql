-- SQL Excercises

-- Get the list of customer ids that have rented out movies that cost 7.99 or more 
-- so i can put them in a platinum category and send them coupons later

SELECT DISTINCT customer_id FROM payment WHERE amount >= 7.99;



-- RETURN MOVIE NAME ,RENTAL COST , REPLACEMENT COST OF MOVIE 
-- WITH HIGH VALUE FILMS HAVING RENTAL RATE MORE THAN 2.99 OR REPLACEMENT COST MORE THAN 19.99

SELECT title as "Movie Name" , rental_rate , replacement_cost 
FROM film 
WHERE rental_rate>2.99 OR replacement_cost>19.99;

-- title and other details of movies sorted by highest to lowest of replacement cost 
-- and rented out for 4-6 weeks also only 100 records

SELECT title as "Movie Name" , rental_rate , replacement_cost  , rental_duration
FROM film
WHERE rental_duration BETWEEN 4 AND 6
ORDER BY replacement_cost DESC
LIMIT 100;


-- list all movies that either have the rating 'G' or 'PG' and must be longer than 120 mins and 
-- should have word 'Action' anywhere in the description

SELECT title as "Movie Name",description , rating , length
FROM film
WHERE rating IN ('G','PG') AND length>120 AND description LIKE '%Action%';


/* the "actor" table has names of all actors. Can you tell me which actor first names are most common and how many records have it? */

SELECT first_name , COUNT(first_name) 
FROM actor
GROUP BY first_name
ORDER BY COUNT(first_name) DESC;


/*I want something to analyse the pattern between film lang and its rental rate Send me as data extract (report) that shows lang of each movie , 
title , and rental rate*/
  
 SELECT f.title , l.name as "Movie Language" , f.rental_rate
 FROM film f
 JOIN language l
 ON f.language_id = l.language_id;
 
 
 /* Show me a list of actors and the count of movies they have acted in 
 .Sort it in a way that the actor who hase done most movies at top*/
 
 
 SELECT CONCAT(a.first_name , ' ' ,a.last_name) as "actor name" , COUNT(fa.film_id)
 FROM actor a
 JOIN film_actor fa
 ON a.actor_id = fa.actor_id
 GROUP BY a.actor_id
 ORDER BY COUNT(fa.film_id) DESC;
 
 
 /*
 report that shows the diff ratings of 
 all movies and count of movies that have been rented for each rating
 */
 
 
--  select * from film -- film_id , rating
--  select * from rental -- rental_id , inventory_id , customer_id
--  select * from inventory -- inventory_id , film_id
 
 SELECT f.rating , COUNT(i.inventory_id) 
 FROM inventory i
 RIGHT JOIN film f ON f.film_id = i.film_id
 JOIN rental r ON r.inventory_id = i.inventory_id
 GROUP BY f.rating
 ORDER BY COUNT(i.inventory_id) DESC;
 
 
 /*The rental table has fields from rental_date and return date .
 We want to reach out customers whose rental duration eas 7 or more days and 
 send them an email.create the report that list down following cols:
 -Rental date
 -Return date
 -Rent duration
 -Customer First name
 -Customer Last Name
 -Customer Email
 
 */
 
 
 SELECT r.rental_date , r.return_date , AGE(r.return_date , r.rental_date) AS "rental duration" , c.first_name, c.last_name , c.email
 FROM rental r 
 JOIN customer c ON r.customer_id = c.customer_id
 WHERE r.return_date IS NOT NULL AND AGE(r.return_date , r.rental_date) > INTERVAL '7 days'
 ORDER BY "rental duration" DESC;
 
 
 
 /*The Film table has a title field containing name of movies. Some of the 
 movie names are pretty long .Practice SUBSTR function to do following
 */
 
 
 --Return string after 10th character
 SELECT title,SUBSTR(title,10) FROM film;
 --after 15
 SELECT title,LENGTH(title),SUBSTR(title,15) FROM film;
 --after 5 character , just 3 char
 SELECT title,SUBSTR(title,5,3) FROM film;
 
 --after 5 character , just 1 char
 SELECT title,SUBSTR(title,5,1) FROM film;
 
 
 
 /*
 create report that shows following cols:-
 CustomerName , Email , Total Rental , 
 Customer Category Based on ELITE - total rentals > 200, 
 Platinum >150
 Gold > 100
 Silver > 0
*/

SELECT CONCAT(c.first_name ,' ',c.last_name) as "Customer Name", c.email,
	SUM(amount) , 
	CASE 
		WHEN SUM(amount) > 200 THEN 'Elite'
		WHEN SUM(amount) > 150 THEN 'Platinum'
		WHEN SUM(amount) > 100 THEN 'Gold'
		WHEN SUM(amount) > 0 THEN 'Silver'
		
	END AS "Catgory"
FROM customer c
LEFT JOIN payment p ON c.customer_id =p.customer_id
GROUP BY c.customer_id;   


--Create view of prev query

CREATE VIEW cutomer_segments AS
	SELECT CONCAT(c.first_name ,' ',c.last_name) as "Customer Name", c.email,
	SUM(amount) , 
		CASE 
			WHEN SUM(amount) > 200 THEN 'Elite'
			WHEN SUM(amount) > 150 THEN 'Platinum'
			WHEN SUM(amount) > 100 THEN 'Gold'
			WHEN SUM(amount) > 0 THEN 'Silver'

		END AS "Catgory"
	FROM customer c
	LEFT JOIN payment p ON c.customer_id =p.customer_id
	GROUP BY c.customer_id;


-- PRIMARY KEY => UNIQUE + NOT NULL
-- FORIEGN KEY => REFERNCE TO SOME OTHER TABLE'S PRIMARY KEY , WE CAN HAVE MULTIPLE FORIEGN KEY IN ONE TABLE

CREATE DATABASE DB_NAME ;
DROP DATABASE IF EXISTS DB_NAME;

--SERIAL => AUTO INCREAMENT

DELETE FROM TB_NAME WHERE [] --> IF NO CONDITION PROVIDED ALL ROWS WILL BE DELETED ! carefull!!!

DROP TABLE TB_NAME CASCADE; --> THIS WILL DELETE THE TABLE ALONG WITH THE OBJECTS THAT ARE RELATED TO THAT TABLE


CREATE DATABASE mycommerce;

CREATE TABLE order_details 
(
	order_id INT PRIMARY KEY,
	customer_name VARCHAR(50) NOT NULL,
	product_name VARCHAR(50) NOT NULL,
	ordered_from VARCHAR(50) NOT NULL,
	order_amount NUMERIC(10,2),
	order_date DATE NOT NULL,
	delivery_date DATE
);

SELECT product_name , COUNT(order_id) AS "no. of orders",
SUM(order_amount) as "Total Sales"
FROM order_details
GROUP BY product_name;

ALTER TABLE order_details
RENAME COLUMN "customer_name" TO "customer_first_name"

ALTER TABLE order_details
ADD COLUMN "cancel_date" DATE NOT NULL;



select * from order_details;

--CASE

SELECT amount , 
CASE
	WHEN amount < 2 THEN 'LOW'
	WHEN amount < 5  AND amount >2 THEN 'MID'
	WHEN amount > 5 THEN 'HIGH'
END
FROM payment



--COALESCE -> to replace null with some specific value
SELECT * FROM address WHERE address2 IS NULL

SELECT address_id , address , address2 , 
COALESCE(address2 , 'house')
FROM address;


--NULLIF -> takes two arguments if they are same then it will return null else it will return first arg
SELECT NULLIF(10,10); --> NULL
SELECT NULLIF(10,154); -->10

SELECT amount as "USD" , 10/amount FROM payment; --> this will throw error bcz there are some amount which are 0;

SELECT amount as "USD" , 10/NULLIF(amount,0) FROM payment;  -->this will replace 0 amount with null so that 10/null will give null and it wont throw error

SELECT amount as "USD" , COALESCE(10/NULLIF(amount,0) , 0) FROM payment; --> to replace NULL with 0
 
 
--VIEW --> Virtual table

CREATE VIEW film_view AS 
SELECT * FROM film LIMIT 10; --you can write as complex query you want

SELECT * FROM film_view WHERE film_id =2; 
/*in reality view doesnt take any storage it is more like virtual table we can add rows or fetch from them put 
where conditions  */

DROP VIEW film_view;




