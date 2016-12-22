
namespace TFSEventsProcessor.Tests
{
    using NUnit.Framework;
    using TFSEventsProcessor.Tests.Helpers;
    using TFSEventsProcessor.Helpers;


    [TestFixture]
    public class JsonData_WorkitemChangedTests
    {
        [Test]
        public void Can_read_the_changed_fields_from_changed_alert_json_block()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.updated");
            var provider = new Providers.JsonDataProvider(alertMessage);

            // act
            var actual = provider.GetWorkItemDetails().ChangedAlertFields;

            // assert
            Assert.AreEqual(9, actual.Count);
            Assert.AreEqual("System.AuthorizedDate", actual[1].ReferenceName);
            Assert.AreEqual("15/07/2014 16:48:44", actual[1].OldValue);
            Assert.AreEqual("15/07/2014 17:42:44", actual[1].NewValue);

        }

        [Test]
        public void Can_read_the_changed_fields_from_created_alert_json_block()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.created");
            var provider = new Providers.JsonDataProvider(alertMessage);

            // act
            var actual = provider.GetWorkItemDetails().ChangedAlertFields;

            // assert
            Assert.AreEqual(13, actual.Count);
            Assert.AreEqual("System.TeamProject", actual[1].ReferenceName);
            Assert.AreEqual(string.Empty, actual[1].OldValue);
            Assert.AreEqual("FabrikamCloud", actual[1].NewValue);

        }

        [Test]
        public void Can_read_who_changed_the_workitem_from_alert_json_block()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.updated");
            var provider = new Providers.JsonDataProvider(alertMessage);

            // act
            var actual = provider.GetWorkItemDetails().ChangedBy;

            // assert
            Assert.AreEqual("Jamal Hartnett", actual);

        }

        [Test]
        public void Can_read_changed_fields_when_value_set_to_null()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.updated");
            var provider = new Providers.JsonDataProvider(alertMessage);


            // act
            var actual = provider.GetWorkItemDetails().ChangedAlertFields;

            // assert
            Assert.AreEqual(9, actual.Count);
            Assert.AreEqual("System.AssignedTo", actual[5].ReferenceName);
            Assert.AreEqual(string.Empty, actual[5].OldValue);
            Assert.AreEqual("Jamal Hartnet", actual[5].NewValue);

        }

        [Test]
        public void Can_get_the_workitem_id()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.updated");
            var provider = new Providers.JsonDataProvider(alertMessage);


            // act
            var actual = provider.GetWorkItemDetails().Id;

            // assert
            Assert.AreEqual(5, actual);
        }


        [Test]
        public void Can_get_the_base_url()
        {
            // Arrange
            var alertMessage = ServiceHookTestData.GetEventJson("workitem.updated");
            var provider = new Providers.JsonDataProvider(alertMessage);

            // act
            var actual = provider.GetServerUrl();

            // assert
            Assert.AreEqual("http://fabrikam-fiber-inc.visualstudio.com/DefaultCollection", actual.ToString());
        }


    }
}
