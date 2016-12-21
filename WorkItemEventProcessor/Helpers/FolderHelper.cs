//------------------------------------------------------------------------------------------------- 
// <copyright file="FolderHelper.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.IO;
using NLog;

namespace TFSEventsProcessor.Helpers
{
    /// <summary>
    /// Helper method to find folders
    /// </summary>
    public static class FolderHelper
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// FIxes a path if it is not rooted
        /// </summary>
        /// <param name="path">Path in</param>
        /// <returns>Fixed path</returns>
        public static string GetRootedPath(string path)
        {
            var returnValue = path;
            if (Path.IsPathRooted(path) == false)
            {
                // ask the web server to sort the path, have to do a but of fiddling
                if (string.IsNullOrEmpty(path))
                {
                    path = "~/";
                }
                returnValue = System.Web.Hosting.HostingEnvironment.MapPath(path);
                if (returnValue == null)
                {
                    // not on a web server, so work it out from our assembly location (used in tests)
                    returnValue = Path.Combine(GetBaseFolder(), path.Replace("~/",string.Empty));
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Get the current working folder
        /// </summary>
        /// <returns>The path</returns>
        private static string GetBaseFolder()
        {
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Returns the name of the script to run
        /// </summary>
        /// <param name="type">The event type</param>
        /// <param name="defaultScript">Default script name</param>
        /// <returns>Script name</returns>
        public static string GetScriptName(string type, string defaultScript)
        {
            var retItem = defaultScript;
            if (string.IsNullOrEmpty(defaultScript))
            {
                retItem = string.Format("{0}.py", type.ToString());
            }
            logger.Info(
                        string.Format(
                            "TFSEventsProcessor: DslScriptService using script file {0}",
                            retItem));
            return retItem;
        }
    }

}
