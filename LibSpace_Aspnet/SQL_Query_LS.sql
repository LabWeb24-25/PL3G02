﻿CREATE TABLE CodigoPostal(
		End_CodPostal	CHAR(8)	NOT NULL,
		End_Localidade	VARCHAR(40)	NOT NULL,
		PRIMARY KEY(End_CodPostal),
		CHECK (End_CodPostal LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]')
)

CREATE TABLE PreRequisita(
	IDReserva	INT	NOT NULL IDENTITY(1,1),
	IDLeitor	INT NOT NULL,
	Idlivro		INT NOT NULL,
	EstadoLevantamento	INT NOT NULL,
	PRIMARY KEY(IDReserva),
)

--Biblioteca(Id_Biblioteca, End_Morada, End_CodigoPostal (FK), Nome, Email, Telefone, Horario)
CREATE TABLE Biblioteca(
		Id_Biblioteca	INT	IDENTITY(1,1)	NOT NULL,
		End_Morada		VARCHAR(250)		NOT NULL,
		End_CodPostal	CHAR(8)		NOT NULL,
		Nome_Biblioteca	VARCHAR(100)	NOT NULL,
		Email			VARCHAR(125)	NOT NULL,
		Telefone		VARCHAR(9)		NOT NULL,
		Horario				TEXT			NOT NULL,
		PRIMARY KEY (Id_Biblioteca),
		CHECK (End_CodPostal LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]'),
		FOREIGN KEY (End_CodPostal) References CodigoPostal(End_CodPostal)
)

--Perfil (ID_perfil (PK), End_Morada, End_CodPostal (FK), Nome_Perfil, DataNascimento_Perfil, Apelido, Img_Perfil)
CREATE TABLE Perfil(
		ID_perfil				INT		IDENTITY(1,1)	NOT NULL,
		End_Morada				VARCHAR(100)	NOT NULL,
		End_CodPostal			CHAR(8)			NOT NULL,
		Nome_Perfil				VARCHAR(50)		NOT NULL,
		DataNascimento_Perfil	DATE			NOT NULL,
		Apelido					VARCHAR(50)		NOT NULL,
		Img_Perfil				VARCHAR(300)	NOT NULL,
		AspNetUserId			NVARCHAR(450)	NOT NULL,
		PRIMARY KEY (ID_perfil),
		CHECK (End_CodPostal LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]'),
		CHECK (DATEDIFF(YEAR, DataNascimento_Perfil, GETDATE()) >= 16),
		FOREIGN KEY (End_CodPostal) References CodigoPostal(End_CodPostal),
		FOREIGN KEY (AspNetUserId) REFERENCES AspNetUsers(Id) -- Chave estrangeira para AspNetUsers
)

--Editora (ID_Editora (PK), Nome_Editora, Info_Editora, Img_Editora)
CREATE TABLE Editora(
		ID_Editora	INT		IDENTITY(1,1)	NOT NULL,
		Nome_Editora	VARCHAR(100)	NOT NULL,
		Info_Editora	TEXT,
		Img_Editora		VARCHAR(300)	NOT NULL,
		PRIMARY KEY (ID_Editora),
)

--Generos (ID_Generos (PK), Nome_Generos)
CREATE TABLE Generos(
		ID_Generos	INT		IDENTITY(1,1)	NOT NULL,
		Nome_Generos	VARCHAR(20)			NOT NULL,
		PRIMARY KEY (ID_Generos),
)

CREATE TABLE Pais(
		ID_Pais	INT		IDENTITY(1,1)	NOT NULL,
		Nome_Pais	VARCHAR(50)	NOT NULL,
		PRIMARY KEY (ID_Pais),
)


--Autor (ID_Autor (PK), Nome_Autor, DataNascimento_Autor, Pseudonimo, DataFalecimento_Autor, Foto_Autor, Bibliografia, Id_Pais)
CREATE TABLE Autor(
		ID_Autor	INT		IDENTITY(1,1) NOT NULL,
		Nome_Autor	VARCHAR(120)		NOT NULL,
		Data_Nascimento	DATE		NOT NULL,
		Pseudonimo		VARCHAR(50),
		Data_Falecimento DATE,
		Foto_Autor		VARCHAR(250)	NOT NULL,
		Bibliografia	TEXT		NOT NULL,	--Guarda até 65535 caracteres
		Id_Lingua		INT			NOT NULL,
		PRIMARY KEY (ID_Autor),
		CHECK (DATEDIFF(YEAR, Data_Nascimento, GETDATE()) >= 16),
		CHECK (Data_Falecimento>Data_Nascimento),
		FOREIGN KEY (Id_Lingua) REFERENCES Pais (ID_Pais)
)

--Livro (ISBN (PK), Data_Edicao, Idioma, num_exemplares, Capa_IMG, Sinopse, Titulo_Livros)
CREATE TABLE Livros(
		ID_Livro	INT		IDENTITY(1,1) NOT NULL,
		ISBN	VARCHAR(20)	NOT NULL,
		DataEdicao	DATE	NOT NULL,
		Titulo_Livros	VARCHAR(100)	NOT NULL,
		Num_Exemplares	INT		NOT NULL,
		Capa_IMG		VARCHAR(250)	NOT NULL,
		ID_Editora	INT		NOT NULL,
		ID_Lingua	INT		NOT NULL,
		Sinopse			TEXT		NOT NULL,  --Guarda até 65535 caracteres
		ID_autor	INT	NOT NULL,
		ID_Generos	INT	NOT NULL,
		PRIMARY KEY (ID_Livro),
		FOREIGN KEY (ID_Editora) REFERENCES Editora(ID_Editora),
		FOREIGN KEY (ID_autor) REFERENCES Autor(ID_Autor),
		FOREIGN KEY (Id_Lingua) REFERENCES Pais (ID_Pais),
		FOREIGN KEY (ID_Generos) REFERENCES Generos(ID_Generos),
		CHECK(Num_Exemplares>=0),
)





-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--   Relacionamentos

--Favorito (ID_Leitor (PK), ISBN (PK, FK))
CREATE TABLE Favorito(
		ID_Livro	INT	NOT NULL,
		ID_Leitor	INT	NOT NULL,
		PRIMARY KEY (ID_Livro, ID_Leitor),
		FOREIGN KEY (ID_Livro) REFERENCES Livros(ID_Livro),

)




--Requisita (Id_Leitor (PK, FK), Id_BibliotecarioRecetor (PK, FK), Id_BibliotecarioRemetente (FK), Data_Requisicao (PK), Data_PreEntrega, Data_Entrega)
CREATE TABLE Requisita (
		ID_Leitor	INT	NOT NULL,
		ID_BibliotecarioRecetor	INT	NOT NULL,
		ID_BibliotecarioRemetente	INT,
		ID_Livro		INT	NOT NULL,
		Data_Requisicao	DATETIME	NOT NULL,
		Data_PrevEntrega	DATE	NOT NULL,
		Data_Entrega	DATETIME	NOT NULL,
		PRIMARY KEY (ID_Leitor, ID_Livro, Data_Requisicao),
		FOREIGN KEY (ID_Livro) REFERENCES Livros(ID_Livro),
)


--Inserir_Livro (Id_Bibliotecario (FK), ISBN (PK, FK))
CREATE TABLE Inserir_Livro (
		ID_Bibliotecario	INT	NOT NULL,
		ID_Livro		INT	NOT NULL,
		PRIMARY KEY (ID_Livro),
		FOREIGN KEY (ID_Livro) REFERENCES Livros(ID_Livro),
)


--Bloquear (ID_Admin, ID_User (PK), Motivo_Bloquear, Data_Bloqueio)
CREATE TABLE Bloquear (
		ID_Admin	INT		NOT NULL,
		ID_User		INT		NOT NULL,
		Motivo_Bloquear	TEXT	NOT NULL,
		Data_Bloqueio	DATE	NOT NULL,
		PRIMARY KEY (ID_User),
)