//------------------------------------------------------------------------------------------------- 
// <copyright file="JsonDataProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TFSEventsProcessor.Tests.ServiceHookAlerts.GenericMethods
{
    [TestClass]
    public class AlertGenericDataTests
    {
        [TestMethod]
        public void Can_get_event_type()
        {
            // arrange
            var provider = new TFSEventsProcessor.Providers.JsonDataProvider(Helpers.ServiceHookTestData.GetEventJson("workitem.updated"));

            // act
            var actual = provider.GetEventType();

            // assert
            Assert.AreEqual("workitem.updated", actual);
        }

        [TestMethod]
        public void Can_get_subscriptionID()
        {
            // arrange
            var provider = new TFSEventsProcessor.Providers.JsonDataProvider(Helpers.ServiceHookTestData.GetEventJson("workitem.updated"));

            // act
            var actual = provider.GetSubsriptionID();

            // assert
            Assert.AreEqual("27646e0e-b520-4d2b-9411-bba7524947cd", actual);
        }
    }
}
