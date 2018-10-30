using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDevOpsEventsProcessor.Helpers;

namespace AzureDevOpsEventsProcessor.Tests.ScriptLoader
{
    [TestClass]
    public class ScriptSelectionTests
    {
        [TestMethod]
        public void Can_set_a_default_script()
        {
            // arrange
            var eventName = "DslScriptService.EventTypes.BuildEvent";
            var subscriptionId = "27646e0e-b520-4d2b-9411-bba7524947cd";
            var scriptName = "default.py";
            var useSubScriptionId = false;

            // act
            var actual = FolderHelper.GetScriptName(
                eventName,
                subscriptionId,
                scriptName,
                useSubScriptionId);
            // assert
            Assert.AreEqual(scriptName, actual);
        }
        
        [TestMethod]
        public void Uses_eventname_if_no_default_script()
        {
            // arrange
            var eventName = "DslScriptService.EventTypes.BuildEvent";
            var subscriptionId = "27646e0e-b520-4d2b-9411-bba7524947cd";
            var scriptName = "";
            var useSubScriptionId = false;

            // act
            var actual = FolderHelper.GetScriptName(
                eventName,
                subscriptionId,
                scriptName,
                useSubScriptionId);

            // assert
            Assert.AreEqual(eventName + ".py", actual);
        }

        [TestMethod]
        public void Uses_subscriptionID_if_no_default_script()
        {
            // arrange
            var eventName = "DslScriptService.EventTypes.BuildEvent";
            var subscriptionId = "27646e0e-b520-4d2b-9411-bba7524947cd";
            var scriptName = "";
            var useSubScriptionId = true;

            // act
            var actual = FolderHelper.GetScriptName(
                eventName,
                subscriptionId,
                scriptName,
                useSubScriptionId);

            // assert
            Assert.AreEqual(subscriptionId + ".py", actual);
        }
    }
}
