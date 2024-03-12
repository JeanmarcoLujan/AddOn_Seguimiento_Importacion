using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2010032023
{
    class Button_btnAsig : B1Item
    {
        public Button_btnAsig()
        {
            FormType = "2010032023";
            ItemUID = "btnAsig";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            var gridContainer = (Grid)Form.Items.Item("grdCont").Specific;
            var gridArtImp = (Grid)Form.Items.Item("grdArtImp").Specific;

            if (gridArtImp.Rows.Count > 0) 
            {
                var cantRow = gridArtImp.Rows.Count;
                for (int i = cantRow - 1; i >= 0 ; i--)
                   Button_btnAdd.AddLineToAssigned(Form, gridContainer, (i+1).ToString(), Form.DataSources.DataTables.Item("DT_ArtImp"), Form.DataSources.DataTables.Item("DT_ArtAsg"));
            }
        }

    }
}
