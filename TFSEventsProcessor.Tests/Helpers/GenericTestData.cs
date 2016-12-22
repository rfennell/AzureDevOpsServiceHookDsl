using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSEventsProcessor.Tests.Helpers
{
    internal class GenericTestData
    {
        /// <summary>
        /// The alerts as they are internally stoed
        /// </summary>
        /// <returns></returns>
        internal static List<WorkItemChangedAlertDetails> DummyAlerts()
        {
            return new List<WorkItemChangedAlertDetails>() {
                new WorkItemChangedAlertDetails() { ReferenceName="r1", OldValue="A", NewValue = "B"},
                new WorkItemChangedAlertDetails() { ReferenceName="r2", OldValue="C", NewValue = "D"}};

        }

        /// <summary>
        /// The alerts as they are internally stoed
        /// </summary>
        /// <param name="from">From UID</param>
        /// <param name="to">To UI</param>
        /// <returns></returns>
        internal static List<WorkItemChangedAlertDetails> AssignedToChangedAlerts(string from, string to)
        {
            return new List<WorkItemChangedAlertDetails>() {
                new WorkItemChangedAlertDetails() { ReferenceName="System.AssignedTo", OldValue=from, NewValue = to}
            };
        }
    }
}
