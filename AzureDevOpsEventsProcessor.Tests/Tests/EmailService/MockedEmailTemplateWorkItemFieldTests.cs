
namespace AzureDevOpsEventsProcessor.Tests
{
    using NUnit.Framework;
    using AzureDevOpsEventsProcessor.Helpers;
    using AzureDevOpsEventsProcessor.Tests.Helpers;

    /// <summary>
    /// Test for the parsing of template files
    /// </summary>
    [TestFixture]
    public class MockedEmailTemplateWorkItemFieldTests
    {
        public MockedEmailTemplateWorkItemFieldTests()
        {
            
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

       [Test]
        public void Process_template_with_two_WI_fields()
        {
            // arrange
            var template = "The title is @@System.Title@@ for the @@System.ID@@";
            var fakeProvider = TestProviderFactory.MockedLookupProvider().Object;
           
            // act
            var actual = EmailHelper.ExpandTemplateFields(fakeProvider, template); 

            // Assert
            Assert.AreEqual("The title is System_Title for the System_ID", actual);
        }

       [Test]
        public void Process_template_with_one_WI_field_in_middle()
        {
            // arrange
            var template = "The title is @@System.Title@@ for message";
            var fakeProvider = TestProviderFactory.MockedLookupProvider().Object;

            // act
            var actual = EmailHelper.ExpandTemplateFields(fakeProvider, template); 

            // Assert
            Assert.AreEqual("The title is System_Title for message", actual);
        }

      
       [Test]
        public void Process_template_with_one_WI_field_at_start()
        {
            // arrange
            var template = "@@System.Title@@ for message";
            var fakeProvider = TestProviderFactory.MockedLookupProvider().Object;

            // act
            var actual = EmailHelper.ExpandTemplateFields(fakeProvider, template); 

            // Assert
            Assert.AreEqual("System_Title for message", actual);
        }

       [Test]
        public void Process_template_with_one_WI_field_at_end()
        {
            // arrange
            var template = "The message is @@System.Title@@";
            var fakeProvider = TestProviderFactory.MockedLookupProvider().Object;

            // act
            var actual = EmailHelper.ExpandTemplateFields(fakeProvider, template); 

            // Assert
            Assert.AreEqual("The message is System_Title", actual);
        }

      
    }
}
