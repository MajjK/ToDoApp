--
-- PostgreSQL database dump
--

-- Dumped from database version 13.2
-- Dumped by pg_dump version 13.2

-- Started on 2021-03-14 13:33:46

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
-- TOC entry 204 (class 1259 OID 24647)
-- Name: Tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tasks" (
    task_id integer NOT NULL,
    user_id integer DEFAULT 1 NOT NULL,
    objective character varying(255) NOT NULL,
    description character varying(255),
    addition_date timestamp without time zone DEFAULT date_trunc('minute'::text, CURRENT_TIMESTAMP),
    closing_date timestamp without time zone,
    finished boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Tasks" OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 24636)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    user_id integer NOT NULL,
    login character varying(50) NOT NULL,
    password character varying(50) NOT NULL,
    addition_date timestamp without time zone DEFAULT date_trunc('minute'::text, CURRENT_TIMESTAMP)
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 24634)
-- Name: operator_operator_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.operator_operator_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.operator_operator_id_seq OWNER TO postgres;

--
-- TOC entry 3012 (class 0 OID 0)
-- Dependencies: 200
-- Name: operator_operator_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.operator_operator_id_seq OWNED BY public."Users".user_id;


--
-- TOC entry 203 (class 1259 OID 24645)
-- Name: task_operator_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.task_operator_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.task_operator_id_seq OWNER TO postgres;

--
-- TOC entry 3013 (class 0 OID 0)
-- Dependencies: 203
-- Name: task_operator_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.task_operator_id_seq OWNED BY public."Tasks".user_id;


--
-- TOC entry 202 (class 1259 OID 24643)
-- Name: task_task_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.task_task_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.task_task_id_seq OWNER TO postgres;

--
-- TOC entry 3014 (class 0 OID 0)
-- Dependencies: 202
-- Name: task_task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.task_task_id_seq OWNED BY public."Tasks".task_id;


--
-- TOC entry 2861 (class 2604 OID 24650)
-- Name: Tasks task_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks" ALTER COLUMN task_id SET DEFAULT nextval('public.task_task_id_seq'::regclass);


--
-- TOC entry 2859 (class 2604 OID 24639)
-- Name: Users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users" ALTER COLUMN user_id SET DEFAULT nextval('public.operator_operator_id_seq'::regclass);


--
-- TOC entry 3006 (class 0 OID 24647)
-- Dependencies: 204
-- Data for Name: Tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Tasks" (task_id, user_id, objective, description, addition_date, closing_date, finished) FROM stdin;
1	1	Example Task #1 User #1	Test Description	2021-03-03 23:22:00	2021-03-03 00:00:00	f
3	1	Example Task #3 User #1	\N	2021-03-08 15:04:00	2021-03-20 16:00:00	t
4	1	Example Task #4 User #1	Test Description	2021-03-09 15:13:00	2021-03-10 00:00:00	f
2	1	Example Task #2 User #1	Test Descritpion	2021-03-03 23:23:00	2021-03-03 00:00:00	t
6	2	Example Task #2 User #2	\N	2021-03-14 00:00:00	2021-03-21 00:00:00	f
5	2	Example Task #1 User #2	Test Description	2021-03-14 00:00:00	2021-03-20 00:00:00	t
\.


--
-- TOC entry 3003 (class 0 OID 24636)
-- Dependencies: 201
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" (user_id, login, password, addition_date) FROM stdin;
1	postgres	postgres	2021-03-03 22:51:00
2	postgres2	postgres	2021-03-14 00:00:00
\.


--
-- TOC entry 3015 (class 0 OID 0)
-- Dependencies: 200
-- Name: operator_operator_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.operator_operator_id_seq', 18, true);


--
-- TOC entry 3016 (class 0 OID 0)
-- Dependencies: 203
-- Name: task_operator_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.task_operator_id_seq', 18, true);


--
-- TOC entry 3017 (class 0 OID 0)
-- Dependencies: 202
-- Name: task_task_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.task_task_id_seq', 52, true);


--
-- TOC entry 2866 (class 2606 OID 24665)
-- Name: Users Users_login_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_login_key" UNIQUE (login);


--
-- TOC entry 2868 (class 2606 OID 24642)
-- Name: Users operator_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT operator_pkey PRIMARY KEY (user_id);


--
-- TOC entry 2870 (class 2606 OID 24658)
-- Name: Tasks task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT task_pkey PRIMARY KEY (task_id);


--
-- TOC entry 2871 (class 2606 OID 24659)
-- Name: Tasks task_operator_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT task_operator_id_fkey FOREIGN KEY (user_id) REFERENCES public."Users"(user_id) ON DELETE CASCADE;


-- Completed on 2021-03-14 13:33:47

--
-- PostgreSQL database dump complete
--

