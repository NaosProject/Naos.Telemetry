﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawQueueItem.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
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
        /// <param name="telemetryObjectDescribedSerializationJson">JSON representation of the telemetry object.</param>
        /// <param name="logItemKindJson">JSON representation of the log item kind.</param>
        /// <param name="logItemContextJson">JSON representation of the log item context.</param>
        /// <param name="logItemCorrelationsJson">JSON representation of the log item correlations.</param>
        public RawQueueItem(
            Guid id,
            DateTime sampledUtc,
            string telemetryObjectDescribedSerializationJson,
            string logItemKindJson,
            string logItemContextJson,
            string logItemCorrelationsJson)
        {
            this.Id = id;
            this.SampledUtc = sampledUtc;
            this.TelemetryObjectDescribedSerializationJson = telemetryObjectDescribedSerializationJson;
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
        /// Gets the JSON representation of the telemetry object as a described serialization.
        /// </summary>
        public string TelemetryObjectDescribedSerializationJson { get; private set; }

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
