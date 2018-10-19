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
        internal static Tuple<string, string> GetPersonalAccessToken(Uri uri)
        {
            // first check for a global PAT
            var location = "AppSettings";
            var pat = Microsoft.Azure.CloudConfigurationManager.GetSetting("PAT");
            if (string.IsNullOrEmpty(pat))
            {
                // check for instance based value
                var instanceCollection = ConfigurationManager.GetSection("VSTSInstance") as NameValueCollection;
                if (instanceCollection != null)
                {
                    // format xxx.visualstudio.com or dev.azure.com/xxx
                    // as of oct18 you always get the prior format, but this could change
                    // could be either
                    // https://richardfennell.visualstudio.com/_apis/git/repositories/3c4e22ee-6148-45a3-913b-454009dac91d/commits/50e062ba3f13715a83ca4bb43dc54ef19630ae31
                    // https://azure.dev.com/richardfennell/_apis/git/repositories/3c4e22ee-6148-45a3-913b-454009dac91d/commits/50e062ba3f13715a83ca4bb43dc54ef19630ae31

                    // Look for just instance name e.g richardfennell
                    location = ConfigHelper.GetInstanceName(uri);
                    pat = GetPATFromCollection(location, instanceCollection);
                    if (string.IsNullOrEmpty(pat))
                    {
                        // fall back to the complete instance name e.g.richardfennell.visualstudio.com
                        location = uri.Host;
                        pat = GetPATFromCollection(location, instanceCollection);
                    }
                }
            }
            return Tuple.Create(location, pat);
        }

        /// <summary>
        /// Looks for a instance name in a collection
        /// Used to catch the expections seen if no entry
        /// </summary>
        /// <param name="host">Host to look for</param>
        /// <param name="collection">Data collection</param>
        /// <returns></returns>
        private static string GetPATFromCollection(string host, NameValueCollection collection)
        {
            var pat = string.Empty;

            try
            {
                // could do with adding some code that checks in the config file
                pat = collection[host].ToString();
            }
            catch (NullReferenceException)
            { }

            return pat;
        }

        /// <summary>
        /// Get the Azure DevOps instance name
        /// </summary>
        /// <param name="uri">The event provided URL</param>
        /// <returns>The instance name</returns>
        public static string GetInstanceName(Uri uri)
        {
            // fallback to the full name
            var instance = uri.Host;
            if (uri.Host.Contains("visualstudio.com"))
            {
                instance = instance.Split('.')[0];
            }
            else if (uri.ToString().Contains("https://dev.azure.com"))
            {
                instance = uri.ToString().Replace("https://dev.azure.com", string.Empty).Split('/')[1];
            }
            return instance;
        }
    }
}