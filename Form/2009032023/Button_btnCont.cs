using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace sapping.Form._2009032023
{
    class Button_btnCont : B1Item
    {
        public Button_btnCont()
        {
            FormType = "2009032023";
            ItemUID = "btnCont";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            OpenFormCont(Form);
        }

        public void OpenFormCont(B1Forms oForm)
        {
            var containerInfo = oForm.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_CONTAI", 0);
            var statusDoc = oForm.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_STATUS", 0);
            var uid = Guid.NewGuid().ToString().Substring(0, 6);
            var xml = string.Format(B1Util.GetEmbeddedResource("sapping.Form._2010032023.srf.Frm_contAsig.srf", GetType().Assembly), uid);
            B1Connections.SboApp.LoadBatchActions(ref xml);

            var oFormCont = new B1Forms(uid);
                oFormCont.DataSources.UserDataSources.Item("U_FrmFath").Value = oForm.UniqueID;
                oFormCont.Top = oForm.Top + 50;
                oFormCont.Left = oForm.Left + 70;

            SetValuesContainerDataTable(Form, oFormCont);

            var grdCont = (Grid)oFormCont.Items.Item("grdCont").Specific;
            if ( !string.IsNullOrEmpty(containerInfo) )
            {
                grdCont.DataTable.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, containerInfo);
            }
            grdCont.Columns.Item("CLine").Visible = false;
            grdCont.Columns.Item("CNroPal").Visible = true;
            //grdCont.Columns.Item("CCAsig").Visible = false;

            var grdArtCont = (Grid)oFormCont.Items.Item("grdCont").Specific;
                grdArtCont.Columns.Item("CPesBr").Editable = true;

            var grdArtSeg = (Grid)oFormCont.Items.Item("grdArtImp").Specific;
                grdArtSeg.Columns.Item("CLine").Visible = false;
                grdArtSeg.Columns.Item("CPes").Visible = false;

            var grdArtAsig = (Grid)oFormCont.Items.Item("grdArtAsg").Specific;
                grdArtAsig.Columns.Item("CLine").Visible = false;
                grdArtAsig.Columns.Item("CPes").Visible = false;

            var colSum = (EditTextColumn)grdArtAsig.Columns.Item("CPesNeto");
                colSum.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

            if(statusDoc.Equals("CA") || statusDoc.Equals("CE") )
            {
                grdArtAsig.Item.Enabled = false;
                grdCont.Item.Enabled = false;
                grdCont.Item.Enabled = false;
                oFormCont.Items.Item("btnAdd").Enabled = false;
                oFormCont.Items.Item("btnRem").Enabled = false;
                oFormCont.Items.Item("btnAct").Enabled = false;
            }

            oFormCont.Visible = true;

            Form.DataSources.UserDataSources.Item("U_Child").Value = oFormCont.UniqueID;
        }

        private void SetValuesContainerDataTable(B1Forms frmSeg, B1Forms frmCntainer)
        {
            var dtItmsCont = frmCntainer.DataSources.DataTables.Item("DT_ItmsUse");
            var mtx = (SAPbouiCOM.Matrix)frmSeg.Items.Item("mtxItms").Specific;
            for( int i=1; i<=mtx.RowCount; i++ )
            {
                dtItmsCont.Rows.Add();
                dtItmsCont.SetValue("CCodItm", dtItmsCont.Rows.Count-1, ((EditText)mtx.GetCellSpecific("Col_2", i)).Value );
                dtItmsCont.SetValue("CDesc", dtItmsCont.Rows.Count-1, ((EditText)mtx.GetCellSpecific("Col_3", i)).Value );
                dtItmsCont.SetValue("CQuantity", dtItmsCont.Rows.Count-1, ((EditText)mtx.GetCellSpecific("Col_5", i)).Value );
                dtItmsCont.SetValue("CPesInv", dtItmsCont.Rows.Count-1, ((EditText)mtx.GetCellSpecific("Col_4", i)).Value );
            }
        }
    }
}
