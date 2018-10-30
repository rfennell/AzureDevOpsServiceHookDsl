//------------------------------------------------------------------------------------------------- 
// <copyright file="AzureDevOpsProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using NLog;
using AzureDevOpsEventsProcessor.Dsl;
using AzureDevOpsEventsProcessor.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace AzureDevOpsEventsProcessor.Providers
{
    /// <summary>
    /// Class to manage the connection to Azure DevOps
    /// </summary>
    public class AzureDevOpsProvider : IAzureDevOpsProvider
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The current Azure DevOps instance
        /// </summary>
        private readonly Uri tpcUri;

        /// <summary>
        /// Username to access Azure DevOps
        /// </summary>
        private readonly string username;

        /// <summary>
        /// Password to access Azure DevOps
        /// </summary>
        private readonly string password;


        /// <summary>
        /// Creates an instance of the class used to communicate with Azure DevOps
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Cannot build if use a more limited exception")]
        public AzureDevOpsProvider()
        {
        }

        /// <summary>
        /// Creates an instance of the class used to communicate with Azure DevOps
        /// </summary>
        /// <param name="serverUri">The server address</param>
        public AzureDevOpsProvider(Uri serverUri)
        {
            this.tpcUri = serverUri;
        }

        /// <summary>
        /// Creates an instance of the class used to communicate with Azure DevOps
        /// </summary>
        /// <param name="serverUri">The server address</param>
        public AzureDevOpsProvider(Uri serverUri, string pat)
        {
            // Instantiate a reference to the Azure DevOps Project Collection
            this.tpcUri = serverUri;
            this.username = string.Empty;
            this.password = pat;
        }

        /// <summary>
        /// Creates an instance of the class used to communicate with Azure DevOps
        /// </summary>
        /// <param name="serverUri">The server address</param>
        public AzureDevOpsProvider(Uri serverUri, string username, string password)
        {
            // Instantiate a reference to the Azure DevOps Project Collection
            this.tpcUri = serverUri;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Get a basic web client
        /// </summary>
        /// <returns>The client</returns>
        private WebClient GetWebClient()
        {
            return GetWebClient("application/json");
        }

        /// <summary>
        /// Gets a basic web client
        /// </summary>
        /// <param name="contentType">THe data content type</param>
        /// <returns>The client</returns>
        private WebClient GetWebClient(string contentType)
        {

            var wc = new System.Net.WebClient();

            wc.Headers["Content-Type"] = contentType;

            if (String.IsNullOrEmpty(password))
            {
                logger.Info("WebClient Using default credentials");
                wc.UseDefaultCredentials = true;
            }
            else
            {
                logger.Info("WebClient PAT");
                var pair = $"{this.username}:{this.password}";
                var bytes = System.Text.Encoding.ASCII.GetBytes(pair);
                var base64 = System.Convert.ToBase64String(bytes);
                wc.Headers.Add("Authorization", string.Format("Basic {0}", base64));
            }

            return wc;
        }


        /// <summary>
        /// Updates the build retension for a build
        /// </summary>
        /// <param name="buildUri">The path to the build</param>
        /// <param name="keepForever">True if the build should be kept</param>
        public JObject SetBuildRetension(Uri uri, bool keepForever)
        {

            logger.Info("Setting build retension for build {0} to {1}", uri, keepForever);
            var wc = GetWebClient();

            if (uri.ToString().EndsWith("?api-version=2.0")==false)
            {
                uri = new Uri($"{uri.ToString()}?api-version=2.0");
            }

            var data = JToken.FromObject(new
            {
                keepForever = keepForever
            });

            var jsondata = wc.UploadString(uri, "PATCH", data.ToString());
            return JObject.Parse(jsondata);
        }

        /// <summary>
        /// Returns the work item with the specified id
        /// </summary>
        /// <param name="id">The work item ID</param>
        /// <returns>The work item</returns>
        public JObject GetWorkItem(int id)
        {
            logger.Info("Getting details for WI {0} via {1}", id, this.tpcUri);
            var uri = $"{this.tpcUri}/_apis/wit/workitems/{id}?$expand=relations";
            return GetGenericApi(uri);
        }

        public JObject GetWorkItem(string uri)
        {
            return GetGenericApi(uri);
        }

        private JObject GetWorkItem(string uri, string extraArgs)
        {
            return GetGenericApi($"{uri}{extraArgs}");
        }

        public JObject GetGenericApi(string uri)
        {
            return GetGenericApi(new Uri(uri));
        }

        public JObject GetGenericApi(Uri uri)
        {
            logger.Info("Getting details of something via URL {0}", uri);
            var wc = GetWebClient();
            var jsondata = wc.DownloadString($"{uri}");
            return JObject.Parse(jsondata);
        }


        /// <summary>
        /// Returns the parent work item with the specified work item
        /// </summary>
        /// <param name="wi">The work item</param>
        /// <returns>The parent work item</returns>
        public JObject GetParentWorkItem(JObject wi)
        {
            JObject parentItem = null;
            foreach (var relation in wi["relations"])
            {
                if (relation["rel"].ToString() == "System.LinkTypes.Hierarchy-Reverse")
                {
                    parentItem = GetWorkItem(relation["url"].ToString(),"?$expand=relations");
                }
            }
            return parentItem;
        }

        /// <summary>
        /// Returns the child work items with the specified work item
        /// </summary>
        /// <param name="wi">The work item</param>
        /// <returns>The parent work item</returns>
        public IEnumerable<JObject> GetChildWorkItems(JObject wi)
        {
            logger.Info("Getting children for WI {0} via {1}", wi["id"], this.tpcUri);

            var returnObject = new List<JObject>();

            // find the children with repeated calls
            // could do a single call with a list, but that involves more processing
            foreach (var relation in wi["relations"])
            {
                if (relation["rel"].ToString() == "System.LinkTypes.Hierarchy-Forward")
                {
                    returnObject.Add(GetWorkItem(relation["url"].ToString()));
                }
            }
            return returnObject;

        }

        /// <summary>
        /// Updates a work work item
        /// </summary>
        /// <param name="wi">The work item</param>
        public JObject UpdateWorkItem(JObject wi)
        {
            logger.Info($"Updating WI via {this.tpcUri}");

            // get the current work item values so we can see what has changed
            var currentValues = GetWorkItem(Convert.ToInt32(wi["id"]));

            var wc = GetWebClient("application/json-patch+json");
            var uri = $"{this.tpcUri}/_apis/wit/workitems/{wi["id"]}?api-version=1.0";

            // need to convert the JObject into a list of changes, we only handle fields
            // we do have to add the rev else the update is rejected
            // we use the one for the orginal call, not our check 
            var list = new List<JToken>();
            list.Add(JToken.FromObject(new
            {
                op = "test",
                path = $"/rev",
                value = wi["rev"]
            })
            );

#pragma warning disable S3217 // "Explicit" conversions of "foreach" loops should not be used
            foreach (JProperty field in wi["fields"])
#pragma warning restore S3217 // "Explicit" conversions of "foreach" loops should not be used
            {
                if (currentValues["fields"][field.Name] == null ||
                    currentValues["fields"][field.Name].ToString() != field.Value.ToString())
                {
                    // it has been edited to add the token
                    var jsonField = JToken.FromObject(new
                    {
                        op = "add",
                        path = $"/fields/{field.Name}",
                        value = field.Value.ToString()

                    });
                    list.Add(jsonField);
                }
            }

            // this is ugly might be a better way in Newtonsoft
            var data = new StringBuilder();
            data.Append("[");

            foreach (var item in list)
            {
                data.Append(item.ToString());
                if (item != list.Last())
                {
                    data.Append(",");
                }
            }
            data.Append("]");
            var jsondata = wc.UploadString(uri, "PATCH", data.ToString());

            return JObject.Parse(jsondata);
        }

        /// <summary>
        ///Creates a work item 
        /// </summary>
        /// <param name="teamproject">The team project to create the work item in</param>
        /// <param name="witName">The work item type to create</param>
        /// <param name="fields">The properties to set</param>
        /// <returns>The work item</returns>
        public JObject CreateWorkItem(string teamproject, string witName, Dictionary<string, object> fields)
        {
            if (string.IsNullOrEmpty(teamproject))
            {
                throw new ArgumentNullException("teamproject");
            }

            if (string.IsNullOrEmpty(witName))
            {
                throw new ArgumentNullException("witName");
            }

            if (fields == null)
            {
                throw new ArgumentNullException("fields");
            }

            logger.Info("Creating WI via {1}", this.tpcUri);
            var wc = GetWebClient("application/json-patch+json");
            var uri = $"{this.tpcUri}/{teamproject}/_apis/wit/workitems/${witName}?api-version=1.0";

            var list = new List<JToken>();
            foreach (var field in fields)
            {
                var jsonField = JToken.FromObject(new
                {
                    op = "add",
                    path = $"/fields/{field.Key}",
                    value = field.Value.ToString()

                });
                list.Add(jsonField);
            }

            // this is ugly might be a better way in Newtonsoft
            var data = new StringBuilder();
            data.Append("[");

            foreach (var item in list)
            {
                data.Append(item.ToString());
                if (item != list.Last())
                {
                    data.Append(",");
                }
            }
            data.Append("]");

            var jsondata = wc.UploadString(uri, "PATCH", data.ToString());
            return JObject.Parse(jsondata);
        }

        /// <summary>
        /// Gets the details of a given build
        /// </summary>
        /// <param name="buildUri">The URI of the build</param>
        /// <returns>The build details</returns>
        public JObject GetBuildDetails(int id)
        {
            logger.Info("Getting details for build {0} via {1}", id, this.tpcUri);
            var uri = $"{this.tpcUri}/_apis/build/Builds/{id}?api-version=2.0";
            return GetGenericApi(uri);

        }

        /// <summary>
        /// Gets the details of a given build
        /// </summary>
        /// <param name="buildUri">The URI of the build</param>
        /// <returns>The build details</returns>
        public JObject GetBuildDetails(Uri uri)
        {
            logger.Info("Getting details for build via {0}", this.tpcUri);
            return GetGenericApi(uri);

        }

        /// <summary>
        /// Get the details of a given changeset
        /// </summary>
        /// <param name="id">Changeset ID</param>
        /// <returns>The changeset</returns>
        public JObject GetChangesetDetails(int id)
        {
            logger.Info("Getting details for changeset {0} via {1}", id, this.tpcUri);
            var wc = GetWebClient();
            var uri = $"{this.tpcUri}/_apis/tfvc/changesets/{id}?maxChangeCount=100&maxCommentLength=2000&api-version=1.0";
            var jsondata = wc.DownloadString(uri);
            return JObject.Parse(jsondata);

        }

        /// <summary>
        /// Get the details of a given commit
        /// </summary>
        /// <param name="id">commit ID</param>
        /// <returns>The changeset</returns>
        public JObject GetCommitDetails(string repo , string id)
        {
            logger.Info("Getting details for commit {0} via {1}", id, this.tpcUri);
            var uri = new Uri( $"{this.tpcUri}/_apis/git/repositories/{repo}/commits/{id}?api-version=1.0");
            return GetCommitDetails(uri);

        }

        /// <summary>
        /// Get the details of a given commit
        /// </summary>
        /// <param name="id">commit ID</param>
        /// <returns>The changeset</returns>
        public JObject GetCommitDetails(Uri uri)
        {
            logger.Info("Getting details for commit via {0}", uri);
            return this.GetGenericApi(uri);
        }


        /// <summary>
        /// Get the details of a given push
        /// </summary>
        /// <param name="id">push ID</param>
        /// <returns>The push</returns>
        public JObject GetPushDetails(string repo, int id)
        {
            logger.Info("Getting details for commit {0} via {1}", id, this.tpcUri);
            var wc = GetWebClient();

            var uri = $"{this.tpcUri}/_apis/git/repositories/{repo}/pushes/{id}?api-version=1.0";
            var jsondata = wc.DownloadString(uri);
            return JObject.Parse(jsondata);

        }

        /// <summary>
        /// Adds a build tag
        /// </summary>
        /// <param name="id">Build Id</param>
        /// <param name="tag">Thebtag</param>
        public JObject GetBuildTags(Uri uri)
        {
            uri = new Uri($"{uri.ToString()}/tags?api-version=2.0");
            return GetGenericApi(uri);
        }

        /// <summary>
        /// Adds a build tag
        /// </summary>
        /// <param name="id">Build Id</param>
        /// <param name="tag">Thebtag</param>
        public JObject AddBuildTag(Uri uri, string tag)
        {
            logger.Info("Adding build tag for build {0} to {1}", uri, tag);

            var wc = GetWebClient();
            uri = new Uri($"{uri.ToString()}/tags/{tag}?api-version=2.0");

            var data = JToken.FromObject(new
            {
                value = tag
            });

            var jsondata = wc.UploadString(uri, "PUT", data.ToString());
            return JObject.Parse(jsondata);
        }

        /// <summary>
        /// Removed a build tag
        /// </summary>
        /// <param name="id">Build Id</param>
        /// <param name="tag">Thebtag</param>
        public JObject RemoveBuildTag(Uri uri, string tag)
        {
            logger.Info("Removing build tag for build {0} to {1}", uri, tag);

            var wc = GetWebClient();
            uri = new Uri($"{uri.ToString()}/tags/{tag}?api-version=2.0");

            var data = JToken.FromObject(new
            {
                value = tag
            });

            var jsondata = wc.UploadString(uri, "DELETE", data.ToString());
            return JObject.Parse(jsondata);
        }




        /// <summary>
        /// Get a parameter argument from a Azure DevOps build
        /// </summary>
        /// <param name="buildDefUri">The Uri of the build definition</param>
        /// <param name="key">The arguement name</param>
        /// <returns>The value of the argument</returns>
        public string GetBuildArgument(Uri buildDefUri, string key)
        {
            var process = this.GetBuildDefintionDetails(buildDefUri);

            logger.Info(string.Format("Getting build argument [{0}] for build definition [{1}]", key, buildDefUri.ToString()));

            try
            {
                return process["variables"][key]["value"].ToString();
            }
            catch (NullReferenceException)
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// Get all parameter argument from a Azure DevOps build
        /// </summary>
        /// <param name="buildDefUri">The Uri of the build definition</param>
        /// <returns>The value of the argument</returns>
        public JObject GetBuildDefintionDetails(Uri uri)
        {
            logger.Info("Getting details for builddefintion via {0}", this.tpcUri);
            var wc = GetWebClient();
            var jsondata = wc.DownloadString(uri);
            return JObject.Parse(jsondata);
        }

        /// <summary>
        /// Set a parameter argument from a Azure DevOps build
        /// </summary>
        /// <param name="buildDefUri">The Uri of the build definition</param>
        /// <param name="key">The arguement name</param>
        /// <param name="value">The value to set</param>
        public JObject SetBuildArgument(Uri buildDefUri, string key, string value)
        {
            logger.Info(string.Format("Setting build argument [{0}] for build definition [{1}] to [{2}]", key, buildDefUri.ToString(), value.ToString()));

            var wc = GetWebClient();
            // you have to submit the whole def
            var def = GetBuildDefintionDetails(buildDefUri);

            def["variables"][key]["value"] = value;

            var jsondata = wc.UploadString($"{buildDefUri}?api-version=2.0", "PUT", def.ToString());
            return JObject.Parse(jsondata);
        }


        public void SetTpcUrl(Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}