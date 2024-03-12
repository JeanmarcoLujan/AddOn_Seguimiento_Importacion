using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbouiCOM;
using sapping.Services;
using System;
using System.Linq;
using System.Xml.Linq;

namespace sapping.Form._2002032023
{
    class Button_btnCreate : B1Item
    {
        public Button_btnCreate()
        {
            FormType = "2002032023";
            ItemUID = "btnCreate";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var gridData = (Grid)Form.Items.Item("grdResPed").Specific;
            var xmlData = gridData.DataTable.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly);
            var xData = XDocument.Parse(xmlData)
                            .Descendants("Cell")
                            .Where(attr => ((XElement)attr.FirstNode).Value.ToString().Equals("Opt") && ((XElement)attr.LastNode).Value.ToString().Equals("Y"))
                            .Select(attr => ((XElement)((XElement)attr.Parent.FirstNode.NextNode.NextNode).LastNode).Value )
                            .ToList();

            if( xData.Count == 0 )
            {
                B1Connections.SboApp.SetStatusBarMessage("No hay registros seleccionados");
                return;
            }

            var listDocs = string.Join(",", xData);
            OpenFormSeg(Form, listDocs);
            Form.Close();
        }

        public void OpenFormSeg(B1Forms oForm, string listDocs) 
        {
            var oFormSeg = new B1Forms(oForm.DataSources.UserDataSources.Item("U_Parent").Value);

            try
            {
                oFormSeg.Freeze(true);
                oFormSeg.DataSources.DBDataSources.Item(0).SetValue("CreateDate", 0, DateTime.Now.ToString("yyyyMMdd"));

                var dt = oFormSeg.DataSources.DataTables.Item("DT_ArtCont");
                    dt.ExecuteQuery(string.Format(GetEmbeddedResource("sapping.SQL.GetDetailsItems.sql"), listDocs));

                var mtxItem = (SAPbouiCOM.Matrix)oFormSeg.Items.Item("mtxItms").Specific;
                if (oFormSeg.Mode == BoFormMode.fm_ADD_MODE)
                {
                    mtxItem.Columns.Item("Col_1").DataBind.Bind("DT_ArtCont", "DocNum");
                    mtxItem.Columns.Item("Col_0").DataBind.Bind("DT_ArtCont", "DocEntry");
                    mtxItem.Columns.Item("Col_2").DataBind.Bind("DT_ArtCont", "ItemCode");
                    mtxItem.Columns.Item("Col_3").DataBind.Bind("DT_ArtCont", "Dscription");
                    mtxItem.Columns.Item("Col_4").DataBind.Bind("DT_ArtCont", "IWeight1");
                    mtxItem.Columns.Item("Col_5").DataBind.Bind("DT_ArtCont", "U_MGS_CL_CANT");
                    mtxItem.Columns.Item("Col_6").DataBind.Bind("DT_ArtCont", "QuantityWeight");
                    mtxItem.Columns.Item("Col_7").DataBind.Bind("DT_ArtCont", "Price");
                    mtxItem.Columns.Item("Col_8").DataBind.Bind("DT_ArtCont", "Total");
                    mtxItem.Columns.Item("Col_9").DataBind.Bind("DT_ArtCont", "LineNum");
                    mtxItem.Columns.Item("Col_11").DataBind.Bind("DT_ArtCont", "BuyUnitMsr");
                    mtxItem.Columns.Item("Col_12").DataBind.Bind("DT_ArtCont", "Quantity");
                    mtxItem.Columns.Item("Col_13").DataBind.Bind("DT_ArtCont", "U_MGS_CL_ANCHO");
                    mtxItem.Columns.Item("Col_14").DataBind.Bind("DT_ArtCont", "U_MGS_CL_LARGO");
                    mtxItem.Columns.Item("Col_15").DataBind.Bind("DT_ArtCont", "U_MGS_CL_CANBOB");
                    mtxItem.Columns.Item("#").DataBind.Bind("DT_ArtCont", "Line");
                    mtxItem.LoadFromDataSource();
                    mtxItem.AutoResizeColumns();
                }
                else if( oFormSeg.Mode == BoFormMode.fm_UPDATE_MODE || oFormSeg.Mode == BoFormMode.fm_OK_MODE )
                {
                    for(int i=0; i< dt.Rows.Count; i++)
                    {
                        Services.Util.AddRow(oFormSeg, "mtxItms", "@MGS_CL_SEGIAR");
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_0", dt.GetValue("DocEntry", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_1", dt.GetValue("DocNum", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_2", dt.GetValue("ItemCode", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_3", dt.GetValue("Dscription", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_4", dt.GetValue("IWeight1", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_5", dt.GetValue("Quantity", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_6", dt.GetValue("QuantityWeight", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_7", dt.GetValue("Price", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_8", dt.GetValue("Total", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_9", dt.GetValue("LineNum", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_11", dt.GetValue("BuyUnitMsr", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_12", dt.GetValue("Quantity", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_13", dt.GetValue("U_MGS_CL_ANCHO", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_14", dt.GetValue("U_MGS_CL_LARGO", i));
                        mtxItem.SetCellWithoutValidation(mtxItem.RowCount, "Col_15", dt.GetValue("U_MGS_CL_CANBOB", i));
                    }
                }

                for (int i = 0; i < mtxItem.Columns.Count; i++)
                {
                    if (mtxItem.Columns.Item(i).UniqueID.Equals("Col_5"))
                    {
                        mtxItem.Columns.Item(i).Editable = true;
                        continue;
                    }

                    mtxItem.Columns.Item(i).Editable = false;
                }

                mtxItem.Columns.Item("Col_0").Visible = false;
                mtxItem.Columns.Item("Col_9").Visible = false;
                mtxItem.Columns.Item("Col_5").Editable = true;
                mtxItem.Columns.Item("Col_13").Editable = true;
                mtxItem.Columns.Item("Col_14").Editable = true;
                mtxItem.Columns.Item("Col_15").Editable = true;
                mtxItem.Columns.Item("Col_6").ColumnSetting.SumType = BoColumnSumType.bst_Auto;
                mtxItem.Columns.Item("Col_8").ColumnSetting.SumType = BoColumnSumType.bst_Auto;

                var dtGas = oFormSeg.DataSources.DataTables.Item("DT_FactGa");
                dtGas.ExecuteQuery(string.Format(GetEmbeddedResource("sapping.SQL.GetDetailsInvoiceExpenses.sql"), oFormSeg.DataSources.DBDataSources.Item(0).GetValue("Code", 0), listDocs));

                oFormSeg.DataSources.UserDataSources.Item("U_Child").Value = string.Empty;
            }
            finally
            {
                oFormSeg.Freeze(false);
            }
        }

        public void FillComboBoxWithQuery(B1Forms oForm, string query, string uidCombo)
        {
            var combo = (ComboBox)oForm.Items.Item(uidCombo).Specific;

            if (combo == null)
                combo = (ComboBox)B1Connections.SboApp.Forms.Item(Form.UniqueID).Items.Item(uidCombo).Specific;

            var valores = Record.Instance.Query(query).Execute().All();

            for (int j = 0; j < valores.Length; j++)
                combo.ValidValues.Add(valores[j]["Value"], valores[j]["Description"]);
        }
    }
}
