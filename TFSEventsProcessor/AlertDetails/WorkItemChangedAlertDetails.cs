//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkItemChangedAlertDetails.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFSEventsProcessor
{
    /// <summary>
    /// Container for the alert information stored in XML
    /// </summary>
    public class WorkItemChangedAlertDetails
    {
        /// <summary>
        /// The reference ID of the field
        /// </summary>
        public string ReferenceName { get; set; }

        /// <summary>
        /// The value pre edit
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// The value post edit
        /// </summary>
        public string NewValue { get; set; }
    }

}