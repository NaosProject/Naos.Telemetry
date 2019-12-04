// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Stopwatches.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Global stopwatch tracker with conveniences to start and stop named <see cref="Stopwatch" />'s as well as pull reports and clear.
    /// This is a nice tool for quickly capturing timings.
    /// </summary>
    public static class Stopwatches
    {
        private static readonly object SyncStopwatchNameToStopwatchMap = new object();

        private static readonly Dictionary<string, Stopwatch> StopwatchNameToStopwatchMap = new Dictionary<string, Stopwatch>();

        /// <summary>
        /// Clears all saved results (will affect all consumers).
        /// </summary>
        /// <param name="nameWhitelist">Names to include (will exclude anything not in the list if any items are supplied).</param>
        /// <param name="nameBlacklist">Names to exclude (will exclude and take precedence over the <paramref name="nameWhitelist" />.</param>
        public static void Clear(IReadOnlyCollection<string> nameWhitelist = null, IReadOnlyCollection<string> nameBlacklist = null)
        {
            lock (SyncStopwatchNameToStopwatchMap)
            {
                var localWhitelist = nameWhitelist ?? new List<string>();
                var localBlacklist = nameBlacklist ?? new List<string>();

                var keysToRemove = StopwatchNameToStopwatchMap.Keys.Where(_ => (!localWhitelist.Any() || localWhitelist.Contains(_)) && !localBlacklist.Contains(_)).ToList();
                foreach (var key in keysToRemove)
                {
                    var stopwatch = StopwatchNameToStopwatchMap[key];
                    stopwatch.Stop();
                    stopwatch.Reset();
                    StopwatchNameToStopwatchMap.Remove(key);
                }
            }
        }

        /// <summary>
        /// Starts a <see cref="Stopwatch" /> under the specified name, will throw if it is already present and running.
        /// </summary>
        /// <param name="name">Name of the <see cref="Stopwatch" />.</param>
        public static void Start(string name)
        {
            lock (SyncStopwatchNameToStopwatchMap)
            {
                if (!StopwatchNameToStopwatchMap.ContainsKey(name))
                {
                    StopwatchNameToStopwatchMap.Add(name, new Stopwatch());
                }

                var stopwatch = StopwatchNameToStopwatchMap[name];
                if (stopwatch.IsRunning)
                {
                    throw new InvalidOperationException("already started: " + name);
                }

                stopwatch.Start();
            }
        }

        /// <summary>
        /// Stops a <see cref="Stopwatch" /> under the specified name, will throw if it is missing.
        /// </summary>
        /// <param name="name">Name of the <see cref="Stopwatch" />.</param>
        public static void Stop(string name)
        {
            lock (SyncStopwatchNameToStopwatchMap)
            {
                if (!StopwatchNameToStopwatchMap.ContainsKey(name))
                {
                    throw new InvalidOperationException("doesn't exist: " + name);
                }

                var stopwatch = StopwatchNameToStopwatchMap[name];
                if (!stopwatch.IsRunning)
                {
                    throw new InvalidOperationException("already stopped: " + name);
                }

                stopwatch.Stop();
            }
        }

        /// <summary>
        /// Build a snapshot of existing stopwatches.
        /// </summary>
        /// <param name="nameWhitelist">Names to include (will exclude anything not in the list if any items are supplied).</param>
        /// <param name="nameBlacklist">Names to exclude (will exclude and take precedence over the <paramref name="nameWhitelist" />.</param>
        /// <returns><see cref="IReadOnlyCollection{T}" /> of <see cref="StopwatchSnapshot" />.</returns>
        public static IReadOnlyCollection<StopwatchSnapshot> BuildSummaryReport(IReadOnlyCollection<string> nameWhitelist = null, IReadOnlyCollection<string> nameBlacklist = null)
        {
            lock (SyncStopwatchNameToStopwatchMap)
            {
                var localWhitelist = nameWhitelist ?? new List<string>();
                var localBlacklist = nameBlacklist ?? new List<string>();

                var result = new List<StopwatchSnapshot>();

                foreach (var stopwatchEntry in StopwatchNameToStopwatchMap)
                {
                    if ((!localWhitelist.Any() || localWhitelist.Contains(stopwatchEntry.Key)) && !localBlacklist.Contains(stopwatchEntry.Key))
                    {
                        var newOne = new StopwatchSnapshot(stopwatchEntry.Key, stopwatchEntry.Value.ElapsedMilliseconds, stopwatchEntry.Value.IsRunning);
                        result.Add(newOne);
                    }
                }

                return result;
            }
        }
    }
}
