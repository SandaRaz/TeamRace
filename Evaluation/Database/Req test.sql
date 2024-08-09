WITH RankedResults AS (
	SELECT
		rc."CategorieId",
		rc."NomCategorie",
		rc."EtapeId",
		rc."NomEtape",
		rc."CoureurId",
		rc."NomCoureur",
		rc."NumDossard",
		rc."DateNaissance",
		rc."EquipeId",
		rc."NomEquipe",
		rc."DateArrivee",
		rc."DateArriveePenalisee"
		DENSE_RANK() OVER(PARTITION BY rc."EtapeId" ORDER BY rc."DateArriveePenalisee") AS "Rang"
	FROM
		"ResultatCategorie" rc
	WHERE
	rc."CategorieId" = '662b1954-198d-4bb9-920b-e17b959d7d65'
)
SELECT
	rr.*,
	COALESCE(pe."Points", 0) AS "Points"
FROM 
	RankedResults rr
LEFT JOIN
	"PointEtape" pe ON  rr."Rang" = pe."Rang";

------------ FIN TEST VIEW ----------------

------------ VIEW CLASSEMENT PAR EQUIPE PAR CATEGORIE ------------

WITH RankedResults AS (
	SELECT
		rc."CategorieId",
		rc."NomCategorie",
		rc."EtapeId",
		rc."NomEtape",
		rc."CoureurId",
		rc."NomCoureur",
		rc."NumDossard",
		rc."DateNaissance",
		rc."EquipeId",
		rc."NomEquipe",
		rc."DateArriveePenalisee"
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
)
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
	"Points" DESC;



-------------------------------------------
-------- REQUETE COLORIAGE EX-AEQUO -------

SELECT
	"Rang"
FROM
	"ClassementEquipe"
GROUP BY "Rang" HAVING COUNT(*) > 1;

---------------------------

WITH "ClassementExAequo" AS
(
	SELECT
		"Rang"
	FROM
		"ClassementEquipe"
	GROUP BY "Rang" HAVING COUNT(*) > 1
)
SELECT
	C.*,
	'rgba(0,136,0)' AS "Color"
FROM 
	"ClassementExAequo" cae
LEFT JOIN
	"ClassementEquipe" c ON cae."Rang" = c."Rang";

               EquipeId               | NomEquipe | Points | Rang |     Color
--------------------------------------+-----------+--------+------+---------------
 2f15f01a-5176-4f63-8ca3-a19d8be3d8d2 | C         |     27 |    3 | rgba(0,136,0)
 5a70470a-7861-4b37-8fc8-7be4200064a4 | E         |     27 |    3 | rgba(0,136,0)

 ----------------- INFO --------------------
 Comment Supprimer un etapes(ou autres):
	- UPDATE: creer colonne supprimer : 1/0 ou true/false
		=> contrainte: update bloc (verrouille) les transactions
	- INSERT: creer table etapes_supprimes: asina id_etape_supprimee
		- lasa mitady ny view sy table rehetra misy table etapes
			- apiana condition de suppression
		- solution: tsy manao jointure @table direct fa manao jointure @ view v_etapes (misy condition de suppression, donc 
			izay etape mbola tsy en etat de suppression no ao)
		=> Manomboka izao rehefa manao jointure de tsy mi-join direct @ le table fa @le view efa sous-condition(exemple supprimee, en attente)