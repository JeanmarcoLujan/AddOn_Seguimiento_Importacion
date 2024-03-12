using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace sapping.Form._2009032023
{
    class ComboBox_Item_22 : B1Item
    {
        public ComboBox_Item_22()
        {
            FormType = "2009032023";
            ItemUID = "Item_22";
        }

        [B1Listener(BoEventTypes.et_COMBO_SELECT, false)]
        public virtual void OnAfterComboSelect(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
        }
    }
}
