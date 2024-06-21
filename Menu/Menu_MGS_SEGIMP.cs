using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace sapping.Menu
{
    class Menu_MGS_SEGIMP : B1XmlFormMenu
    {
        public Menu_MGS_SEGIMP()
        {
            MenuUID = "MGS_SEGIMP";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            var uid = Guid.NewGuid().ToString().Substring(0, 6);
            var xml = string.Format(B1Util.GetEmbeddedResource("sapping.Form._2009032023.srf.Frm_SegImp.srf", GetType().Assembly), uid);
            _init(xml, uid);
        }

        private void _init(string xml, string uid)
        {
            B1Connections.SboApp.LoadBatchActions(ref xml);
            Form = new B1Forms(uid);
            FillComboBoxWithQuery($@"SELECT ""Code"" AS ""Value"", ""Name"" AS ""Description"" FROM ""@MGS_CL_TIPARA""", "Item_1");
            FillComboBoxWithQuery($@"SELECT ""Code"" AS ""Value"", ""Name"" AS ""Description"" FROM ""@MGS_CL_PUERTO"" WHERE ""U_MGS_CL_TIPPUE"" = '1'", "Item_13");
            FillComboBoxWithQuery($@"SELECT ""Code"" AS ""Value"", ""Name"" AS ""Description"" FROM ""@MGS_CL_PUERTO"" WHERE ""U_MGS_CL_TIPPUE"" = '2'", "Item_15");
            FillComboBoxWithQuery($@"SELECT ""Code"" AS ""Value"", ""Name"" AS ""Description"" FROM ""@MGS_CL_STALLE""", "Item_19");
            FillComboBoxWithQuery($@"SELECT ""Code"" AS ""Value"", ""Name"" AS ""Description"" FROM ""@MGS_CL_STADOC""", "Item_16");
            FillComboBoxWithQuery($@"SELECT ""Code"" AS ""Value"", ""Name"" AS ""Description"" FROM ""@MGS_CL_STAIMP""", "Item_37");
            FillComboBoxWithQuery($@"SELECT ""Code"" AS ""Value"", ""Name"" AS ""Description"" FROM ""@MGS_CL_CANAL""", "Item_17");
            //Form.SetCflConditions("C_AgAdC", "CardType", BoConditionOperation.co_EQUAL, "S");
            //Form.SetCflConditions("C_ForC", "CardType", BoConditionOperation.co_EQUAL, "S");
            //Form.SetCflConditions("C_TransC", "CardType", BoConditionOperation.co_EQUAL, "S");
            Form.SetCflConditions("C_AgAdC", new string[] { "CardType", "QryGroup6" }, new BoConditionOperation[] { BoConditionOperation.co_EQUAL, BoConditionOperation.co_EQUAL }, new string[] { "S", "Y" });
            Form.SetCflConditions("C_ForC", new string[] { "CardType", "QryGroup7" }, new BoConditionOperation[] { BoConditionOperation.co_EQUAL, BoConditionOperation.co_EQUAL }, new string[] { "S", "Y" });
            Form.SetCflConditions("C_TransC", new string[] { "CardType", "QryGroup8" }, new BoConditionOperation[] { BoConditionOperation.co_EQUAL, BoConditionOperation.co_EQUAL }, new string[] { "S", "Y" });
            Form.SetCflConditions("C_lleg","Location", BoConditionOperation.co_EQUAL, "1");
            //Form.SetCflConditions("C_Pue1", "Code", BoConditionOperation.co_EQUAL, "01");
            //Form.Items.Item("cmbParcial").Specific.Value = "asd";
            Form.DataSources.DBDataSources.Item(0).SetValue("U_MGS_CL_ESPARC", 0, "N");
            Form.Items.Item("Item_41").Height =  20;

            Form.Visible = true;
        }
    }
}
