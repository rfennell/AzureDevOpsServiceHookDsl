//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkItemAlertDetails.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFSEventsProcessor.AlertDetails
{
    /// <summary>
    /// Container for the alert information stored in XML
    /// </summary>
    public class WorkItemAlertDetails
    {
        public int Id { get; set; }
        public string ChangedBy { get; set; }

        public List<WorkItemChangedAlertDetails> ChangedAlertFields { get; set; }
    }

}