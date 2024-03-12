using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form._2009032023
{
    class Matrix_mtxItms : B1Item
    {
        public Matrix_mtxItms()
        {
            FormType = "2009032023";
            ItemUID = "mtxItms";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if ( pVal.ItemUID == "mtxItms")
            {
                if( pVal.ColUID == "Col_1" )
                {
                    var mtx = ((SAPbouiCOM.Matrix)Form.Items.Item("mtxItms").Specific);
                    var docEntry = ((EditText)mtx.GetCellSpecific("Col_0", pVal.Row)).Value;
                    B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_PurchaseOrder, "", docEntry);
                    return false;
                }
            }

            return true;
        }

       [B1Listener(BoEventTypes.et_VALIDATE, false)]
        public virtual void OnAfterValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            try
            {
                Form.Freeze(true);
                if (pVal.ColUID == "Col_5" && pVal.ItemChanged)
                {
                    var mtx = ((SAPbouiCOM.Matrix)Form.Items.Item("mtxItms").Specific);
                    var qty = double.Parse(((EditText)mtx.GetCellSpecific(pVal.ColUID, pVal.Row)).Value.Replace(",", "."));
                    var weight = double.Parse(((EditText)mtx.GetCellSpecific("Col_4", pVal.Row)).Value.Replace(", ", "."));
                    ((EditText)mtx.GetCellSpecific("Col_6", pVal.Row)).Value = (qty * weight).ToString().Replace(",", ".");                    
                    mtx.FlushToDataSource();                    
                    //mtx.LoadFromDataSource();
                }
            }
            finally
            {
                Form.Freeze(false);
                Form.Update();
            }
        }

    }
}
