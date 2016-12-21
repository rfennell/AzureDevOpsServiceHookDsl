//------------------------------------------------------------------------------------------------- 
// <copyright file="RouteConfig.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System.Web.Mvc;
using System.Web.Routing;

namespace TFSEventsProcessor
{
    /// <summary>
    /// Holds the set of routes
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Rwegister the routes
        /// </summary>
        /// <param name="routes">root set</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
