//------------------------------------------------------------------------------------------------- 
// <copyright file="FilterConfig.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System.Web.Mvc;

namespace AzureDevOpsEventsProcessor
{
    /// <summary>
    /// Holds the filter config
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Register the filters
        /// </summary>
        /// <param name="filters">Set of filters</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
