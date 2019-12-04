// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventTelemetrySource.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Stores some context of the source of a <see cref="EventTelemetry" />
    /// (e.g. the name of machine and process within which the item was generated).
    /// </summary>
    public class EventTelemetrySource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetrySource"/> class.
        /// </summary>
        /// <param name="machineName">Optional name of the machine that generated the log-item.</param>
        /// <param name="processName">Optional name of the process that generated the log-item.</param>
        /// <param name="processFileVersion">Optional file version of the process that generated the log-item.</param>
        /// <param name="callingMethod">Optional calling method.</param>
        /// <param name="callingType">Optional description of the calling type.</param>
        /// <param name="stackTrace">Optional stack trace.</param>
        public EventTelemetrySource(
            string machineName,
            string processName = null,
            string processFileVersion = null,
            string callingMethod = null,
            TypeRepresentation callingType = null,
            string stackTrace = null)
        {
            this.MachineName = machineName ?? throw new ArgumentNullException(nameof(machineName));
            this.ProcessName = processName;
            this.ProcessFileVersion = processFileVersion;
            this.CallingMethod = callingMethod;
            this.CallingType = callingType;
            this.StackTrace = stackTrace;
        }

        /// <summary>
        /// Gets the name of the machine that generated the log-item.
        /// </summary>
        public string MachineName { get; private set; }

        /// <summary>
        /// Gets the name of the process that generated the log-item.
        /// </summary>
        public string ProcessName { get; private set; }

        /// <summary>
        /// Gets the file version of the process that generated the log-item.
        /// </summary>
        public string ProcessFileVersion { get; private set; }

        /// <summary>
        /// Gets the calling method.
        /// </summary>
        public string CallingMethod { get; private set; }

        /// <summary>
        /// Gets a description of the calling type.
        /// </summary>
        public TypeRepresentation CallingType { get; private set; }

        /// <summary>
        /// Gets the stack trace.
        /// </summary>
        public string StackTrace { get; private set; }
    }
}
