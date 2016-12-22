//------------------------------------------------------------------------------------------------- 
// <copyright file="ConfigHelper.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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

        /// <summary>
        /// Get the global PAT token or one specific to an instane
        /// </summary>
        /// <param name="uri">The source URI</param>
        /// <returns>The PAT if any</returns>
        internal static string GetPersonalAccessToken(Uri uri)
        {
            var pat = Microsoft.Azure.CloudConfigurationManager.GetSetting("PAT");
            if (string.IsNullOrEmpty(pat))
            {
                // check for instance based value
                var instanceCollection = ConfigurationManager.GetSection("VSTSInstance") as NameValueCollection;
                if (instanceCollection != null)
                {
                    try
                    {
                        pat = instanceCollection[uri.Host].ToString();
                    }
                    catch (NullReferenceException)
                    { }
                }
            }
            return pat;
        }
    }
}