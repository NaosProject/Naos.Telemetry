// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventTelemetry.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Model to represent an event.
    /// </summary>
    public class EventTelemetry : IAmTelemetryItem
    {
        private readonly Dictionary<string, string> propertyNameToValueMap;

        private readonly Dictionary<string, decimal?> metricNameToValueMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetry"/> class.
        /// </summary>
        /// <param name="sampledUtc">Sampled date and time in UTC.</param>
        /// <param name="name">Name of the event.</param>
        /// <param name="propertyNameToValueMap">Optional properties associated with the event.</param>
        /// <param name="metricNameToValueMap">Optional metrics associated with the event.</param>
        public EventTelemetry(DateTime sampledUtc, string name, IReadOnlyDictionary<string, string> propertyNameToValueMap = null, IReadOnlyDictionary<string, decimal?> metricNameToValueMap = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.SampledUtc = sampledUtc;
            this.Name = name;
            this.propertyNameToValueMap = propertyNameToValueMap?.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, string>();
            this.metricNameToValueMap = metricNameToValueMap?.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, decimal?>();
        }

        /// <inheritdoc />
        public DateTime SampledUtc { get; private set; }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets any metrics associated with the event.
        /// </summary>
        public IReadOnlyDictionary<string, decimal?> MetricNameToValueMap => this.metricNameToValueMap;

        /// <summary>
        /// Add a metric to collection.
        /// </summary>
        /// <param name="name">Name of the metric.</param>
        /// <param name="value">Value of the metric.</param>
        public void AddMetric(string name, decimal? value)
        {
            this.metricNameToValueMap.Add(name, value);
        }

        /// <summary>
        /// Gets an properties of the event.
        /// </summary>
        public IReadOnlyDictionary<string, string> PropertyNameToValueMap => this.propertyNameToValueMap;

        /// <summary>
        /// Add a property to collection.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public void AddProperty(string name, string value)
        {
            this.propertyNameToValueMap.Add(name, value);
        }
    }

    /// <summary>
    /// Common keys to allow easy referencing of standard properties.
    /// </summary>
    public static class CommonPropertyKeys
    {
        /// <summary>
        /// Begin time in UTC as a <see cref="System.DateTime" />.
        /// </summary>
        public const string BeginTimeUtc = "BeginTimeUtc";

        /// <summary>
        /// End time in UTC as a <see cref="System.DateTime" />.
        /// </summary>
        public const string EndTimeUtc = "EndTimeUtc";
    }

    /// <summary>
    /// Common keys to allow easy referencing of standard metrics.
    /// </summary>
    public static class CommonMetricKeys
    {
        /// <summary>
        /// Duration of an event in seconds.
        /// </summary>
        public const string Duration = "DurationInSeconds";

        /// <summary>
        /// Min value.
        /// </summary>
        public const string Min = "Min";

        /// <summary>
        /// Max value.
        /// </summary>
        public const string Max = "Max";

        /// <summary>
        /// Sum of values.
        /// </summary>
        public const string Sum = "Sum";

        /// <summary>
        /// Count of values.
        /// </summary>
        public const string Count = "Count";
    }
}