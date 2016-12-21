//------------------------------------------------------------------------------------------------- 
// <copyright file="ConsoleRedirectStreamToNlog.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using NLog;
using System;
using System.IO;

namespace TFSEventsProcessor.Helpers
{

    /// <summary>
    /// Class to enable the redirection of logging 
    /// </summary>
    public class ConsoleRedirectStreamToNlog : Stream
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Stream inner;

        private readonly LogLevel logLevel;

        private readonly string label;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logLevel">Level to log</param>
        /// <param name="label">Label for all messages</param>
        public ConsoleRedirectStreamToNlog(LogLevel logLevel, string label)
        {
            this.inner = new MemoryStream();
            this.logLevel = logLevel;
            this.label = label;
        }

        public override bool CanRead
        {
            get
            {
                return false ;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return 0;
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            // Nlog deals with this;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.logger.Log(
                logLevel,
                string.Format("{0}: {1}", this.label, System.Text.Encoding.UTF8.GetString(buffer, 0, count)));
        }

        ///
    }
}
