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
	('1231', 'Tiago', 'Antunes', 'Pereira', '19960622', NULL, '931111111', 'ramalho@ua.pt', 'ramalho', 'estudante', 'destro', 'Futebol'),
	('1232', 'Andre', 'Gomes', 'Gomes', '19960402', NULL, '931111112', 'andre@ua.pt', 'andre', 'estudante', 'destro', 'Futebol'),
	('1233', 'Joao', 'Tiago', 'Branquinho', '19930402', NULL, '931111113', 'joao@ua.pt', 'joao', 'professor', 'esquerdino', 'Futebol'),
	('1234', 'Pedro', 'Jorge', 'Moreira', '19960622', NULL, '931111114', 'pedro@ua.pt', 'pedro', 'estudante', 'destro', 'Futebol'),
	('1235', 'Gabriel', 'Soares', 'Patricio', '19960402', NULL, '931111115', 'gabriel@ua.pt', 'gabriel', 'engenheiro agronomo', 'destro', 'Futebol'),
	('1236', 'Jorge', 'Miguel', 'Silva', '19930402', NULL, '931111116', 'jorge@ua.pt', 'jorge', 'estudante', 'destro', 'Basquetebol'),
	('1237', 'Samuel', 'Antunes', 'Biscaia', '19960622', NULL, '931111117', 'samuel@ua.pt', 'samuel', 'estudante', 'destro', 'Basquetebol'),
	('1238', 'Joao', 'Ferreira', 'Gomes', '19960402', NULL, '931111118', 'ferreira@ua.pt', 'ferreira', 'estudante', 'destro', 'Basquetebol'),
	('1239', 'Diogo', 'Filipe', 'Catraio', '19930402', NULL, '931111119', 'diogo@ua.pt', 'diogo', 'estudante', 'destro', 'Basquetebol'),
	('1241', 'Cilio', NULL, 'Sousa', '19960622', NULL, '931111121', 'cilio@ua.pt', 'cilio', 'estudante', 'destro', 'Futebol'),
	('1242', 'Jose', 'Pedro', 'Ribeiro', '19960402', NULL, '931111122', 'jose@ua.pt', 'jose', 'estudante', 'esquerdino', 'Futebol'),
	('1243', 'Fabio', 'Miguel', 'Maio', '19930402', NULL, '931111123', 'fabio@ua.pt', 'fabio', 'vendedor', 'esquerdino', 'Basquetebol'),
	('1244', 'Filipe', NULL, 'Santos', '19960622', NULL, '931111124', 'filipe@ua.pt', 'filipe', 'professor', 'esquerdino', 'Basquetebol'),
	('1245', 'Nuno', 'Silva', 'Cruz', '19960402', NULL, '931111125', 'nuno@ua.pt', 'nuno', 'arquiteto', 'esquerdino', 'Futebol'),
	('1246', 'Cristiano', 'Rolando', 'Santos', '19930402', NULL, '931111126', 'cristiano@ua.pt', 'cristiano', 'estudante', 'destro', 'Basquetebol'),
	('1247', 'Tiago', 'André Ribeiro', 'Ramalho', '19930402', NULL, '931111127', 'ribeiro@ua.pt', 'ribeiro', 'designer', 'destro', 'Futebol'),
	('1248', 'Ricardo', NULL, 'Jesus', '19930402', NULL, '931111128', 'ricardo@ua.pt', 'ricardo', 'estudante', 'destro', 'Futebol');


INSERT INTO Physiotherapist Values
	('12123', 'Joao', 'Martins', 'Cabrita', '19940321', NULL, '911343454', 'cabrita@ua.pt', 'cabrita'),
	('121231', 'Rui', 'Filipe', 'Martins', '19940311', NULL, '911343454', 'rui@ua.pt', 'rui');

INSERT INTO Coach Values
	('123123', 'Antonio', 'Cruz', 'Ferreira', '20121212', NULL, '938445155', 'antonio@gmail.com', 'Antonio', 'Professor');

INSERT INTO Trains Values
	('Futebol', 'Seniores', '123123', 1);