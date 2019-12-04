// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventTelemetry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Equality.Recipes;

    /// <summary>
    /// Model to represent an event.
    /// </summary>
    public class EventTelemetry : IAmTelemetryItem, IEquatable<EventTelemetry>
    {
        private Dictionary<string, string> propertyNameToValueMap;

        private Dictionary<string, decimal?> metricNameToValueMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetry"/> class.
        /// </summary>
        /// <param name="sampledUtc">Sampled date and time in UTC.</param>
        /// <param name="name">Name of the event.</param>
        /// <param name="propertyNameToValueMap">Optional properties associated with the event.</param>
        /// <param name="metricNameToValueMap">Optional metrics associated with the event.</param>
        public EventTelemetry(
            DateTime sampledUtc,
            string name,
            IReadOnlyDictionary<string, string> propertyNameToValueMap = null,
            IReadOnlyDictionary<string, decimal?> metricNameToValueMap = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.SampledUtc = sampledUtc;
            this.Name = name;
            this.PropertyNameToValueMap = propertyNameToValueMap;
            this.MetricNameToValueMap = metricNameToValueMap;
        }

        /// <inheritdoc />
        public DateTime SampledUtc { get; private set; }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets an properties of the event.
        /// </summary>
        public IReadOnlyDictionary<string, string> PropertyNameToValueMap
        {
            get { return this.propertyNameToValueMap; }
            private set { this.propertyNameToValueMap = value?.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, string>(); }
        }

        /// <summary>
        /// Gets any metrics associated with the event.
        /// </summary>
        public IReadOnlyDictionary<string, decimal?> MetricNameToValueMap
        {
            get { return this.metricNameToValueMap; }
            private set { this.metricNameToValueMap = value?.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, decimal?>(); }
        }

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
        /// Add a property to collection.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public void AddProperty(string name, string value)
        {
            this.propertyNameToValueMap.Add(name, value);
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are equal.</returns>
        public static bool operator ==(EventTelemetry first, EventTelemetry second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            var result = first.SampledUtc == second.SampledUtc &&
                         string.Equals(first.Name, second.Name, StringComparison.OrdinalIgnoreCase) &&
                         first.MetricNameToValueMap.IsEqualTo(second.MetricNameToValueMap) &&
                         first.PropertyNameToValueMap.IsEqualTo(second.PropertyNameToValueMap);

            return result;
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are inequal.</returns>
        public static bool operator !=(EventTelemetry first, EventTelemetry second) => !(first == second);

        /// <inheritdoc />
        public bool Equals(EventTelemetry other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as EventTelemetry);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.SampledUtc)
            .Hash(this.Name)
            .Hash(this.MetricNameToValueMap)
            .Hash(this.PropertyNameToValueMap)
            .Value;
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
