Drop Database FoodWaste;
Create Database FoodWaste;
use FoodWaste;
Create Table Access 
(
Id int not null Identity(1,1) Primary key,
AccessName varchar(255) not null
);

create table Zipcodes (
  Code nchar(4) primary key,
  City nvarchar(30) not null
);

Create Table Users 
(
Id int not null Identity(1,1) Primary key,
Email varchar(255) Not Null,
Password varchar(255) Not null,
Access int DEFAULT 2 FOREIGN KEY REFERENCES Access(Id),
PersonName Varchar(255) not null,
HomePhone nchar(10) not null,
WorkPhone nchar(10) not null,
Address varchar(255) not null,
Zipcode nchar(4) not null FOREIGN KEY REFERENCES Zipcodes(Code)
);

Create Table Category 
(
Id int not null Identity(1,1) Primary key,
CategoryName varchar(255) not null
);

Create Table Goods 
(
Id int not null Identity(1,1) Primary key,
ProductName Varchar(255) not null,
Price float not null,
Amount int not null,
AmountLimit int not null,
Category int Foreign Key references Category(id),
PicturePath varchar(255)
);


/*Create Table Orders
(
Id int not null Identity(1,1) Primary key,
PersonId int not null FOREIGN KEY REFERENCES Users(Id),
FullPrice float not null,
BoughtDato nchar(10) not null
)*/

Create table Basket 
(
Id int not null Identity(1,1) Primary key,
PersonId int not null FOREIGN KEY REFERENCES Users(Id),
ProductId int not null FOREIGN KEY REFERENCES Goods(Id),
Price float not null,
Amount int not null,
BasketDato nchar(14) not null,
Bought Varchar(5) DEFAULT  'false',
BoughtDato nchar(14),
)