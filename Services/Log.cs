using System;

namespace sapping.Services
{
    class Log
    {
        private static Log4net log4net = new Log4net();

        public static void Info(String value)
        {
            log4net.Info(value);
        }

        public static void Error(String value)
        {
            log4net.Error(value);
        }

    }
}
