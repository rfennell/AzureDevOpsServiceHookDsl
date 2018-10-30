using NUnit.Framework;
using AzureDevOpsEventsProcessor.Interfaces;
using AzureDevOpsEventsProcessor.Providers;

namespace AzureDevOpsEventsProcessor.Tests.Dsl
{
    [TestFixture]
    public class DslEmailProcessingTests
    {
   
        [Test]
        public void Can_use_Dsl_to_send_an_email()
        {

            // arrange
            // redirect the console
            var consoleOut = Helpers.Logging.RedirectConsoleOut();

            var emailProvider = new Moq.Mock<IEmailProvider>();
            var azureDevOpsProvider = new Moq.Mock<IAzureDevOpsProvider>();
            var eventDataProvider = new Moq.Mock<IEventDataProvider>();

            var engine = new AzureDevOpsEventsProcessor.Dsl.DslProcessor();

            // act
            engine.RunScript(@"TestDataFiles\Scripts\email\sendemail.py", azureDevOpsProvider.Object, emailProvider.Object, eventDataProvider.Object);

            // assert
            emailProvider.Verify( e => e.SendEmailAlert("fred@test.com", "The subject", "The body of the email"));
        }


     
    }
}
