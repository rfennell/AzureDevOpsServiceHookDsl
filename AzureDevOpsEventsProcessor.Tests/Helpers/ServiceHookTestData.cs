using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using AzureDevOpsEventsProcessor.Helpers;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AzureDevOpsEventsProcessor.Tests.Helpers
{
    static class ServiceHookTestData
    {

        /// <summary>
        /// The json we get from the Azure DevOps server call
        /// </summary>
        /// <returns></returns>
        internal static JObject GetEventJson(string eventName)
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath($".\\TestDataFiles\\RestData\\Alerts\\{eventName}.json")));
        }

    }
}
