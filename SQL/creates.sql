CREATE TABLE Product(
	Name VARCHAR(30) NOT NULL,
	Bar_code INT IDENTITY(1,1) PRIMARY KEY,
	Size VARCHAR(3) NOT NULL,
	Color VARCHAR(20) NOT NULL,
	Price MONEY NOT NULL,
	Clothes_type VARCHAR(100) NOT NULL,	
	Amount_Reserved INT NOT NULL,
	Amount_To_Reserve INT NOT NULL
);
CREATE TABLE Client_order(
	ID_client_order INT PRIMARY KEY,
	Adress VARCHAR (200) NOT NULL,
    Order_status VARCHAR (200) CHECK (Order_status IN ('Complete', 'Processing', 'Canceled'))
);
CREATE TABLE Order_products(
	ID_order_product INT IDENTITY(1,1) PRIMARY KEY,
	Amount INT NOT NULL,
	Bar_code INT REFERENCES Product NOT NULL,
	ID_client_order INT REFERENCES Client_order NOT NULL
);
CREATE TABLE Client(
	Client_ID INT IDENTITY(1,1) PRIMARY KEY,
	Firstname VARCHAR(100) NOT NULL,
	Surname VARCHAR(100) NOT NULL,
	ID_client_order INT REFERENCES Client_order NOT NULL
);