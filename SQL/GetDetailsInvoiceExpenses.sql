SELECT
	OPCH."DocNum",
	OPCH."DocEntry",
	OPCH."TaxDate",
	OPCH."NumAtCard",
	OPCH."CardCode",
	OCRD."CardName",
	OPCH."DocTotal",
	OPCH."DocRate",
	OPCH."Comments"
FROM OPCH
JOIN PCH1 ON OPCH."DocEntry" = PCH1."DocEntry"
JOIN OCRD ON OPCH."CardCode" = OCRD."CardCode"
WHERE
	OPCH."U_MGS_CL_NROIMP" = '{0}' AND
	PCH1."BaseEntry" NOT IN ({1})
GROUP BY 
	OPCH."DocNum",
	OPCH."DocEntry",
	OPCH."TaxDate",
	OPCH."NumAtCard",
	OPCH."CardCode",
	OCRD."CardName",
	OPCH."DocTotal",
	OPCH."DocRate",
	OPCH."Comments"