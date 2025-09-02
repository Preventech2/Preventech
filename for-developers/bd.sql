-- Database generated with pgModeler (PostgreSQL Database Modeler).
-- pgModeler version: 1.1.0-beta1
-- PostgreSQL version: 16.0
-- Project Site: pgmodeler.io
-- Model Author: ---

-- Database creation must be performed outside a multi lined SQL file. 
-- These commands were put in this file only as a convenience.
-- 
-- object: new_database | type: DATABASE --
-- DROP DATABASE IF EXISTS new_database;
CREATE DATABASE new_database;
-- ddl-end --


-- object: public."Usuario" | type: TABLE --
-- DROP TABLE IF EXISTS public."Usuario" CASCADE;
CREATE TABLE public."Usuario" (
	id uuid NOT NULL,
	nome text,
	telefone varchar(20),
	email varchar(50),
	tipo smallint NOT NULL,
	CONSTRAINT "Usuario_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Usuario" OWNER TO postgres;
-- ddl-end --

-- object: public."Ordem_Servico" | type: TABLE --
-- DROP TABLE IF EXISTS public."Ordem_Servico" CASCADE;
CREATE TABLE public."Ordem_Servico" (
	id uuid NOT NULL,
	descricao varchar(100),
	"dataInicio" date,
	"dataConclusao" date,
	status smallint,
	CONSTRAINT "Ordem_Servico_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Ordem_Servico" OWNER TO postgres;
-- ddl-end --

-- object: public."Patrimonio" | type: TABLE --
-- DROP TABLE IF EXISTS public."Patrimonio" CASCADE;
CREATE TABLE public."Patrimonio" (
	id uuid NOT NULL,
	nome text,
	numero text,
	descricao json,
	CONSTRAINT "Maquina_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Patrimonio" OWNER TO postgres;
-- ddl-end --

-- object: public."Peca" | type: TABLE --
-- DROP TABLE IF EXISTS public."Peca" CASCADE;
CREATE TABLE public."Peca" (
	id uuid NOT NULL,
	descricao json,
	quantidade smallint,
	"id_Ordem_Servico" uuid,
	CONSTRAINT "Peca_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Peca" OWNER TO postgres;
-- ddl-end --

-- object: public."Setor" | type: TABLE --
-- DROP TABLE IF EXISTS public."Setor" CASCADE;
CREATE TABLE public."Setor" (
	id uuid NOT NULL,
	nome varchar(50),
	descricao varchar(100),
	telefone varchar(20),
	email varchar(50),
	CONSTRAINT "Setor_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Setor" OWNER TO postgres;
-- ddl-end --

-- object: public."Sala" | type: TABLE --
-- DROP TABLE IF EXISTS public."Sala" CASCADE;
CREATE TABLE public."Sala" (
	id uuid NOT NULL,
	"numeroSala" smallint,
	"numeroPredio" smallint,
	"id_Setor" uuid,
	prazo_reqisicao date,
	CONSTRAINT "Sala_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Sala" OWNER TO postgres;
-- ddl-end --

-- object: "Setor_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Sala" DROP CONSTRAINT IF EXISTS "Setor_fk" CASCADE;
ALTER TABLE public."Sala" ADD CONSTRAINT "Setor_fk" FOREIGN KEY ("id_Setor")
REFERENCES public."Setor" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "Ordem_Servico_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Peca" DROP CONSTRAINT IF EXISTS "Ordem_Servico_fk" CASCADE;
ALTER TABLE public."Peca" ADD CONSTRAINT "Ordem_Servico_fk" FOREIGN KEY ("id_Ordem_Servico")
REFERENCES public."Ordem_Servico" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: public."Habilidade" | type: TABLE --
-- DROP TABLE IF EXISTS public."Habilidade" CASCADE;
CREATE TABLE public."Habilidade" (
	id uuid NOT NULL,
	descricao varchar(20),
	"id_Usuario" uuid,
	nome varchar(20),
	CONSTRAINT "Habilidade_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Habilidade" OWNER TO postgres;
-- ddl-end --

-- object: "Usuario_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Habilidade" DROP CONSTRAINT IF EXISTS "Usuario_fk" CASCADE;
ALTER TABLE public."Habilidade" ADD CONSTRAINT "Usuario_fk" FOREIGN KEY ("id_Usuario")
REFERENCES public."Usuario" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: public."Requiscao" | type: TABLE --
-- DROP TABLE IF EXISTS public."Requiscao" CASCADE;
CREATE TABLE public."Requiscao" (
	id uuid NOT NULL,
	data_requisicao date,
	descricao text,
	titulo varchar(20),
	"id_Usuario" uuid,
	CONSTRAINT "Requiscao_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Requiscao" OWNER TO postgres;
-- ddl-end --

-- object: "Usuario_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Requiscao" DROP CONSTRAINT IF EXISTS "Usuario_fk" CASCADE;
ALTER TABLE public."Requiscao" ADD CONSTRAINT "Usuario_fk" FOREIGN KEY ("id_Usuario")
REFERENCES public."Usuario" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --


