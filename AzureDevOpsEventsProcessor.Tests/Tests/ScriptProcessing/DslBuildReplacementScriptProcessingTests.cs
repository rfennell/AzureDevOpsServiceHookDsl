using System;
using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.QualityTools.Testing.Fakes;

namespace AzureDevOpsEventsProcessor.Tests.Dsl
{
    using IronPython.Hosting;


    using Moq;

    using NLog;

    using AzureDevOpsEventsProcessor.Providers;
    using Interfaces;
    using Helpers;

    [TestFixture]
    public class DslBuildReplacementScriptProcessingTests
    {

        [Test]
        public void Can_use_Dsl_to_set_build_retension_by_result()
        {
            // arrange
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Debug);

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();
            
            azureDevOpsProvider.Setup(t => t.GetBuildDetails(It.IsAny<int>())).Returns(RestTestData.GetBuildDetails());

            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();
            var args = new Dictionary<string, object>
            {
                { "Arguments", new[] { "build.complete", "391" } },
            };

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\AzureDevOps\alerts\setbuildretensionbyresult.py",
                args,
                azureDevOpsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object
                );

            // assert
            azureDevOpsProvider.Verify(t => t.SetBuildRetension(It.IsAny<Uri>(), true));
            emailProvider.Verify(e => e.SendEmailAlert("richard@blackmarble.co.uk", "20150716.2 retension set to True", "'20150716.2' retension set to 'True' as result was 'succeeded'"));

            Assert.AreEqual(4, memLogger.Logs.Count);
            // memLogger.Logs[0] is the log message from the runscript method
            // memLogger.Logs[1] is the log message from the runscript method
            // memLogger.Logs[2] is the log message from the runscript method
            Assert.AreEqual("INFO | AzureDevOpsEventsProcessor.Dsl.DslLibrary | '20150716.2' retension set to 'True' as result was 'succeeded'", memLogger.Logs[3]);


        }

        [Test]
        public void Can_use_Dsl_to_unset_build_retension_by_quality()
        {
            // arrange
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Debug);

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            var build = RestTestData.GetBuildDetails();
            build["result"] = "failed";

            azureDevOpsProvider.Setup(t => t.GetBuildDetails(It.IsAny<int>())).Returns(build);

            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();
            var args = new Dictionary<string, object>
            {
                { "Arguments", new[] { "build.complete", "391" } },
            };

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\AzureDevOps\alerts\setbuildretensionbyresult.py",
                args,
                azureDevOpsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object
                );

            // assert
            azureDevOpsProvider.Verify(t => t.SetBuildRetension(It.IsAny<Uri>(), false));
            emailProvider.Verify(e => e.SendEmailAlert("richard@blackmarble.co.uk", "20150716.2 retension set to False", "'20150716.2' retension set to 'False' as result was 'failed'"));

            Assert.AreEqual(4, memLogger.Logs.Count);
            // memLogger.Logs[0] is the log message from the runscript method
            // memLogger.Logs[1] is the log message from the runscript method
            // memLogger.Logs[2] is the log message from the runscript method
            Assert.AreEqual("INFO | AzureDevOpsEventsProcessor.Dsl.DslLibrary | '20150716.2' retension set to 'False' as result was 'failed'", memLogger.Logs[3]);



        }


        [Test]
        public void Get_error_log_if_pass_invalid_eventype_provided()
        {

            // arrange
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Error);

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();


            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();
            var args = new Dictionary<string, object>
            {
                { "Arguments", new[] { "Invalidstring", "ignored" } },
            };

            // act
            engine.RunScript(
                @"TestDataFiles\Scripts\AzureDevOps\alerts\setbuildretensionbyresult.py", 
                args, 
                azureDevOpsProvider.Object, 
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(2, memLogger.Logs.Count);
            Assert.AreEqual("ERROR | AzureDevOpsEventsProcessor.Dsl.DslLibrary | Was not expecting to get here", memLogger.Logs[0]);
            Assert.AreEqual("ERROR | AzureDevOpsEventsProcessor.Dsl.DslLibrary | List: [Invalidstring] [ignored] ", memLogger.Logs[1]);

        }
    }
}
