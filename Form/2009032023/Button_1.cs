using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace sapping.Form._2009032023
{
    class Button_1 : B1Item
    {
        public Button_1()
        {
            FormType = "2009032023";
            ItemUID = "1";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if (Form.Mode != BoFormMode.fm_FIND_MODE)
            {
                var tipEnv = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_TIPENV", 0);
                var tipImp = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_TIPIMP", 0);
                var nroBL = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_NROBL", 0);
                var code = Form.DataSources.DBDataSources.Item(0).GetValue("Code", 0);
                var cmbo = (ComboBox)Form.Items.Item("Item_22").Specific;
                var SADD = (ComboBox)Form.Items.Item("Item_37").Specific;
                
                var mtxItem = (SAPbouiCOM.Matrix)Form.Items.Item("mtxItms").Specific;
                var mtxAnex = (SAPbouiCOM.Matrix)Form.Items.Item("mtxAnex").Specific;

                if (tipEnv.Equals(string.Empty))
                {
                    B1Connections.SboApp.SetStatusBarMessage("No puede dejar el tipo de envio sin seleccionar");
                    return false;
                }
                else if (code.Equals(string.Empty))
                {
                    B1Connections.SboApp.SetStatusBarMessage("No puede dejar el campo codigo vacio");
                    return false;
                }
                else if (tipImp.Equals(string.Empty))
                {
                    B1Connections.SboApp.SetStatusBarMessage("No puede dejar el tipo de importacion sin seleccionar");
                    return false;
                }
                else if (nroBL.Equals(string.Empty) && tipImp.Equals("ORD"))
                {
                    B1Connections.SboApp.SetStatusBarMessage("Debe ingresar el NRO. BL / Guia si seleccionó tipo de importacion: Ordinario");
                    return false;
                }else if(SADD.Selected == null)
                {
                    B1Connections.SboApp.SetStatusBarMessage("Debe seleccionar el status de la importación");
                    return false;
                }
                else if (mtxItem.RowCount == 0)
                {
                    B1Connections.SboApp.SetStatusBarMessage("Debe seleccionar al menos un pedido");
                    return false;
                }
                else if (mtxAnex.RowCount > 0 && mtxItem.RowCount > 0)
                {
                    int cc = 0;
                    for (int i = 1; i <= mtxAnex.RowCount; i++)
                    {
                            for (int k = 1; k <= mtxItem.RowCount; k++)
                            {
                                if (mtxAnex.Columns.Item("Col_3").Cells.Item(i).Specific.Value != "" && (mtxAnex.Columns.Item("Col_3").Cells.Item(i).Specific.Value != mtxItem.Columns.Item("Col_0").Cells.Item(k).Specific.Value))
                                {
                                    cc++;
                                }
                            }
                    }

                    if (cc > 0)
                    {
                        B1Connections.SboApp.SetStatusBarMessage("Unos de los pedidos relacionado de la pestaña Anexos no coincide con el de seguimiento de importación");
                        return false;
                    }
                }

                var stateSelected = cmbo.Selected.Value;
                var record = (SAPbobsCOM.Recordset)B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                var recordCount = (SAPbobsCOM.Recordset)B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                var qryValueBef = string.Format(GetEmbeddedResource("sapping.SQL.GetBeforeStateSegImp.sql"), Form.DataSources.DBDataSources.Item(0).GetValue("Code", 0));
                record.DoQuery(qryValueBef);

                if (record.Fields.Item("Status").Value != stateSelected)
                {
                    if (stateSelected == "CE" || stateSelected == "CA")
                    {
                        if (stateSelected == "CE")
                        {
                            var qryCntner= string.Format(GetEmbeddedResource("sapping.SQL.GetCantContainer.sql"), Form.DataSources.DBDataSources.Item(0).GetValue("Code", 0));
                            recordCount.DoQuery(qryCntner);

                            if( recordCount.Fields.Item("Cant").Value == 0 )
                            {
                                B1Connections.SboApp.SetStatusBarMessage("Tiene que asignar un contenedor antes de cerrar el documento");
                                return false;
                            }
                        }

                        if (B1Connections.SboApp.MessageBox($"El estado del documento se modificará a \"{cmbo.Selected.Description}\" y no se podrá deshacer, ¿ Desea continuar ?", 1, "Si", "No") != 1)
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
