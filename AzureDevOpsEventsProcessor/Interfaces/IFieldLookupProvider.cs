//------------------------------------------------------------------------------------------------- 
// <copyright file="IFieldLookupProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureDevOpsEventsProcessor.Interfaces
{
    /// <summary>
    /// The interface that defines how to lookup detail for a Azure DevOps work item
    /// </summary>
    public interface IFieldLookupProvider
    {
        /// <summary>
        /// Gets a single field value
        /// </summary>
        /// <param name="fieldName">The field name to lookup</param>
        /// <returns>The current value</returns>
        string LookupWorkItemFieldValue(string fieldName);

        /// <summary>
        /// Returns all the field values in a work item
        /// </summary>
        /// <param name="title">The title to start the block with</param>
        /// <returns>Format set of HTML data</returns>
        string GetAllWorkItemFields(string title);

        /// <summary>
        /// Get a field from the XML event
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The value</returns>
        string LookupAlertFieldValue(string fieldName);

        /// <summary>
        /// Gets all XML field data
        /// </summary>
        /// <param name="title">The title to start the block with</param>
        /// <returns>Format set of HTML data</returns>
        string GetAllAlertFields(string title);

        /// <summary>
        /// Gets the work item type that triggered the alert
        /// </summary>
        string GetWorkItemType { get; }

        /// <summary>
        /// Gets all the people who need to get an email due to the change
        /// </summary>
        /// <param name="domain">The domain to append to the names to make an email address</param>
        /// <returns>An comma separated list of email addresses</returns>
        string GetInterestedEmailAddresses(string domain);

        /// <summary>
        /// Gets the user ID for a displayname
        /// </summary>
        /// <param name="displayName">The display name</param>
        /// <returns>The AD account</returns>
        string GetUserIdFromDisplayName(string displayName);
    }
}
