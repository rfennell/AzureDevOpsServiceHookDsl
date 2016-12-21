//------------------------------------------------------------------------------------------------- 
// <copyright file="CheckInAlertDetails.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace TFSEventsProcessor.AlertDetails
{
    /// <summary>
    /// The details of the checkin stored in the XML alert
    /// </summary>
    public class CheckInAlertDetails
    {
        /// <summary>
        /// Files added
        /// </summary>
        private List<string> filesAdded = new List<string>();

        /// <summary>
        /// Files deleted
        /// </summary>
        private List<string> filesDeleted = new List<string>();

        /// <summary>
        /// Files edited
        /// </summary>
        private List<string> filesEdited = new List<string>();

        /// <summary>
        /// The text that describes the event as a whole
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The person who commited the changeset
        /// </summary>
        public string Committer { get; set; }

        /// <summary>
        /// The changeset ID
        /// </summary>
        public int Changeset { get; set; }

        /// <summary>
        /// The parent team project
        /// </summary>
        public string TeamProject { get; set; }

        /// <summary>
        /// The comment for the check in
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The files added in the changeset
        /// </summary>
        public List<string> FilesAdded
        {
            get

            {
                return this.filesAdded;
            }
        }

        /// <summary>
        /// The files edited in the changeset
        /// </summary>
        public List<string> FilesEdited
        {
            get
            {
                return this.filesEdited;
            }
        }

        /// <summary>
        /// The files deelted in the changeset
        /// </summary>
        public List<string> FilesDeleted
        {
            get
            {
                return this.filesDeleted;
            }
        }
    }
}