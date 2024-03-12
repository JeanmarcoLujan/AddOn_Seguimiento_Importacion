using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2009032023
{
    class Form_Events : B1Item
    {
        public Form_Events()
        {
            FormType = "2009032023";
            ItemUID = "*";
        }

        [B1Listener(BoEventTypes.et_FORM_RESIZE, false)]
        public virtual void OnAfterFormResize(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Form.Items.Item("Item_46").Height = Form.Items.Item("mtxItms").Height + 20;
            Form.Items.Item("Item_46").Top = Form.Items.Item("Item_6").Top + 60;
            Form.Items.Item("Item_47").Top = Form.Items.Item("Item_6").Top + 41;
            Form.Items.Item("Item_53").Top = Form.Items.Item("Item_6").Top + 41;
            Form.Items.Item("Item_2").Top = Form.Items.Item("Item_6").Top + 41;
        }


        [B1Listener(BoEventTypes.et_CLICK, true)]
        public virtual bool OnBeforeFormClose(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (!string.IsNullOrEmpty(Form.DataSources.UserDataSources.Item("U_Child").Value))
            {
                B1Connections.SboApp.Forms.Item(Form.DataSources.UserDataSources.Item("U_Child").Value).Select();
                return false;
            }

            return true;
        }

    }
}
