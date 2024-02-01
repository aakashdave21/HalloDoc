-- Create a stored procedure that checks the freight value
-- Assignment 3 -> Question 1 using PostgreSql
CREATE OR REPLACE PROCEDURE check_freight()
LANGUAGE plpgsql AS $$
DECLARE
    avg_freight numeric;
BEGIN
    -- Calculate the average Freight for the specified customer
    SELECT AVG(Freight) INTO avg_freight
    FROM orders
    WHERE customerid = NEW.customerid;

    -- Check if the new Freight value exceeds the average
    IF NEW.freight > avg_freight THEN
        RAISE EXCEPTION 'Freight exceeds average for customer %', NEW.customerid;
    END IF;
END;
$$;

-- Step 3: Create a trigger that will call the trigger function before every Update and Insert command
CREATE TRIGGER before_orders_update_insert
BEFORE INSERT OR UPDATE
ON salesorder
FOR EACH ROW
EXECUTE FUNCTION check_freight();
