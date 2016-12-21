//------------------------------------------------------------------------------------------------- 
// <copyright file="EmailTemplate.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using TFSEventsProcessor.Dsl;
using TFSEventsProcessor.Helpers;

namespace TFSEventsProcessor
{
    /// <summary>
    /// Class to manage email templates
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Tag used in file for WI fields
        /// </summary>
        public static readonly string FieldTag = "@@";

        /// <summary>
        /// Tag used in file for alert fields
        /// </summary>
        public static readonly string AlertTag = "##";

        /// <summary>
        /// The email title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The body for the email
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// Text for the dump of WI fields
        /// </summary>
        public string WiFieldHeader { get; private set; }

        /// <summary>
        /// Text for the dump of Alert fields
        /// </summary>
        public string AlertFieldHeader { get; private set; }

        /// <summary>
        /// Creates a class to manage email templates
        /// </summary>
        /// <param name="contents">A string containing the template text</param>
        public EmailTemplate(string contents)
        {
            // using string handling as this is not a fully formed HTML or XML schema based file
            this.Title = GetTagContents(contents, "subject");
            this.Body = GetTagContents(contents, "body");
            this.AlertFieldHeader = GetTagContents(contents, "alertfieldheader");
            this.WiFieldHeader = GetTagContents(contents, "wifieldheader");
        }

        /// <summary>
        /// Expands a single tag in a template, used to separate out the parts of the template
        /// </summary>
        /// <param name="contents">The template contents</param>
        /// <param name="tag">The tag to expand</param>
        /// <returns>The portion of the template requested</returns>
        private static string GetTagContents(string contents, string tag)
        {
            var start = contents.ToLower().IndexOf(string.Format("<{0}>", tag)) + tag.Length + 2;
            var end = contents.ToLower().IndexOf(string.Format("</{0}>", tag), start);
            return contents.Substring(start, end - start).Trim().Replace("&gt;", ">").Replace("&lt;", "<");
        }

        /// <summary>
        /// A factory method that will create an instance of the template from a file
        /// </summary>
        /// <param name="path">Path to the file or folder</param>
        /// <param name="workItemType">The work item type, used as a file name if a file is not provided in the first parameter </param>
        /// <returns>Instance of the email template</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1",Justification="our load is the only validate we can try")]
        public static EmailTemplate LoadTemplate(string path, string workItemType)
        {
            var templatePath = FindTemplate(path, workItemType);
            if (string.IsNullOrEmpty(templatePath) == false)
            {
                return new EmailTemplate(File.ReadAllText(templatePath));
            } 
            else
            {
                return null;
            }
        }

        /// <summary>
        /// A factory method that will find a template file
        /// </summary>
        /// <param name="path">Path to the file or folder</param>
        /// <param name="workItemType">The work item type, used as a file name if a file is not provided in the first parameter </param>
        /// <returns>Path of the email template</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1",Justification="our load is the only validate we can try")]
        public static string FindTemplate(string path, string workItemType)
        {
            path = FolderHelper.GetRootedPath(path);
            if (System.IO.Directory.Exists(path) == false)
            {
                logger.Info(string.Format("TFSEventsProcessor: Using template:{0}", path));
                if (System.IO.File.Exists(path) == false)
                {
                    logger.Error(string.Format("TFSEventsProcessor: Cannot find template file [{0}]", path));
                    return string.Empty;
                }
                else
                {
                    logger.Info(string.Format("TFSEventsProcessor: Using template:{0}", path));
                    // this is not a folder so must be a file
                    return path;
                }
            }
            else
            {
                // look for a file with the name of the work item type
                var fileName = string.Format(@"{0}\{1}.htm", path, workItemType.Replace(" ", string.Empty));
                logger.Info(string.Format("TFSEventsProcessor: Using template:{0}", fileName));
                if (System.IO.File.Exists(fileName) == false)
                {
                    logger.Error(string.Format("TFSEventsProcessor: Cannot find template file [{0}] or work item name based template [{1}]", path, fileName));
                    return string.Empty;
                }
                else
                {
                    return fileName;
                }
            }
        }

    }
}
