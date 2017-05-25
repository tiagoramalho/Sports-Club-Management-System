USE CluSys;
GO

INSERT INTO Modality VALUES
  ('Futebol', '1914'),
  ('Basquetebol', '1927'),
  ('Andebol', '1939'),
  ('Hóquei em patins', '1933'),
  ('Voleibol', '1947'),
  ('Futsal', '1914');

INSERT INTO Class VALUES
  ('Futebol', 'Petizes', 4, 6),
  ('Futebol', 'Traquinas', 7, 8),
  ('Futebol', 'Benjamins', 9, 10),
  ('Futebol', 'Infantis', 11, 12),
  ('Futebol', 'Iniciados', 13, 14),
  ('Futebol', 'Juvenis', 15, 16),
  ('Futebol', 'Juniores', 17, 18),
  ('Futebol', 'Seniores', 19, 50),
  ('Basquetebol', 'Infantil A', 4, 10),
  ('Basquetebol', 'Infantil B', 11, 12),
  ('Basquetebol', 'Iniciado', 13, 14),
  ('Basquetebol', 'Juvenil', 15, 16),
  ('Basquetebol', 'Juniores', 17, 18),
  ('Basquetebol', 'Seniores', 19, 50),
  ('Andebol', 'Infantil A', 4, 10),
  ('Andebol', 'Infantil B', 11, 12),
  ('Andebol', 'Iniciado', 13, 14),
  ('Andebol', 'Juvenil', 15, 16),
  ('Andebol', 'Juniores', 17, 18),
  ('Andebol', 'Seniores', 19, 50);

INSERT INTO Athlete VALUES
  ('1231', 'Tiago', 'Antunes', 'Pereira', '19960622', '/assets/img/2.jpg', '931111111', 'ramalho@ua.pt', HASHBYTES('SHA2_512', 'ramalho'), 'estudante', 'destro', 'Futebol'),
  ('1232', 'Andre', 'Gomes', 'Gomes', '19960402', '/assets/img/3.jpg', '931111112', 'andre@ua.pt', HASHBYTES('SHA2_512', 'andre'), 'estudante', 'destro', 'Futebol'),
  ('1233', 'Joao', 'Tiago', 'Branquinho', '19930402', '/assets/img/14.jpg', '931111113', 'joao@ua.pt', HASHBYTES('SHA2_512', 'joao'), 'professor', 'esquerdino', 'Futebol'),
  ('1234', 'Pedro', 'Jorge', 'Moreira', '19960622', '/assets/img/17.jpg', '931111114', 'pedro@ua.pt', HASHBYTES('SHA2_512', 'pedro'), 'estudante', 'destro', 'Futebol'),
  ('1235', 'Gabriel', 'Soares', 'Patricio', '19960402', '/assets/img/25.jpg', '931111115', 'gabriel@ua.pt', HASHBYTES('SHA2_512', 'gabriel'), 'engenheiro agronomo', 'destro', 'Futebol'),
  ('1236', 'Jorge', 'Miguel', 'Silva', '19930402', '/assets/img/27.jpg', '931111116', 'jorge@ua.pt', HASHBYTES('SHA2_512', 'jorge'), 'estudante', 'destro', 'Basquetebol'),
  ('1237', 'Samuel', 'Antunes', 'Biscaia', '19960622', '/assets/img/37.jpg', '931111117', 'samuel@ua.pt', HASHBYTES('SHA2_512', 'samuel'), 'estudante', 'destro', 'Basquetebol'),
  ('1238', 'Joao', 'Ferreira', 'Gomes', '19960402', '/assets/img/64.jpg', '931111118', 'ferreira@ua.pt', HASHBYTES('SHA2_512', 'ferreira'), 'estudante', 'destro', 'Basquetebol'),
  ('1239', 'Diogo', 'Filipe', 'Catraio', '19930402', '/assets/img/66.jpg', '931111119', 'diogo@ua.pt', HASHBYTES('SHA2_512', 'diogo'), 'estudante', 'destro', 'Basquetebol'),
  ('1241', 'Cilio', NULL, 'Sousa', '19960622', '/assets/img/68.jpg', '931111121', 'cilio@ua.pt', HASHBYTES('SHA2_512', 'cilio'), 'estudante', 'destro', 'Futebol'),
  ('1242', 'Jose', 'Pedro', 'Ribeiro', '19960402', '/assets/img/74.jpg', '931111122', 'jose@ua.pt', HASHBYTES('SHA2_512', 'jose'), 'estudante', 'esquerdino', 'Futebol'),
  ('1243', 'Fabio', 'Miguel', 'Maio', '19930402', '/assets/img/77.jpg', '931111123', 'fabio@ua.pt', HASHBYTES('SHA2_512', 'fabio'), 'vendedor', 'esquerdino', 'Basquetebol'),
  ('1244', 'Filipe', NULL, 'Santos', '19960622', '/assets/img/79.jpg', '931111124', 'filipe@ua.pt', HASHBYTES('SHA2_512', 'filipe'), 'professor', 'esquerdino', 'Basquetebol'),
  ('1245', 'Nuno', 'Silva', 'Cruz', '19960402', '/assets/img/85.jpg', '931111125', 'nuno@ua.pt', HASHBYTES('SHA2_512', 'nuno'), 'arquiteto', 'esquerdino', 'Futebol'),
  ('1246', 'Cristiano', 'Rolando', 'Santos', '19930402', '/assets/img/7.jpg', '931111126', 'cristiano@ua.pt', HASHBYTES('SHA2_512', 'cristiano'), 'estudante', 'destro', 'Basquetebol'),
  ('1247', 'Tiago', 'André Ribeiro', 'Ramalho', '19930402', '/assets/img/61.jpg', '931111127', 'ribeiro@ua.pt', HASHBYTES('SHA2_512', 'ribeiro'), 'designer', 'destro', 'Futebol'),
  ('1248', 'Ricardo', NULL, 'Jesus', '19930402', '/assets/img/35.jpg', '931111128', 'ricardo@ua.pt', HASHBYTES('SHA2_512', 'ricardo'), 'estudante', 'destro', 'Futebol');

INSERT INTO Physiotherapist VALUES
  ('12123', 'Joao', 'Martins', 'Cabrita', '19940321', NULL, '911343454', 'cabrita@ua.pt', HASHBYTES('SHA2_512', 'cabrita')),
  ('121231', 'Rui', 'Filipe', 'Martins', '19940311', NULL, '911343455', 'rui@ua.pt', HASHBYTES('SHA2_512', 'rui'));

INSERT INTO Coach VALUES
  ('123123', 'Antonio', 'Cruz', 'Ferreira', '20121212', NULL, '938445155', 'antonio@gmail.com', HASHBYTES('SHA2_512', 'antonio'), 'Professor');

INSERT INTO Trains VALUES
  ('Futebol', 'Seniores', '123123', 2017);

<<<<<<< HEAD:BD/SQL files/Insert_Tables.sql
INSERT INTO MedicalEvaluation VALUES 
  ('70.2', '1.70', 'Torci o pé ao cair de um salto', '20150622','20150630', '20150629', '1247', '12123'),
=======
INSERT INTO MedicalEvaluation VALUES
  ('70.2', '1.70', 'Torci o pé ao cair de um salto', '20150622', '20150630', '20150629', '1247', '12123'),
>>>>>>> cf9ed6e570148bf289236bfe3e573bf2256c2a68:BD/SQL files/InsertTables.sql
  ('81.5', '1.85', NULL, '20160518', '20160601', NULL, '1247', '12123'),
  ('82.0', '1.85', NULL, '20160801', '20160901', NULL, '1247', '12123'),
  ('82.5', '1.90', NULL, '20170511', NULL, NULL, '1247', '12123'),
  ('72.5', '1.70', NULL, '20170506', NULL, NULL, '1248', '12123'),
  ('50.5', '1.60', NULL, '20170411', '20170511', NULL, '1245', '12123'),
  ('82.5', '1.84', NULL, '20170421', NULL, '20170421', '1241', '12123');

INSERT INTO EvaluationSession (EvalId, Date) VALUES
  ('1', '20150622'),
  ('1', '20150624'),
  ('1', '20150626'),
  ('2', '20160518'),
  ('2', '20160522'),
  ('2', '20160526'),
  ('2', '20160601'),
  ('3', '20160801'),
  ('3', '20160811'),
  ('3', '20160821'),
  ('3', '20160901'),
  ('4', '20170511'),
  ('4', '20170513');

INSERT INTO MajorProblem VALUES
  ('Dores no ombro', '2', '1'),
  ('Clavícula deslocada', '2', '1'),
  ('Clavícula deslocada', '2', '2'),
  ('Clavícula com poucas alteraçoes', '2', '3'),
  ('Pé partido', '3', '1'),
  ('Ligamentos rasgados', '3', '2'),
  ('Pé com pouca mobilidade', '3', '3'),
  ('Corrigir estabilidade', '3', '3'),
  ('Entorse de 2 grau', '1', '1'),
  ('Dores na zona do aquiles', '1', '2');

INSERT INTO BodyChartView VALUES
  ('/assets/views/1.png', '1'),
  ('/assets/views/2.png', '2'),
  ('/assets/views/3.png', '3'),
  ('/assets/views/4.png', '4');

INSERT INTO Annotation VALUES
  ('ArrowLeft', 'desviado para a esquerda'),
  ('ArrowUp', 'desviado para cima'),
  ('ArrowRight', 'desviado para a direita'),
  ('ArrowDown', 'desviado para baixo'),
  ('RotateLeft', 'desviado com rotação para a esquerda'),
  ('RotateRight', 'desviado com rotação para a direita');