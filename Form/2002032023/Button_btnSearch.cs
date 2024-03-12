using B1Framework.B1Frame;
using SAPbouiCOM;
using sapping.Services;
using System.Reflection;

namespace sapping.Form._2002032023
{
    class Button_btnSearch : B1Item
    {
        public Button_btnSearch()
        {
            FormType = "2002032023";
            ItemUID = "btnSearch";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            try
            {
                Form.Freeze(true);
                LoadDataSearch(Form, GetType().Assembly);
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        public static void LoadDataSearch(B1Forms oForm, Assembly asm)
        {
            var cardCode = oForm.DataSources.UserDataSources.Item("UD_CardC").Value;
            var grid = (Grid)oForm.Items.Item("grdResPed").Specific;
            var dt = oForm.DataSources.DataTables.Item("DT_Data");
            dt.ExecuteQuery(string.Format(Services.Util.GetEmbeddedResource("sapping.SQL.GetPurchaseOrderData.sql", asm), cardCode));

            if (dt.Rows.Count == 1)
            {
                if (dt.GetValue("N° Pedido", 0).ToString().Equals("0"))
                {
                    grid.DataTable = oForm.DataSources.DataTables.Item("DT_NoData");
                    grid.AutoResizeColumns();
                    return;
                }
            }

            grid.DataTable = dt;
            grid.Columns.Item("Opt").Type = BoGridColumnType.gct_CheckBox;
            grid.Columns.Item("Opt").TitleObject.Caption = string.Empty;
            grid.Columns.Item("Opt").BackColor = 16777215;
            ((EditTextColumn)grid.Columns.Item("N° Pedido")).LinkedObjectType = "14";
            ((EditTextColumn)grid.Columns.Item("Codigo del Proveedor")).LinkedObjectType = "2";
            grid.Columns.Item("N° Pedido interno").Visible = false;

            grid.AutoResizeColumns();
        }
    }
}
