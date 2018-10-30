//------------------------------------------------------------------------------------------------- 
// <copyright file="DslLibrary.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Text;
using NLog;
using System;

namespace AzureDevOpsEventsProcessor.Dsl
{
    using Interfaces;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.Composition;
    using System.Globalization;

    using AzureDevOpsEventsProcessor.Helpers;
    using AzureDevOpsEventsProcessor.Providers;

    /// <summary>
    /// Contains the DSL API
    /// </summary>
    [Export(typeof(IDslLibrary))]
    public class DslLibrary : IDslLibrary
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The script folder
        /// </summary>
        public string ScriptFolder { get; set; }
 
        /// <summary>
        /// Instance of TFS provider
        /// </summary>
        public IAzureDevOpsProvider TfsProvider { get; set; }
  
        /// <summary>
        /// Instance of TFS provider
        /// </summary>
        public IEventDataProvider EventData { get; set; }

        /// <summary>
        /// Instance of the Email provider
        /// </summary>
        public IEmailProvider EmailProvider { get; set; }

        /// <summary>
        /// Constructor for DSL library
        /// </summary>
        /// <param name="IAzureDevOpsProvider">The TFS provider</param>
        /// <param name="iEmailProvider">The Email provider</param>
        public DslLibrary(IAzureDevOpsProvider iAzureDevOpsProvider, IEmailProvider iEmailProvider)
        {
            this.TfsProvider = iAzureDevOpsProvider;
            this.EmailProvider = iEmailProvider;
        }

        /// <summary>
        /// Constructor for DSL library, used when loading from MEF
        /// </summary>
        public DslLibrary()
        {

        }

        /// <summary>
        /// Sends a message to the info level logger
        /// </summary>
        /// <param name="msg">The message</param>
        public static void LogInfoMessage(object msg)
        {
            logger.Info(BuildMessageText(msg));
        }

        /// <summary>
        /// Sends a message to the debug level logger
        /// </summary>
        /// <param name="msg">The message</param>
        public static void LogDebugMessage(object msg)
        {
            logger.Debug(BuildMessageText(msg));
        }

        /// <summary>
        /// Sends a message to the error level logger
        /// </summary>
        /// <param name="msg">The message</param>
        public static void LogErrorMessage(object msg)
        {
            logger.Error(BuildMessageText(msg));
        }

        /// <summary>
        /// Works out the value passed and tries to convert it into something that can go into the text logger
        /// </summary>
        /// <param name="msg">The message</param>
        /// <returns>A string</returns>
        private static string BuildMessageText(object msg)
        {
            var retValue = new StringBuilder();
            if (msg.GetType() == typeof(IronPython.Runtime.List))
            {
                retValue.Append("List: ");
                foreach (var item in (IronPython.Runtime.List)msg)
                {
                    retValue.Append(string.Format("[{0}] ", item.ToString()));
                }
            }
            else
            {
                retValue.Append(msg.ToString());
            }
            return retValue.ToString();
        }

        /// <summary>
        /// Gets a work item from TFS
        /// </summary>
        /// <param name="id">The work item ID</param>
        /// <returns>A workitem</returns>
        public JObject GetWorkItem(int id)
        {
            return this.TfsProvider.GetWorkItem(id);
        }

        /// <summary>
        /// Gets a parent work item from TFS
        /// </summary>
        /// <param name="wi">The work item to find parent for</param>
        /// <returns>A workitem</returns>
        public JObject GetParentWorkItem(JObject wi)
        {
            return this.TfsProvider.GetParentWorkItem(wi);
        }

        /// <summary>
        /// Gets the child work items for a work item from TFS
        /// </summary>
        /// <param name="wi">The work item to find parent for</param>
        /// <returns>A workitem</returns>
        public IEnumerable<JObject> GetChildWorkItems(JObject wi)
        {
            return this.TfsProvider.GetChildWorkItems(wi);
        }

        /// <summary>
        /// Gets the details of a build
        /// </summary>
        /// <param name="buildUri">The build url</param>
        /// <returns>The build details</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "We are doing the conversion to ease life in the dsl")]
        public JObject GetBuildDetails(int id)
        {
            return this.TfsProvider.GetBuildDetails(id);
        }

        /// <summary>
        /// Gets the details of a build
        /// </summary>
        /// <param name="buildUri">The build url</param>
        /// <param name="keepForever">True of should keep build forever</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "We are doing the conversion to ease life in the dsl")]
        public JObject SetBuildRetension(string uri, bool keepForever)
        {
            return this.TfsProvider.SetBuildRetension(new Uri(uri), keepForever);
        }

        /// <summary>
        /// Gets a changeset
        /// </summary>
        /// <param name="id">The changeset id</param>
        /// <returns>The changeset</returns>
        public JObject GetChangesetDetails(int id)
        {
            return this.TfsProvider.GetChangesetDetails(id);
        }

        /// <summary>
        /// Gets a commit
        /// </summary>
        /// <param name="id">The commit id</param>
        /// <returns>The commit</returns>
        public JObject GetCommitDetails(string repo, string id)
        {
            return this.TfsProvider.GetCommitDetails(repo, id);
        }

        /// <summary>
        /// Gets a commit
        /// </summary>
        /// <param name="id">The commit id</param>
        /// <returns>The commit</returns>
        public JObject GetCommitDetails(string uri)
        {
            return this.TfsProvider.GetCommitDetails(new Uri(uri));
        }

        /// <summary>
        /// Gets the build tags
        /// </summary>
        /// <param name="id">The commit id</param>
        /// <returns>The commit</returns>
        public JObject GetBuildTags(string uri)
        {
            return this.TfsProvider.GetBuildTags(new Uri(uri));
        }

        /// <summary>
        /// Adds a build tag
        /// </summary>
        /// <param name="id">Build Id</param>
        /// <param name="tag">Thebtag</param>
        public JObject AddBuildTag(string uri, string tag)
        {
            return this.TfsProvider.AddBuildTag(new Uri(uri), tag);
        }

        /// <summary>
        /// Adds a build tag
        /// </summary>
        /// <param name="id">Build Id</param>
        /// <param name="tag">Thebtag</param>
        public JObject RemoveBuildTag(string uri, string tag)
        {
            return this.TfsProvider.RemoveBuildTag(new Uri(uri), tag);
        }
        
        /// <summary>
        /// Gets a push
        /// </summary>
        /// <param name="id">The push id</param>
        /// <returns>The push</returns>
        public JObject GetPushDetails(string repo, int id)
        {
            return this.TfsProvider.GetPushDetails(repo, id);
        }

        /// <summary>
        /// Creates a work item
        /// </summary>
        /// <param name="tp">The team project to create wi in</param>
        /// <param name="witName">The work item type</param>
        /// <param name="fields">The fields to set</param>
        /// <returns>The created work item</returns>
        public JObject CreateWorkItem(string tp, string witName, IDictionary<string, object> fields)
        {
            return this.TfsProvider.CreateWorkItem(tp, witName, new Dictionary<string, object>(fields));
        }

        /// <summary>
        /// Updates a work item
        /// </summary>
        /// <param name="wi">The work item to save</param>
        public JObject UpdateWorkItem(JObject wi)
        {
            return this.TfsProvider.UpdateWorkItem(wi);
        }

        /// <summary>
        /// Send an email 
        /// </summary>
        /// <param name="to">To address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        public void SendEmail(string to, string subject, string body)
        {
            this.EmailProvider.SendEmailAlert(to, subject, body);
        }

        /// <summary>
        /// Sends an email based on a template
        /// </summary>
        /// <param name="workItemId">The work item ID</param>
        /// <param name="templatePath">Path to the email template</param>
        /// <param name="dumpAllWorkItemFields">If true appends all work item fields to the email</param>
        /// <param name="dumpAllAlertFields">If true appends all alert fields to the email</param>
        /// <param name="showMissingFieldNames">If true adds error messages for incorrect field names</param>
        public void SendEmail(int workItemId, string templatePath, bool dumpAllWorkItemFields, bool dumpAllAlertFields, bool showMissingFieldNames)
        {

            // Get this list of changes
            var alertItems = this.EventData.GetWorkItemDetails();

            // Create a new Tfs helper
            var fieldLookupProvider = new TfsFieldLookupProvider(
                this.TfsProvider.GetWorkItem(workItemId),
                alertItems.ChangedAlertFields,
                alertItems.ChangedBy,
                showMissingFieldNames);

            //Process the email using a template
            this.EmailProvider.SendEmailAlert(
                fieldLookupProvider,
                templatePath,
                dumpAllWorkItemFields,
                dumpAllAlertFields);
        }

        /// <summary>
        /// The folder the script engine is using to load scripts
        /// </summary>
        /// <returns>A folder path</returns>
        public string CurrentScriptFolder()
        {
            return this.ScriptFolder;
        }

        /// <summary>
        /// Gets argument field in a build process definition.
        /// </summary>
        /// <param name="buildUri">The URI of the build instance</param>
        /// <param name="argumentName">The field to return</param>
        /// <returns>The argument value</returns>
        public string GetBuildArgument(string buildDefUri, string argumentName)
        {
            return this.TfsProvider.GetBuildArgument(new Uri(buildDefUri), argumentName);
        }


        /// <summary>
        /// Sets argument field in a build process definition.
        /// </summary>
        /// <param name="buildUri">The URI of the build instance</param>
        /// <param name="argumentName">The field to return</param>
        /// <param name="value">The value to set</param>
        public JObject SetBuildArgument(string buildDefUri, string argumentName, object value)
        {
            return this.TfsProvider.SetBuildArgument(new Uri(buildDefUri), argumentName, value.ToString());
        }

        /// <summary>
        /// Increment a string argument field in a build process definition.
        /// This can be used to raise a version number when some event occurs
        /// </summary>
        /// <param name="buildUri">The URI of the build instance</param>
        public int IncrementBuildArgument(string builddDefUri, string argumentName)
        {
            var value = this.TfsProvider.GetBuildArgument(new Uri(builddDefUri), argumentName);
            var number = -1;

            if (value == null)
            {
                LogInfoMessage(string.Format("Argument {0} is not set. This could be because it is not used in this build template, or because it only has a default value not an explicitaly set one", argumentName));
            }
            else
            {
                LogInfoMessage(string.Format("Argument {0} old value is {1}", argumentName, value.ToString()));

                // as we are incrementing we need to treat this an Int
                // the most likey field to be updating is on for the TFSVersion activity which
                // stores the values as strings, so we need to parse it
                int.TryParse(value.ToString(), out number);
                number++;

                LogInfoMessage(string.Format("Updated argument {0} value is {1}", argumentName, number));

                this.TfsProvider.SetBuildArgument(new Uri(builddDefUri), argumentName, number.ToString(CultureInfo.InvariantCulture));
            }
            return number;
        }
    }
}
