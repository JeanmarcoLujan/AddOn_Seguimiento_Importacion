using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2002032023
{
    class Grid_grdResPed : B1Item
    {
        public Grid_grdResPed()
        {
            FormType = "2002032023";
            ItemUID = "grdResPed";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if( pVal.ColUID == "N° Pedido")
            {
                var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;
                var purchaseNumber = grid.DataTable.GetValue("N° Pedido interno", pVal.Row);
                B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_PurchaseOrder, "", purchaseNumber);
                return false;
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if( pVal.ColUID == "Opt" )
            {
                if (pVal.Row > -1)
                {
                    var dt = ((Grid)Form.Items.Item(pVal.ItemUID).Specific).DataTable;
                    var opt = dt.GetValue(pVal.ColUID, pVal.Row).ToString();
                    dt.SetValue(pVal.ColUID, pVal.Row, opt.Equals("Y") ? "N" : "Y");
                }
            }
        }
    }
}
