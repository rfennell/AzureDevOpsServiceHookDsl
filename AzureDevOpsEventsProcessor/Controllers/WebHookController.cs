//------------------------------------------------------------------------------------------------- 
// <copyright file="WebHookController.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using BasicAuthentication.Filters;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureDevOpsEventsProcessor.Helpers;
using AzureDevOpsEventsProcessor.Interfaces;

namespace AzureDevOpsEventsProcessor.Controllers
{
    /// <summary>
    /// The main MVC controller
    /// </summary>
    public class WebHookController : ApiController
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Instance of email provider
        /// </summary>
        private readonly IEmailProvider iEmailProvider;

        /// <summary>
        /// Instance of Azure DevOps provider
        /// </summary>
        private IAzureDevOpsProvider iAzureDevOpsProvider;

        /// <summary>
        /// The script to run
        /// </summary>
        private readonly string scriptFile;

        /// <summary>
        /// Folder to scan for DSL assemblies
        /// </summary>
        private readonly string dslFolder;

        /// <summary>
        /// Folder to look for scripts in
        /// </summary>
        private readonly string scriptFolder;

        /// <summary>
        /// Flag to set of redirecting console, only used for test
        /// </summary>
        private readonly bool redirectScriptEngineOutputtoLogging = false;

        /// <summary>
        /// If true subscription id used in place over event type to find script name
        /// </summary>
        private readonly bool useSubscriptionID = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebHookController()
        {
            this.iEmailProvider = new Providers.SmtpEmailProvider(
             Microsoft.Azure.CloudConfigurationManager.GetSetting("SMTPServer"),
             Convert.ToInt32(Microsoft.Azure.CloudConfigurationManager.GetSetting("SMTPPort")),
             Microsoft.Azure.CloudConfigurationManager.GetSetting("FromEmail"),
             Microsoft.Azure.CloudConfigurationManager.GetSetting("EmailDomain"),
             Microsoft.Azure.CloudConfigurationManager.GetSetting("SMTPUsername"),
             Microsoft.Azure.CloudConfigurationManager.GetSetting("SMTPPassword"));

            // default false
            this.useSubscriptionID = ConfigHelper.ParseOrDefault(Microsoft.Azure.CloudConfigurationManager.GetSetting("UseSubscriptionId"));
            this.scriptFile = Microsoft.Azure.CloudConfigurationManager.GetSetting("ScriptFile");
            this.dslFolder = FolderHelper.GetRootedPath(Microsoft.Azure.CloudConfigurationManager.GetSetting("DSLFolder"));
            this.scriptFolder = FolderHelper.GetRootedPath(Microsoft.Azure.CloudConfigurationManager.GetSetting("ScriptFolder"));
            this.redirectScriptEngineOutputtoLogging = true;
        }

        /// <summary>
        /// Test constructor
        /// </summary>
        /// <param name="iEmailProvider">Email provider</param>
        /// <param name="iAzureDevOpsProvider">Smpt Provider</param>
        /// <param name="scriptFile">The script file to run</param>
        /// <param name="dslFolder">Folder to scan for DSL assemblies</param>
        public WebHookController(IEmailProvider iEmailProvider, IAzureDevOpsProvider iAzureDevOpsProvider, string scriptFile, string dslFolder)
        {
            this.iEmailProvider = iEmailProvider;
            this.scriptFile = scriptFile;
            this.dslFolder = dslFolder;
        }

        /// <summary>
        /// Handler for a GET call
        /// </summary>
        /// <returns>HTTP Response</returns>
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var msg = $"Get: AzureDevOpsersion {this.GetType().Assembly.GetName().Version} running on server @{Environment.MachineName}";
            logger.Info(msg);
            response.Content = new StringContent(msg);
            return response;
        }

        /// <summary>
        /// The main method that catches the alert
        /// POST api/<controller>
        /// </summary>
        /// <param name="jsondata">The json data package</param>
        /// <returns>HTTP response</returns>
        [IdentityBasicAuthentication] // Enable authentication via an ASP.NET Identity user name and password
        [Authorize] // Require some form of authentication

        public HttpResponseMessage Post([FromBody]JObject jsondata)
        {
            try
            {
                if (ConfigHelper.ParseOrDefault(Microsoft.Azure.CloudConfigurationManager.GetSetting("LogEventsToFile")) == true)
                {
                    var logPath = ConfigHelper.GetLoggingPath();
                    logger.Info(string.Format("Post: Event being logged to [{0}]", logPath));
                    LoggingHelper.DumpEventToDisk(jsondata, logPath);
                }

                var dataProvider = new Providers.JsonDataProvider(jsondata);

                var uri = dataProvider.GetServerUrl();
                var locationpat = ConfigHelper.GetPersonalAccessToken(uri);
                logger.Info($"Post: Using a {uri}");
                if (string.IsNullOrEmpty(locationpat.Item2) == false)
                {
                    logger.Info($"Post: Using a PAT token and obtained from {locationpat.Item1}");
                    this.iAzureDevOpsProvider = new Providers.AzureDevOpsProvider(uri, locationpat.Item2);
                }
                else
                {
                    logger.Info(string.Format("Post: Using default credentials and url {0}", uri));
                    this.iAzureDevOpsProvider = new Providers.AzureDevOpsProvider(uri);
                }

                // work out the event type
                var eventType = dataProvider.GetEventType();
                // work out the subscription ID
                var subscriptionID = dataProvider.GetSubsriptionID();
                string[] argItems = null;
                switch (eventType)
                {
                    case "workitem.updated":
                    case "workitem.created":
                    case "workitem.deleted":
                    case "workitem.restored":
                    case "workitem.commented":
                        var workItemId = dataProvider.GetWorkItemDetails().Id;
                        if (workItemId > 0)
                        {
                            argItems = new[] { eventType, workItemId.ToString() };
                            logger.Info(
                                string.Format("Post: {1} Event being processed for WI:{0}", workItemId, eventType));
                        }
                        else
                        {
                            logger.Error(
                                string.Format("Post: {0} Event cannot find workitem ID", eventType));
                            return new HttpResponseMessage(HttpStatusCode.BadRequest);
                        }
                        break;
                    case "build.complete":
                        var buildDetails = dataProvider.GetBuildDetails();
                        argItems = new[] { eventType, buildDetails.Id.ToString() };
                        logger.Info(string.Format(
                            "Post: Event being processed for Build:{0}",
                            buildDetails.BuildUri));
                        break;
                    case "ms.vss-release.deployment-approval-completed-event":
                    case "ms.vss-release.deployment-approval-pending-event":
                    case "ms.vss-release.deployment-completed-event":
                    case "ms.vss-release.deployment-started-event":
                    case "ms.vss-release.release-abandoned-event":
                    case "ms.vss-release.release-created-event":
                        var releaseDetails = dataProvider.GetReleaseDetails();
                        argItems = new[] { eventType, releaseDetails.Id.ToString() };
                        logger.Info(string.Format(
                            "Post: Event being processed for Release:{0}",
                            releaseDetails.Id));
                        break;
                    case "tfvc.checkin":
                        var checkInDetails = dataProvider.GetCheckInDetails();
                        argItems = new[] { eventType, checkInDetails.Changeset.ToString() };
                        logger.Info(
                            string.Format(
                                "Post: Event being processed for Checkin:{0}",
                                checkInDetails.Changeset));

                        break;
                    case "message.posted":
                        var messagePostDetails = dataProvider.GetMessagePostDetails();
                        argItems = new[] { eventType, messagePostDetails.PostRoomId.ToString() };
                        logger.Info(
                            string.Format(
                                "Post: Event being processed for Meesage Post in Room:{0}",
                                messagePostDetails.PostRoomId));

                        break;
                    case "git.push":
                        var pushDetails = dataProvider.GetPushDetails();
                        argItems = new[] { eventType, pushDetails.Repo, pushDetails.PushId.ToString() };
                        logger.Info(
                            string.Format(
                                "Post: Event being processed for Push:{0}",
                                pushDetails.PushId));

                        break;
                    case "git.pullrequest.created":
                    case "git.pullrequest.merged":
                    case "git.pullrequest.updated":
                        var pullDetails = dataProvider.GetPullDetails();
                        argItems = new[] { eventType, pullDetails.Repo, pullDetails.PullId.ToString() };
                        logger.Info(
                            string.Format(
                                "Post: Event being processed for Pull:{0}",
                                pullDetails.PullId));

                        break;


                    default:
                        logger.Info(string.Format("Post: Unhandled event cannot processed:{0}", eventType));
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                var args = new Dictionary<string, object>
                {
                    { "Arguments", argItems },
                };

                var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor(redirectScriptEngineOutputtoLogging);
                engine.RunScript(
                    this.dslFolder,
                    this.scriptFolder,
                    FolderHelper.GetScriptName(
                        eventType,
                        subscriptionID,
                        this.scriptFile, 
                        this.useSubscriptionID),
                    args,
                    this.iAzureDevOpsProvider,
                    this.iEmailProvider,
                    dataProvider);


                return new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                // using a global exception catch to make sure we don't block any threads
                LoggingHelper.DumpException(logger, ex);

                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            }


        }

    }
}