using System;
using NUnit.Framework;
using TFSEventsProcessor.Tests.Helpers;

namespace TFSEventsProcessor.Tests.Dsl
{
    using Interfaces;
    using TFSEventsProcessor.Providers;

    [TestFixture]
    public class DslWithEventJsonMessagesTests
    {
        [Test]
        public void Can_run_a_script_using_work_item_updated_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.WorkItemUpdatedServiceHook();
            
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();

            var sut = new Controllers.WebHookController(
                emailProvider.Object, 
                tfsProvider.Object,
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py", @".\dsl");

            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();
            
            // act
            sut.Post(alertMessage);

            // assert
            Assert.AreEqual("A JSON workitem.updated event 5" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_checkin_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.CheckInServiceHook();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var sut = new Controllers.WebHookController(
                emailProvider.Object,
                tfsProvider.Object,
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py",
                @".\dsl");

            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            // act
            sut.Post(alertMessage);

            // assert
            Assert.AreEqual("A JSON tfvc.checkin event 18" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_push_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.PushServiceHook();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();
            var sut = new Controllers.WebHookController(
                emailProvider.Object,
                tfsProvider.Object,
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py",
                @".\dsl");

            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            // act
            sut.Post(alertMessage);

            // assert
            Assert.AreEqual("A JSON git.push event on repo 3c4e22ee-6148-45a3-913b-454009dac91d with id 73" + Environment.NewLine, consoleOut.ToString());

        }
        [Test]
        public void Can_run_a_script_using_work_item_created_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.WorkItemCreatedServiceHook();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();

            var sut = new Controllers.WebHookController(
                emailProvider.Object,
                tfsProvider.Object,
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py", @".\dsl");

            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            // act
            sut.Post(alertMessage);

            // assert
            Assert.AreEqual("A JSON workitem.created event 5" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_build_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.BuildCompletesServiceHook();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();

            var sut = new Controllers.WebHookController(
                emailProvider.Object, 
                tfsProvider.Object,
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py", 
                @".\dsl");

            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            // act
            sut.Post(alertMessage);

            // assert
            Assert.AreEqual("A JSON build.complete event 2" + Environment.NewLine, consoleOut.ToString());

        }

        
    }
}
