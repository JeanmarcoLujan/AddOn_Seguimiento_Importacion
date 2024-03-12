using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2010032023
{
    class Button_btnCanc : B1Item
    {
        public Button_btnCanc()
        {
            FormType = "2010032023";
            ItemUID = "btnCanc";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Form.Close();
        }
    }
}
