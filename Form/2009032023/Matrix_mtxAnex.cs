using B1Framework.B1Frame;
using SAPbouiCOM;
using sapping.Util;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace sapping.Form._2009032023
{
    class Matrix_mtxAnex : B1Item
    {
        public Matrix_mtxAnex()
        {
            FormType = "2009032023";
            ItemUID = "mtxAnex";
        }

        [B1Listener(BoEventTypes.et_DOUBLE_CLICK, false)]
        public virtual void OnAfterValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            try
            {
                Form.Freeze(true);
                if (pVal.ColUID == "#")
                {
                    var mtx = ((SAPbouiCOM.Matrix)Form.Items.Item("mtxAnex").Specific);
                    var ruta = OpenFileDialogForProcess();
                    var lineSelected = mtx.GetNextSelectedRow();
                    mtx.SetCellWithoutValidation(lineSelected, "Col_0", ruta);
                    mtx.SetCellWithoutValidation(lineSelected, "Col_1", Path.GetFileName(ruta));
                    mtx.SetCellWithoutValidation(lineSelected, "Col_2", DateTime.Now.ToString("yyyyMMdd"));
                    mtx.AutoResizeColumns();

                    if (Form.Mode == BoFormMode.fm_OK_MODE)
                        Form.Mode = BoFormMode.fm_UPDATE_MODE;
                }
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        public static string OpenFileDialogForProcess()
        {
            try
            {
                var oGetFileName = new GetFileNameClass
                { };

                Thread FileThread = new Thread(new ThreadStart(oGetFileName.GetFileName));
                FileThread.SetApartmentState(ApartmentState.STA);
                FileThread.Priority = ThreadPriority.Highest;
                FileThread.Start();

                while (!FileThread.IsAlive) ;
                Thread.Sleep(1);
                FileThread.Join();

                return oGetFileName.Path;
            }
            catch (COMException comEx)
            {
            }
            catch (Exception er)
            {
            }

            return "";
        }
    }
}
