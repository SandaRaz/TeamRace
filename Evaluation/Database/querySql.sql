--------------- POUR Points ----------------

INSERT INTO "PointEtape"("Id","Rang","Points")
SELECT uuid_generate_v4(),"Classement","Points"
FROM (
	SELECT "Classement","Points"
	FROM "CsvPoints"
	GROUP BY "Classement","Points"
) as csvpt
WHERE NOT EXISTS (
	SELECT 1 FROM "PointEtape" pe
	WHERE pe."Rang" = csvpt."Classement"
);

------------------------------------------------
--------------- POUR ETAPE ----------------

INSERT INTO "Etape" ("Id","Nom","Lieu","Longueur","NombreCoureur","DateDepart","CourseId","RangEtape")
SELECT uuid_generate_v4(), "Etape",'',"Longueur","NbCoureur","DateDepart",'1a785405-e30a-4617-9761-a638856079b6',"Rang"
FROM (
	WITH EtapeRang AS
	(
		SELECT "Etape","Rang"
		FROM "CsvEtape"
		GROUP BY "Etape","Rang"
	)
	SELECT
		er."Etape",
		ce."Longueur",
		ce."NbCoureur",
		er."Rang",
		ce."DateDepart"
	FROM
		EtapeRang er
	LEFT JOIN
		"CsvEtape" ce ON er."Etape" = ce."Etape" AND er."Rang" = ce."Rang"
) as csvetp
WHERE NOT EXISTS (
	SELECT 1 FROM "Etape" et
	WHERE et."Nom" = csvetp."Etape" AND et."RangEtape" = csvetp."Rang"
);

--------------------------------------------
---------------- POUR EQUIPE -------------
INSERT INTO "Equipe"("Id","Nom","Email","MotDePasse", "Profil", "DateCreation")
SELECT uuid_generate_v4(),"Equipe","Equipe","Equipe",'Equipe',Now()
FROM(
	SELECT "Equipe"
	FROM "CsvResultat"
	GROUP BY "Equipe"
) as csveq
WHERE NOT EXISTS (
	SELECT 1 FROM "Equipe" eq
	WHERE eq."Nom" = csveq."Equipe"
);

--------------- POUR COUREUR --------------

INSERT INTO "Coureur"("Id","Nom","Genre","DateNaissance","NumDossard","EquipeId")
SELECT uuid_generate_v4(),"Nom","Genre","DateNaissance","NumeroDossard","EquipeId"
FROM(
	WITH UniqueCoureur AS
	(
		SELECT "NumeroDossard","Nom","Genre","DateNaissance","Equipe"
		FROM "CsvResultat"
		GROUP BY "NumeroDossard","Nom","Genre","DateNaissance","Equipe"
	)
	SELECT
		uc."Nom",
		uc."Genre",
		uc."DateNaissance",
		uc."NumeroDossard",
		eq."Id" AS "EquipeId"
	FROM
		UniqueCoureur uc
	JOIN
		"Equipe" eq ON uc."Equipe" = eq."Nom"
) AS csvcr
WHERE NOT EXISTS
(
	SELECT 1 FROM "Coureur" cr
	WHERE cr."Nom" = csvcr."Nom" AND cr."NumDossard" = csvcr."NumeroDossard"
);

-------------- POUR COUREUR ETAPE --------------
INSERT INTO "CoureurEtape"("CoureurId","EtapeId")
SELECT "CoureurId","EtapeId"
FROM (
	SELECT 
		c."Id" AS "CoureurId",
		et."Id" AS "EtapeId"
	FROM "CsvResultat" crs
	JOIN "Etape" et ON crs."EtapeRang" = et."RangEtape"
	JOIN "Coureur" c ON crs."NumeroDossard" = c."NumDossard"
) AS csvce
WHERE NOT EXISTS 
(
	SELECT 1 FROM "CoureurEtape" ce
	WHERE ce."CoureurId" = csvce."CoureurId" AND ce."EtapeId" = csvce."EtapeId"
);

--------------- POUR RESULTAT -----------------
INSERT INTO "Resultat"("Id","EtapeId","CoureurId","DateArrivee")
SELECT uuid_generate_v4(),"EtapeId","CoureurId","DateArrivee"
FROM (
	SELECT 
		et."Id" AS "EtapeId",
		c."Id" AS "CoureurId",
		crs."Arrivee" AS "DateArrivee"
	FROM "CsvResultat" crs
	JOIN "Etape" et ON crs."EtapeRang" = et."RangEtape"
	JOIN "Coureur" c ON crs."NumeroDossard" = c."NumDossard"
) AS csvrs
WHERE NOT EXISTS
(
	SELECT 1 FROM "Resultat" rs
	WHERE rs."EtapeId" = csvrs."EtapeId" AND rs."CoureurId" = csvrs."CoureurId"
);