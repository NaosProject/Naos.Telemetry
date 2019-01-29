// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryWriter.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Writer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Naos.Serialization.Domain;
    using Naos.Serialization.Json;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.StorageModel;

    using static System.FormattableString;

    /// <summary>
    /// Implementation of the <see cref="IReadTelemetry" />.
    /// </summary>
    public class TelemetryWriter : IWriteTelemetry
    {
        /// <summary>
        /// Serializer for the fields that are stored as JSON.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Is immutable and I want a field.")]
        public static readonly IStringSerializeAndDeserialize JsonSerializer = new NaosJsonSerializer();

        private readonly EnqueueItemsCommandHandler enqueueItemsCommandHandler;
        private readonly RemoveEnqueuedItemsCommandHandler removeEnqueuedItemsCommandHandler;
        private readonly WriteDiagnosticsCommandHandler writeDiagnosticsCommandHandler;
        private readonly WriteEventCommandHandler writeEventCommandHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryWriter"/> class.
        /// </summary>
        /// <param name="enqueueItemsCommandHandler">Enqueue items handler.</param>
        /// <param name="removeEnqueuedItemsCommandHandler">Removed enqueued items handler.</param>
        /// <param name="writeDiagnosticsCommandHandler">Write diagnostics handler.</param>
        /// <param name="writeEventCommandHandler">Write event handler.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Enqueued", Justification = "Spelling/name is correct.")]
        public TelemetryWriter(
            EnqueueItemsCommandHandler enqueueItemsCommandHandler,
            RemoveEnqueuedItemsCommandHandler removeEnqueuedItemsCommandHandler,
            WriteDiagnosticsCommandHandler writeDiagnosticsCommandHandler,
            WriteEventCommandHandler writeEventCommandHandler)
        {
            this.enqueueItemsCommandHandler = enqueueItemsCommandHandler;
            this.removeEnqueuedItemsCommandHandler = removeEnqueuedItemsCommandHandler;
            this.writeDiagnosticsCommandHandler = writeDiagnosticsCommandHandler;
            this.writeEventCommandHandler = writeEventCommandHandler;
        }

        /// <inheritdoc />
        public async Task WriteRawItemToQueueAsync(IReadOnlyCollection<RawQueueItem> items)
        {
            foreach (var item in items)
            {
                var enqueueCommand = new EnqueueItemsCommand(item);
                await this.enqueueItemsCommandHandler.HandleAsync(enqueueCommand);
            }
        }

        /// <inheritdoc />
        public async Task RemoveItemsFromQueueAsync(IReadOnlyCollection<Guid> idsToRemove)
        {
            var removeCommand = new RemoveEnqueuedItemsCommand(idsToRemove);
            await this.removeEnqueuedItemsCommandHandler.HandleAsync(removeCommand);
        }

        /// <inheritdoc />
        public async Task WriteEventTelemetryAsync(IReadOnlyCollection<Tuple<EventTelemetrySource, EventTelemetry>> eventTelemetryRecords)
        {
            foreach (var item in eventTelemetryRecords)
            {
                var writeEventCommand = new WriteEventCommand(item.Item1, item.Item2);
                await this.writeEventCommandHandler.HandleAsync(writeEventCommand);
            }
        }

        /// <inheritdoc />
        public async Task WriteDiagnosticsTelemetryAsync(IReadOnlyCollection<DiagnosticsTelemetry> diagnosticsTelemetryRecords)
        {
            foreach (var diagnosticsItem in diagnosticsTelemetryRecords)
            {
                var writeDiagnosticsCommand = new WriteDiagnosticsCommand(diagnosticsItem);
                await this.writeDiagnosticsCommandHandler.HandleAsync(writeDiagnosticsCommand);
            }
        }
    }

    /// <summary>
    /// Builder for <see cref="TelemetryWriter"/>.
    /// </summary>
    public static class TelemetryWriterBuilder
    {
        /// <summary>
        /// Builds a new instance of the <see cref="TelemetryWriter"/> class.
        /// </summary>
        /// <param name="telemetryDatabase">Telemetry database.</param>
        /// <returns>New instance of <see cref="TelemetryWriter"/></returns>
        public static TelemetryWriter Build(TelemetryDatabase telemetryDatabase)
        {
            if (telemetryDatabase == null)
            {
                throw new ArgumentNullException(nameof(telemetryDatabase));
            }

            var enqueueItemsCommandHandler = new EnqueueItemsCommandHandler(telemetryDatabase);
            var removeEnqueuedItemsCommandHandler = new RemoveEnqueuedItemsCommandHandler(telemetryDatabase);
            var writeDiagnosticsCommandHandler = new WriteDiagnosticsCommandHandler(telemetryDatabase);
            var writeEventCommandHandler = new WriteEventCommandHandler(telemetryDatabase);

            return new TelemetryWriter(enqueueItemsCommandHandler, removeEnqueuedItemsCommandHandler, writeDiagnosticsCommandHandler, writeEventCommandHandler);
        }

        /// <summary>
        /// Builds a new instance of the <see cref="TelemetryWriter"/> class.
        /// </summary>
        /// <param name="databaseFetcher">Lambda to get database settings.</param>
        /// <returns>New instance of <see cref="TelemetryWriter"/></returns>
        public static TelemetryWriter Build(Func<Type, object> databaseFetcher)
        {
            if (databaseFetcher == null)
            {
                throw new ArgumentNullException(nameof(databaseFetcher));
            }

            var telemetryDatabase = databaseFetcher(typeof(TelemetryDatabase)) as TelemetryDatabase;

            if (telemetryDatabase == null)
            {
                throw new ArgumentException(Invariant($"Could not find {nameof(TelemetryDatabase)} in provided {nameof(databaseFetcher)}"));
            }

            return Build(telemetryDatabase);
        }
    }
}
