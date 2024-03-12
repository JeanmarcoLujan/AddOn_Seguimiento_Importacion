using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2010032023
{
    class Button_btnRemo : B1Item
    {
        public Button_btnRemo()
        {
            FormType = "2010032023";
            ItemUID = "btnRemo";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var mtx = ((SAPbouiCOM.Matrix)Form.Items.Item("mtxCont").Specific);
            for(int i=1; i<=mtx.RowCount; i++)
            {
                if (mtx.IsRowSelected(i))
                    mtx.DeleteRow(i);
            }
        }
    }
}
