using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace sapping.Form._2009032023
{
    class ComboBox_cmbTipImp : B1Item
    {
        public ComboBox_cmbTipImp()
        {
            FormType = "2009032023";
            ItemUID = "cmbTipImp";
        }

        [B1Listener(BoEventTypes.et_COMBO_SELECT, false)]
        public virtual void OnAfterComboSelect(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var tipEnv = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_TIPENV", 0);
            var tipImp = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_TIPIMP", 0);
            var parc = "P1";
            var currYear = DateTime.Now.ToString("yyyy");
            var queryCode = string.Format(GetEmbeddedResource("sapping.SQL.GetCountSegImp.sql"), tipEnv);
            var record = (SAPbobsCOM.Recordset)B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            record.DoQuery(queryCode);

            Form.DataSources.DBDataSources.Item(0).SetValue("Code", 0, $"{tipImp}-{currYear}-{tipEnv}{record.Fields.Item("Code").Value}-{parc}");
        }
    }
}
