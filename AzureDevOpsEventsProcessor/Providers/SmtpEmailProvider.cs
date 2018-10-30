//------------------------------------------------------------------------------------------------- 
// <copyright file="SmtpEmailProvider.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Mail;
using NLog;
using AzureDevOpsEventsProcessor.Providers;
using AzureDevOpsEventsProcessor.Helpers;
using AzureDevOpsEventsProcessor.Interfaces;

namespace AzureDevOpsEventsProcessor.Providers
{
    /// <summary>
    /// A class that does the sending of emails
    /// </summary>
    public class SmtpEmailProvider : IEmailProvider
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The target SMTP server
        /// </summary>
        private readonly string smptServer;

        /// <summary>
        /// The address emails should appear to come from
        /// </summary>
        private readonly string fromAddress;

        /// <summary>
        /// The domain to append to user names to get an email address
        /// </summary>
        private readonly string domain;
        
        /// <summary>
        /// The account to authenticate with SMTP server
        /// </summary>
        private readonly string username;

        /// <summary>
        /// The account to authenticate with SMTP server
        /// </summary>
        private readonly string password;

        /// <summary>
        /// SMTP Port
        /// </summary>
        private readonly int port = 25;

        
        /// <summary>
        /// Create an instance of the EmailHelper
        /// </summary>
        /// <param name="smptServer">Target SMTP server</param>
        /// <param name="port">SMTP port</param>
        /// <param name="fromAddress">The address emails should appear to come from</param>
        /// <param name="domain">The domain to append to user names to get an email address</param>
        /// <param name="username">SMTP user name</param>
        /// <param name="password">SMTP password</param>
        public SmtpEmailProvider(string smptServer, int port, string fromAddress, string domain, string username, string password)
        {
            this.smptServer = smptServer;
            this.port = port;
            this.fromAddress = fromAddress;
            this.domain = domain;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Sends a simple email
        /// </summary>
        /// <param name="to">Who the email goes to, a , separated list</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        public void SendEmailAlert(
            string to,
            string subject,
            string body)
        {

            using (var msg = new MailMessage())
            {
                msg.To.Add(to);
                msg.From = new MailAddress(this.fromAddress);
                msg.Subject = subject;
                msg.IsBodyHtml = true;
                msg.Body = body;
                using (var client = new SmtpClient(this.smptServer, this.port))
                {
                    if (string.IsNullOrEmpty(this.username))
                    {
                        client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    } else
                    {
                        client.Credentials = new System.Net.NetworkCredential(this.username, this.password);
                    }
                    client.Send(msg);
                }

                logger.Info(
                    string.Format(
                        "AzureDevOpsEventsProcessor: '{0}' email sent to '{1}' from '{2}' with subject '{3}' using server '{4}'",
                        msg.Subject,
                        msg.To[0].Address, 
                    msg.From.Address, 
                    msg.Subject, 
                    this.smptServer));

            }

        }

        /// <summary>
        /// Sends the email build from a work item
        /// </summary>
        /// <param name="fieldsLookupProvider">The provider that extracts the data from Azure DevOps</param>
        /// <param name="templatePath">The path to the HTML (like) template file</param>
        /// <param name="includeAllWorkItemFields">If true all available work item fields are appended to the email body</param>
        /// <param name="includeAllAlertFields">If true all available alert fields are appended to the email body</param>
        public void SendEmailAlert(
            IFieldLookupProvider fieldsLookupProvider,
            string templatePath,
            bool includeAllWorkItemFields,
            bool includeAllAlertFields)
        {
            if (fieldsLookupProvider == null)
            {
                throw new ArgumentNullException("fieldsLookupProvider", "The fieldsLookupProvider cannot be null");
            }

            var template = EmailTemplate.LoadTemplate(templatePath, fieldsLookupProvider.GetWorkItemType);

            if (template != null)
            {
                using (var msg = new MailMessage())
                {
                    var addresses = fieldsLookupProvider.GetInterestedEmailAddresses(this.domain);
                    if (string.IsNullOrEmpty(addresses) == false)
                    {
                        msg.To.Add(addresses);
                        msg.From = new MailAddress(this.fromAddress);
                        msg.Subject = EmailHelper.ExpandTemplateFields(fieldsLookupProvider, template.Title);
                        msg.IsBodyHtml = true;
                        msg.Body = EmailHelper.ExpandTemplateFields(fieldsLookupProvider, template.Body);

                        if (includeAllWorkItemFields == true)
                        {
                            msg.Body += fieldsLookupProvider.GetAllWorkItemFields(template.WiFieldHeader);
                        }

                        if (includeAllAlertFields == true)
                        {
                            msg.Body += fieldsLookupProvider.GetAllAlertFields(template.AlertFieldHeader);
                        }

                        using (var client = new SmtpClient(this.smptServer, this.port))
                        {
                            if (string.IsNullOrEmpty(this.username))
                            {
                                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                            }
                            else
                            {
                                client.Credentials = new System.Net.NetworkCredential(this.username, this.password);
                            }
                            client.Send(msg);
                        }

                        logger.Info(string.Format("AzureDevOpsEventsProcessor: '{0}' email sent to {1}", msg.Subject, msg.To[0].Address));
                    }
                    else
                    {
                        logger.Info(string.Format("AzureDevOpsEventsProcessor: '{0}' No email sent as no interested parties", msg.Subject));
                    }
                }
            }

        }
             
    }
}