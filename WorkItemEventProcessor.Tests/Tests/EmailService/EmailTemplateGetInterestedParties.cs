
namespace TFSEventsProcessor.Tests
{
    using NUnit.Framework;
    using TFSEventsProcessor.Helpers;
    using TFSEventsProcessor.Tests.Helpers;

    /// <summary>
    /// Test for the parsing of template files
    /// </summary>
    [TestFixture]
    public class EmailTemplateGetInterestedParties
    {
        public EmailTemplateGetInterestedParties()
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
        public void Can_get_email_address_from_simple_form()
        {
            // arrange
            var provider = new TFSEventsProcessor.Providers.TfsFieldLookupProvider();

            // act
            var actual = provider.BuildEmailAddress("bm-richard.fennell@outlook.com", string.Empty);

            // Assert
            Assert.AreEqual("bm-richard.fennell@outlook.com", actual);
        }

        [Test]
        public void Can_get_email_address_from_long_form()
        {
            // arrange
            var provider = new TFSEventsProcessor.Providers.TfsFieldLookupProvider();

            // act
            var actual = provider.BuildEmailAddress("Richard Fennell(Work) < bm - richard.fennell@outlook.com >",string.Empty);

            // Assert
            Assert.AreEqual("bm-richard.fennell@outlook.com", actual);
        }


        [Test]
        public void Can_get_email_address_from_name_only()
        {
            // arrange
            var provider = new TFSEventsProcessor.Providers.TfsFieldLookupProvider();

            // act
            var actual = provider.BuildEmailAddress("richard.fennell", "outlook.com");

            // Assert
            Assert.AreEqual("richard.fennell@outlook.com", actual);
        }

    }
}
