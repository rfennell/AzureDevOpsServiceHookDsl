//------------------------------------------------------------------------------------------------- 
// <copyright file="MessagePostDetails.cs" company="Black Marble">
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
    public class MessagePostDetails
    {
        /// <summary>
        /// The post room ID
        /// </summary>
        public int PostRoomId { get; set; }

        /// <summary>
        /// The text that describes the message
        /// </summary>
        public string Content { get; set; }

    }

}