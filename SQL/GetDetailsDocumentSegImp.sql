SELECT 
          X1."CardCode",
          X0."U_MGS_CL_NROINT",
          X0."U_MGS_CL_CODART",
          X0."U_MGS_CL_CANT",
          X1."DocEntry",
          X1."ObjType",
          X2."LineNum"
FROM "@MGS_CL_SEGIAR" X0
JOIN OPOR X1 ON X0."U_MGS_CL_NROINT" = X1."DocEntry"
JOIN POR1 X2 ON X1."DocEntry" = X2."DocEntry" AND X2."ItemCode" = X0."U_MGS_CL_CODART" AND X2."LineNum" = X0."U_MGS_CL_NROLNE"
WHERE X0."Code" = '{0}' AND X0."U_MGS_CL_NROINT" = '{1}'
