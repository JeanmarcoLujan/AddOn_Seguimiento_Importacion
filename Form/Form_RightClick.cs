using B1Framework.B1Frame;
using SAPbouiCOM;

namespace sapping.Form
{
    class Form_RightClick : B1Event
    {
        [B1Listener(BoEventTypes.et_RIGHT_CLICK, true, new string[] { "2010032023", "2009032023" })]
        public bool OnBeforeRightClick(ContextMenuInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (Form.TypeEx == "2010032023")
            {
                if (pVal.ItemUID == "grdCont")
                {
                    AddSubMenu("AddRow", "Agregar Linea", 0);
                    AddSubMenu("RemRow", "Eliminar Linea", 1);
                }
            }
            else if( Form.TypeEx == "2009032023")
            {
                var stateDoc = Form.DataSources.DBDataSources.Item(0).GetValue("U_MGS_CL_STATUS", 0);

                if (pVal.ItemUID == string.Empty)
                {
                    if (stateDoc.Equals("AB") && Form.Mode == BoFormMode.fm_ADD_MODE)
                    {
                        AddSubMenu("CpyPed", "Copiar de Pedido", 0);
                    }
                }
                else if(pVal.ItemUID == "mtxAnex")
                {
                    AddSubMenu("AddRow", "Agregar Linea", 0);
                    AddSubMenu("RemRow", "Eliminar Linea", 1);
                }
                else if (pVal.ItemUID == "mtxItms")
                {
                    if (stateDoc.Equals("AB"))
                    {
                        AddSubMenu("RemRow", "Eliminar Linea", 1);
                    }
                }
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_RIGHT_CLICK, false)]
        public virtual void OnAfterRightClick(ContextMenuInfo pVal)
        {
            DeleteSubMenu("AddRow");
            DeleteSubMenu("RemRow");
            DeleteSubMenu("CpyPed");
        }
    }
}
