//------------------------------------------------------------------------------------------------- 
// <copyright file="LoggingHelper.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;

namespace AzureDevOpsEventsProcessor.Helpers
{
    /// <summary>
    /// Helper methods to aid logging
    /// </summary>
    public static class LoggingHelper
    {
        /// <summary>
        /// Dumps the event details as an XML for debug using a GUID for the file name
        /// </summary>
        /// <param name="eventdata">The details of the event</param>
        /// <param name="path">Base folder to dump file to</param>
        public static void DumpEventToDisk(string eventdata, string path)
        {
            File.WriteAllText(Path.Combine(path, string.Format("{0}.xml", Guid.NewGuid().ToString())), eventdata);
        }

        /// <summary>
        /// Dumps the event details as an Json for debug using a GUID for the file name
        /// </summary>
        /// <param name="eventdata">The details of the event</param>
        /// <param name="path">Base folder to dump file to</param>
        public static void DumpEventToDisk(JObject eventdata, string path)
        {
            File.WriteAllText(Path.Combine(path, string.Format("{0}.json", Guid.NewGuid().ToString())), eventdata.ToString());
        }

             /// <summary>
        /// Outputs error exception
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="ex">The exception to dump</param>
        public static void DumpException(Logger logger,Exception ex)
        {
            logger.Error(string.Format("AzureDevOpsEventsProcessor.DslScriptService: {0}", ex.Message));
            logger.Error(string.Format("AzureDevOpsEventsProcessor.DslScriptService: {0}", ex.StackTrace));
            if (ex.InnerException != null)
            {
                DumpException(logger, ex.InnerException);
            }
        }
    }
}