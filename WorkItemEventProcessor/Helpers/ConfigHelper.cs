//------------------------------------------------------------------------------------------------- 
// <copyright file="ConfigHelper.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TFSEventsProcessor.Helpers
{
    /// <summary>
    /// Helper methods for reading config information
    /// </summary>
    internal class ConfigHelper
    {
        /// <summary>
        /// Get the logging path from the config file
        /// </summary>
        /// <returns>The fully qualified logging path</returns>
        internal static string GetLoggingPath()
        {
            var logFolder = Path.GetTempPath();
            if (string.IsNullOrEmpty(Microsoft.Azure.CloudConfigurationManager.GetSetting("LoggingFolder")) == false)
            {
                logFolder = Microsoft.Azure.CloudConfigurationManager.GetSetting("LoggingFolder");
            }
            return FolderHelper.GetRootedPath(logFolder);
        }

        /// <summary>
        /// Default a boolean value when parsing a string
        /// </summary>
        /// <param name="value">The string value</param>
        /// <returns>The boolean equivalent</returns>
        internal static bool ParseOrDefault(string value)
        {
            bool result = false;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            return false;
        }
    }
}