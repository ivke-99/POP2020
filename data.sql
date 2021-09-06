SET FOREIGN_KEY_CHECKS=0;

INSERT INTO Address VALUES (1, 'Gordon Street', '14', 'London', 'England', 0);
INSERT INTO Address VALUES (2, 'Hot Freight', '1', 'London', 'England', 1);
INSERT INTO Address VALUES (3, 'The Queens Tale', '16', 'London', 'England', 0);
INSERT INTO Address VALUES (4, '45th Avenue', '10', 'New York', 'America', 1);
INSERT INTO Address VALUES (5, '1st Avenue', '23', 'New York', 'America', 0);
INSERT INTO Address VALUES (6, 'Westbankof', '50', 'Wien', 'Austria', 0);
INSERT INTO Address VALUES (7, 'Stoglergasse', '12', 'Wien', 'Austria', 0);
INSERT INTO Address VALUES (8, 'Holochergasse', '5A', 'Linc', 'Austria', 0);
INSERT INTO Address VALUES (9, 'Wall Street', '4A', 'New York', 'America', 0);
INSERT INTO Address VALUES (10, '79th Avenue', '20', 'New York', 'America', 0);
INSERT INTO Address VALUES (11, 'Novosadska', '20', 'Temerin', 'Serbia', 0);
INSERT INTO Address VALUES (12, 'Petra Kocica', '14', 'Temerin', 'Serbia', 0);
INSERT INTO Address VALUES (13, 'Steilengasse', '25', 'Wien', 'Austria', 0);
INSERT INTO Address VALUES (14, '32th Avenue', '2', 'New York', 'America', 0);
INSERT INTO Address VALUES (15, 'Jevrejska', '20', 'Novi Sad', 'Serbia', 0);

INSERT clinic VALUES (1, 'Psychiatric clinic', 10, 0);
INSERT clinic VALUES (2, 'Cardiovascular clinic', 1, 0);
INSERT clinic VALUES (3, 'Respiratoral clinic', 1, 1);
INSERT clinic VALUES (4, 'Wellness clinic', 10, 0);

INSERT users VALUES ('1234567891234', 'Mark', 'Robinson', 'markrobinson@gmail.com', 0, 'mark123', 2, NULL, 3, 0);
INSERT users VALUES ('1235001235001', 'Geralt', 'Frankbauer', 'geralt123@gmail.com', 0, 'geralt123', 1, 2, 8, 0);
INSERT users VALUES ('1237007007001', 'John', 'Brown', 'johnbrown@gmail.com', 0, 'john1234', 1, 1, 9, 0);
INSERT users VALUES ('1608000886782', 'Hailey', 'Grot', 'haileygrot@gmail.com', 1, 'hailey123', 0, NULL, 12, 0);
INSERT users VALUES ('2003002003002', 'Mark', 'Hatovski', 'markhatovski@gmail.com', 1, 'markhatovski123', 1, 1, 1, 0);
INSERT users VALUES ('2422422422422', 'Jovana', 'Jovanovic', 'jovanajovanovic@email.com', 1, 'jovana123', 1, 1, 1, 0);
INSERT users VALUES ('4504502002002', 'John', 'Balgauer', 'johnbalgauer@mail.com', 1, 'john123', 0, NULL, 15, 0);
INSERT users VALUES ('4524524524521', 'Joana', 'Brown', 'joanabrown@gmail.com', 0, 'joana123', 1, 3, 6, 0);
INSERT users VALUES ('5004005004001', 'Geralt', 'Riviera', 'geraltriviera@gmail.com', 1, 'geralt123', 1, 1, 1, 0);
INSERT users VALUES ('5004005004007', 'Lara', 'Croft', 'laracroft@gmail.com', 1, 'lara123', 1, 1, 1, 0);

INSERT prescription VALUES (1, '3x6mg Oxycontin', '4524524524521', 1);
INSERT prescription VALUES (2, '2x0.25mg Levothyroxine', '2422422422422', 0);
INSERT prescription VALUES (3, '3x6mg Lipitor', '4524524524521', 0);
INSERT prescription VALUES (4, 'Vitamin C 200mg', '4524524524521', 0);

INSERT patient_prescription VALUES ('1608000886782', 3, 1);
INSERT patient_prescription VALUES ('1608000886782', 2, 1);
INSERT patient_prescription VALUES ('1608000886782', 3, 0);
INSERT patient_prescription VALUES ('4504502002002', 3, 1);
INSERT patient_prescription VALUES ('4504502002002', 2, 0);
INSERT patient_prescription VALUES ('4504502002002', 4, 0);
INSERT patient_prescription VALUES ('4504502002002', 3, 0);

INSERT appointment VALUES (2, '4524524524521', 0, NULL, 1, CAST('2022-08-26' AS Date));
INSERT appointment VALUES (3, '2422422422422', 1, '1608000886782', 0, CAST('2021-10-04' AS Date));
INSERT appointment VALUES (4, '4524524524521', 1, '4504502002002', 0, CAST('2021-10-04' AS Date));
INSERT appointment VALUES (5, '4524524524521', 0, NULL, 0, CAST('2021-11-05' AS Date));
INSERT appointment VALUES (6, '4524524524521', 0, NULL, 1, CAST('2021-11-29' AS Date));

SET FOREIGN_KEY_CHECKS=1;