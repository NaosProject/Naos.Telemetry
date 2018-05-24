// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawQueueItem.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;

    /// <summary>
    /// Item to be processed on the queue of telemetry.
    /// </summary>
    public class RawQueueItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawQueueItem"/> class.
        /// </summary>
        /// <param name="id">ID of the item.</param>
        /// <param name="sampledUtc">Sampled date time in UTC.</param>
        /// <param name="telemetryObjectJson">JSON representation of the telemetry object.</param>
        /// <param name="telemetryObjectTypeDescriptionJson">JSON representation of the telemetry object's type description.</param>
        /// <param name="logItemKindJson">JSON representation of the log item kind.</param>
        /// <param name="logItemContextJson">JSON representation of the log item context.</param>
        /// <param name="logItemCorrelationsJson">JSON representation of the log item correlations.</param>
        public RawQueueItem(
            Guid id,
            DateTime sampledUtc,
            string telemetryObjectJson,
            string telemetryObjectTypeDescriptionJson,
            string logItemKindJson,
            string logItemContextJson,
            string logItemCorrelationsJson)
        {
            this.Id = id;
            this.SampledUtc = sampledUtc;
            this.TelemetryObjectJson = telemetryObjectJson;
            this.TelemetryObjectTypeDescriptionJson = telemetryObjectTypeDescriptionJson;
            this.LogItemKindJson = logItemKindJson;
            this.LogItemContextJson = logItemContextJson;
            this.LogItemCorrelationsJson = logItemCorrelationsJson;
        }

        /// <summary>
        /// Gets the ID of the item.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the sampled date time in UTC.
        /// </summary>
        public DateTime SampledUtc { get; private set; }

        /// <summary>
        /// Gets the JSON representation of the telemetry object.
        /// </summary>
        public string TelemetryObjectJson { get; private set; }

        /// <summary>
        /// Gets the JSON representation of the telemetry object's type description.
        /// </summary>
        public string TelemetryObjectTypeDescriptionJson { get; private set; }

        /// <summary>
        /// Gets the JSON representation of the log item kind.
        /// </summary>
        public string LogItemKindJson { get; private set; }

        /// <summary>
        /// Gets the JSON representation of the log item context.
        /// </summary>
        public string LogItemContextJson { get; private set; }

        /// <summary>
        /// Gets the JSON representation of the log item correlations.
        /// </summary>
        public string LogItemCorrelationsJson { get; private set; }
    }
}