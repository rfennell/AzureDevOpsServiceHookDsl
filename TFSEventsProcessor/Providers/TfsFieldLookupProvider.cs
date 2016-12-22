//------------------------------------------------------------------------------------------------- 
// <copyright file="TfsFieldLookupProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using TFSEventsProcessor.Providers;
using TFSEventsProcessor.Helpers;
using System.DirectoryServices.AccountManagement;
using TFSEventsProcessor.Interfaces;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace TFSEventsProcessor.Providers
{
    /// <summary>
    /// Expands a work item using a TFS server
    /// </summary>
    public class TfsFieldLookupProvider : IFieldLookupProvider
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The current work item
        /// </summary>
        private readonly JObject workItem;

        /// <summary>
        /// Who triggered the alert due their change (this is hidden in XML alert data not the work item
        /// </summary>
        private string changedBy;

        /// <summary>
        /// The changes from the xml alert
        /// </summary>
        private List<WorkItemChangedAlertDetails> alertItems;

        /// <summary>
        /// The field to check to see who made the change
        /// </summary>
        private readonly string changedByReference = "System.ChangedBy";

        /// <summary>
        /// The field to check to see who the wi is assigned to 
        /// </summary>
        private readonly string assignedToReference = "System.AssignedTo";


        /// <summary>
        /// If try the name of any missing field is shown, if false a blank is shown
        /// </summary>
        private readonly bool showMissingFieldNames;


        /// <summary>
        /// A constructor that is provided to allow mocking of the 
        /// virtual methods. This allows us to use MOQ without the need
        /// to mock out the whole of TFS
        /// </summary>
        public TfsFieldLookupProvider()
        {
        }

        /// <summary>
        /// Expands a TFS work item
        /// </summary>
        /// <param name="workItem">The work item</param>
        /// <param name="alertItems">List of alert changes</param>
        /// <param name="changedBy">Who edited the work item</param>
        /// <param name="showMissingFieldNames">If true the name of any missing field is printed</param>
        public TfsFieldLookupProvider(JObject workItem, List<WorkItemChangedAlertDetails> alertItems, string changedBy, bool showMissingFieldNames)
        {
            this.workItem = workItem;
            this.alertItems = alertItems;
            this.changedBy = changedBy;
            this.showMissingFieldNames = showMissingFieldNames;
        }

        /// <summary>
        /// gets a field value
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The current value</returns>
        public virtual string LookupWorkItemFieldValue(string fieldName)
        {
            string value;
            if (fieldName.Equals("System.Id"))
            {
                logger.Debug($"Special handling of ID as it is not present in the field list");
                value = this.workItem["id"].ToString();
            } else
            {
                try
                {
                    logger.Debug(string.Format("LookupWorkItemFieldValue for {0}", fieldName));

                    value = this.workItem["fields"][fieldName].ToString();
                }
                catch (NullReferenceException)
                {
                    // needs as non-set numeric fields return null
                    value = string.Empty;
                }
                catch (System.ApplicationException)
                {
                    // should just be checking for Microsoft.TeamFoundation.WorkItemTracking.Client.FieldDefinitionNotExistException
                    // but gives compile error must be missing an extension reference 
                    if (this.showMissingFieldNames == true)
                    {
                        value = string.Format("ERROR: [{1}{0}{1}]", fieldName, EmailTemplate.FieldTag);
                    }
                    else
                    {
                        value = string.Empty;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Returns all fields in the work item
        /// </summary>
        /// <param name="title">The title for the format block</param>
        /// <returns>HTML formated section</returns>
        public string GetAllWorkItemFields(string title)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(title);
            sb.Append("<table border=1>");
            sb.Append("<tr><th>Field</th><th>Current Value</th></tr>");

#pragma warning disable S3217 // "Explicit" conversions of "foreach" loops should not be used
            foreach (JProperty field in this.workItem["fields"])
#pragma warning restore S3217 // "Explicit" conversions of "foreach" loops should not be used
            {
                logger.Debug(string.Format("GetAllWorkItemFields  for {0}", field.Name));
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", field.Name, field.Value);
            }
            sb.Append("</table>");
            return sb.ToString();
        }


        /// <summary>
        /// Gets a field from the alert data
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Needed else we seem to swallow the exception")]
        public string LookupAlertFieldValue(string fieldName)
        {

            string value;
            try
            {
                if (fieldName == null)
                {
                    throw new ArgumentException("Missing fieldname");
                }
                if (fieldName.Equals(this.changedByReference))
                {
                    // fix up as this is stored in the alert section but never changes
                    // if you get it was you would expect from the WI is always the service account
                    logger.Info(string.Format("TFSEventsProcessor: WI changed by:{0}", this.changedBy));
                    value = this.changedBy;
                }
                else
                {
                    try
                    {
                        logger.Info(string.Format("LookupAlertFieldValue for {0}", fieldName));

                        value = this.alertItems.Single(i => i.ReferenceName == fieldName).NewValue;
                        if (value == null)
                        {
                            // needs as non-set numeric fields return null
                            value = string.Empty;
                        }
                    }
                    catch (System.InvalidOperationException)
                    {
                        throw new System.ApplicationException("Cannot find the fieldname");
                    }
                }
            }
            catch (System.ApplicationException)
            {
                // should just be checking for Microsoft.TeamFoundation.WorkItemTracking.Client.FieldDefinitionNotExistException
                // but gives compile error must be missing an extension reference 
                if (this.showMissingFieldNames == true)
                {
                    value = string.Format("ERROR: [{1}{0}{1}]", fieldName, EmailTemplate.AlertTag);
                }
                else
                {
                    value = string.Empty;
                }
            }
            return value;
        }


        /// <summary>
        /// Gets all the alert information
        /// </summary>
        /// <param name="title">The title for the format block</param>
        /// <returns>HTML formated section</returns>
        public string GetAllAlertFields(string title)
        {

            var sb = new System.Text.StringBuilder();
            sb.Append(title);
            sb.Append("<table border=1>");
            sb.Append("<tr><th>Field</th><th>Old Value</th><th>New Value</th></tr>");
            foreach (var field in this.alertItems)
            {
                if (field.ReferenceName.Equals(this.changedByReference) == false)
                {
                    logger.Debug(string.Format("GetAllAlertFields for {0}", field.ReferenceName));
                    // we don't want to return the changeby field as this has a inconistant value, we have to do a manual fixup
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", field.ReferenceName, field.OldValue, field.NewValue);
                }
            }
            sb.Append("</table>");
            return sb.ToString();

        }

        /// <summary>
        /// Gets the work item type that triggered the alerts
        /// </summary>
        public string GetWorkItemType
        {
            get
            {
                return "this.workItem.Type.Name";
            }
        }

        /// <summary>
        /// Gets all the people who need to get an email due to the change
        /// </summary>
        /// <param name="domain">The domain to append to the names to make an email address</param>
        /// <returns>An comma separated list of email addresses</returns>
        public string GetInterestedEmailAddresses(string domain)
        {
            var list = new List<string>();
            var whoWasItAssignedToChanges = this.alertItems.SingleOrDefault(i => i.ReferenceName == this.assignedToReference);
            var currentOwner = this.LookupWorkItemFieldValue(this.assignedToReference);
            logger.Debug(string.Format("TFSEventsProcessor: WI owned by by:{0}", currentOwner));
            if (this.changedBy == currentOwner)
            {
                if (whoWasItAssignedToChanges == null)
                {
                    // User changes the details of a work item assigned to himself -> no mail sent
                }
                else
                {
                    list.Add(this.BuildEmailAddress(whoWasItAssignedToChanges.OldValue, domain));
                }
            }
            else
            {
                // the new owner needs to know
                if (string.IsNullOrEmpty(currentOwner) == false)
                {
                    list.Add(this.BuildEmailAddress(currentOwner, domain));
                }

                if (whoWasItAssignedToChanges == null)
                {
                    // user did not change the assigned field so no extra names to inform
                }
                else
                {
                    if (this.changedBy != whoWasItAssignedToChanges.OldValue)
                    {
                        list.Add(this.BuildEmailAddress(whoWasItAssignedToChanges.OldValue, domain));
                    }
                }

            }
            return string.Join(",", list.ToArray());
        }

        /// <summary>
        /// Format the email address
        /// </summary>
        /// <param name="name">User name</param>
        /// <param name="domain">Domain name</param>
        /// <returns>Formated email address</returns>
        public string BuildEmailAddress(string name, string domain)
        {
            try
            {
                // use the constructor as a parser
#pragma warning disable S1848 // Objects should not be created to be dropped immediately without being used
                new System.Net.Mail.MailAddress(name);
#pragma warning restore S1848 // Objects should not be created to be dropped immediately without being used
                return name;
            }
            catch
            {
                var strippedName = name.Substring(name.IndexOf("<")+1).Replace(">",string.Empty).Replace(" ", string.Empty) ;
                try
                {
                    // use the constructor as a parser
#pragma warning disable S1848 // Objects should not be created to be dropped immediately without being used
                    new System.Net.Mail.MailAddress(strippedName);
#pragma warning restore S1848 // Objects should not be created to be dropped immediately without being used
                    return strippedName;
                }
                catch
                {
                    // on tfs 2012 we get the displayname as opposed to uid, so we to check
                    var tmpName = GetUserIdFromDisplayName(name);
                    if (string.IsNullOrEmpty(tmpName))
                    {
                        // could not do a lookup so use the original
                        tmpName = name;
                    }
                    var builtName = string.Format("{0}@{1}", tmpName, domain);
                    try
                    {
#pragma warning disable S1848 // Objects should not be created to be dropped immediately without being used
                        new System.Net.Mail.MailAddress(builtName);
#pragma warning restore S1848 // Objects should not be created to be dropped immediately without being used
                        return builtName;
                    }
                    catch
                    {
                        return string.Empty;
                    } 
                }
            }
        }

        /// <summary>
        /// Gets the user ID for a displayname
        /// </summary>
        /// <param name="displayName">The display name</param>
        /// <returns>The AD account</returns>
        public virtual string GetUserIdFromDisplayName(string displayName)
        {
            string name = string.Empty;

            try
            {
                // set up domain context
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
                {
                    // find user by display name
                    UserPrincipal user = UserPrincipal.FindByIdentity(ctx, displayName);

                    if (user != null)
                    {
                        name = user.SamAccountName;
                        // or maybe you need user.UserPrincipalName;
                    }
                }
            }
            catch (PrincipalServerDownException)
            {
                logger.Warn(string.Format("TFSEventsProcessor: Principal Server Down so cannot return ID displayname {0}", displayName));
            }

            return name;

        }

        /// <summary>
        /// Allows us to insert a set of test items when moqing
        /// </summary>
        /// <param name="items">The items</param>
        internal void SetTestAlertItems(List<WorkItemChangedAlertDetails> items)
        {
            this.alertItems = items;
        }

        /// <summary>
        /// Allows us to insert name when moqing
        /// </summary>
        /// <param name="name">The items</param>
        internal void SetChangedBy(string name)
        {
            this.changedBy = name;
        }
    }
}