//------------------------------------------------------------------------------------------------- 
// <copyright file="BuildAlertDetails.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureDevOpsEventsProcessor.AlertDetails
{
    /// <summary>
    /// Container for the alert information stored in XML
    /// </summary>
    public class BuildAlertDetails
    {
        /// <summary>
        /// The build Uri (short form)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The build Uri (short form)
        /// </summary>
        public Uri BuildUri { get; set; }

        /// <summary>
        /// The build Url (the REST call)
        /// </summary>
        public Uri BuildUrl { get; set; }

        /// <summary>
        /// The text that describes the event as a whole
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The new status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The new build quality
        /// </summary>
        public string NewQuality { get; set; }
    }

}