
namespace TFSEventsProcessor.Tests
{
    using NUnit.Framework;
    using TFSEventsProcessor.Tests.Helpers;
    using TFSEventsProcessor.Helpers;


    [TestFixture]
    public class JsonData_CheckinAlertsTests
    {
       [Test]
        public void Can_read_the_changed_files_from_alert_json_block()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.GetEventJson("tfvc.checkin");
            var dataProvider = new Providers.JsonDataProvider(alertMessage);

            // act
           var actual = dataProvider.GetCheckInDetails();

            // assert
           Assert.AreEqual(18, actual.Changeset);
           Assert.AreEqual(@"Dropping in new Java sample", actual.Comment);
           Assert.AreEqual("Normal Paulk checked in changeset 18: Dropping in new Java sample", actual.Summary);
           Assert.AreEqual(@"Normal Paulk", actual.Committer);
     
        }
 
    
    }
}
