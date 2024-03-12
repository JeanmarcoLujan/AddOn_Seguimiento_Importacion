using B1Framework.B1Frame;
using Newtonsoft.Json;
using SAPbouiCOM;

namespace sapping.Form._2010032023
{
    class Button_btnAct : B1Item
    {
        public Button_btnAct()
        {
            FormType = "2010032023";
            ItemUID = "btnAct";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var dt = Form.DataSources.DataTables.Item("DT_ArtCont");
            var dtTemp = Form.DataSources.DataTables.Item("DT_Temp");

            int val = 0;
            int line = 0;
            var grdCont = (Grid)Form.Items.Item("grdCont").Specific;
            for (int j = 0; j < grdCont.Rows.Count; j++)
            {
                string v1 = grdCont.DataTable.GetValue(4, j);
                string v2 = grdCont.DataTable.GetValue(5, j);
                dtTemp.ExecuteQuery(string.Format(GetEmbeddedResource("sapping.SQL.GetCargaSuelta.sql"), grdCont.DataTable.GetValue(1, j)));
                
                if (dtTemp.Rows.Count == 1)
                {
                    if (dtTemp.GetValue("Cant", 0).ToString().Equals("0"))
                    {
                        if (grdCont.DataTable.GetValue(4, j) == "" || grdCont.DataTable.GetValue(5, j) == "")
                        {
                            val = val + 1;
                            line = j;
                        }
                    }
                }                    
            }

            if (val > 0)
            {
                B1Connections.SboApp.SetStatusBarMessage("Debe Ingresar el Nro. de Contenedor y Precinto, Linea: " + (line+1).ToString());
            }
            else
            {
                var formFather = B1Connections.SboApp.Forms.Item(Form.DataSources.UserDataSources.Item("U_FrmFath").Value);
                var dtFather = formFather.DataSources.DataTables.Item("DT_CAsg");
                dtFather.Rows.Clear();
                formFather.DataSources.DBDataSources.Item(0).SetValue("U_MGS_CL_CONTAI", 0, dt.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var structXml = dt.GetValue("CCAsig", 0);
                    var jsonObject = JsonConvert.DeserializeObject<JsonTemp>(structXml);
                    var cntCode = dt.GetValue("CCont", 0);
                   
                    dtTemp.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, jsonObject.ArtAsig);
                    for (int j = 0; j < dtTemp.Rows.Count; j++)
                    {
                        dtFather.Rows.Add();
                        dtFather.SetValue("CLine", dtFather.Rows.Count - 1, dtTemp.GetValue("CLine", j));
                        dtFather.SetValue("CCod", dtFather.Rows.Count - 1, dtTemp.GetValue("CCod", j));
                        dtFather.SetValue("CCant", dtFather.Rows.Count - 1, dtTemp.GetValue("CCantAsg", j));
                        dtFather.SetValue("CPeso", dtFather.Rows.Count - 1, dtTemp.GetValue("CPesNeto", j));
                        dtFather.SetValue("CTipCnt", dtFather.Rows.Count - 1, cntCode);
                    }
                }

                if (formFather.Mode == BoFormMode.fm_OK_MODE)
                    formFather.Mode = BoFormMode.fm_UPDATE_MODE;

                Form.Close();
            }                  
        }
    }

    public class JsonTemp
    {
        public string ArtImp { get; set; }
        public string ArtAsig { get; set; }
    }
}
