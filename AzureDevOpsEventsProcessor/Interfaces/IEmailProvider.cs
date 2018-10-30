//------------------------------------------------------------------------------------------------- 
// <copyright file="IEmailProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureDevOpsEventsProcessor.Interfaces
{
    /// <summary>
    /// Smtp Email Provider
    /// </summary>
    public interface IEmailProvider
    {
        /// <summary>
        /// Sends a simple email
        /// </summary>
        /// <param name="to">Who the email goes to, a , separated list</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        void SendEmailAlert(
          string to,
          string subject,
          string body);

        /// <summary>
        /// Sends the email build from a work item
        /// </summary>
        /// <param name="fieldsLookupProvider">The provider that extracts the data from Azure DevOps</param>
        /// <param name="templatePath">The path to the HTM (like) template file</param>
        /// <param name="includeAllWorkItemFields">If true all available work item fields are appended to the email body</param>
        /// <param name="includeAllAlertFields">If true all available alert fields are appended to the email body</param>
        void SendEmailAlert(
            IFieldLookupProvider fieldsLookupProvider,
            string templatePath,
            bool includeAllWorkItemFields,
            bool includeAllAlertFields);
    }
}
