// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetQueuedItemsQuery.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Naos.Telemetry.Domain;
    using Naos.Telemetry.StorageModel;

    using Spritely.Cqrs;

    using static System.FormattableString;

    /// <summary>
    /// Query to get the queued items.
    /// </summary>
    public class GetQueuedItemsQuery : IQuery<IReadOnlyCollection<RawQueueItem>>
    {
    }

    /// <summary>
    /// Handler for <see cref="GetQueuedItemsQuery" />.
    /// </summary>
    public class GetQueuedItemsQueryHandler : IQueryHandlerAsync<GetQueuedItemsQuery, IReadOnlyCollection<RawQueueItem>>
    {
        private readonly TelemetryDatabase telemetryDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQueuedItemsQueryHandler"/> class.
        /// </summary>
        /// <param name="telemetryDatabase">Telemetry database.</param>
        public GetQueuedItemsQueryHandler(TelemetryDatabase telemetryDatabase)
        {
            this.telemetryDatabase = telemetryDatabase ?? throw new ArgumentNullException(nameof(telemetryDatabase));
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<RawQueueItem>> HandleAsync(GetQueuedItemsQuery query)
        {
            var sql = Invariant($"SELECT {RawQueueSchema.Id}, {RawQueueSchema.SampledUtc}, {RawQueueSchema.TelemetryObjectDescribedSerializationJson}, {RawQueueSchema.LogItemKindJson}, {RawQueueSchema.LogItemContextJson}, {RawQueueSchema.LogItemCorrelationsJson} FROM {RawQueueSchema.TableName}");

            using (var connection = this.telemetryDatabase.CreateConnection())
            {
                var result = await connection.QueryAsync<RawQueueItem>(sql, commandTimeout: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds);

                return result.ToList();
            }
        }
    }
}