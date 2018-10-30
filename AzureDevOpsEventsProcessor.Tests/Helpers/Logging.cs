using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace AzureDevOpsEventsProcessor.Tests.Helpers
{
    public class Logging
    {
        public static NLog.Targets.MemoryTarget CreateMemoryTargetLogger(LogLevel level)
        {
            var memLogger = new NLog.Targets.MemoryTarget();
            memLogger.Layout = "${level:uppercase=true} | ${logger} | ${message}";

            var rule = new NLog.Config.LoggingRule("*", level, memLogger);
            NLog.LogManager.Configuration.LoggingRules.Add(rule);
            NLog.LogManager.Configuration.Reload();
            return memLogger;
        }

        public static StringWriter RedirectConsoleOut()
        {
            var consoleOut = new StringWriter();
            Console.SetOut(consoleOut);
            return consoleOut;
        }
    }
}
