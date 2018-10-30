using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using AzureDevOpsEventsProcessor.Helpers;
using AzureDevOpsEventsProcessor.Providers;
using AzureDevOpsEventsProcessor.Interfaces;

namespace AzureDevOpsEventsProcessor.Tests.Helpers
{
    class TestProviderFactory
    {
        /// <summary>
        /// Creates a mock instance using Moq
        /// </summary>
        /// <returns>The fake instance</returns>
        internal static Mock<IFieldLookupProvider> MockedLookupProvider()
        {
            var mock = new Mock<IFieldLookupProvider>();
            mock.Setup(f => f.LookupWorkItemFieldValue(It.IsAny<string>())).Returns((string input) => input.Replace(".", "_"));
            mock.Setup(f => f.LookupAlertFieldValue(It.IsAny<string>())).Returns((string input) => input.Replace(".", "|"));
                        
            return mock;

           
        }

        /// <summary>
        /// Creates a real instance but use Moq to intercept calls to virtual methods
        /// Allows who the call is owned by to bet set
        /// </summary>
        /// <param name="assignedTo">Who the call is assigned to</param>
        /// <returns>The fake instance</returns>
        internal static Mock<AzureDevOpsFieldLookupProvider> MockedAzureDevOpsFieldLookupProvider(string assignedTo)
        {
            var mock = new Moq.Mock<AzureDevOpsFieldLookupProvider>();
            mock.Setup(f => f.LookupWorkItemFieldValue("System.AssignedTo")).Returns(assignedTo);
            mock.Setup(f => f.GetUserIdFromDisplayName(assignedTo)).Returns(assignedTo);
            return mock;
        }

        /// <summary>
        /// Creates a mock instance using Moq
        /// </summary>
        /// <returns>The fake instance</returns>
        internal static Mock<IEmailProvider> MockedEmailProvider()
        {
            var mock = new Mock<IEmailProvider>();
       
            return mock;
        }

        /// <summary>
        /// Creates a mock instance using Moq
        /// </summary>
        /// <returns>The fake instance</returns>
        internal static Mock<IAzureDevOpsProvider> MockedazureDevOpsProvider()
        {
            var mock = new Mock<IAzureDevOpsProvider>();
            return mock;
        }

    }
}
