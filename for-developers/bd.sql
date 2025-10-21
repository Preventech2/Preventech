-- Database generated with pgModeler (PostgreSQL Database Modeler).
-- pgModeler version: 1.1.0-beta1
-- PostgreSQL version: 16.0
-- Project Site: pgmodeler.io
-- Model Author: ---

-- Database creation must be performed outside a multi lined SQL file. 
-- These commands were put in this file only as a convenience.
-- 
-- object: "Preventech" | type: DATABASE --
-- DROP DATABASE IF EXISTS "Preventech";
CREATE DATABASE "Preventech"
	ENCODING = 'UTF8'
	LC_COLLATE = 'pt_BR.UTF-8'
	LC_CTYPE = 'pt_BR.UTF-8'
	TABLESPACE = pg_default
	OWNER = postgres;
-- ddl-end --


-- object: public."Usuario" | type: TABLE --
-- DROP TABLE IF EXISTS public."Usuario" CASCADE;
CREATE TABLE public."Usuario" (
	id uuid NOT NULL,
	nome text,
	telefone character varying(50),
	email character varying(50),
	tipo smallint NOT NULL,
	senha bigint NOT NULL,
	CONSTRAINT "Usuario_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Usuario" OWNER TO postgres;
-- ddl-end --

-- object: public."Ordem_Servico" | type: TABLE --
-- DROP TABLE IF EXISTS public."Ordem_Servico" CASCADE;
CREATE TABLE public."Ordem_Servico" (
	id uuid NOT NULL,
	descricao character varying(100),
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
	"id_Preventiva" uuid,
	"id_Usuario" uuid,
	CONSTRAINT "Maquina_pk" PRIMARY KEY (id),
	CONSTRAINT "Patrimonio_uq" UNIQUE ("id_Preventiva")
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
	nome character varying(50),
	descricao character varying(100),
	telefone character varying(50),
	email character varying(50),
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
	prazo_requisicao date,
	CONSTRAINT "Sala_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Sala" OWNER TO postgres;
-- ddl-end --

-- object: public."Habilidade" | type: TABLE --
-- DROP TABLE IF EXISTS public."Habilidade" CASCADE;
CREATE TABLE public."Habilidade" (
	id uuid NOT NULL,
	descricao character varying(50),
	"id_Usuario" uuid,
	"id_HabilidadeSistema" uuid,
	CONSTRAINT "Habilidade_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Habilidade" OWNER TO postgres;
-- ddl-end --

-- object: public."Requiscao" | type: TABLE --
-- DROP TABLE IF EXISTS public."Requiscao" CASCADE;
CREATE TABLE public."Requiscao" (
	id uuid NOT NULL,
	data_requisicao date,
	descricao text,
	titulo character varying(50),
	"id_Usuario" uuid,
	prazo date,
	CONSTRAINT "Requiscao_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Requiscao" OWNER TO postgres;
-- ddl-end --

-- object: public."many_Requiscao_has_many_Setor" | type: TABLE --
-- DROP TABLE IF EXISTS public."many_Requiscao_has_many_Setor" CASCADE;
CREATE TABLE public."many_Requiscao_has_many_Setor" (
	"id_Requiscao" uuid NOT NULL,
	"id_Setor" uuid NOT NULL,
	CONSTRAINT "many_Requiscao_has_many_Setor_pk" PRIMARY KEY ("id_Requiscao","id_Setor")
);
-- ddl-end --
ALTER TABLE public."many_Requiscao_has_many_Setor" OWNER TO postgres;
-- ddl-end --

-- object: public."many_Requiscao_has_many_Patrimonio" | type: TABLE --
-- DROP TABLE IF EXISTS public."many_Requiscao_has_many_Patrimonio" CASCADE;
CREATE TABLE public."many_Requiscao_has_many_Patrimonio" (
	"id_Requiscao" uuid NOT NULL,
	"id_Patrimonio" uuid NOT NULL,
	CONSTRAINT "many_Requiscao_has_many_Patrimonio_pk" PRIMARY KEY ("id_Requiscao","id_Patrimonio")
);
-- ddl-end --
ALTER TABLE public."many_Requiscao_has_many_Patrimonio" OWNER TO postgres;
-- ddl-end --

-- object: public."HabilidadeSistema" | type: TABLE --
-- DROP TABLE IF EXISTS public."HabilidadeSistema" CASCADE;
CREATE TABLE public."HabilidadeSistema" (
	id uuid NOT NULL,
	nome character varying(20) NOT NULL,
	CONSTRAINT "HabilidadesSistema_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."HabilidadeSistema" OWNER TO postgres;
-- ddl-end --

-- object: public."Preditiva" | type: TABLE --
-- DROP TABLE IF EXISTS public."Preditiva" CASCADE;
CREATE TABLE public."Preditiva" (
	id uuid NOT NULL,
	"id_Patrimonio" uuid,
	CONSTRAINT "Preditiva_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Preditiva" OWNER TO postgres;
-- ddl-end --

-- object: public."Preventiva" | type: TABLE --
-- DROP TABLE IF EXISTS public."Preventiva" CASCADE;
CREATE TABLE public."Preventiva" (
	id uuid NOT NULL,
	pdf text,
	frequencia date,
	CONSTRAINT "Preventiva_pk" PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public."Preventiva" OWNER TO postgres;
-- ddl-end --

-- object: pgcrypto | type: EXTENSION --
-- DROP EXTENSION IF EXISTS pgcrypto CASCADE;
CREATE EXTENSION pgcrypto
WITH SCHEMA public
VERSION '1.3';
-- ddl-end --
COMMENT ON EXTENSION pgcrypto IS E'cryptographic functions';
-- ddl-end --

-- object: "Usuario_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Patrimonio" DROP CONSTRAINT IF EXISTS "Usuario_fk" CASCADE;
ALTER TABLE public."Patrimonio" ADD CONSTRAINT "Usuario_fk" FOREIGN KEY ("id_Usuario")
REFERENCES public."Usuario" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "Patrimonio_uq1" | type: CONSTRAINT --
-- ALTER TABLE public."Patrimonio" DROP CONSTRAINT IF EXISTS "Patrimonio_uq1" CASCADE;
ALTER TABLE public."Patrimonio" ADD CONSTRAINT "Patrimonio_uq1" UNIQUE ("id_Usuario");
-- ddl-end --

-- object: "Preventiva_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Patrimonio" DROP CONSTRAINT IF EXISTS "Preventiva_fk" CASCADE;
ALTER TABLE public."Patrimonio" ADD CONSTRAINT "Preventiva_fk" FOREIGN KEY ("id_Preventiva")
REFERENCES public."Preventiva" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "Ordem_Servico_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Peca" DROP CONSTRAINT IF EXISTS "Ordem_Servico_fk" CASCADE;
ALTER TABLE public."Peca" ADD CONSTRAINT "Ordem_Servico_fk" FOREIGN KEY ("id_Ordem_Servico")
REFERENCES public."Ordem_Servico" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "Setor_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Sala" DROP CONSTRAINT IF EXISTS "Setor_fk" CASCADE;
ALTER TABLE public."Sala" ADD CONSTRAINT "Setor_fk" FOREIGN KEY ("id_Setor")
REFERENCES public."Setor" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "HabilidadeSistema_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Habilidade" DROP CONSTRAINT IF EXISTS "HabilidadeSistema_fk" CASCADE;
ALTER TABLE public."Habilidade" ADD CONSTRAINT "HabilidadeSistema_fk" FOREIGN KEY ("id_HabilidadeSistema")
REFERENCES public."HabilidadeSistema" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "Usuario_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Habilidade" DROP CONSTRAINT IF EXISTS "Usuario_fk" CASCADE;
ALTER TABLE public."Habilidade" ADD CONSTRAINT "Usuario_fk" FOREIGN KEY ("id_Usuario")
REFERENCES public."Usuario" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "Usuario_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Requiscao" DROP CONSTRAINT IF EXISTS "Usuario_fk" CASCADE;
ALTER TABLE public."Requiscao" ADD CONSTRAINT "Usuario_fk" FOREIGN KEY ("id_Usuario")
REFERENCES public."Usuario" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: "Requiscao_fk" | type: CONSTRAINT --
-- ALTER TABLE public."many_Requiscao_has_many_Setor" DROP CONSTRAINT IF EXISTS "Requiscao_fk" CASCADE;
ALTER TABLE public."many_Requiscao_has_many_Setor" ADD CONSTRAINT "Requiscao_fk" FOREIGN KEY ("id_Requiscao")
REFERENCES public."Requiscao" (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: "Setor_fk" | type: CONSTRAINT --
-- ALTER TABLE public."many_Requiscao_has_many_Setor" DROP CONSTRAINT IF EXISTS "Setor_fk" CASCADE;
ALTER TABLE public."many_Requiscao_has_many_Setor" ADD CONSTRAINT "Setor_fk" FOREIGN KEY ("id_Setor")
REFERENCES public."Setor" (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: "Requiscao_fk" | type: CONSTRAINT --
-- ALTER TABLE public."many_Requiscao_has_many_Patrimonio" DROP CONSTRAINT IF EXISTS "Requiscao_fk" CASCADE;
ALTER TABLE public."many_Requiscao_has_many_Patrimonio" ADD CONSTRAINT "Requiscao_fk" FOREIGN KEY ("id_Requiscao")
REFERENCES public."Requiscao" (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: "Patrimonio_fk" | type: CONSTRAINT --
-- ALTER TABLE public."many_Requiscao_has_many_Patrimonio" DROP CONSTRAINT IF EXISTS "Patrimonio_fk" CASCADE;
ALTER TABLE public."many_Requiscao_has_many_Patrimonio" ADD CONSTRAINT "Patrimonio_fk" FOREIGN KEY ("id_Patrimonio")
REFERENCES public."Patrimonio" (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: "Patrimonio_fk" | type: CONSTRAINT --
-- ALTER TABLE public."Preditiva" DROP CONSTRAINT IF EXISTS "Patrimonio_fk" CASCADE;
ALTER TABLE public."Preditiva" ADD CONSTRAINT "Patrimonio_fk" FOREIGN KEY ("id_Patrimonio")
REFERENCES public."Patrimonio" (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

CREATE TABLE IF NOT EXISTS "OrdemServicoPeca" (
    "OrdensDeServicoId" uuid NOT NULL,
    "PecasId" uuid NOT NULL,
    CONSTRAINT "PK_OrdemServicoPeca" PRIMARY KEY ("OrdensDeServicoId", "PecasId"),
    CONSTRAINT "FK_OrdemServicoPeca_OrdensServico_OrdensDeServicoId" FOREIGN KEY ("OrdensDeServicoId") 
        REFERENCES "OrdensServico" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrdemServicoPeca_Pecas_PecasId" FOREIGN KEY ("PecasId") 
        REFERENCES "Pecas" ("Id") ON DELETE CASCADE
);