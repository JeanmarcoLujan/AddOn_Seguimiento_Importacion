using B1Framework.B1Frame;
using SAPbobsCOM;
using SAPbouiCOM;

namespace sapping.Form._2009032023
{
    class Button_btnFacPr : B1Item
    {
        public Button_btnFacPr()
        {
            FormType = "2009032023";
            ItemUID = "btnFacPr";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            var estado = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_STAIMP", 0);

            if(estado == "06")
            {
                if (B1Connections.SboApp.MessageBox("Desea crear el documento preeliminar de entrada de mercancías de compra?", 1, "Si", "No") == 1)
                {
                    B1Connections.SboApp.SetStatusBarMessage("Comenzando proceso, por favor espere....", BoMessageTime.bmt_Medium, false);
                    var code = Form.DataSources.DBDataSources.Item(0).GetValue("Code", 0);
                    var recordDoc = B1Framework.RecordSet.Record.Instance.Query(string.Format(GetEmbeddedResource("sapping.SQL.GetVendorDocumentCreate.sql"), code)).Execute().All();
                    var record = B1Framework.RecordSet.Record.Instance.Query(string.Format(GetEmbeddedResource("sapping.SQL.GetNumSegGroup.sql"), code)).Execute().All();

                    for (int h = 0; h < recordDoc.Length; h++)
                    {
                        var invoice = (Documents)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                        invoice.DocObjectCode = BoObjectTypes.oPurchaseDeliveryNotes;
                        //invoice.ReserveInvoice = BoYesNoEnum.tYES;
                        invoice.CardCode = recordDoc[h]["CardCode"];
                        invoice.NumAtCard = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_PACLIS", 0);
                        invoice.UserFields.Fields.Item("U_MGS_CL_SEGIMP").Value = code;
                        invoice.UserFields.Fields.Item("U_MGS_CL_NROBL").Value = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_NROBL", 0);

                        for (int i = 0; i < record.Length; i++)
                        {
                            if (recordDoc[h]["CardCode"] == record[i]["CardCode"])
                            {
                                var recordDetails = B1Framework.RecordSet.Record.Instance.Query(string.Format(GetEmbeddedResource("sapping.SQL.GetDetailsDocumentSegImp.sql"), code, record[i]["U_MGS_CL_NROINT"])).Execute().All();
                                for (int j = 0; j < recordDetails.Length; j++)
                                {
                                    invoice.Lines.BaseEntry = int.Parse(recordDetails[j]["DocEntry"]);
                                    invoice.Lines.BaseLine = int.Parse(recordDetails[j]["LineNum"]);
                                    invoice.Lines.BaseType = int.Parse(recordDetails[j]["ObjType"]);
                                    invoice.Lines.ItemCode = recordDetails[j]["U_MGS_CL_CODART"];
                                    invoice.Lines.Quantity = double.Parse(recordDetails[j]["U_MGS_CL_CANT"]);
                                    invoice.Lines.Add();
                                }
                            }
                        }

                        var resp = invoice.Add();
                        var msg = B1Connections.DiCompany.GetLastErrorDescription();

                        if (resp == 0)
                        {
                            UpdateSegImp(code, B1Connections.DiCompany.GetNewObjectKey());
                            Form.Items.Item(pVal.ItemUID).Enabled = false;
                            B1Connections.SboApp.SetStatusBarMessage("Documento creado exitosamente", BoMessageTime.bmt_Medium, false);
                        }
                        else
                        {
                            B1Connections.SboApp.MessageBox("Error al crear el documento = " + msg);
                        }
                    }

                    B1Connections.SboApp.Menus.Item("1304").Activate();
                    B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado", BoMessageTime.bmt_Medium, false);
                }
            }
            else
            {
                B1Connections.SboApp.MessageBox("Solo es posible generar el documento, si el estado es 06 - RECIBIDO EN EL ALMACÉN.");
            }

            
        }

        private void UpdateSegImp(string codeSeg, string nroFactPre)
        {
            try
            {
                var cs = B1Connections.DiCompany.GetCompanyService();
                var gs = cs.GetGeneralService("MGS_CL_SEGIMP");
                var gdp = (GeneralDataParams)gs.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                gdp.SetProperty("Code", codeSeg);

                var gd = gs.GetByParams(gdp);
                    gd.SetProperty("U_MGS_CL_FCTGEN", "Y");
                var gdc = gd.Child("MGS_CL_SEGIAR");

                for (int i = 0; i < gdc.Count; i++)
                {
                    var child = gdc.Item(i);
                    child.SetProperty("U_MGS_CL_FCTPRL", nroFactPre);
                }

                gs.Update(gd);
            }
            catch
            {

            }
        }
    }
}
