//------------------------------------------------------------------------------------------------- 
// <copyright file="PullAlertDetails.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace AzureDevOpsEventsProcessor.AlertDetails
{
    /// <summary>
    /// The details of the checkin stored in the XML alert
    /// </summary>
    public class PullAlertDetails
    {
        /// <summary>
        /// The push ID
        /// </summary>
        public int PullId { get; set; }

        /// <summary>
        /// The repo
        /// </summary>
        public string Repo { get; set; }

    }
}