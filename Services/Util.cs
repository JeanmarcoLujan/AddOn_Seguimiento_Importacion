using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbouiCOM;
using System.IO;
using System.Reflection;

namespace sapping.Services
{
    class Util
    {
        public static string GetEmbeddedResource(string resource, Assembly asm)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return new StreamReader(asm.GetManifestResourceStream(resource)).ReadToEnd();
        }

        public static void AddRow(SAPbouiCOM.Form Form, string itemUID, string tableDetail)
        {
            try
            {
                Form.Freeze(true);
                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(itemUID).Specific;

                Form.DataSources.DBDataSources.Item(tableDetail).InsertRecord(Form.DataSources.DBDataSources.Item(tableDetail).Size);
                Form.DataSources.DBDataSources.Item(tableDetail).Offset = Form.DataSources.DBDataSources.Item(tableDetail).Size - 1;

                matrix.AddRow(1);

                for (int i = 1; i <= matrix.RowCount; i++)
                    matrix.SetCellWithoutValidation(i, "#", i.ToString());

                if (Form.Mode == BoFormMode.fm_OK_MODE)
                    Form.Mode = BoFormMode.fm_UPDATE_MODE;
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        public static void RemoveRow(SAPbouiCOM.Form Form, string itemUID, string tableDetail)
        {
            try
            {
                Form.Freeze(true);
                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(itemUID).Specific;
                var cantidad = matrix.RowCount;

                for (int i = cantidad; i >= 1; i--)
                {
                    if (matrix.IsRowSelected(i))
                        matrix.DeleteRow(i);
                }

                for (int i = 1; i <= matrix.RowCount; i++)
                    matrix.SetCellWithoutValidation(i, "#", i.ToString());

                if (Form.Mode == BoFormMode.fm_OK_MODE)
                    Form.Mode = BoFormMode.fm_UPDATE_MODE;
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        public void FillColumnComboBoxWithQuery(B1Forms oForm, string query, string matrixUID, string columnuUID)
        {
            SAPbouiCOM.Column combo = null;

            if ( ((SAPbouiCOM.Matrix)oForm.Items.Item(matrixUID).Specific) == null)
                combo = ((SAPbouiCOM.Matrix)oForm.Items.Item(matrixUID).Specific).Columns.Item(columnuUID);
            else
                combo = ((SAPbouiCOM.Matrix)oForm.Items.Item(matrixUID).Specific).Columns.Item(columnuUID);

            var valores = Record.Instance.Query(query).Execute().All();

            for (int j = 0; j < valores.Length; j++)
                combo.ValidValues.Add(valores[j]["Value"], valores[j]["Description"]);
        }
    }
}
