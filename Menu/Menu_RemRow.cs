using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Menu
{
    class Menu_RemRow : B1XmlFormMenu
    {
        public Menu_RemRow()
        {
            MenuUID = "RemRow";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            var oForm = B1Connections.SboApp.Forms.ActiveForm;
            if (oForm.TypeEx == "2010032023")
            {
                var grd = (Grid)oForm.Items.Item("grdCont").Specific;
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    if (grd.Rows.IsSelected(i))
                    {
                        oForm.DataSources.DataTables.Item("DT_ArtCont").Rows.Remove(i);
                    }
                }
            }
            else if (oForm.TypeEx == "2009032023")
            {
                var ma = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxItms").Specific;
                    ma.DeleteRow(ma.GetNextSelectedRow());

                if (oForm.Mode == BoFormMode.fm_OK_MODE)
                    oForm.Mode = BoFormMode.fm_UPDATE_MODE;
            }
        }
    }
}
