using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2009032023
{
    class EditText_txtLocLl : B1Item
    {
        public EditText_txtLocLl()
        {
            FormType = "2009032023";
            ItemUID = "txtLocLl";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((SAPbouiCOM.IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            var whsCode = ListChoiceListener(pVal, "WhsCode")[0].ToString();

            Form.DataSources.DBDataSources.Item(0).SetValue("U_MGS_CL_LOCLLE", 0, whsCode);

            if (Form.Mode == BoFormMode.fm_OK_MODE)
                Form.Mode = BoFormMode.fm_UPDATE_MODE;
        }
    }
}
