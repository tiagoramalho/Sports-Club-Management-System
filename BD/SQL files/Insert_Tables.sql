USE CluSys;

INSERT INTO  Modality VALUES
	('Futebol', '1914'),
	('Basquetebol', '1927'),
	('Andebol', '1939');

INSERT INTO Class Values
	('Futebol', 'Petizes', 4,6),
	('Futebol', 'Traquinas', 7,8),
	('Futebol', 'Benjamins', 9,10),
	('Futebol', 'Infantis', 11,12),
	('Futebol', 'Iniciados',13,14),
	('Futebol', 'Juvenis', 15,16),
	('Futebol', 'Juniores', 17,18),
	('Futebol', 'Seniores', 19, 50),
	('Basquetebol', 'Infantil A',4,10),
	('Basquetebol', 'Infantil B',11,12),
	('Basquetebol', 'Iniciado',13,14),
	('Basquetebol', 'Juvenil',15,16),
	('Basquetebol', 'Juniores',17,18),
	('Basquetebol', 'Seniores',19,50),
	('Andebol', 'Infantil A',4,10),
	('Andebol', 'Infantil B',11,12),
	('Andebol', 'Iniciado',13,14),
	('Andebol', 'Juvenil',15,16),
	('Andebol', 'Juniores',17,18),
	('Andebol', 'Seniores',19,50);

INSERT INTO Athlete Values
	('123', 'Tiago', '19960622', NULL, '931111111', 'ramalho@ua.pt', 'tiago', 'estudante', 'destro', 'Futebol'),
	('1234', 'Andre', '19960402', NULL, '931111111', 'andre@ua.pt', 'andre', 'estudante', 'destro', 'Futebol'),
	('1236', 'Joao', '19930402', NULL, '931111111', 'joao@ua.pt', 'joao', 'estudante', 'destro', 'Basquetebol');

INSERT INTO Physiotherapist Values
	('12123', 'Cabrita', '19940321', NULL, '911343454', 'cabrita@ua.pt', 'cabrita');

INSERT INTO Coach Values
	('123123', 'Antonio', '20121212', NULL, '938445155', 'antonio@gmail.com', 'Antonio', 'Professor');

INSERT INTO Trains Values
	('Futebol', 'Seniores', '123123', 1);