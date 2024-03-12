using System;
using System.Diagnostics;

namespace sapping
{
    class Init
    {
        private static string cadena = "";
        public static int id = -1;

        [STAThread]
        public static void Main(string[] args)
        {
            if (Environment.GetCommandLineArgs().Length > 1)
                cadena = Environment.GetCommandLineArgs().GetValue(1).ToString();

            var app = new Init();
        }

        public static void loadRef(string connstr, int appId)
        {
            cadena = connstr;
            id = appId;
        }

        public Init()
        {
            Run();
        }

        private int getAppIdRunning()
        {
            return B1Framework.Services.ProcessExtensions.Parent(Process.GetCurrentProcess()).Id;
        }

        [STAThread]
        // ReSharper disable once ObjectCreationAsStatement
        public void Run()
        {
            var isQA = false;
            var cadCon = isQA ? "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056" : Environment.GetCommandLineArgs().GetValue(1).ToString();
            var appId = isQA ? - 1 : B1Framework.Services.ProcessExtensions.Parent(Process.GetCurrentProcess()).Id;

            var obj = new Program();
                obj.LoadProgram(GetType().Assembly, cadCon, appId);

            System.Windows.Forms.Application.Run();
        }
    }
}
