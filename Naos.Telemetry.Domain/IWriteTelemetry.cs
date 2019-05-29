// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriteTelemetry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to define a writer.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Want an interface NOT an attribute.")]
    public interface IWriteTelemetry
    {
        /// <summary>
        /// Write raw items to queue for processing.
        /// </summary>
        /// <param name="items">Items to write.</param>
        /// <returns>Task for async.</returns>
        Task WriteRawItemToQueueAsync(IReadOnlyCollection<RawQueueItem> items);

        /// <summary>
        /// Removes the specified ID's from the queue.
        /// </summary>
        /// <param name="idsToRemove">ID's to remove.</param>
        /// <returns>Task for async.</returns>
        Task RemoveItemsFromQueueAsync(IReadOnlyCollection<Guid> idsToRemove);

        /// <summary>
        /// Writes <see cref="EventTelemetry" />.
        /// </summary>
        /// <param name="eventTelemetryRecords"><see cref="EventTelemetry" /> records.</param>
        /// <returns>Task for async.</returns>
        Task WriteEventTelemetryAsync(IReadOnlyCollection<Tuple<EventTelemetrySource, EventTelemetry>> eventTelemetryRecords);

        /// <summary>
        /// Writes <see cref="DiagnosticsTelemetry" />.
        /// </summary>
        /// <param name="diagnosticsTelemetryRecords"><see cref="DiagnosticsTelemetry" /> records.</param>
        /// <returns>Task for async.</returns>
        Task WriteDiagnosticsTelemetryAsync(IReadOnlyCollection<DiagnosticsTelemetry> diagnosticsTelemetryRecords);
    }
}