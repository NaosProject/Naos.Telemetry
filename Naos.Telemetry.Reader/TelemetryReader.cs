// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryReader.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Naos.Telemetry.Domain;
    using Naos.Telemetry.StorageModel;

    using static System.FormattableString;

    /// <summary>
    /// Implementation of the <see cref="IReadTelemetry" />.
    /// </summary>
    public class TelemetryReader : IReadTelemetry
    {
        private readonly GetQueuedItemsQueryHandler queuedItemsQueryHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryReader"/> class.
        /// </summary>
        /// <param name="queuedItemsQueryHandler">Query handler for queued items.</param>
        public TelemetryReader(GetQueuedItemsQueryHandler queuedItemsQueryHandler)
        {
            this.queuedItemsQueryHandler = queuedItemsQueryHandler ?? throw new ArgumentNullException(nameof(queuedItemsQueryHandler));
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<RawQueueItem>> GetQueuedRawItemsAsync()
        {
            var query = new GetQueuedItemsQuery();
            var result = await this.queuedItemsQueryHandler.HandleAsync(query);
            return result;
        }
    }

    /// <summary>
    /// Builder for <see cref="TelemetryReader"/>.
    /// </summary>
    public static class TelemetryReaderBuilder
    {
        /// <summary>
        /// Builds a new instance of the <see cref="TelemetryReader"/> class.
        /// </summary>
        /// <param name="telemetryDatabase">Telemetry database.</param>
        /// <returns>New instance of <see cref="TelemetryReader"/>.</returns>
        public static TelemetryReader Build(TelemetryDatabase telemetryDatabase)
        {
            if (telemetryDatabase == null)
            {
                throw new ArgumentNullException(nameof(telemetryDatabase));
            }

            var queryHandler = new GetQueuedItemsQueryHandler(telemetryDatabase);

            return new TelemetryReader(queryHandler);
        }

        /// <summary>
        /// Builds a new instance of the <see cref="TelemetryReader"/> class.
        /// </summary>
        /// <param name="databaseFetcher">Lambda to get database settings.</param>
        /// <returns>New instance of <see cref="TelemetryReader"/>.</returns>
        public static TelemetryReader Build(Func<Type, object> databaseFetcher)
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