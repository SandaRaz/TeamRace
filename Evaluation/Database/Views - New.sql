CREATE OR REPLACE VIEW "ResultatAvecPenalite" AS
SELECT
	r."Id",
	r."EtapeId",
	r."CoureurId",
	r."DateArrivee",
	r."DateArrivee" + COALESCE(SUM(p."TempsPenalite"), '0 seconds'::interval) AS "DateArriveePenalisee" 
FROM
	"Resultat" r
LEFT JOIN
	"Coureur" c ON r."CoureurId" = c."Id"
LEFT JOIN
	"Penalite" p ON c."EquipeId" = p."EquipeId"
	AND r."EtapeId" = p."EtapeId"
	AND p."Etat" != -1
GROUP BY
	r."Id",r."EtapeId",r."CoureurId",r."DateArrivee";


CREATE OR REPLACE VIEW "Classement" AS
WITH RankedResults AS (
	SELECT
		rap."EtapeId" AS "EtapeId",
		e."Nom" AS "NomEtape",
		e."Longueur" AS "LongueurEtape",
		e."NombreCoureur" AS "NombreCoureur",
		e."DateDepart" AS "DateDepart",
		e."RangEtape" AS "RangEtape",
		rap."CoureurId" AS "CoureurId",
		c."Nom" AS "NomCoureur",
		c."Genre" AS "GenreCoureur",
		c."NumDossard" AS "NumDossard",
		rap."DateArrivee" AS "DateArrivee",
		rap."DateArriveePenalisee" AS "DateArriveePenalisee",
		c."EquipeId" AS "EquipeId",
		eq."Nom" AS "NomEquipe",
		DENSE_RANK() OVER (PARTITION BY rap."EtapeId" ORDER BY rap."DateArriveePenalisee") AS "Rang"
	FROM
		"ResultatAvecPenalite" rap
	JOIN
		"Etape" e ON rap."EtapeId" = e."Id"
	JOIN 
		"Coureur" c ON rap."CoureurId" = c."Id"
	JOIN
		"Equipe" eq ON c."EquipeId" = eq."Id"
)
SELECT
	rr."EtapeId",
	rr."NomEtape",
	rr."LongueurEtape",
	rr."NombreCoureur",
	rr."DateDepart",
	rr."RangEtape",
	rr."CoureurId",
	rr."NomCoureur",
	rr."GenreCoureur",
	rr."NumDossard",
	rr."DateArrivee",
	rr."DateArriveePenalisee",
	rr."EquipeId",
	rr."NomEquipe",
	rr."Rang",
	COALESCE(pe."Points", 0) AS "Points"
FROM 
	RankedResults rr
LEFT JOIN
	"PointEtape" pe ON rr."Rang" = pe."Rang";

---------------- VIEW CLASSEMENT EQUIPE ----------------

CREATE OR REPLACE VIEW "ClassementEquipe" AS
SELECT
	e."Id" AS "EquipeId",
	e."Nom" AS "NomEquipe",
	COALESCE(re."Points", 0) AS "Points",
	DENSE_RANK() OVER (ORDER BY COALESCE(re."Points", 0) DESC) AS "Rang"
FROM
	"Equipe" e
LEFT JOIN
	(SELECT 
		c."EquipeId",
		SUM(c."Points") AS "Points"
	FROM 
		"Classement" c
	GROUP BY
		c."EquipeId") re ON e."Id" = re."EquipeId";

---------------------- CLASSEMENT EQUIPE PAR CATEGORIE ----------------------
---------- Resultat nampiana Categorie par jointure ---------
CREATE OR REPLACE VIEW "ResultatCategorie" AS
SELECT 
	cc."CategorieId",
	cg."Nom" as "NomCategorie",
	r."EtapeId",
	et."Nom" as "NomEtape",
	r."CoureurId",
	c."Nom" as "NomCoureur",
	c."NumDossard",
	c."DateNaissance",
	c."EquipeId",
	eq."Nom" as "NomEquipe",
	r."DateArrivee",
	r."DateArriveePenalisee"
FROM 
	"Categorie" cg
JOIN
	"CoureurCategorie" cc ON cg."Id" = cc."CategorieId"
JOIN
	"ResultatAvecPenalite" r ON cc."CoureurId" = r."CoureurId"
JOIN
	"Coureur" c ON r."CoureurId" = c."Id"
JOIN 
	"Etape" et ON r."EtapeId" = et."Id"
JOIN
	"Equipe" eq ON c."EquipeId" = eq."Id";

-------------------- RANG DES COUREUR PAR CATEGORIE ET PAR ETAPE -------------

		-------- VIEW A UTILISER DANS LE PROJET ------

WITH RankedResults AS (
	SELECT
		rc."CategorieId",
		rc."EtapeId",
		rc."CoureurId",
		rc."EquipeId" AS "EquipeId",
		rc."NomEquipe",
		rc."DateArriveePenalisee",
		DENSE_RANK() OVER(PARTITION BY rc."EtapeId" ORDER BY rc."DateArriveePenalisee") AS "Rang"
	FROM
		"ResultatCategorie" rc
	WHERE
		rc."CategorieId" = '662b1954-198d-4bb9-920b-e17b959d7d65'
),
RankedResultsWithPoints AS (
	SELECT
		rr.*,
		COALESCE(pe."Points", 0) AS "Points"
	FROM
		RankedResults rr
	LEFT JOIN
		"PointEtape" pe ON  rr."Rang" = pe."Rang"
),
ResultatParEquipe AS (
	SELECT
		rr."EquipeId",
		rr."NomEquipe",
		SUM(rr."Points") AS "Points"
	FROM
		RankedResultsWithPoints rr
	GROUP BY
		rr."EquipeId",
		rr."NomEquipe"
	ORDER BY
		"Points" DESC
)
SELECT
	e."Id" AS "EquipeId",
	e."Nom" AS "NomEquipe",
	COALESCE(re."Points", 0) AS "Points",
	DENSE_RANK() OVER(ORDER BY COALESCE(re."Points", 0) DESC) AS "Rang"
FROM 
	"Equipe" e
LEFT JOIN
	ResultatParEquipe re ON e."Id" = re."EquipeId";
