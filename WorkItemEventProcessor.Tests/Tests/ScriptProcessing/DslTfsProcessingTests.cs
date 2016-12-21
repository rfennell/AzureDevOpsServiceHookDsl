using System;
using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.QualityTools.Testing.Fakes;

namespace TFSEventsProcessor.Tests.Dsl
{
    using System.Linq;
    using Moq;

    using NLog;

    using TFSEventsProcessor.Providers;
    using Interfaces;
    using Newtonsoft.Json.Linq;
    using Helpers;
    using Newtonsoft.Json;

    [TestFixture]
    public class DslTfsProcessingTests
    {

        [Test]
        public void Can_use_Dsl_to_retrieve_a_work_item()
        {


            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.GetWorkItem(297)).Returns(RestTestData.GetSingleWorkItemByID());
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\api\loadwi.py",
                tfsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(
                "Work item '309' has the title 'Customer can sign in using their Microsoft Account'" + Environment.NewLine,
                consoleOut.ToString());

        }

        [Test]
        public void Can_use_Dsl_to_update_a_build_tag()
        {
            // arrange
            var consoleOut = Helpers.Logging.RedirectConsoleOut();
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.GetBuildDetails(It.IsAny<int>())).Returns(RestTestData.GetBuildDetails());

            // act
            engine.RunScript(@"TestDataFiles\Scripts\tfs\api\tagbuild.py", tfsProvider.Object, emailProvider.Object, eventDataProvider.Object);

            // assert
            tfsProvider.Verify(t => t.AddBuildTag(It.IsAny<Uri>(), "The Tag"));
            Assert.AreEqual(
                "Set tag for build for '123'" + Environment.NewLine,
                consoleOut.ToString());

        }

        [Test]
        public void Can_use_Dsl_to_retrieve_a_changeset()
        {
            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.GetChangesetDetails(7)).Returns(RestTestData.GetChangeSetDetails());
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\api\loadchangeset.py",
                tfsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(
                "Changeset '7' has the comment 'Added 2 files to SampleProject' and contains 2 files" + Environment.NewLine,
                consoleOut.ToString());
        }

        [Test]
        public void Can_use_Dsl_to_retrieve_a_push()
        {
            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.GetPushDetails("3c4e22ee-6148-45a3-913b-454009dac91d", 73)).Returns(RestTestData.GetPushDetails());
            tfsProvider.Setup(t => t.GetCommitDetails(It.IsAny<Uri>())).Returns(RestTestData.GetCommitDetails());
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\api\loadpush.py",
                tfsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(
                "Push '73' contains 2 commits\r\nCommit be67f8871a4d2c75f13a51c1d3c30ac0d74d4ef4\r\nCommit be67f8871a4d2c75f13a51c1d3c30ac0d74d4ef4\r\n",
                consoleOut.ToString());

        }
    
        [Test]
        public void Can_use_Dsl_to_create_a_work_item()
        {
            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.CreateWorkItem(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny < Dictionary<string, object>>())).Returns(RestTestData.CreateWorkItem());
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\api\createwi.py",
                tfsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            tfsProvider.Verify(t => t.CreateWorkItem(
                  "SampleProject",
                  "Bug",
                  new Dictionary<string, object>()
                {
                        {"System.Title", "The Title"},
                        {"Microsoft.VSTS.Scheduling.Effort", 2}
                }));

            Assert.AreEqual(
             "Work item '299' has been created with the title 'JavaScript implementation for Microsoft Account'" + Environment.NewLine,
             consoleOut.ToString());
        }

        [Test]
        public void Can_use_Dsl_to_update_a_work_item()
        {
            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.CreateWorkItem(
                "tp",
                "Bug",
                new Dictionary<string, object>()
                {
                        {"Title", "The Title"},
                        {"Estimate", 2}
                })).Returns(RestTestData.CreateWorkItem());
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\api\updatewi.py",
                tfsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            tfsProvider.Verify(t => t.UpdateWorkItem(It.IsAny<JObject>()));
        }


        [Test]
        public void Can_pass_realistic_build_arguments_to_script()
        {
            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();


            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            var args = new Dictionary<string, object>
            {
                { "Arguments", new[] { "build.complete", "123" } },
            };
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py",
                args,
                tfsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert

            Assert.AreEqual("A JSON build.complete event 123" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_use_Dsl_to_get_build_details()
        {
            // arrange
            var consoleOut = Helpers.Logging.RedirectConsoleOut();
         
            var emailProvider = new Moq.Mock<IEmailProvider>();
            
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            tfsProvider.Setup(t => t.GetBuildDetails(It.IsAny<int>())).Returns(RestTestData.GetBuildDetails());
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            var eventDataProvider = new Moq.Mock<IEventDataProvider>();


            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\api\loadbuild.py",
                tfsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            Assert.AreEqual("Build 'vstfs:///Build/Build/391' has the result 'succeeded'" + Environment.NewLine, consoleOut.ToString());
        }

        [Test]
        public void Can_use_Dsl_to_set_build_retension()
        {

            // arrange
            var consoleOut = Helpers.Logging.RedirectConsoleOut();
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();


            // act
            engine.RunScript(@"TestDataFiles\Scripts\tfs\api\keepbuild.py", tfsProvider.Object, emailProvider.Object, eventDataProvider.Object);

            // assert
            tfsProvider.Verify(t => t.SetBuildRetension(It.IsAny<Uri>(), true));
            Assert.AreEqual(
                "Set build retension for 'http://dummy/_api/build'" + Environment.NewLine,
                consoleOut.ToString());

        }

        [Test]
        public void Can_use_Dsl_to_retrieve_a_parent_work_item()
        {


            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.GetWorkItem(297)).Returns(RestTestData.GetSingleWorkItemByID());
            tfsProvider.Setup(t => t.GetParentWorkItem(It.IsAny<JObject>())).Returns(RestTestData.GetSingleWorkItemByID());
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(@"TestDataFiles\Scripts\tfs\api\loadparentwi.py", tfsProvider.Object, emailProvider.Object, eventDataProvider.Object);

            // assert
            Assert.AreEqual(
                "Work item '309' has a parent '309' with the title 'Customer can sign in using their Microsoft Account'" + Environment.NewLine,
                consoleOut.ToString());


        }

        [Test]
        public void Can_use_Dsl_to_find_if_no_parent_work_item()
        {


            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.GetWorkItem(297)).Returns(RestTestData.GetSingleWorkItemByID());
            // don't need to assign a value for the parent call as will return null by default
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\tfs\api\loadparentwi.py", 
                tfsProvider.Object, 
                emailProvider.Object, 
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(
                "Work item '309' has no parent" + Environment.NewLine,
                consoleOut.ToString());

        }


        [Test]
        public void Can_use_Dsl_to_retrieve_a_child_work_items()
        {
            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            tfsProvider.Setup(t => t.GetWorkItem(It.IsAny<int>())).Returns(RestTestData.GetSingleWorkItemWithReleationshipsByID());
            tfsProvider.Setup(t => t.GetChildWorkItems(It.IsAny<JObject>())).Returns(RestTestData.GetSetOfWorkItemsByID(false));
            var engine = new TFSEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(@"TestDataFiles\Scripts\tfs\api\loadchildwi.py", tfsProvider.Object, emailProvider.Object, eventDataProvider.Object);

            // assert
            Assert.AreEqual(
                "Work item '23' has a child '297' with the title 'Customer can sign in using their Microsoft Account'" + Environment.NewLine +
                "Work item '23' has a child '299' with the title 'JavaScript implementation for Microsoft Account'" + Environment.NewLine +
                "Work item '23' has a child '300' with the title 'Unit Testing for MSA login'" + Environment.NewLine,
                consoleOut.ToString());
        }


    }
}
