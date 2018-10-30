
namespace AzureDevOpsEventsProcessor.Tests
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using AzureDevOpsEventsProcessor.Tests.Helpers;
    using AzureDevOpsEventsProcessor.Providers;

    /// <summary>
    /// These test focus on the XML alerts as it deamed not a good investment in time to 
    /// try to fake out the whole of TFS, hence passing in nulls for TFS work item
    /// We would need to Fake Shims or Typemock to do this
    /// </summary>
    [TestFixture]
    public class GenericData_WorkItemChangedTests
    {
       [Test]
        public void Can_get_an_alert_change_field()
        {
            // arrange
            var alerts = GenericTestData.DummyAlerts();

            var provider = new  TfsFieldLookupProvider(null, alerts, "user1", false);

            //// act
            var actual = provider.LookupAlertFieldValue("r1");

            //// assert
            Assert.AreEqual("B", actual);
        }

       [Test]
        public void Can_get_the_changing_user_from_the_alerts_data()
        {
            //// arrange
            var alerts = GenericTestData.DummyAlerts();

            var provider = new TfsFieldLookupProvider(null, alerts, "user1", false);

            // act
            var actual = provider.LookupAlertFieldValue("System.ChangedBy");

            // assert
            Assert.AreEqual("user1", actual);
        }

       [Test]
        public void Cannot_get_an_invalid_field_from_the_alerts_data()
        {
            // arrange
            var alerts = GenericTestData.DummyAlerts();

            var provider = new TfsFieldLookupProvider(null, alerts, "user1", true);

            // act
            var actual = provider.LookupAlertFieldValue("NOT.REAL");

            // assert
            Assert.AreEqual("ERROR: [##NOT.REAL##]", actual);
        }

       [Test]
        public void Invalid_field_can_be_returned_as_empty_strings()
        {
            // arrange
            var alerts = GenericTestData.DummyAlerts();

            var provider = new TfsFieldLookupProvider(null, alerts, "user1", false);

            // act
            var actual = provider.LookupAlertFieldValue("NOT.REAL");

            // assert
            Assert.AreEqual(string.Empty, actual);
        }




        [Test]
        public void Integer_field_should_return_empty_string_if_null()
        {
            // arrange
            var alerts = new List<WorkItemChangedAlertDetails>() {
                new WorkItemChangedAlertDetails() { ReferenceName="r1", OldValue=null, NewValue = null}};

            var provider = new TfsFieldLookupProvider(null, alerts, "user1", false);

            // act
            var actual = provider.LookupAlertFieldValue("r1");

            // assert
            Assert.AreEqual(string.Empty, actual);


        }



       [Test]
        public void User1_changes_details_WI_assigned_User2_Mail_sent_to_User2 ()
        {
            // arrange
            var provider = TestProviderFactory.MockedTfsFieldLookupProvider("user2").Object;
            provider.SetChangedBy("user1");
            provider.SetTestAlertItems(GenericTestData.DummyAlerts());
   
            // act
            var actual = provider.GetInterestedEmailAddresses("test.com");

            // assert
            Assert.AreEqual("user2@test.com", actual);

        }

       [Test]
        public void User_changes_details_of_WI_assigned_to_self_no_mail_sent()
        {
            // arrange
            var provider = TestProviderFactory.MockedTfsFieldLookupProvider("user1").Object;
            provider.SetChangedBy("user1");
            provider.SetTestAlertItems (GenericTestData.DummyAlerts());
         
            // act
            var actual = provider.GetInterestedEmailAddresses("test.com");

            // assert
            Assert.AreEqual(string.Empty, actual);
        }

       [Test]
        public void User1_reassigns_WI_from_self_to_User2_mail_sent_to_user2()
        {
            // arrange
            var provider = TestProviderFactory.MockedTfsFieldLookupProvider("user2").Object;
            provider.SetChangedBy("user1");
            provider.SetTestAlertItems(GenericTestData.AssignedToChangedAlerts("user1","user2"));

            // act
            var actual = provider.GetInterestedEmailAddresses("test.com");

            // assert
            Assert.AreEqual("user2@test.com", actual);
        }

       [Test]
        public void User1_reassigns_WI_from_User2_to_self_mail_sent_to_user2()
        {
            // arrange
            var provider = TestProviderFactory.MockedTfsFieldLookupProvider("user1").Object;
            provider.SetChangedBy("user1");
            provider.SetTestAlertItems(GenericTestData.AssignedToChangedAlerts("user2", "user1"));

            // act
            var actual = provider.GetInterestedEmailAddresses("test.com");

            // assert
            Assert.AreEqual("user2@test.com", actual);
        }

       [Test]
        public void User1_reassigns_WI_User2_to_User3_mail_sent_to_both()
        {
            // arrange
            var provider = TestProviderFactory.MockedTfsFieldLookupProvider("user3").Object;
            provider.SetChangedBy("user1");
            provider.SetTestAlertItems(GenericTestData.AssignedToChangedAlerts("user2", "user3"));

            // act
            var actual = provider.GetInterestedEmailAddresses("test.com");

            // assert
            Assert.AreEqual("user3@test.com,user2@test.com", actual);
        }
    }
}
