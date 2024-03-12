CALL "MGS_HDB_PE_SP_ADDON_SEGIMP_LISTAPED";
/*
SELECT
	'N' AS "Opt",
	"Group"."N° Pedido",
	"Group"."N° Pedido interno",
	"Group"."Fecha",
	"Group"."Pais",
	"Group"."Codigo del Proveedor",
	"Group"."Nombre del Proveedor",
	"Group"."Total"
FROM
(
	SELECT		
		OPOR."DocNum" AS "N° Pedido",
		OPOR."DocEntry" AS "N° Pedido interno",
		OPOR."DocDate" AS "Fecha",
		OCRY."Name" AS "Pais",
		OCRD."CardCode" AS "Codigo del Proveedor",
		OCRD."CardName" AS "Nombre del Proveedor",
		OPOR."DocTotal" AS "Total",
		(POR1."Quantity" - IFNULL("Imp"."Qty",0)) AS "TotalQty"
	FROM OPOR
	INNER JOIN POR1 ON OPOR."DocEntry" = POR1."DocEntry"
	INNER JOIN OCRD ON OPOR."CardCode" = OCRD."CardCode" 
	INNER JOIN OCRY ON OCRD."Country" = OCRY."Code"
	LEFT JOIN
	(
		SELECT 
			"U_MGS_CL_NROINT", 
			"U_MGS_CL_CODART", 
			SUM("U_MGS_CL_CANT") AS "Qty" 
		FROM "@MGS_CL_SEGIAR" INNER JOIN "@MGS_CL_SEGIMP" ON "@MGS_CL_SEGIAR"."Code" = "@MGS_CL_SEGIMP"."Code"
		WHERE
			"@MGS_CL_SEGIMP"."U_MGS_CL_STATUS" NOT IN ('CA')
		GROUP BY 
			"U_MGS_CL_NROINT", "U_MGS_CL_CODART"
	) "Imp" ON OPOR."DocEntry" = "Imp"."U_MGS_CL_NROINT" AND POR1."ItemCode" = "Imp"."U_MGS_CL_CODART"
	WHERE
		OPOR."U_MGS_CL_TIPCOM" = '02' 
		AND OPOR."DocStatus" = 'O'
) "Group"
WHERE
	"Group"."TotalQty" > 0
GROUP BY 	
	"Group"."N° Pedido",
	"Group"."N° Pedido interno",
	"Group"."Fecha",
	"Group"."Pais",
	"Group"."Codigo del Proveedor",
	"Group"."Nombre del Proveedor",
	"Group"."Total"
*/