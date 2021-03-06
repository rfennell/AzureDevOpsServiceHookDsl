﻿namespace AzureDevOpsEventsProcessor.Tests.Dsl
{
    using System;
    using System.Collections.Generic;


    using NUnit.Framework;


    using Moq;

    using NLog;

    using AzureDevOpsEventsProcessor.Providers;
    using Interfaces;
    using Helpers;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class DslTfsProductionScriptTests
    {
             

        [Test]
        public void Can_use_Dsl_to_send_templated_email()
        {

            // arrange
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            azureDevOpsProvider.Setup(t => t.GetWorkItem(99)).Returns(RestTestData.GetSingleWorkItemByID());

            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            var args = new Dictionary<string, object> { { "Arguments", new[] { "WorkItemEvent", "99" } }, };

            // act
            engine.RunScript(
                @".\dsl",
                @"TestDataFiles\Scripts\AzureDevOps\alerts",
                "sendtemplatedemail.py",
                args,
                azureDevOpsProvider.Object,
                emailProvider.Object,
                new Providers.JsonDataProvider(ServiceHookTestData.GetEventJson("workitem.updated")));

            // assert
            emailProvider.Verify(
                e =>
                e.SendEmailAlert(
                    Moq.It.IsAny<IFieldLookupProvider>(),
                    System.IO.Path.Combine(engine.BasePath, @"TestDataFiles\Scripts\AzureDevOps\alerts\EmailTemplate.htm"),
                    true,
                    true));
        }

        [Test]
        public void Can_use_Dsl_to_increment_build_argument()
        {
            // arrange
            var emailProvider = new Moq.Mock<IEmailProvider>();

            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            azureDevOpsProvider.Setup(t => t.GetBuildDetails(It.IsAny<int>())).Returns(RestTestData.GetBuildDetails());
            azureDevOpsProvider.Setup(t => t.GetBuildArgument(It.IsAny<Uri>(), "MajorVersion")).Returns("1");
            azureDevOpsProvider.Setup(t => t.GetBuildArgument(It.IsAny<Uri>(), "MinorVersion")).Returns("6");
    
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor(true);

            var args = new Dictionary<string, object>
                           {
                               {
                                   "Arguments",
                                   new[] { "build.complete", "391" }
                               },
                           };

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\AzureDevOps\alerts\incrementbuildargument.py",
                args,
                azureDevOpsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert

            emailProvider.Verify(
                e =>
                e.SendEmailAlert(
                    "richard@blackmarble.co.uk",
                    "VsoBuildApi version incremented",
                    "'VsoBuildApi' version incremented to 1.7.x.x"));

        }

        [Test]
        public void Can_use_Dsl_to_update_parent_work_item_when_all_children_done()
        {
            // arrange
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Debug);

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            azureDevOpsProvider.Setup(t => t.GetWorkItem(It.IsAny<int>())).Returns(RestTestData.GetSingleWorkItemByID());
            azureDevOpsProvider.Setup(t => t.GetParentWorkItem(It.IsAny<JObject>())).Returns(RestTestData.GetSingleWorkItemByID());
            azureDevOpsProvider.Setup(t => t.GetChildWorkItems(It.IsAny<JObject>())).Returns(RestTestData.GetSetOfWorkItemsByID(true));

            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            var args = new Dictionary<string, object> { { "Arguments", new[] { "workitem.updated", "100" } } };

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\AzureDevOps\alerts\changeparentworkitemstate.py",
                args,
                azureDevOpsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            foreach (var line in memLogger.Logs)
            {
                Console.WriteLine(line);
            }

            azureDevOpsProvider.Verify(t => t.UpdateWorkItem(It.IsAny<JObject>()));
            emailProvider.Verify(
                e => e.SendEmailAlert(
                    "richard@blackmarble.co.uk",
                    "Work item '309' has been updated",
                    "Work item '309' has been set as 'Done' as all its child work items are done"));
        }


        [Test]
        public void Cannot_use_Dsl_to_update_parent_work_item_if_children_not_complete()
        {
            // arrange
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Debug);

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            azureDevOpsProvider.Setup(t => t.GetWorkItem(It.IsAny<int>())).Returns(RestTestData.GetSingleWorkItemByID());
            azureDevOpsProvider.Setup(t => t.GetParentWorkItem(It.IsAny<JObject>())).Returns(RestTestData.GetSingleWorkItemByID());
            azureDevOpsProvider.Setup(t => t.GetChildWorkItems(It.IsAny<JObject>())).Returns(RestTestData.GetSetOfWorkItemsByID(false));

            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            var args = new Dictionary<string, object> { { "Arguments", new[] { "WorkItemEvent", "100" } } };

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\AzureDevOps\alerts\changeparentworkitemstate.py",
                args,
                azureDevOpsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            foreach (var line in memLogger.Logs)
            {
                Console.WriteLine(line);
            }

            azureDevOpsProvider.Verify(t => t.UpdateWorkItem(It.IsAny<JObject>()), Times.Never());

        }
    }
}

