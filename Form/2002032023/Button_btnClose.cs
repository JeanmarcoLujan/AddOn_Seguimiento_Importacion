using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2002032023
{
    class Button_btnClose : B1Item
    {
        public Button_btnClose()
        {
            FormType = "2002032023";
            ItemUID = "btnClose";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Form.Close();
        }
    }
}
