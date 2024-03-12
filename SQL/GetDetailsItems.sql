SELECT
	ROW_NUMBER() OVER (ORDER BY "DocNum", "LineNum") AS "Line",  
	OPOR."DocNum",
	OPOR."DocEntry",
	POR1."ItemCode",
	POR1."Dscription",
	POR1."Quantity"-IFNULL("DocSeg"."U_MGS_CL_CANT",0) AS "Quantity",
	POR1."Quantity"-IFNULL("DocSeg"."U_MGS_CL_CANT",0) AS "U_MGS_CL_CANT",
	OITM."BuyUnitMsr",
	OITM."IWeight1",
	(POR1."Quantity"*OITM."IWeight1") AS "QuantityWeight",
	POR1."Price",
	(POR1."Price"*POR1."Quantity") AS "Total",
	POR1."LineNum",
	"DocSeg"."U_MGS_CL_NROINT",
	"DocSeg"."Code",
    "DocSeg"."U_MGS_CL_CANT",
	'' AS "FacPre",
	POR1."U_MGS_CL_ANCHO",
	POR1."U_MGS_CL_LARGO",
	POR1."U_MGS_CL_CANBOB"
FROM OPOR
JOIN POR1 ON OPOR."DocEntry" = POR1."DocEntry"
JOIN OITM ON POR1."ItemCode" = OITM."ItemCode"
LEFT JOIN 
(
	SELECT
		X0."Code",
		"U_MGS_CL_CODART",
		"U_MGS_CL_NROINT",
        SUM("U_MGS_CL_CANT") AS "U_MGS_CL_CANT"
	FROM "@MGS_CL_SEGIAR" X0 
	INNER JOIN "@MGS_CL_SEGIMP" X1 ON X0."Code" = X1."Code"
    WHERE 
		"U_MGS_CL_NROINT" IN ({0}) AND 
		"U_MGS_CL_STATUS" NOT IN ('CA') 
GROUP BY 
		X0."Code",
		"U_MGS_CL_CODART",
		"U_MGS_CL_NROINT"
) "DocSeg" ON POR1."DocEntry" = "DocSeg"."U_MGS_CL_NROINT"  AND POR1."ItemCode" = "U_MGS_CL_CODART" 
WHERE

	OPOR."DocEntry" IN ({0})
AND POR1."Quantity"-IFNULL("DocSeg"."U_MGS_CL_CANT",0) > 0
/*AND OITM."InvntItem" = 'Y'*/ /*LV 20230914: SE SOLICITO INCLUIR TAMBIEN LOS PRODUCTOS TIPO SERVICIO*/