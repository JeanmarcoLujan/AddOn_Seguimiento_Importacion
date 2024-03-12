using B1Framework.B1Frame;
using SAPbobsCOM;
using SAPbouiCOM;
using System;

namespace sapping.Form._2009032023
{
    public enum FORM_STATUS { CERRADO, ABIERTO, CANCELADO}
    class Form_DataEvent : B1Form
    {
        public Form_DataEvent()
        {
            FormType = "2009032023";
        }             

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (Form.TypeEx == "2009032023")
            {
                if (pVal.ActionSuccess)
                {
                    AddLineData(Form, Form.DataSources.DBDataSources.Item(0).GetValue("Code", 0));
                }
            }
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_UPDATE, false)]
        public virtual void OnAfterFormDataUpdate(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (Form.TypeEx == "2009032023")
            {
                if (pVal.ActionSuccess)
                {
                    CheckStatusDocument(Form);
                    UpdateLineData(Form, Form.DataSources.DBDataSources.Item(0).GetValue("Code", 0));                  
                }
            }
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        public virtual void OnAfterFormDataLoad(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (Form.TypeEx == "2009032023")
            {
                if (pVal.ActionSuccess)
                {
                    LoadTabItesmContains1(Form);
                    CheckStatusDocument(Form);
                    LoadInvoiceTab(Form);

                    //Desactiva Tipo Envio y Tipo de Importación
                    Form.Items.Item("cmbTipEnv").Enabled = false;
                    Form.Items.Item("cmbTipImp").Enabled = false;
                    Form.Items.Item("cmbParcial").Enabled = false;
                    Form.Items.Item("txtParAnt").Enabled = false;

                    var record = (SAPbobsCOM.Recordset)B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    var qryValueBef = string.Format(GetEmbeddedResource("sapping.SQL.GetUserGenerateEM.sql"), B1Connections.DiCompany.UserSignature);
                    record.DoQuery(qryValueBef);

                    if (record.Fields.Item("Cant").Value > 0)
                    {
                        if (Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_FCTGEN", 0).ToString().Equals("Y"))
                            Form.Items.Item("btnFacPr").Enabled = false;
                    }
                    else
                    {
                        Form.Items.Item("btnFacPr").Enabled = false;
                    }
                    
                    var mtxGaInv = (SAPbouiCOM.Matrix)Form.Items.Item("mtxItms").Specific;
                        mtxGaInv.Columns.Item("Col_5").Editable = true;
                        mtxGaInv.Columns.Item("Col_6").ColumnSetting.SumType = BoColumnSumType.bst_Auto;
                        mtxGaInv.Columns.Item("Col_8").ColumnSetting.SumType = BoColumnSumType.bst_Auto;

                    Form.DataSources.DataTables.Item("DT_CAsg");
                }
            }
        }

        public void LoadInvoiceTab(B1Forms oForm)
        {
            var grid = (Grid)oForm.Items.Item("mtxInvEx").Specific;
                grid.Item.Enabled = false;
            var code = oForm.DataSources.DBDataSources.Item(0).GetValue("Code", 0);
            var queryInv = string.Format(GetEmbeddedResource("sapping.SQL.GetInvoicesTab.sql"), code);
            var dt = oForm.DataSources.DataTables.Item("DT_Inv");
                dt.ExecuteQuery(queryInv);

            if( dt.Rows.Count == 1 )
            {
                if( dt.GetValue("N° Interno", 0) == 0 )
                {
                    dt.ExecuteQuery(GetEmbeddedResource("sapping.SQL.NoDataFound.sql"));
                    return;
                }
            }

            grid.Columns.Item("N° Interno").Visible = false;
            ((EditTextColumn)grid.Columns.Item("N° Documento")).LinkedObjectType = "18";
            grid.AutoResizeColumns();
        }

        private void CheckStatusDocument(B1Forms oForm)
        {
            var state = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_STATUS", 0);
            switch (state)
            {
                case "AB":
                    SetStatusMode(FORM_STATUS.ABIERTO, Form);
                    break;
                case "CE":
                    SetStatusMode(FORM_STATUS.CERRADO, Form);
                    break;
                case "CA":
                    SetStatusMode(FORM_STATUS.CANCELADO, Form);
                    break;
            }
        }

        private void SetStatusMode(FORM_STATUS status, B1Forms oForm)
        {
            oForm.Items.Item("txtDummy").Click();
            var items = new string[] { "txtCode", "cmbTipEnv", "cmbTipImp", "Item_14", "txtAgAd", "Item_1", "txtPais", "Item_18", "txtForw", "txtTrans", "Item_13", "Item_15", "Item_22", "Item_17", "Item_34", "Item_35", "txtLocLl", "Item_19", "Item_40", "Item_16", "mtxItms", "mtxInvEx", "btnFacPr", "1", "Item_38", "Item_37", "cmbParcial", "txtParAnt" };
            var isEnabled = false;
            if (status == FORM_STATUS.ABIERTO)
                isEnabled = true;

            for( int i=0; i <items.Length; i++ )
                oForm.Items.Item(items[i]).Enabled = isEnabled;
        }

        private void AddLineData(B1Forms oForm, string code)
        {
            try
            {
                var cs = B1Connections.DiCompany.GetCompanyService();
                var gs = cs.GetGeneralService("MGS_CL_SEGIMP");
                var gdp = (GeneralDataParams)gs.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    gdp.SetProperty("Code", code);

                var gd = gs.GetByParams(gdp);
                var gdc = gd.Child("MGS_CL_SEGIAR");
                var cantidad = gdc.Count;
                var dtArtCont = oForm.DataSources.DataTables.Item("DT_ArtCont");
                var dtArtAsg = oForm.DataSources.DataTables.Item("DT_CAsg");
                var mtx = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxItms").Specific;
                for (int i = gdc.Count - 1; i >= 0; i--)
                {
                    gdc.Remove(i);
                }

                for (int i = 0; i < dtArtCont.Rows.Count; i++)
                {
                    var child = gdc.Add();
                    child.SetProperty("U_MGS_CL_NROINT", dtArtCont.GetValue("DocEntry", i).ToString());
                    child.SetProperty("U_MGS_CL_NRODOC", dtArtCont.GetValue("DocNum", i).ToString());
                    child.SetProperty("U_MGS_CL_CODART", dtArtCont.GetValue("ItemCode", i));
                    child.SetProperty("U_MGS_CL_DESCRI", dtArtCont.GetValue("Dscription", i));
                    child.SetProperty("U_MGS_CL_PESO", dtArtCont.GetValue("IWeight1", i));
                    child.SetProperty("U_MGS_CL_CANT", dtArtCont.GetValue("U_MGS_CL_CANT", i));
                    child.SetProperty("U_MGS_CL_QTYPES", dtArtCont.GetValue("QuantityWeight", i));
                    child.SetProperty("U_MGS_CL_PRICE", dtArtCont.GetValue("Price", i));
                    child.SetProperty("U_MGS_CL_TOTAL", dtArtCont.GetValue("Total", i));
                    child.SetProperty("U_MGS_CL_NROLNE", dtArtCont.GetValue("LineNum", i));
                    child.SetProperty("U_MGS_CL_UM", dtArtCont.GetValue("BuyUnitMsr", i));
                    //child.SetProperty("U_MGS_CL_CANPED", dtArtCont.GetValue("Quantity", i));'Validar si se debe crear el CU y relacionarlo
                    child.SetProperty("U_MGS_CL_ANCHO", dtArtCont.GetValue("U_MGS_CL_ANCHO", i));
                    child.SetProperty("U_MGS_CL_LARGO", dtArtCont.GetValue("U_MGS_CL_LARGO", i));
                    child.SetProperty("U_MGS_CL_CANBOB", dtArtCont.GetValue("U_MGS_CL_CANBOB", i));
                }


                var gdcont = gd.Child("MGS_CL_SEGCNT");
                for (int i = gdcont.Count - 1; i >= 0; i--)
                {
                    gdcont.Remove(i);
                }


                for (int i=0; i < dtArtAsg.Rows.Count; i++)
                {
                    var child = gdcont.Add();
                    child.SetProperty("U_MGS_CL_TIPCNT", dtArtAsg.GetValue("CTipCnt", i));
                    child.SetProperty("U_MGS_CL_ITMCDE", dtArtAsg.GetValue("CCod", i));
                    child.SetProperty("U_MGS_CL_CNTASG", dtArtAsg.GetValue("CCant", i));
                    child.SetProperty("U_MGS_CL_PESNET", dtArtAsg.GetValue("CPeso", i));
                }

                gs.Update(gd);
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage($"Excepption (AddLineData) => {ex.Message}");
            }
        }

        private void UpdateLineData(B1Forms oForm, string code)
        {
            var cs = B1Connections.DiCompany.GetCompanyService();
            var gs = cs.GetGeneralService("MGS_CL_SEGIMP");
            var gdp = (GeneralDataParams)gs.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                gdp.SetProperty("Code", code);

            var gd = gs.GetByParams(gdp);
            var gdcont = gd.Child("MGS_CL_SEGCNT");
            var dtArtAsg = oForm.DataSources.DataTables.Item("DT_CAsg");

            for (int i = gdcont.Count - 1; i >= 0; i--)
            {
                gdcont.Remove(i);
            }

            for (int i = 0; i < dtArtAsg.Rows.Count; i++)
            {
                var child = gdcont.Add();
                child.SetProperty("U_MGS_CL_TIPCNT", dtArtAsg.GetValue("CTipCnt", i));
                child.SetProperty("U_MGS_CL_ITMCDE", dtArtAsg.GetValue("CCod", i));
                child.SetProperty("U_MGS_CL_CNTASG", dtArtAsg.GetValue("CCant", i));
                child.SetProperty("U_MGS_CL_PESNET", dtArtAsg.GetValue("CPeso", i));
            }

            gs.Update(gd);
        }

        private void LoadTabItesmContains1(B1Forms oForm)
        {
            var ma = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxItms").Specific;
            ma.Clear();

            ma.Columns.Item("Col_0").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_NROINT");
            ma.Columns.Item("Col_1").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_NRODOC");
            ma.Columns.Item("Col_2").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_CODART");
            ma.Columns.Item("Col_3").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_DESCRI");
            ma.Columns.Item("Col_4").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_PESO");
            ma.Columns.Item("Col_5").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_CANT");
            ma.Columns.Item("Col_6").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_QTYPES");
            ma.Columns.Item("Col_7").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_PRICE");
            ma.Columns.Item("Col_8").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_TOTAL");
            ma.Columns.Item("Col_9").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_NROLNE");
            ma.Columns.Item("Col_10").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_FCTPRL");
            ma.Columns.Item("Col_13").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_ANCHO");
            ma.Columns.Item("Col_14").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_LARGO");
            ma.Columns.Item("Col_15").DataBind.SetBound(true, "@MGS_CL_SEGIAR", "U_MGS_CL_CANBOB");

            ma.LoadFromDataSource();

            for (int i = 0; i < ma.Columns.Count; i++)
            {
                if(ma.Columns.Item(i).UniqueID == "Col_5")
                    ma.Columns.Item(i).Editable = true;

                ma.Columns.Item(i).Editable = false;
            }

            ma.Columns.Item("Col_0").Visible = false;
            ma.Columns.Item("Col_9").Visible = false;
            ma.Columns.Item("Col_5").Editable = true;
            ma.Columns.Item("Col_13").Editable = true;
            ma.Columns.Item("Col_14").Editable = true;
            ma.Columns.Item("Col_15").Editable = true;
        }
    }
}
