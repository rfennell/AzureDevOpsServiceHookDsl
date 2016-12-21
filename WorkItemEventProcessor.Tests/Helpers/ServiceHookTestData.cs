using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using TFSEventsProcessor.Helpers;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TFSEventsProcessor.Tests.Helpers
{
    static class ServiceHookTestData
    {

        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject CheckInServiceHook()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\Alerts\tfvc.checkin.json")));
        }


        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject WorkItemUpdatedServiceHook()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\Alerts\workitem.updated.json")));
        }



        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject WorkItemCreatedServiceHook()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\Alerts\workitem.created.json")));
        }

        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject PushServiceHook()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\Alerts\git.push.json")));
        }

        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject BuildCompletesServiceHook()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\Alerts\build.complete.json")));
        }
        
    }
}
