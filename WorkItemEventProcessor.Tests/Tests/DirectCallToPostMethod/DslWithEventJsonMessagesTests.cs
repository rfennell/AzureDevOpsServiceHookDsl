using System;
using NUnit.Framework;
using TFSEventsProcessor.Tests.Helpers;

namespace TFSEventsProcessor.Tests.Dsl
{
    using Interfaces;
    using System.Net;
    using TFSEventsProcessor.Providers;

    [TestFixture]
    public class DslWithEventJsonMessagesTests
    {
        [Test]
        public void Can_run_a_script_using_work_item_updated_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.updated");


            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();

            var sut = new Controllers.WebHookController(
                emailProvider.Object,
                tfsProvider.Object,
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py", @".\dsl");

            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            // act
            var actual = sut.Post(alertMessage);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
            Assert.AreEqual("Got a known workitem.updated event type with id 5" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_checkin_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("tfvc.checkin");

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
            var actual = sut.Post(alertMessage);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
            Assert.AreEqual("Got a known tfvc.checkin event type with id 18" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_push_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("git.push");

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
            var actual = sut.Post(alertMessage);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
            Assert.AreEqual("Got a known git.push event type on repo 3c4e22ee-6148-45a3-913b-454009dac91d with id 73" + Environment.NewLine, consoleOut.ToString());

        }
        [Test]
        public void Can_run_a_script_using_work_item_created_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.created");

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var tfsProvider = new Moq.Mock<ITfsProvider>();

            var sut = new Controllers.WebHookController(
                emailProvider.Object,
                tfsProvider.Object,
                @"TestDataFiles\Scripts\tfs\alerts\fullscript.py", @".\dsl");

            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            // act
            var actual = sut.Post(alertMessage);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
            Assert.AreEqual("Got a known workitem.created event type with id 5" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Cannot_run_a_script_using_an_unknown_event_type()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("dummy");

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
            var actual = sut.Post(alertMessage);

            // assert
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual("", consoleOut.ToString());

        }


    }
}
