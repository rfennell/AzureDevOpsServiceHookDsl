

namespace TFSEventsProcessor.Tests
{
    using NUnit.Framework;
    using TFSEventsProcessor.Helpers;
    using TFSEventsProcessor.Tests.Helpers;

    [TestFixture]
    public class JsonData_BuildAlertsTests
    {
        [Test]
        public void Can_read_the_build_fields_from_alert_json_block()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.BuildCompletesServiceHook();
            var dataProvider = new Providers.JsonDataProvider(alertMessage);

            // act
            var actual = dataProvider.GetBuildDetails();

            // assert
            Assert.AreEqual("vstfs:///Build/Build/2", actual.BuildUri.ToString());
            Assert.AreEqual(2,actual.Id);
            Assert.AreEqual("ConsumerAddressModule_20150407.1", actual.Summary);
            Assert.AreEqual("succeeded", actual.Status);
            Assert.AreEqual("https://fabrikam-fiber-inc.visualstudio.com/DefaultCollection/71777fbc-1cf2-4bd1-9540-128c1c71f766/_apis/build/Builds/2", actual.BuildUrl.ToString());

        }

    }
}
