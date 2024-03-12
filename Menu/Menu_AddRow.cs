using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Menu
{
    class Menu_AddRow : B1XmlFormMenu
    {
        public Menu_AddRow()
        {
            MenuUID = "AddRow";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            var oForm = B1Connections.SboApp.Forms.ActiveForm;

            if (oForm.TypeEx == "2010032023")
            {
                var grid = (Grid)oForm.Items.Item("grdCont").Specific;
                var dt = oForm.DataSources.DataTables.Item("DT_ArtCont");
                dt.Rows.Add();
                dt.SetValue("CLine", dt.Rows.Count - 1, dt.Rows.Count);

                grid.RowHeaders.SetText(dt.Rows.Count - 1, dt.Rows.Count.ToString());
                grid.AutoResizeColumns();
            }
            else if (oForm.TypeEx == "2009032023")
            {
                if( oForm.PaneLevel == 2 )
                {
                    var objMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxAnex").Specific;
                    oForm.DataSources.DBDataSources.Item("@MGS_CL_SEGANX").InsertRecord(oForm.DataSources.DBDataSources.Item("@MGS_CL_SEGANX").Size);
                    oForm.DataSources.DBDataSources.Item("@MGS_CL_SEGANX").Offset = oForm.DataSources.DBDataSources.Item("@MGS_CL_SEGANX").Size - 1;
                    objMatrix.AddRow(1);
                    objMatrix.FlushToDataSource();

                    for (int i = 1; i <= objMatrix.RowCount; i++)
                         ((EditText)objMatrix.GetCellSpecific("#", i)).Value = i.ToString();
                }
            }
        }
    }
}
