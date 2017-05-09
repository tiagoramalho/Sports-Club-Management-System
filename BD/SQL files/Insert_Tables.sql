USE CluSys;

INSERT INTO  Modality VALUES
	('Futebol', '1914'),
	('Basquetebol', '1927'),
	('Andebol', '1939'),
	('Hóquei em patins', '1933'),
	('Voleibol', '1947'),
	('Futsal','1914');

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

INSERT INTO MedicalEvaluation VALUES 
	('81.5', '1.85', NULL, '20170412', '20170509', NULL, '1247', '12123'),
	('82.5', '1.87', NULL, '20170511', NULL, NULL, '1247', '12123'),
	('72.5', '1.70', NULL, '20170506', NULL, NULL, '1248', '12123'),
	('50.5', '1.60', NULL, '20170411', '20170511', NULL, '1245', '12123'),
	('82.5', '1.84', NULL, '20170521', NULL, NULL, '1241', '12123');

INSERT INTO EvaluationSession VALUES
    ('1', '20170422'),
    ('1', '20170423'),
    ('1', '20170424'),
    ('2', '20170424'),
	('3', '20170422');

INSERT INTO MajorProblem VALUES 
	('Dores no ombro', '1', '1'),
	('Clavícula deslocada', '1', '1'),
	('Clavícula deslocada', '1', '2'),
	('Clavícula com poucas alteraçoes', '1', '3'),
	('Pé partido', '2', '4');

INSERT INTO TreatmentPlan Values
	('Repouso total', null, '2','4','5'),
	('Fazer gelo', 'Aliviar a dor no ombro', '1', '1', '1'),
	('Ligadura na zona do ombro e clavicula', null, '1', '1','2');

	
