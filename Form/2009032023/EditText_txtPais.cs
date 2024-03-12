using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2009032023
{
    class EditText_txtPais : B1Item
    {
        public EditText_txtPais()
        {
            FormType = "2009032023";
            ItemUID = "txtPais";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((SAPbouiCOM.IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            var country = ListChoiceListener(pVal, "Code")[0].ToString();
            var countryName = ListChoiceListener(pVal, "Name")[0].ToString();

            Form.DataSources.DBDataSources.Item(0).SetValue("U_MGS_CL_PAISOR", 0, country);
            Form.DataSources.DBDataSources.Item(0).SetValue("U_MGS_CL_PAISNM", 0, countryName);

            if (Form.Mode == BoFormMode.fm_OK_MODE)
                Form.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        [B1Listener(BoEventTypes.et_LOST_FOCUS, false)]
        public virtual void OnAfterLostFocus(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (string.IsNullOrEmpty(Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PAISOR", 0)))
            {
                Form.DataSources.DBDataSources.Item(0).SetValue("U_MGS_CL_PAISNM", 0, "");
            }

            if (Form.Mode == BoFormMode.fm_OK_MODE)
                Form.Mode = BoFormMode.fm_UPDATE_MODE;
        }
    }
}
