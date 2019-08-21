CREATE TABLE Product(
	Name VARCHAR(30) NOT NULL,
	Bar_code VARCHAR(5) PRIMARY KEY,
	Size VARCHAR(3) NOT NULL,
	Color VARCHAR(20) NOT NULL,
	Price MONEY NOT NULL,
	Clothes_type VARCHAR(100) NOT NULL,
	Amount INTEGER NOT null
);
CREATE TABLE CLient_order(
	Order_ID INT PRIMARY KEY,
	Adress VARCHAR (200) NOT NULL,
    Order_status VARCHAR (200) CHECK (Order_status IN ('Complete', 'Processing', 'Canceled'))
);
CREATE TABLE Order_products(
	ID_order_product INT PRIMARY KEY,
	Amount INT NOT NULL,
	Bar_code VARCHAR(5) REFERENCES Product NOT NULL,
	ID_client_order INT REFERENCES Client_order NOT NULL
);
CREATE TABLE Client(
	Client_ID INT IDENTITY(1,1) PRIMARY KEY,
	Firstname VARCHAR(100) NOT NULL,
	Surname VARCHAR(100) NOT NULL,
	Order_ID INT REFERENCES Client_order NOT NULL
);