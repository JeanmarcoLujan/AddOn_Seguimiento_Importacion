using B1Framework.B1Frame;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapping.Services
{
    class UDT
    {
        public static string[] AddFieldUdF(string tabla, string code, string valueName, string[] fields, string[] valores)
        {
            var userDefinedTable = B1Connections.DiCompany.UserTables.Item(tabla);
            var error = "";
            var tempColumnName = "";
            var tempValueName = "";

            try
            {
                userDefinedTable.Code = code;
                userDefinedTable.Name = valueName;

                for (var i = 0; i < fields.Length; i++)
                {
                    userDefinedTable.UserFields.Fields.Item(fields[i]).Value = valores[i];
                }

                var temp = userDefinedTable.Add();
                error = B1Connections.DiCompany.GetLastErrorDescription() + " - " + tempColumnName + " - " + tempValueName;

                Log.Info("enviarCarvajal AddFieldUdF temp ->> " + temp);
                Log.Info("enviarCarvajal AddFieldUdF error ->> " + error);

                return new string[] { temp.ToString(), error };
            }
            catch (Exception ex)
            {
                Log.Info("enviarCarvajal AddFieldUdF ->> " + ex.Message);
                return new string[] { "-1.", ex.Message + " - " + tempColumnName + " - " + tempValueName };
            }
            finally
            {

            }
        }

        public static void UpdateFieldUdf(string tabla, string docnum, string field, object value, BoObjectTypes tipo = BoObjectTypes.oUserTables)
        {
            dynamic userDefinedTable;

            if (tipo == BoObjectTypes.oUserTables)
                userDefinedTable = B1Connections.DiCompany.UserTables.Item(tabla);
            else
                userDefinedTable = B1Connections.DiCompany.GetBusinessObject(tipo);

            var error = "";
            try
            {
                Log.Info("Entro UpdateFieldUdf 0 ->> "+docnum);

                if (!userDefinedTable.GetByKey(docnum)) return;

                Log.Info("Entro UpdateFieldUdf 1");
                userDefinedTable.UserFields.Fields.Item(field).Value = value;
                userDefinedTable.Update();
                error = B1Connections.DiCompany.GetLastErrorDescription();
                Log.Info("Entro UpdateFieldUdf error "+error);
            }
            catch (Exception ex)
            {
                Log.Error("UpdateFieldUdf Exception ->> " + ex.Message);
            }
            finally
            {
                B1Connections.FlushMemory(userDefinedTable);
            }
        }

    }
}
