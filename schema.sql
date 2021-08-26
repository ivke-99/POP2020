SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS Patient_Prescription;
DROP TABLE IF EXISTS Prescription;
DROP TABLE IF EXISTS Appointment;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Clinic;
DROP TABLE IF EXISTS Address;
SET FOREIGN_KEY_CHECKS = 1;
CREATE TABLE Address
(
	id int auto_increment,
	street varchar(255) NOT NULL,
	number varchar(10) NOT NULL,
	town varchar(255) NOT NULL,
	country varchar(255) NOT NULL,
	deleted tinyint default 0,
	primary key(id)
);


CREATE TABLE Clinic
(
	id int auto_increment,
	clinic_name varchar(255) not null,
	address_id int not null,
	deleted tinyint default 0,
	primary key(id),
	foreign key (address_id) references Address(id)
);


CREATE TABLE Users
(
	pin char(13) not null primary key,	
	first_name varchar(255) not null, 
	last_name varchar(255) not null,
	email varchar(255) not null,
	gender bit not null,
	password varchar(255) not null,
	role tinyint not null,
	clinic_id int default null,
	address_id int not null,
	deleted tinyint default 0,
	foreign key (clinic_id) references Clinic (id),
	foreign key (address_id) references Address (id)
);


CREATE TABLE Prescription
(
	id int auto_increment,
	description varchar(255) not null,
	doctor_pin char(13) not null,
	deleted tinyint default 0,
	primary key(id),
	foreign key (doctor_pin) references Users (pin)
);


CREATE TABLE Appointment
(
	id int auto_increment,
	doctor_pin char(13) not null,
	status tinyint,
	patient_pin char(13),
	deleted tinyint default 0,
	date_of_appointment date not null,
	primary key(id),
	foreign key (doctor_pin) references Users (pin),
	foreign key (patient_pin) references Users (pin)
);

CREATE TABLE Patient_Prescription
(
	patient_pin char(13) not null,
	prescription_id int not null,
	deleted tinyint default 0,
	foreign key (patient_pin) references Users (pin),
	foreign key (prescription_id) references Prescription (id)
);