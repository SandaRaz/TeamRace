uuid_generate_v4()

------------ COURSE ---------------

INSERT INTO "Course" ("Id","Nom","DateCourse","DureeHeure") 
VALUES('1a785405-e30a-4617-9761-a638856079b6','ULTIMATE TEAM RACE','2024-06-01','48 hours');

SELECT * FROM "Course";
-----------------------------------

------------ ETAPE ----------------

INSERT INTO "Etape" ("Id","Nom","Lieu","Longueur","NombreCoureur","DateDepart","CourseId","RangEtape")
VALUES('5a0053fb-501f-4b5b-9429-ee72f73f508b','Betsizaraina','Antananarivo','4','2','2024-06-01 07:00:00','1a785405-e30a-4617-9761-a638856079b6','1');

	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('4a33be1a-1e8f-4961-8401-2b4fdf7bc0ee','5a0053fb-501f-4b5b-9429-ee72f73f508b','1','10');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('64333498-2616-4003-80be-312d244b1adf','5a0053fb-501f-4b5b-9429-ee72f73f508b','2','6');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('814f9032-0058-4563-96b4-e97a1b27e99e','5a0053fb-501f-4b5b-9429-ee72f73f508b','3','4');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('339f7b14-f346-4578-97b5-4499e3ec9c39','5a0053fb-501f-4b5b-9429-ee72f73f508b','4','2');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('3b7c512d-ff73-4320-bd6b-a91ff335c415','5a0053fb-501f-4b5b-9429-ee72f73f508b','5','1');
	
INSERT INTO "Etape" ("Id","Nom","Lieu","Longueur","NombreCoureur","DateDepart","CourseId","RangEtape")
VALUES('587d158d-e839-4b1f-88a7-17bb649b16fb','Ambohimangakely','Antananarivo','10','3','2024-06-01 14:00:00','1a785405-e30a-4617-9761-a638856079b6','2');

	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('99237201-a99c-4110-951a-5d5f61aa5ca7','587d158d-e839-4b1f-88a7-17bb649b16fb','1','10');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('278675da-0c0f-4d03-b595-46d263edf3e4','587d158d-e839-4b1f-88a7-17bb649b16fb','2','6');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('173ecf43-9a4e-4450-965b-532bdd0dcb7a','587d158d-e839-4b1f-88a7-17bb649b16fb','3','4');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('9e08c9cd-038a-42f1-876a-4d46da7ff84f','587d158d-e839-4b1f-88a7-17bb649b16fb','4','2');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('57bfa9d3-0b56-48ad-8372-e4ed23ad27c4','587d158d-e839-4b1f-88a7-17bb649b16fb','5','1');

INSERT INTO "Etape" ("Id","Nom","Lieu","Longueur","NombreCoureur","DateDepart","CourseId","RangEtape")
VALUES('0cef9a26-bbaf-4d9a-9a2e-1aee65d45b79','Ampasimbe','Antananarivo','6','1','2024-06-02 08:00:00','1a785405-e30a-4617-9761-a638856079b6','3');

	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('06cb5660-1c2b-4b6d-a62c-929166c47196','0cef9a26-bbaf-4d9a-9a2e-1aee65d45b79','1','10');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('06b5bdc6-d516-44cd-93c7-411ba68f42df','0cef9a26-bbaf-4d9a-9a2e-1aee65d45b79','2','6');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('9bf6403e-773f-4d73-8038-da76e90310cd','0cef9a26-bbaf-4d9a-9a2e-1aee65d45b79','3','4');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('e640d8e5-d1da-4753-b887-ffdbb9a74695','0cef9a26-bbaf-4d9a-9a2e-1aee65d45b79','6','2');
	INSERT INTO "PointEtape" ("Id","EtapeId","Rang","Points")
	VALUES('63010d81-cbda-46ec-a5d1-166b2b0c4747','0cef9a26-bbaf-4d9a-9a2e-1aee65d45b79','6','1');

SELECT * FROM "Etape";
-----------------------------------

----------- EQUIPE ----------------

INSERT INTO "Equipe" ("Id","Nom","Email","MotDePasse","Profil","DateCreation")
VALUES('cf3798fa-62ce-4fca-ab49-5d4216036dd2','Team Requin', 'requin@gmail.com','AQAAAAEAACcQAAAAEOiCn4BAznbBHboVtEuz5SFTq9f/QvQnezl+LbsEb7c2BKsRxxq9HZ9CVdxTMTX5NQ==','Equipe',now());

INSERT INTO "Equipe" ("Id","Nom","Email","MotDePasse","Profil","DateCreation")
VALUES('d2777faf-e932-465f-ada8-af8a1070cd62','Red Hat', 'redhat@gmail.com','AQAAAAEAACcQAAAAEJwdIprPb6RcjaUDRU/eklMaSInK1TXFnUGUbP45TDii+fNwbV8vOqzXFy/iVx/NBw==','Equipe',now());

INSERT INTO "Equipe" ("Id","Nom","Email","MotDePasse","Profil","DateCreation")
VALUES('593a0b92-2d96-490c-b93c-a8bd653227d7','Tigre', 'tigre@gmail.com','AQAAAAEAACcQAAAAEGs4EnEJYz02X+oOLTi6JSMeE6AZDQdf8e+WX8DqNAVGhBes6HTA6rnNwKXyOo9luw==','Equipe',now());

SELECT * FROM "Equipe";
-----------------------------------

----------- CATEGORIE -------------

INSERT INTO "Categorie" ("Id","Nom","DateCreation")
VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','Homme', now());

INSERT INTO "Categorie" ("Id","Nom","DateCreation")
VALUES('7dd017a2-a3c0-41f8-b242-cc644b8312c4','Femme', now());

INSERT INTO "Categorie" ("Id","Nom","DateCreation")
VALUES('feb6c13e-731e-4a6b-be51-688fb6a03378','Junior', now());

SELECT * FROM "Categorie";

-----------------------------------

----------- COUREUR ---------------
-- Insertions pour l'équipe "Team Requin"

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('184dbf23-c4b1-4ba6-9cbb-11e3f2ec19d1','Liam','Homme','2008-06-02',1,'cf3798fa-62ce-4fca-ab49-5d4216036dd2');

	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','184dbf23-c4b1-4ba6-9cbb-11e3f2ec19d1');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('ad749c75-76f2-4dcd-835c-7d92f61d611c','Hugo','Homme','1991-03-15',2,'cf3798fa-62ce-4fca-ab49-5d4216036dd2');

	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','ad749c75-76f2-4dcd-835c-7d92f61d611c');

	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--VALUES('4aed3843-582d-46b8-b438-191ff8acfe8a','ad749c75-76f2-4dcd-835c-7d92f61d611c');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('00bb74ee-5c0b-451b-be7a-e4b418d9d44b','Lucas','Homme','1995-08-21',3,'cf3798fa-62ce-4fca-ab49-5d4216036dd2');

	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','00bb74ee-5c0b-451b-be7a-e4b418d9d44b');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('f691a5c7-be3c-46ec-a8f3-8fb69de985db','Emma','Femme','1992-12-10',4,'cf3798fa-62ce-4fca-ab49-5d4216036dd2');

	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--VALUES('7dd017a2-a3c0-41f8-b242-cc644b8312c4','f691a5c7-be3c-46ec-a8f3-8fb69de985db');

	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--VALUES('4aed3843-582d-46b8-b438-191ff8acfe8a','f691a5c7-be3c-46ec-a8f3-8fb69de985db');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('456af4dc-cb4b-40a9-80a8-e7b1611a6fe5','Anna','Femme','2000-10-21',5,'cf3798fa-62ce-4fca-ab49-5d4216036dd2');

	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--VALUES('7dd017a2-a3c0-41f8-b242-cc644b8312c4','456af4dc-cb4b-40a9-80a8-e7b1611a6fe5');
	
	--INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	--('feb6c13e-731e-4a6b-be51-688fb6a03378','456af4dc-cb4b-40a9-80a8-e7b1611a6fe5');


-- Insertions pour l'équipe "Red Hat"
INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('9819401c-69ef-400d-a483-4facd00a6d33','Alice','Femme','1994-05-17',6,'d2777faf-e932-465f-ada8-af8a1070cd62');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('7dd017a2-a3c0-41f8-b242-cc644b8312c4','9819401c-69ef-400d-a483-4facd00a6d33');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('a4283071-ae91-4f09-a98b-5980adefb4d8','Gabriel','Homme','1991-10-30',7,'d2777faf-e932-465f-ada8-af8a1070cd62');
	
	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','a4283071-ae91-4f09-a98b-5980adefb4d8');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('4aed3843-582d-46b8-b438-191ff8acfe8a','a4283071-ae91-4f09-a98b-5980adefb4d8');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('7a577273-1618-4cdb-af55-77d22e4575f3','Raphael','Homme','2001-07-15',8,'d2777faf-e932-465f-ada8-af8a1070cd62');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','7a577273-1618-4cdb-af55-77d22e4575f3');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('feb6c13e-731e-4a6b-be51-688fb6a03378','7a577273-1618-4cdb-af55-77d22e4575f3');

-- Insertions pour l'équipe "Tigre"
INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('da1195b6-60a1-427d-adb2-02b73d8f360b','Louis','Homme','1996-02-28',9,'593a0b92-2d96-490c-b93c-a8bd653227d7');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','da1195b6-60a1-427d-adb2-02b73d8f360b');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('2f7e4a16-2331-49a0-9f79-c79a714279d9','Lea','Femme','1993-09-05',10,'593a0b92-2d96-490c-b93c-a8bd653227d7');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','2f7e4a16-2331-49a0-9f79-c79a714279d9');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('4aed3843-582d-46b8-b438-191ff8acfe8a','2f7e4a16-2331-49a0-9f79-c79a714279d9');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('1b3735f1-a39d-4a8e-8cec-71aa5bf8c854','Sarah','Femme','1997-01-15',11,'593a0b92-2d96-490c-b93c-a8bd653227d7');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('7dd017a2-a3c0-41f8-b242-cc644b8312c4','1b3735f1-a39d-4a8e-8cec-71aa5bf8c854');

INSERT INTO "Coureur" ("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
VALUES('3459d2e7-b53d-4dc0-aa72-6069597ba37d','Adam','Homme','1999-12-07',12,'593a0b92-2d96-490c-b93c-a8bd653227d7');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('662b1954-198d-4bb9-920b-e17b959d7d65','3459d2e7-b53d-4dc0-aa72-6069597ba37d');

	INSERT INTO "CoureurCategorie" ("CategorieId","CoureurId")
	VALUES('feb6c13e-731e-4a6b-be51-688fb6a03378','3459d2e7-b53d-4dc0-aa72-6069597ba37d');


-----------------------------------