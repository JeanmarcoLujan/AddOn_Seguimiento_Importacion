using B1Framework.B1Frame;
using SAPbouiCOM;
using sapping.Form._2002032023;
using System;


namespace sapping.Menu
{
    public class Menu_CpyPed : B1XmlFormMenu
    {
        public Menu_CpyPed()
        {
            MenuUID = "CpyPed";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            var parentForm = B1Connections.SboApp.Forms.ActiveForm;
            var uid = Guid.NewGuid().ToString().Substring(0, 6);
            var xml = string.Format(B1Util.GetEmbeddedResource("sapping.Form._2002032023.srf.Frm_Pedidos.srf", GetType().Assembly), uid);
            _init(parentForm, xml, uid);
        }

        private void _init(SAPbouiCOM.Form oForm, string xml, string uid)
        {
            B1Connections.SboApp.LoadBatchActions(ref xml);
            Form = new B1Forms(uid);
            var grid = (Grid)Form.Items.Item("grdResPed").Specific;
            grid.Item.Enabled = false;
            Form.SetCflConditions("C_Prov", "CardType", BoConditionOperation.co_EQUAL, "S");
            Button_btnSearch.LoadDataSearch(Form, GetType().Assembly);
            //Form.DataSources.UserDataSources.Item("U_Parent").Value = oForm.UniqueID;
            oForm.DataSources.UserDataSources.Item("U_Child").Value = Form.UniqueID;
            Form.DataSources.UserDataSources.Item("U_Parent").Value = oForm.UniqueID;
            Form.Visible = true;
        }

    }
}
