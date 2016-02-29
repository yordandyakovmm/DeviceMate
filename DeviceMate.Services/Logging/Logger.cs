using System;
using N = NLog;
using DeviceMate.Core.Extensions;

namespace DeviceMate.Services.Logging
{
    public static class Logger
    {
        public static void LogException(Exception ex)
        {
            ex = ex.RecursiveGetInnerException();

            N.Logger nlog = N.LogManager.GetLogger("ServiceExeptionHandler");
            nlog.LogNlogException(ex);
        }

        private static void LogNlogException(this N.Logger logger, Exception exception)
        {
            logger.Fatal(
                string.Format(
                    "Exception[{1}]=\"{2}\"{0}===== Inner Exception ====={0}{3}{0}===== Stack Trace ====={0}{4}\n\n",
                    Environment.NewLine,
                    exception.GetType().Name,
                    exception.Message,
                    exception.InnerException,
                    exception.StackTrace));
        }
    }
}
