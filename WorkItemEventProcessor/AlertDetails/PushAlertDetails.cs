//------------------------------------------------------------------------------------------------- 
// <copyright file="PushAlertDetails.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace TFSEventsProcessor.AlertDetails
{
    /// <summary>
    /// The details of the checkin stored in the XML alert
    /// </summary>
    public class PushAlertDetails
    {
        /// <summary>
        /// The push ID
        /// </summary>
        public int PushId { get; set; }

        /// <summary>
        /// The repo
        /// </summary>
        public string Repo { get; set; }

    }
}