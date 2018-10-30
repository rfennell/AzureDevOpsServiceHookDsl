//------------------------------------------------------------------------------------------------- 
// <copyright file="IAzureDevOpsProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AzureDevOpsEventsProcessor.Interfaces
{
    /// <summary>
    /// Azure DevOps service provider
    /// </summary>
    public interface IAzureDevOpsProvider
    {
        /// <summary>
        /// Creates an instance of the class used to communicate with Azure DevOps
        /// </summary>
        /// <param name="uri">The TPC Url</param>
        void SetTpcUrl(Uri uri);

        /// <summary>
        /// Updates the build retension for a build
        /// </summary>
        /// <param name="uri">Build uri</param>
        /// <param name="keepForever">True if the build should be kept</param>
        /// <returns>Json Object</returns>
        JObject SetBuildRetension(Uri uri, bool keepForever);

        /// <summary>
        /// Adds a build tag
        /// </summary>
        /// <param name="uri">Build uri</param>
        /// <param name="tag">The tag</param>
        /// <returns>Json Object</returns>
        JObject AddBuildTag(Uri uri, string tag);

        /// <summary>
        /// Removed a build tag
        /// </summary>
        /// <param name="uri">Build uri</param>
        /// <param name="tag">The tag</param>
        /// <returns>Json Object</returns>
        JObject RemoveBuildTag(Uri uri, string tag);

        /// Get build tags
        /// </summary>
        /// <param name="uri">Build uri</param>
        JObject GetBuildTags(Uri uri);

        /// <summary>
        /// Returns the work item with the specified id
        /// </summary>
        /// <param name="id">The work item ID</param>
        /// <returns>The work item</returns>
        JObject GetWorkItem(int id);

        /// <summary>
        ///Creates a work item 
        /// </summary>
        /// <param name="teamproject">The team project to create the work item in</param>
        /// <param name="witName">The work item type to create</param>
        /// <param name="fields">The properties to set</param>
        /// <returns>The work item</returns>
        JObject CreateWorkItem(string teamproject, string witName, Dictionary<string, object> fields);

        /// <summary>
        /// Gets the details of a given build
        /// </summary>
        /// <param name="id">The id of the build</param>
        /// <returns>The build details</returns>
        JObject GetBuildDetails(int id);

        /// <summary>
        /// Gets the details of a given build
        /// </summary>
        /// <param name="uri">The URI of the build</param>
        /// <returns>The build details</returns>
        JObject GetBuildDetails(Uri uri);

        /// <summary>
        /// Updates a work work item
        /// </summary>
        /// <param name="wi">The work item</param>
        /// <returns>Json Object</returns>
        JObject UpdateWorkItem(JObject wi);

        /// <summary>
        /// Returns the parent work item with the specified work item
        /// </summary>
        /// <param name="wi">The work item</param>
        /// <returns>The parent work item</returns>
        JObject GetParentWorkItem(JObject wi);

        /// <summary>
        /// Returns the child work items with the specified work item
        /// </summary>
        /// <param name="wi">The work item</param>
        /// <returns>The parent work item</returns>
        IEnumerable<JObject> GetChildWorkItems(JObject wi);



        /// <summary>
        /// Get the details of a given changeset
        /// </summary>
        /// <param name="id">Changeset ID</param>
        /// <returns>The changeset</returns>
        JObject GetChangesetDetails(int id);

        /// <summary>
        /// Get the details of a given push
        /// </summary>
        /// <param name="repo">The repo ID</param>
        /// <param name="id">push ID</param>
        /// <returns>The push</returns>
        JObject GetPushDetails(string repo, int id);

        /// <summary>
        /// Get the details of a given commit
        /// </summary>
        /// <param name="repo">The repo id</param>
        /// <param name="id">Commit SHA</param>
        /// <returns>The commit</returns>
        JObject GetCommitDetails(string repo, string id);

        /// <summary>
        /// Get the details of a given commit
        /// </summary>
        /// <param name="uri">Commit uri</param>
        /// <returns>The commit</returns>
        JObject GetCommitDetails(Uri uri);

        /// <summary>
        /// Get a parameter argument from a Azure DevOps build
        /// </summary>
        /// <param name="buildDefUri">The Uri of the build definition</param>
        /// <param name="key">The arguement name</param>
        /// <returns>The value of the argument</returns>
        string GetBuildArgument(Uri buildDefUri, string key);

        /// <summary>
        /// Set a parameter argument from a Azure DevOps build
        /// </summary>
        /// <param name="buildDefUri">The Uri of the build definition</param>
        /// <param name="key">The arguement name</param>
        /// <param name="value">The value to set</param>
        /// <returns>Json Object</returns>
        JObject SetBuildArgument(Uri buildDefUri, string key, string value);

        /// <summary>
        /// Get all parameter argument from a Azure DevOps build
        /// </summary>
        /// <param name="buildDefUri">The Uri of the build definition</param>
        /// <returns>The value of the argument</returns>
        JObject GetBuildDefintionDetails(Uri buildDefUri);


    }
}
