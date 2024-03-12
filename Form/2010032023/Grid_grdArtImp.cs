using B1Framework.B1Frame;
using SAPbouiCOM;


namespace sapping.Form._2010032023
{
    class Grid_grdArtImp : B1Item
    {
        public Grid_grdArtImp()
        {
            FormType = "2010032023";
            ItemUID = "grdArtImp";
        }

        [B1Listener(BoEventTypes.et_VALIDATE, true)]
        public virtual bool OnBeforeGridValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            try
            {
                Form.Freeze(true);
                if (pVal.ColUID == "CCantAsg" && pVal.Row > -1)
                {
                    var datatable = Form.DataSources.DataTables.Item("DT_ArtImp");
                    var qtyDisp = datatable.GetValue("CCantDsp", pVal.Row);
                    var qtyAsig = datatable.GetValue("CCantAsg", pVal.Row);
                    if(qtyAsig > qtyDisp )
                    {
                        B1Connections.SboApp.SetStatusBarMessage("La cantidad no puede superar la cantidad disponible");
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        [B1Listener(BoEventTypes.et_VALIDATE, false)]
        public virtual void OnAfterGridValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            try
            {
                Form.Freeze(true);
                if (pVal.ColUID == "CCantAsg" && pVal.Row > -1)
                {
                    var datatable = Form.DataSources.DataTables.Item("DT_ArtImp");
                    var pesoItem = datatable.GetValue("CPes", pVal.Row);
                    var qtyAsig = datatable.GetValue("CCantAsg", pVal.Row);
                    datatable.SetValue("CPesNeto", pVal.Row, (pesoItem * qtyAsig));
                }
            }
            finally
            {
                Form.Freeze(false);
            }
        }
    }
}
