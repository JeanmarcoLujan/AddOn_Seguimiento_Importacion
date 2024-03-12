using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace sapping.Form._2009032023
{
    class EditText_txtParAnt : B1Item
    {
        public EditText_txtParAnt()
        {
            FormType = "2009032023";
            ItemUID = "txtParAnt";
        }       

        [B1Listener(BoEventTypes.et_LOST_FOCUS, false)]
        public virtual void OnAfterLostFocus(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
           
            //var tipImp = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_TIPIMP", 0);
            //var currYear = DateTime.Now.ToString("yyyy");
            //var queryCode = string.Format(GetEmbeddedResource("sapping.SQL.GetCountSegImp.sql"), tipEnv);
            //var record = (SAPbobsCOM.Recordset)B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //record.DoQuery(queryCode);

            //Form.DataSources.DBDataSources.Item(0).SetValue("Code", 0, $"{tipImp}-{currYear}-{tipEnv}{record.Fields.Item("Code").Value}");
            if (Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_ESPARC", 0) == "Y" && Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0) != "")
            {
                if (Convert.ToString(Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0)).Substring(Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0).Length - 3).Contains("-P"))
                {
                    string oldCode = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0);
                    string oldChar = Convert.ToString(Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0)).Substring(Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0).Length - 1);

                    int nextChar = Convert.ToInt32(oldChar) + 1;
                    Form.DataSources.DBDataSources.Item(0).SetValue("Code", 0, oldCode.Replace("-P" + oldChar, "-P" + nextChar.ToString()));
                    //string letter = Convert.ToString(Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0)).Substring(Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PARANT", 0).Length - 1);
                    //char cc = Convert.ToChar(letter);
                    //char nextChar = (char)(((int)cc) + 1);


                }
            }


            if (Form.Mode == BoFormMode.fm_OK_MODE)
                Form.Mode = BoFormMode.fm_UPDATE_MODE;
        }
    }
}
