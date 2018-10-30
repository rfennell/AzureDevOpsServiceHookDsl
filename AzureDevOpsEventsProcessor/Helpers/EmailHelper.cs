//------------------------------------------------------------------------------------------------- 
// <copyright file="EmailHelper.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureDevOpsEventsProcessor.Interfaces;
using AzureDevOpsEventsProcessor.Providers;

namespace AzureDevOpsEventsProcessor.Helpers
{
    /// <summary>
    /// Helper methods related to email processing
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// Expands the fields in the template using the lookup provider
        /// </summary>
        /// <param name="fieldsLookup">A Azure DevOps lookup provider</param>
        /// <param name="template">The HTML like template</param>
        /// <returns>An expanded message</returns>
        internal static string ExpandTemplateFields(IFieldLookupProvider fieldsLookup, string template)
        {
            // process the work item fields
            string returnValue = template;

            // Process work item fields
            int endIndex = -1;
            int startIndex = template.IndexOf(EmailTemplate.FieldTag, endIndex + 1);
            while (startIndex > -1)
            {
                endIndex = template.IndexOf(EmailTemplate.FieldTag, startIndex + 1);
                var fieldName = template.Substring(startIndex + 2, endIndex - startIndex - 2);
                returnValue = returnValue.Replace(string.Format("{1}{0}{1}", fieldName, EmailTemplate.FieldTag), fieldsLookup.LookupWorkItemFieldValue(fieldName));
                startIndex = template.IndexOf(EmailTemplate.FieldTag, endIndex + 1);
            }

            // Process alert fields
            endIndex = -1;
            startIndex = template.IndexOf(EmailTemplate.AlertTag, endIndex + 1);
            while (startIndex > -1)
            {
                endIndex = template.IndexOf(EmailTemplate.AlertTag, startIndex + 1);
                var fieldName = template.Substring(startIndex + 2, endIndex - startIndex - 2);
                returnValue = returnValue.Replace(string.Format("{1}{0}{1}", fieldName, EmailTemplate.AlertTag), fieldsLookup.LookupAlertFieldValue(fieldName));
                startIndex = template.IndexOf(EmailTemplate.AlertTag, endIndex + 1);
            }

            return returnValue;
        }
    }
}