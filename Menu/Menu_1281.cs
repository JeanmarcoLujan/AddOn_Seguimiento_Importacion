using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Menu
{
    class Menu_1281 : B1XmlFormMenu
    {
        public Menu_1281()
        {
            MenuUID = "1281";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            var oForm = B1Connections.SboApp.Forms.ActiveForm;
            if (oForm.TypeEx == "2009032023")
            {
                //Activa Code, Tipo Envio y Tipo de Importación
                oForm.Items.Item("1").Enabled = true;
                oForm.Items.Item("txtCode").Enabled = true;
                oForm.Items.Item("cmbTipEnv").Enabled = true;
                oForm.Items.Item("cmbTipImp").Enabled = true;
            }            
        }
    }
}
