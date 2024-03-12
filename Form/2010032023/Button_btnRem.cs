using B1Framework.B1Frame;
using Newtonsoft.Json;
using SAPbouiCOM;
using System.Linq;
using System.Xml.Linq;

namespace sapping.Form._2010032023
{
    class Button_btnRem : B1Item
    {
        public Button_btnRem()
        {
            FormType = "2010032023";
            ItemUID = "btnRem";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            var gridContainer = (Grid)Form.Items.Item("grdCont").Specific;
            var gridArtAsig = (Grid)Form.Items.Item("grdArtAsg").Specific;
            if (gridArtAsig.Rows.Count > 0)
                AddLineToImported(gridContainer, GetLineCode(gridArtAsig).ToString(), Form.DataSources.DataTables.Item("DT_ArtImp"), Form.DataSources.DataTables.Item("DT_ArtAsg"));
        }

        private void AddLineToImported(Grid gridContainer, string lineCode, DataTable dtImp, DataTable dtAsig)
        {
            if (lineCode.Equals("-1")) return;

            var docElementImp = XDocument.Parse(dtImp.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly));
            var docElementAsg = XDocument.Parse(dtAsig.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly));

            var nodeLine = docElementAsg.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CLine") && ((XElement)attr.NextNode).Value.Equals(lineCode));
            var nodeLineImp = docElementImp.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CLine") && ((XElement)attr.NextNode).Value.Equals(lineCode));
            var nodeToTake = nodeLine.First().Parent.Parent.Parent;
            var nodeToAdd = nodeToTake.ToString();
            var nodesColumnId = nodeToTake.Descendants("ColumnUid");
            var cantAsig = double.Parse(((XElement)nodesColumnId.Where(attr => attr.Value.ToString().Equals("CCantAsg")).First().NextNode).Value);

            nodeToAdd = nodeToTake.ToString();
            nodeToTake.Remove();

            #region Se verifica si el item existe o no en el grid de Articulos asignados
            if (nodeLineImp.Count() > 0)
            {
                var nodeAssgn = ((XElement)nodeLineImp.First().Parent.Parent.Parent.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantAsg")).First().NextNode);
                var nodeDisp = ((XElement)nodeLineImp.First().Parent.Parent.Parent.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantDsp")).First().NextNode);
                var nodePed = ((XElement)nodeLineImp.First().Parent.Parent.Parent.Descendants("ColumnUid").Where(attr => attr.Value.ToString().Equals("CCantPed")).First().NextNode);

                nodeAssgn.Value = (double.Parse(nodeAssgn.Value) + cantAsig).ToString();
                nodeDisp.Value = (cantAsig + double.Parse(nodeDisp.Value)).ToString();
            }
            else
            {
                var nodeMod = XDocument.Parse(nodeToAdd);
                var cantDsp = ((XElement)nodeMod.Descendants().Where(attr => attr.Value.ToString().Equals("CCantDsp")).First().Parent.LastNode);
                var cantPed = ((XElement)nodeMod.Descendants().Where(attr => attr.Value.ToString().Equals("CCantPed")).First().Parent.LastNode);
                cantDsp.Value = cantPed.Value;

                docElementImp.Descendants("Rows").First().Add(XElement.Parse(nodeMod.ToString()));
            }
            #endregion

            dtImp.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, docElementImp.ToString());
            dtAsig.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, docElementAsg.ToString());

            var valueToSerial = new { ArtImp = dtImp.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly), ArtAsig = dtAsig.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly) };
            gridContainer.DataTable.SetValue("CCAsig", GetContainerSelected(gridContainer), JsonConvert.SerializeObject(valueToSerial));
        }

        private int GetLineCode(Grid grdItm)
        {
            for (int i = 0; i < grdItm.Rows.Count; i++)
            {
                if (grdItm.Rows.IsSelected(i))
                    return grdItm.DataTable.GetValue("CLine", i);
            }

            return -1;
        }

        private int GetContainerSelected(Grid grdItm)
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
