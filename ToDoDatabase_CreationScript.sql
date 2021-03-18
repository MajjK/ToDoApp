--
-- PostgreSQL database dump
--

-- Dumped from database version 13.2
-- Dumped by pg_dump version 13.2

-- Started on 2021-03-18 12:59:29

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 200 (class 1259 OID 24728)
-- Name: Tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tasks" (
    task_id integer NOT NULL,
    user_id integer NOT NULL,
    objective character varying(255) NOT NULL,
    description character varying(255),
    addition_date timestamp without time zone DEFAULT date_trunc('minute'::text, CURRENT_TIMESTAMP) NOT NULL,
    closing_date timestamp without time zone,
    finished boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Tasks" OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 24737)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    user_id integer NOT NULL,
    login character varying(50) NOT NULL,
    password character varying NOT NULL,
    password_salt character varying NOT NULL,
    addition_date timestamp without time zone DEFAULT date_trunc('minute'::text, CURRENT_TIMESTAMP) NOT NULL,
    role character varying DEFAULT USER NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 24749)
-- Name: task_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.task_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.task_id_seq OWNER TO postgres;

--
-- TOC entry 3010 (class 0 OID 0)
-- Dependencies: 203
-- Name: task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.task_id_seq OWNED BY public."Tasks".task_id;


--
-- TOC entry 202 (class 1259 OID 24745)
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_id_seq OWNER TO postgres;

--
-- TOC entry 3011 (class 0 OID 0)
-- Dependencies: 202
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_id_seq OWNED BY public."Users".user_id;


--
-- TOC entry 2860 (class 2604 OID 24751)
-- Name: Tasks task_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks" ALTER COLUMN task_id SET DEFAULT nextval('public.task_id_seq'::regclass);


--
-- TOC entry 2862 (class 2604 OID 24752)
-- Name: Users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users" ALTER COLUMN user_id SET DEFAULT nextval('public.user_id_seq'::regclass);


--
-- TOC entry 3001 (class 0 OID 24728)
-- Dependencies: 200
-- Data for Name: Tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Tasks" (task_id, user_id, objective, description, addition_date, closing_date, finished) FROM stdin;
2	1	Example Task #2 User #1	\N	2021-03-18 01:27:00	2021-03-26 02:30:00	f
4	1	Example Task #4 User #1	\N	2021-03-18 01:28:00	2021-03-30 16:00:00	f
3	1	Example Task #3 User #1	Example Description	2021-03-18 01:27:00	2021-03-24 00:00:00	t
6	2	Example Task #2 User #2	\N	2021-03-18 01:29:00	2021-03-30 00:00:00	f
1	1	Example Task #1 User #1	Example Description	2021-03-18 01:26:00	2021-03-24 00:00:00	t
5	2	Example Task #1 User #2	Example Description	2021-03-18 01:29:00	2021-03-24 01:30:00	t
\.


--
-- TOC entry 3002 (class 0 OID 24737)
-- Dependencies: 201
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" (user_id, login, password, password_salt, addition_date, role) FROM stdin;
1	postgres	Y3RQlX3U7266G9VYAuAlpv1GxkkY5Kj1i7k9ACes+WjE2aq/4abGd7R4B7llhYDLc2hk1SKsMxdmdrEDMzm5VA==	QՁH\r��GOd���,���ޯ+ʼ��S�.���y�xt���0<�T���T��a�Ɋ1&�l�	2021-03-18 01:24:00	admin
2	postgres2	uAWb4Lv225+8fTEU4dxEWHCa0uuVWA8AWDOShn42pUlOzVJZZJE1PBG/Ij/521Q7ATS0rr/6CJ2BtReikT7hVw==	r��R���zi7}�\\��ݚ[�B�S*�76<EMg�;�e37�̵�Ȓ\fc��j(��y�\rkc�u!�	2021-03-18 01:24:00	user
\.


--
-- TOC entry 3012 (class 0 OID 0)
-- Dependencies: 203
-- Name: task_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.task_id_seq', 6, true);


--
-- TOC entry 3013 (class 0 OID 0)
-- Dependencies: 202
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 2, true);


--
-- TOC entry 2867 (class 2606 OID 24754)
-- Name: Users login_unique; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT login_unique UNIQUE (login);


--
-- TOC entry 2865 (class 2606 OID 24756)
-- Name: Tasks task_id_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT task_id_pkey PRIMARY KEY (task_id);


--
-- TOC entry 2869 (class 2606 OID 24758)
-- Name: Users user_id_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT user_id_pkey PRIMARY KEY (user_id);


--
-- TOC entry 2870 (class 2606 OID 24759)
-- Name: Tasks task_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT task_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."Users"(user_id) ON DELETE CASCADE;


-- Completed on 2021-03-18 12:59:29

--
-- PostgreSQL database dump complete
--

