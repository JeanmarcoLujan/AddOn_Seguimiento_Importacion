using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2009032023
{
    class Grid_mtxInvEx : B1Item
    {
        public Grid_mtxInvEx()
        {
            FormType = "2009032023";
            ItemUID = "mtxInvEx";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if (pVal.ItemUID == ItemUID)
            {
                if (pVal.ColUID == "N° Documento")
                {
                    var grid = (Grid)Form.Items.Item("mtxInvEx").Specific;
                    var docEntry = grid.DataTable.GetValue("N° Interno", pVal.Row);
                    B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_PurchaseInvoice, "", docEntry);
                    return false;
                }
            }

            return true;
        }
    }
}
