// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceCounterFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Naos.Diagnostics.Domain;

    /// <summary>
    /// Model to represent an event.
    /// </summary>
    public static class PerformanceCounterFactory
    {
        /// <summary>
        /// Convert a set of performance counter samples into metrics on an <see cref="EventTelemetry" />.
        /// </summary>
        /// <param name="performanceCounterSamples">Sampled performance counters.</param>
        /// <param name="name">Name of event.</param>
        /// <param name="sampledUtc">Date time of sample in UTC.</param>
        /// <returns>Converted <see cref="EventTelemetry" />.</returns>
        public static EventTelemetry ToEventTelemetry(this ICollection<PerformanceCounterSample> performanceCounterSamples, string name, DateTime sampledUtc)
        {
            // TODO: should we try to capture the in range idea here?
            var metrics = performanceCounterSamples.ToDictionary(k => k.Description.ToString(), v => (decimal?)v.Value);
            var properties = new Dictionary<string, string>();
            var result = new EventTelemetry(sampledUtc, name, properties, metrics);
            return result;
        }
    }
}
