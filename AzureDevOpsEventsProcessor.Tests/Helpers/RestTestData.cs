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
    static class RestTestData
    {

        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject GetSingleWorkItemWithReleationshipsByID()
        {
            var rawObject = JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetWorkItemByIdWithRelationshops.json")));
            // this test files contains set of work items, we just need one 
            return rawObject;
       }

        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject GetSingleWorkItemByID()
        {
            var rawObject = JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetWorkItemById.json")));
            // this test files contains set of work items, we just need one 
            return rawObject;
        }

        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject GetSingleWorkItemByIndexFromSet(int index)
        {
            var rawObject = JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetWorkItemSetById.json")));
            // this test files contains set of work items, we just need one 
            return rawObject["value"][index].Value<JObject>();
        }


        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static JObject CreateWorkItem()
        {
            var rawObject = JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\CreateWorkItem.json")));
            // this test files contains set of work items, we just need one 
            return rawObject;
        }

        internal static JObject GetChangeSetDetails()
        {
            var rawObject = JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetChangesetDetails.json")));
            // this test files contains set of work items, we just need one 
            return rawObject;
        }

        /// <summary>
        /// The json we get from the TFS server call
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<JObject> GetSetOfWorkItemsByID(bool allMarkedAsDone)
        {
            JObject rawObject;
            if (allMarkedAsDone == true)
            {
                rawObject = JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetWorkItemSetByIdAllDone.json")));
            } else
            {
                rawObject = JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetWorkItemSetById.json")));
            }
            // we do a bit of processing on the JObject to make it easier to consume in Python
            var returnObject = new List<JObject>();
            foreach (var wi in rawObject["value"])
            {
                returnObject.Add(wi.ToObject<JObject>());
            }
            return returnObject;
        }

        internal static JObject GetPushDetails()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetPushDetails.json")));
        }

        internal static JObject GetCommitDetails()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetCommitDetails.json")));
        }


        /// <summary>

        internal static JObject GetBuildDetails()
        {
            return JObject.Parse(File.ReadAllText(FolderHelper.GetRootedPath(@".\TestDataFiles\RestData\API\GetBuildDetails.json")));
        }
    }
}
