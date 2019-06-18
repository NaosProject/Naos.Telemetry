// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopwatchSnapshot.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using OBeautifulCode.Math.Recipes;

    /// <summary>
    /// Model object that contains a summary of the <see cref="Stopwatch" />.
    /// </summary>
    public class StopwatchSnapshot : IEquatable<StopwatchSnapshot>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopwatchSnapshot"/> class.
        /// </summary>
        /// <param name="name">The name assigned to the <see cref="Stopwatch" />.</param>
        /// <param name="elapsedMilliseconds">The elapsed milliseconds of the <see cref="Stopwatch" />.</param>
        /// <param name="isRunning">A value indicating whether or not the <see cref="Stopwatch" /> is running.</param>
        public StopwatchSnapshot(string name, decimal elapsedMilliseconds, bool isRunning)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.ElapsedMilliseconds = elapsedMilliseconds;
            this.IsRunning = isRunning;
        }

        /// <summary>
        /// Gets the name assigned to the <see cref="Stopwatch" />.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the elapsed milliseconds of the <see cref="Stopwatch" />.
        /// </summary>
        public decimal ElapsedMilliseconds { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="Stopwatch" /> is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are equal.</returns>
        public static bool operator ==(StopwatchSnapshot first, StopwatchSnapshot second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            return string.Equals(first.Name, second.Name, StringComparison.Ordinal) && first.ElapsedMilliseconds == second.ElapsedMilliseconds && first.IsRunning == second.IsRunning;
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are unequal.</returns>
        public static bool operator !=(StopwatchSnapshot first, StopwatchSnapshot second) => !(first == second);

        /// <inheritdoc />
        public bool Equals(StopwatchSnapshot other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as StopwatchSnapshot);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Name).Hash(this.ElapsedMilliseconds).Hash(this.IsRunning).Value;
    }

    /// <summary>
    /// Extensions to <see cref="StopwatchSnapshot" />.
    /// </summary>
    public static class StopwatchSnapshotExtensions
    {
        /// <summary>
        /// Converts a collection of <see cref="StopwatchSnapshot" />'s into an <see cref="EventTelemetry" />.
        /// </summary>
        /// <param name="stopwatchSnapshots">Collection of <see cref="StopwatchSnapshot"/> to convert.</param>
        /// <param name="eventName">Name to use for the event.</param>
        /// <param name="sampledDateTime">Optional <see cref="DateTime" /> to use as sampled timestamp; DEFAULT is <see cref="DateTime.UtcNow" />.</param>
        /// <param name="metadata">Optional additional metadata to add to the property bag on <see cref="EventTelemetry" />.</param>
        /// <returns><see cref="EventTelemetry" /> representation of the information.</returns>
        public static EventTelemetry ToEventTelemetry(
            this IReadOnlyCollection<StopwatchSnapshot> stopwatchSnapshots,
            string eventName,
            DateTime? sampledDateTime = null,
            IReadOnlyDictionary<string, string> metadata = null)
        {
            if (stopwatchSnapshots == null)
            {
                throw new ArgumentNullException(nameof(stopwatchSnapshots));
            }

            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            var properties = metadata == null ? new Dictionary<string, string>() : metadata.ToDictionary(k => k.Key, v => v.Value);
            var metrics = new Dictionary<string, decimal?>();

            foreach (var stopwatchSnapshot in stopwatchSnapshots)
            {
                properties.Add(stopwatchSnapshot.Name, stopwatchSnapshot.IsRunning ? "Running" : "NotRunning");
                metrics.Add(stopwatchSnapshot.Name, stopwatchSnapshot.ElapsedMilliseconds);
            }

            var result = new EventTelemetry(
                sampledDateTime ?? DateTime.UtcNow,
                eventName,
                properties,
                metrics);

            return result;
        }
    }
}