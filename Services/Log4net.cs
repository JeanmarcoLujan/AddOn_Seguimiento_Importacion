using B1Framework.B1Frame;
using B1Framework.RecordSet;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace sapping.Services
{
    public class Log4net
    {
        private readonly ILog Logggin = LogManager.GetLogger(typeof(Log4net));

        public Log4net()
        {
        }

        public void Info(string message)
        {
            try
            {
                Logggin.Info(message);
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.MessageBox("Error Info " + ex.Message);
            }
        }

        public void Error(string message)
        {
            Logggin.Error(message);
        }

    }
}
