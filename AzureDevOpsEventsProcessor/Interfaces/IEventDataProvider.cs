//------------------------------------------------------------------------------------------------- 
// <copyright file="IEventDataProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using AzureDevOpsEventsProcessor.AlertDetails;

namespace AzureDevOpsEventsProcessor.Interfaces
{
    /// <summary>
    /// Interface for all raw event decoders
    /// </summary>
    public interface IEventDataProvider
    {
        /// <summary>
        /// Get the source URL
        /// </summary>
        /// <returns></returns>
        Uri GetServerUrl();

        /// <summary>
        /// get work item details
        /// </summary>
        /// <returns></returns>
        WorkItemAlertDetails GetWorkItemDetails();
        
        /// <summary>
        /// Get build details
        /// </summary>
        /// <returns></returns>
        BuildAlertDetails GetBuildDetails();

        /// <summary>
        /// Get push details
        /// </summary>
        /// <returns></returns>
        PushAlertDetails GetPushDetails();

        /// <summary>
        /// Get chekcin details
        /// </summary>
        /// <returns></returns>
        CheckInAlertDetails GetCheckInDetails();


    }
}