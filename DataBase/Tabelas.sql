USE MASTER
GO

CREATE DATABASE Projeto_Lab
Go

Use Projeto_Lab 
Go

-- ENTIDADES  -----------------------------
CREATE TABLE Perfil(
		ID_perfil	INT		IDENTITY(1,1)	NOT NULL,
		End_Morada	VARCHAR(100)	NOT NULL,
		End_CodPostal	CHAR(8)		NOT NULL,
		End_Localidade	VARCHAR(50)	NOT NULL,
		Nome_Perfil		VARCHAR(50)	NOT NULL,
		DataNascimento_Perfil	DATE	NOT NULL,
		Apelido			VARCHAR(50)	NOT NULL,
		Img_Perfil		VARCHAR(250)	NOT NULL,
		PRIMARY KEY (ID_perfil),
		CHECK (End_CodPostal LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]')
)

CREATE TABLE Editora(
		ID_Editora	INT		IDENTITY(1,1)	NOT NULL,
		Nome_Editora	VARCHAR(100)	NOT NULL,
		Info_Editora	VARCHAR(100),
		Img_Editora		VARCHAR(250)	NOT NULL,
		PRIMARY KEY (ID_Editora),
)

CREATE TABLE Generos(
		ID_Generos	INT		IDENTITY(1,1)	NOT NULL,
		Nome_Generos	VARCHAR(20)			NOT NULL,
		PRIMARY KEY (ID_Generos)
)

CREATE TABLE Livros(
		ISBN	BIGINT	NOT NULL,
		DataEdicao	DATE	NOT NULL,
		Titulo_Livros	VARCHAR(100)	NOT NULL,
		Idioma		VARCHAR(50)		NOT NULL,
		Num_Exemplares	INT		NOT NULL,
		Capa_IMG		VARCHAR(250)	NOT NULL,
		Sinopse			TEXT		NOT NULL,  --Guarda até 65535 caracteres
		PRIMARY KEY (ISBN),
		CHECK(Num_Exemplares>=0),

)

CREATE TABLE Autor(
		ID_Autor	INT		IDENTITY(1,1) NOT NULL,
		Nome_Autor	VARCHAR(120)		NOT NULL,
		--Nacionalidade	VARCHAR(50)		NOT NULL,
		Data_Nascimento	DATE		NOT NULL,
		Pseudonimo		VARCHAR(50),
		Data_Falecimento DATE,
		Foto_Autor		VARCHAR(250)	NOT NULL,
		Bibliografia	TEXT		NOT NULL,	--Guarda até 65535 caracteres
		PRIMARY KEY (ID_Autor)
)

CREATE TABLE Pais(
		ID_Pais	INT		IDENTITY(1,1)	NOT NULL,
		Nome_Pais	VARCHAR(50)	NOT NULL,
		PRIMARY KEY (ID_Pais),
)







--PARA PERFIL AUTOR REQUISITA BLOQUEAR

--CREATE TRIGGER verificar_data_edicao
--BEFORE INSERT ON Livros
--FOR EACH ROW
--BEGIN
--    IF NEW.DataEdicao >= CURDATE() THEN
--        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Data de edição não pode ser no futuro.';
--    END IF;
--END;