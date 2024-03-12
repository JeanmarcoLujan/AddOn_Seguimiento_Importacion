using B1Framework.B1Frame;
using SAPbouiCOM;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace sapping.Form._2010032023
{
    class Button_btnAdd : B1Item
    {
        public Button_btnAdd()
        {
            FormType = "2010032023";
            ItemUID = "btnAdd";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            var gridContainer = (Grid)Form.Items.Item("grdCont").Specific;
            var gridArtImpp = (Grid)Form.Items.Item("grdArtImp").Specific;
            if (gridArtImpp.Rows.Count > 0)
                AddLineToAssigned(Form, gridContainer, GetLineCode(gridArtImpp).ToString(), Form.DataSources.DataTables.Item("DT_ArtImp"), Form.DataSources.DataTables.Item("DT_ArtAsg"));
        }

        public static void AddLineToAssigned(B1Forms oForm, Grid gridContainer, string lineCode, DataTable dtImp, DataTable dtAsig)
        {
            if (lineCode.Equals("-1")) return;

            var docElementImp = XDocument.Parse(dtImp.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly));
            var docElementAsg = XDocument.Parse(dtAsig.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly));

            var nodeLine = docElementImp.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CLine") && ((XElement)attr.NextNode).Value.Equals(lineCode));
            var nodeLineAsg = docElementAsg.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CLine") && ((XElement)attr.NextNode).Value.Equals(lineCode));
            var nodeToTake = nodeLine.First().Parent.Parent.Parent;
            var nodesColumnId = nodeToTake.Descendants("ColumnUid");
            var cantAsig = double.Parse(((XElement)nodesColumnId.Where(attr => attr.Value.ToString().Equals("CCantAsg")).First().NextNode).Value);
            var cantDisp = double.Parse(((XElement)nodesColumnId.Where(attr => attr.Value.ToString().Equals("CCantDsp")).First().NextNode).Value);
            var cantPed = double.Parse(((XElement)nodesColumnId.Where(attr => attr.Value.ToString().Equals("CCantPed")).First().NextNode).Value);
            var nodeToAdd = string.Empty;

            //cambio temporal de cantidad asignada Luis Vicente
            var itemCode = Convert.ToString(((XElement)nodesColumnId.Where(attr => attr.Value.ToString().Equals("CCod")).First().NextNode).Value);
            var record = (SAPbobsCOM.Recordset)B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            var qryContAsig = "UPDATE \"@MGS_CL_SEGIAR\" SET \"U_MGS_CL_CASIG\" = IFNULL(\"U_MGS_CL_CASIG\",0) + " + Convert.ToString(cantAsig) + " WHERE \"U_MGS_CL_CODART\" = '" + itemCode + "' AND \"Code\" = '" + "DTD-2023-AE0001" + "';";
            record.DoQuery(qryContAsig);
            //*****************************************************

            #region Se verifica si se esta haciendo un traspaso parcial o completo al grid de Articulos asignados
            if (cantAsig == cantDisp)
            {
                nodeToAdd = nodeToTake.ToString();
                nodeToTake.Remove();
            }
            else if(cantAsig > cantDisp)
            {
                return;
            }
            else
            {
                var nodeAsig = ((XElement)nodeToTake.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantAsg")).First().NextNode);
                var nodeDisp = ((XElement)nodeToTake.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantDsp")).First().NextNode);

                nodeAsig.Value = (cantAsig).ToString();
                nodeDisp.Value = (cantDisp - cantAsig).ToString();

                var nodeAdded = ((XElement)XDocument.Parse(nodeToTake.ToString()).Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantAsg")).First().NextNode);
                nodeAdded.Value = cantAsig.ToString();

                nodeToAdd = nodeAdded.Parent.Parent.Parent.ToString();
            }
            #endregion

            #region Se verifica si el item existe o no en el grid de Articulos asignados
            if (nodeLineAsg.Count() > 0)
            {
                var nodeAssgn = ((XElement)nodeLineAsg.First().Parent.Parent.Parent.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantAsg")).First().NextNode);
                var nodeDisp = ((XElement)nodeLineAsg.First().Parent.Parent.Parent.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantDsp")).First().NextNode);
                var nodePed = ((XElement)nodeLineAsg.First().Parent.Parent.Parent.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantPed")).First().NextNode);
                
                nodeAssgn.Value = (double.Parse(nodeAssgn.Value) + cantAsig).ToString();
                nodeDisp.Value = (double.Parse(nodePed.Value) - double.Parse(nodeAssgn.Value)).ToString();
            }
            else
            {
                docElementAsg.Descendants("Rows").First().Add(XElement.Parse(nodeToAdd));
            }
            #endregion

            dtImp.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, docElementImp.ToString());
            dtAsig.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, docElementAsg.ToString());

            var valueToSerial = new { ArtImp = dtImp.SerializeAsXML(BoDataTableXmlSelect.dxs_All), ArtAsig = dtAsig.SerializeAsXML(BoDataTableXmlSelect.dxs_All) };
            gridContainer.DataTable.SetValue("CCAsig", GetContainerSelected(gridContainer), JsonConvert.SerializeObject(valueToSerial));

            var grdArtAsig = (Grid)oForm.Items.Item("grdArtAsg").Specific;
            grdArtAsig.Columns.Item("CCantAsg").Visible = true;
        }

        public static int GetLineCode(Grid grdItm)
        {
 
            for (int i = 0; i < grdItm.Rows.Count; i++)
            {
                if(grdItm.Rows.IsSelected(i) )
                    return grdItm.DataTable.GetValue("CLine", i);
            }

            return -1;
        }

        public static int GetContainerSelected(Grid grdItm)
        {

            for (int i = 0; i < grdItm.Rows.Count; i++)
            {
                if (grdItm.Rows.IsSelected(i))
                    return i;
            }

            return -1;
        }
    }
}
