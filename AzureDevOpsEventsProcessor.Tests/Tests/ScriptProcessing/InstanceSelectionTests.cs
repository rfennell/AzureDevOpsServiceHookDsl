using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDevOpsEventsProcessor.Helpers;

namespace AzureDevOpsEventsProcessor.Tests.ScriptLoader
{
    [TestClass]
    public class InstanceSelectionTests
    {
        [TestMethod]
        public void Can_intstance_from_old_form_url()
        {
            // arrange
            Uri uri = new Uri("https://richardfennell.visualstudio.com/_apis/git/repositories/3c4e22ee-6148-45a3-913b-454009dac91d/commits/50e062ba3f13715a83ca4bb43dc54ef19630ae31");

            // act
            var actual = ConfigHelper.GetInstanceName(uri);
            
            // assert
            Assert.AreEqual("richardfennell", actual);
        }

        [TestMethod]
        public void Can_intstance_from_new_form_url()
        {
            // arrange
            Uri uri = new Uri("https://dev.azure.com/richardfennell/_apis/git/repositories/3c4e22ee-6148-45a3-913b-454009dac91d/commits/50e062ba3f13715a83ca4bb43dc54ef19630ae31");

            // act
            var actual = ConfigHelper.GetInstanceName(uri);

            // assert
            Assert.AreEqual("richardfennell", actual);
        }

        [TestMethod]
        public void Can_intstance_from_any_other_form_url()
        {
            // arrange
            Uri uri = new Uri("https://server.mydomain.com/richardfennell/_apis/git/repositories/3c4e22ee-6148-45a3-913b-454009dac91d/commits/50e062ba3f13715a83ca4bb43dc54ef19630ae31");

            // act
            var actual = ConfigHelper.GetInstanceName(uri);

            // assert
            Assert.AreEqual("server.mydomain.com", actual);
        }
    }
}
