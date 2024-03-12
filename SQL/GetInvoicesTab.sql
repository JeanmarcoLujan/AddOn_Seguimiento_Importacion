SELECT 
	OPCH."DocEntry" AS "N° Interno",
	OPCH."DocNum" AS "N° Documento",
	OPCH."DocDate" AS "Fecha",
	OPCH."NumAtCard" AS "N° Control",
	OCRD."CardCode" AS "Cod. Proveedor",
	OCRD."CardName" AS "Nombre",
	OPCH."DocTotal" AS "Importe",
	OPCH."DocRate" AS "Tipo de Cambio",
	OPCH."Comments" AS "Comentarios"
FROM OPCH
INNER JOIN OCRD ON OPCH."CardCode" = OCRD."CardCode"
WHERE
	"U_MGS_CL_NROIMP" = '{0}'