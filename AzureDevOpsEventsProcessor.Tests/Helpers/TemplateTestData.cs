using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using AzureDevOpsEventsProcessor.Helpers;

namespace AzureDevOpsEventsProcessor.Tests.Helpers
{
    class TemplateTestData
    {

        /// <summary>
        /// A simple template for test
        /// </summary>
        /// <returns>The template</returns>
        internal static string EmailTemplate()
        {
            return @"<subject>The Work Item @@System.ID@@ has been edited</subject>
              <body>
              The title is @@System.Title@@ &lt;u&gt;for&lt;/u&gt; the @@System.ID@@ by ##System.ChangedBy##
              </body>
              <wifieldheader>&lt;br /&gt;&lt;strong&gt;&lt;u&gt;All wi fields in the alert&lt;/strong&gt;&lt;/u&gt;</wifieldheader>
              <alertfieldheader>&lt;br /&gt;&lt;strong&gt;&lt;u&gt;All changed fields in the alert&lt;/strong&gt;&lt;/u&gt;</alertfieldheader>";
        }

    }
}
