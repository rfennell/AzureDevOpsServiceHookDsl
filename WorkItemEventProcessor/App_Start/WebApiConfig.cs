//------------------------------------------------------------------------------------------------- 
// <copyright file="WebApiConfig.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System.Web.Http;

namespace TFSEventsProcessor
{
    /// <summary>
    /// Holds the web api config
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the config
        /// </summary>
        /// <param name="config">The config set</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
