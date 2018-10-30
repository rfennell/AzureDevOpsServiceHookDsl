using System;
using System.Collections.Generic;
using System.IO;
using IronPython.Runtime;
using Microsoft.Scripting;
using NUnit.Framework;

namespace AzureDevOpsEventsProcessor.Tests.Dsl
{
    using AzureDevOpsEventsProcessor.Providers;
    using NLog;
    using Interfaces;

    [TestFixture]
    public class DslScriptingTests
    {
        [Test]
        public void A_null_dsl_script_throws_exception()
        {
            // arrange
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act // assert
           Assert.Throws<ArgumentNullException>(() => engine.RunScript(null, null, null, null));
        }

        [Test]
        public void A_missing_AzureDevOps_provider_throws_exception()
        {
            // arrange
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();
            // act // assert
            Assert.Throws<ArgumentNullException>(() => engine.RunScript(@"testDataFiles\scripts\scripting\badscript1.py", null, null,null));
        }

        [Test]
        public void A_missing_Email_provider_throws_exception()
        {
            // arrange
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act // assert
            Assert.Throws<ArgumentNullException>(() => engine.RunScript(@"testDataFiles\scripts\scripting\badscript1.py", azureDevOpsProvider.Object, null,null));
        }

        [Test]
        public void A_missing_EventData_provider_throws_exception()
        {
            // arrange
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act // assert
            Assert.Throws<ArgumentNullException>(() => engine.RunScript(@"testDataFiles\scripts\scripting\badscript1.py", azureDevOpsProvider.Object, emailProvider.Object, null));
        }

        [Test]
        public void A_script_with_syntax_error_throws_exception()
        {
            // arrange
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            // act
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act // assert
            Assert.Throws<SyntaxErrorException>(() => engine.RunScript(@"testDataFiles\scripts\scripting\badscript1.py", azureDevOpsProvider.Object, emailProvider.Object,eventDataProvider.Object));
        }

        [Test]
        public void A_script_with_an_invalid_DSL_call_throws_exception()
        {
            // arrange
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act // assert
            Assert.Throws<UnboundNameException>(() => engine.RunScript(@"testDataFiles\scripts\scripting\badscript2.py", azureDevOpsProvider.Object, emailProvider.Object,eventDataProvider.Object));
        }


        [Test]
        public void Can_pass_argument_to_named_script()
        {
            // arrange
            // redirect the console
            var consoleOut = new StringWriter();
            Console.SetOut(consoleOut);

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();


            var args = new Dictionary<string, object>
            {
                { "Arguments", new[] { "foo", "bar", "biz baz" } },
            };
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @".\testDataFiles\scripts\scripting\args.py", 
                args, 
                azureDevOpsProvider.Object, 
                emailProvider.Object,
                eventDataProvider.Object);

            // assert

            Assert.AreEqual("['foo', 'bar', 'biz baz']" + Environment.NewLine, consoleOut.ToString());

        }

        [Test]
        public void Can_pass_argument_to_script_when_scripts_found_by_folder()
        {
            // arrange
            // redirect the console
            var consoleOut = new StringWriter();
            Console.SetOut(consoleOut);

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            var args = new Dictionary<string, object>
            {
                { "Arguments", new[] { "foo", "bar", "biz baz" } },
            };
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @".\dsl", 
                @".\testDataFiles\scripts\scripting", 
                "args.py", 
                args, 
                azureDevOpsProvider.Object, 
                emailProvider.Object, 
                eventDataProvider.Object);

            // assert

            Assert.AreEqual("['foo', 'bar', 'biz baz']" + Environment.NewLine, consoleOut.ToString());

        }


        [Test]
        public void Can_use_methods_in_two_dsl_libraries_script()
        {
            // arrange
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();


            // create a memory logger
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Info);
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"testDataFiles\scripts\scripting\twolibraries.py",
                azureDevOpsProvider.Object,
                emailProvider.Object,
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(4, memLogger.Logs.Count);
            // memLogger.Logs[0] is the log message from the runscript method
            // memLogger.Logs[1] is the log message from the runscript method
            // memLogger.Logs[2] is the log message from the runscript method
            Assert.AreEqual("INFO | AzureDevOpsEventsProcessor.Dsl.DslLibrary | When you add 1 and 2 you get 3", memLogger.Logs[3]);

            emailProvider.Verify(e => e.SendEmailAlert("fred@test.com", "The subject", "When you add 1 and 2 you get 3"));
        }


        [Test]
        public void Error_logged_if_no_Dsl_library_folder_found()
        {
            // arrange
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            // create a memory logger
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Info);
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"c:\dummy", 
                @"testDataFiles\scripts\scripting\args.py", 
                string.Empty, 
                null, 
                azureDevOpsProvider.Object, 
                emailProvider.Object, 
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(1, memLogger.Logs.Count);
            Assert.AreEqual(@"ERROR | AzureDevOpsEventsProcessor.Dsl.DslProcessor | DslProcessor: DslProcessor cannot find DSL folder c:\dummy", memLogger.Logs[0]);

        }

        [Test]
        public void Error_logged_if_no_Dsl_library_in_dsl_folder()
        {
            // arrange
            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            // create a memory logger
            var memLogger = Helpers.Logging.CreateMemoryTargetLogger(LogLevel.Info);
            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(
                @"testDataFiles\scripts\scripting", 
                @"testDataFiles\scripts\scripting\args.py", 
                string.Empty, 
                null, 
                azureDevOpsProvider.Object, 
                emailProvider.Object, 
                eventDataProvider.Object);

            // assert
            Assert.AreEqual(3, memLogger.Logs.Count);
            // memLogger.Logs[0] is the log message from the runscript method
            // memLogger.Logs[1] is the log message from the runscript method
            Assert.IsTrue(memLogger.Logs[2].StartsWith(@"ERROR | AzureDevOpsEventsProcessor.Dsl.DslProcessor | DslProcessor: DslProcessor cannot find DSL libraries in folder "));

        }

    }
}
