CREATE DATABASE EvalTest;

-- Fonction Varchar Primary Key --


SELECT pac.*,atu."Id" as "AccountTypeId"
FROM "AccountTypeUtils" as atu
JOIN "PersonAccountCsv" as pac
ON pac."TypeCompte" = atu."Type";