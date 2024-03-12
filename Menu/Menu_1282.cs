using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Menu
{
    class Menu_1282 : B1XmlFormMenu
    {
        public Menu_1282()
        {
            MenuUID = "1282";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            var oForm = B1Connections.SboApp.Forms.ActiveForm;
            if (oForm.TypeEx == "2009032023")
            {
                //Activa Tipo Envio y Tipo de Importación
                oForm.Items.Item("cmbTipEnv").Enabled = true;
                oForm.Items.Item("cmbTipImp").Enabled = true;
                
            }            
        }
    }
}
