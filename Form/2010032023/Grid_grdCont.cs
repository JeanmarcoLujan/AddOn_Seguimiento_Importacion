using B1Framework.B1Frame;
using Newtonsoft.Json;
using SAPbouiCOM;

namespace sapping.Form._2010032023
{
    class Grid_grdCont : B1Item
    {
        public Grid_grdCont()
        {
            FormType = "2010032023";
            ItemUID = "grdCont";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form.DataSources.DataTables.Item("DT_ArtCont").SetValue("CCont", pVal.Row, ListChoiceListener(pVal, "Code")[0].ToString());
            Form.DataSources.DataTables.Item("DT_ArtCont").SetValue("CDesc", pVal.Row, ListChoiceListener(pVal, "Name")[0].ToString());
            Form.DataSources.DataTables.Item("DT_ArtCont").SetValue("CPesMax", pVal.Row, ListChoiceListener(pVal, "U_MGS_CL_PESMAX")[0].ToString());
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            try
            {
                Form.Freeze(true);
                if (pVal.ColUID == "RowsHeader" && pVal.Row > -1)
                {
                    var hasValueAsign = Form.DataSources.DataTables.Item("DT_ArtCont").GetValue("CCAsig", pVal.Row);
                    var dtItemsToCppy = Form.DataSources.DataTables.Item("DT_ItmsUse");
                    var dtArtSeg = Form.DataSources.DataTables.Item("DT_ArtImp");
                    var dtArtAsig = Form.DataSources.DataTables.Item("DT_ArtAsg");

                    var record = (SAPbobsCOM.Recordset)B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    if ( string.IsNullOrEmpty(hasValueAsign) )
                    {
                        dtArtSeg.Rows.Clear();
                        dtArtAsig.Rows.Clear();
                        var grdArtSeg = (Grid)Form.Items.Item("grdArtImp").Specific;                      

                        for ( int i=0; i<dtItemsToCppy.Rows.Count; i++ )
                        {
                            var qryValueBef = string.Format(GetEmbeddedResource("sapping.SQL.GetItemInventory.sql"), dtItemsToCppy.GetValue("CCodItm", i));
                            record.DoQuery(qryValueBef);
                            if (record.Fields.Item("Cant").Value > 0)
                            {
                                dtArtSeg.Rows.Add();
                                dtArtSeg.SetValue("CLine", dtArtSeg.Rows.Count - 1, i + 1);
                                dtArtSeg.SetValue("CCod", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CCodItm", i));
                                dtArtSeg.SetValue("CDesc", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CDesc", i));
                                dtArtSeg.SetValue("CCantPed", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CQuantity", i));
                                dtArtSeg.SetValue("CCantDsp", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CQuantity", i));
                                //dtArtSeg.SetValue("CCantAsg", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CQuantity", i) - record.Fields.Item("VAL").Value);

                                //cambio temporal de cantidad asignada Luis Vicente
                                var itemCode = dtItemsToCppy.GetValue("CCodItm", i);
                                var qryContAsig = "SELECT MAX(\"U_MGS_CL_CASIG\") AS VAL FROM \"@MGS_CL_SEGIAR\" WHERE \"U_MGS_CL_CODART\" = '" + itemCode + "' AND \"Code\" = '" + "DTD-2023-AE0001" + "';";
                                record.DoQuery(qryContAsig);
                                dtArtSeg.SetValue("CCantAsg", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CQuantity", i) - record.Fields.Item("VAL").Value);
                                //*****************************************************

                                dtArtSeg.SetValue("CPes", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CPesInv", i));
                                dtArtSeg.SetValue("CPesNeto", dtArtSeg.Rows.Count - 1, dtItemsToCppy.GetValue("CPesInv", i) * dtItemsToCppy.GetValue("CQuantity", i));
                                grdArtSeg.RowHeaders.SetText(i, (i + 1).ToString());
                            }
                        }

                        var valueToSerial = new { ArtImp = dtArtSeg.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly), ArtAsig = dtArtAsig.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly) };
                        Form.DataSources.DataTables.Item("DT_ArtCont").SetValue("CCAsig", pVal.Row, JsonConvert.SerializeObject(valueToSerial));
                    }
                    else
                    {
                        var valueDeserDef = new { ArtImp = "", ArtAsig = "" };
                        var objectValues = JsonConvert.DeserializeAnonymousType(hasValueAsign, valueDeserDef);
                        dtArtSeg.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, objectValues.ArtImp);
                        dtArtAsig.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, objectValues.ArtAsig);
                    }
                }
            }
            finally
            {
                Form.Freeze(false);
            }
        }
    }
}
