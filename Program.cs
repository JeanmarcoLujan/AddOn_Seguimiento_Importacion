using System.Reflection;
using B1Framework.B1Frame;
using sapping.Services;

namespace sapping
{
    class Program : B1AddOn
    {
        public void LoadProgram(Assembly asm, string connStr, int appid)
        {
            //loadConnection(connStr, appid);
            loadConnection(connStr);
            loadResources(connStr, appid, asm, "sapping.Menu.xml.addMenus.xml");

            B1Connections.SboApp.SetStatusBarMessage("Addon Seguimiento de Importacion Inicializado", SAPbouiCOM.BoMessageTime.bmt_Medium, false);
        }

    }
}
