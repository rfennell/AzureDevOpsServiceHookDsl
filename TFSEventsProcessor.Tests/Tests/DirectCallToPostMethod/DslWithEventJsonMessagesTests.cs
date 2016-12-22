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
        public void Can_run_a_script_using_work_item_commented_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.commented");


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
            Assert.AreEqual("Got a known workitem.commented event type with id 5" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_pull_request_created_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("git.pullrequest.created");


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
            Assert.AreEqual("Got a known git.pullrequest.created event type on repo 4bc14d40-c903-45e2-872e-0462c7748079 with id 1" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_pull_request_merged_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("git.pullrequest.merged");


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
            Assert.AreEqual("Got a known git.pullrequest.merged event type on repo 4bc14d40-c903-45e2-872e-0462c7748079 with id 1" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_pull_request_updated_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("git.pullrequest.updated");


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
            Assert.AreEqual("Got a known git.pullrequest.updated event type on repo 4bc14d40-c903-45e2-872e-0462c7748079 with id 1" + Environment.NewLine, consoleOut.ToString());

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
        public void Can_run_a_script_using_message_posted_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("message.posted");

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
            Assert.AreEqual("Got a known message.posted event type with id 1" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_release_approval_complete_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("ms.vss-release.deployment-approval-completed-event");

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
            Assert.AreEqual("Got a known ms.vss-release.deployment-approval-completed-event event type with id 1" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_release_approval_pending_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("ms.vss-release.deployment-approval-pending-event");

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
            Assert.AreEqual("Got a known ms.vss-release.deployment-approval-pending-event event type with id 1" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_release_completed_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("ms.vss-release.deployment-completed-event");

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
            Assert.AreEqual("Got a known ms.vss-release.deployment-completed-event event type with id 1" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_release_started_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("ms.vss-release.deployment-started-event");

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
            Assert.AreEqual("Got a known ms.vss-release.deployment-started-event event type with id 5" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_release_abandoned_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("ms.vss-release.release-abandoned-event");

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
            Assert.AreEqual("Got a known ms.vss-release.release-abandoned-event event type with id 4" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_release_created_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("ms.vss-release.release-created-event");

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
            Assert.AreEqual("Got a known ms.vss-release.release-created-event event type with id 4" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_work_item_restored_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.restored");

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
            Assert.AreEqual("Got a known workitem.restored event type with id 5" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_run_a_script_using_work_item_deleted_data()
        {
            // arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.deleted");

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
            Assert.AreEqual("Got a known workitem.deleted event type with id 5" + Environment.NewLine, consoleOut.ToString());

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
