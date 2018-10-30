//------------------------------------------------------------------------------------------------- 
// <copyright file="IDslLibrary.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using AzureDevOpsEventsProcessor.Interfaces;

namespace AzureDevOpsEventsProcessor.Dsl
{
    /// <summary>
    /// The definition of the DSL libray 
    /// </summary>
    public interface IDslLibrary
    {
        /// <summary>
        /// Instance of TFS provider
        /// </summary>
        IAzureDevOpsProvider TfsProvider { get; set; }

        /// <summary>
        /// Instance of the Email provider
        /// </summary>
        IEmailProvider EmailProvider { get; set; }

        /// <summary>
        /// The raw event data
        /// </summary>
        IEventDataProvider EventData { get; set; }
        
        /// <summary>
        /// The folder the script was load from
        /// </summary>
        string ScriptFolder { get; set; }


    }
}
