using B1Framework.B1Frame;
using SAPbobsCOM;
using SAPbouiCOM;
using System;

namespace sapping.Form._2010032023
{
    class Form_Events : B1Form
    {
        public Form_Events()
        {
            FormType = "2010032023";
        }

        [B1Listener(BoEventTypes.et_FORM_RESIZE, false)]
        public virtual void OnAfterFormResize(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var grdCont = (Grid)Form.Items.Item("grdCont").Specific;
            var grdArtImp = (Grid)Form.Items.Item("grdArtImp").Specific;
            var grdArtAsg = (Grid)Form.Items.Item("grdArtAsg").Specific;

            grdCont.Item.Height = 140;
            grdArtImp.Item.Width = 300;
            grdArtAsg.Item.Width = 300;
            Form.Items.Item("Item_0").Height = 170;
            Form.Items.Item("Item_1").Height = 150;
            

            
             grdArtImp.Item.Width = (Form.Width/2) - 150;
            Form.Items.Item("~GRP#1").Width = (Form.Width / 2) - 130;
            Form.Items.Item("Item_8").Width = (Form.Width / 2) - 130;
            Form.Items.Item("~GRP#1").Height = grdArtImp.Item.Height + 20;
            Form.Items.Item("Item_8").Height = grdArtImp.Item.Height + 20;

            //grdArtAsg
            grdArtAsg.Item.Width = (Form.Width/2) - 150;
            grdArtAsg.Item.Left = (Form.Width / 2) + 100;
            Form.Items.Item("~GRP#2").Left = (Form.Width / 2) + 80;
            Form.Items.Item("~GRP#2").Width = (Form.Width / 2) - 120;
            Form.Items.Item("Item_5").Left = (Form.Width / 2) + 80;
            Form.Items.Item("Item_5").Width = (Form.Width / 2) - 120;

            Form.Items.Item("~GRP#2").Height = grdArtAsg.Item.Height + 20;
            Form.Items.Item("Item_5").Height = grdArtAsg.Item.Height + 20;

            Form.Items.Item("btnAdd").Left = (Form.Width / 2) - 30;
            Form.Items.Item("btnAdd").Top = (Form.Height / 2);

            Form.Items.Item("btnRem").Left = Form.Items.Item("btnAdd").Left;
            Form.Items.Item("btnRem").Top = Form.Items.Item("btnAdd").Top + 30;

            grdArtImp.AutoResizeColumns();
            grdArtAsg.AutoResizeColumns();
        }

       [B1Listener(BoEventTypes.et_FORM_CLOSE, false)]
        public virtual void OnAfterFormClose(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if( !string.IsNullOrEmpty(Form.DataSources.UserDataSources.Item("U_FrmFath").Value) )
            {
                B1Connections.SboApp.Forms.Item(Form.DataSources.UserDataSources.Item("U_FrmFath").Value).DataSources.UserDataSources.Item("U_Child").Value = string.Empty;
            }
        }

        //[B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        //public virtual void OnAfterFormDataLoad(BusinessObjectInfo pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);
        //    if (Form.TypeEx == "2010032023")
        //    {
        //        if (pVal.ActionSuccess)
        //        {
        //            var grdCont = (SAPbouiCOM.Grid)Form.Items.Item("grdCont").Specific;
        //            grdCont.Columns.Item("CNroPal").Visible = true;
        //        }
        //    }
        //}
    }
}
