using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2002032023
{
    class Form_Events : B1Item
    {
        public Form_Events()
        {
            FormType = "2002032023";
            ItemUID = "*";
        }

        [B1Listener(BoEventTypes.et_CLICK, true)]
        public virtual bool OnBeforeClick(ItemEvent pVal)
        {        
            return true;
        }

        [B1Listener(BoEventTypes.et_FORM_CLOSE, false)]
        public virtual void OnBeforeFormClose(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (!string.IsNullOrEmpty(Form.DataSources.UserDataSources.Item("U_Parent").Value))
            {
                B1Connections.SboApp.Forms.Item(Form.DataSources.UserDataSources.Item("U_Parent").Value).DataSources.UserDataSources.Item("U_Child").Value = string.Empty;
            }
        }
    }
}
