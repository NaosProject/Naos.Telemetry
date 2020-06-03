// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnqueueItemsCommand.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Writer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Naos.Telemetry.Domain;
    using Naos.Telemetry.StorageModel;

    using OBeautifulCode.Database.Recipes;

    using Spritely.Cqrs;

    /// <summary>
    /// Query to get the queued items.
    /// </summary>
    public class EnqueueItemsCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnqueueItemsCommand"/> class.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public EnqueueItemsCommand(RawQueueItem item)
        {
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        /// <summary>
        /// Gets the item to enqueue.
        /// </summary>
        public RawQueueItem Item { get; private set; }
    }

    /// <summary>
    /// Handler for <see cref="EnqueueItemsCommand" />.
    /// </summary>
    public class EnqueueItemsCommandHandler : ICommandHandlerAsync<EnqueueItemsCommand>
    {
        private readonly TelemetryDatabase telemetryDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnqueueItemsCommandHandler"/> class.
        /// </summary>
        /// <param name="telemetryDatabase">Telemetry database.</param>
        public EnqueueItemsCommandHandler(TelemetryDatabase telemetryDatabase)
        {
            this.telemetryDatabase = telemetryDatabase ?? throw new ArgumentNullException(nameof(telemetryDatabase));
        }

        /// <inheritdoc />
        public async Task HandleAsync(EnqueueItemsCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await Task.Run(
                () =>
                    {
                        /* no-op */
                    });

            using (var connection = this.telemetryDatabase.CreateOpenedConnection())
            {
                var sqlCommand = this.BuildEnqueueCommand(connection, command.Item);
                sqlCommand.ExecuteNonQuery();
            }
        }

        private SqlCommand BuildEnqueueCommand(SqlConnection connection, RawQueueItem item)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(RawQueueSchema.Id, DbType.Guid, item.Id),
                                    new ColumnObject(RawQueueSchema.SampledUtc, DbType.DateTime, item.SampledUtc),
                                    new ColumnObject(RawQueueSchema.TelemetryObjectDescribedSerializationJson, DbType.String, item.TelemetryObjectDescribedSerializationJson),
                                    new ColumnObject(RawQueueSchema.LogItemKindJson, DbType.String, item.LogItemKindJson),
                                    new ColumnObject(RawQueueSchema.LogItemContextJson, DbType.String, item.LogItemContextJson),
                                    new ColumnObject(RawQueueSchema.LogItemCorrelationsJson, DbType.String, item.LogItemCorrelationsJson),
                                };

            var sql = SqlCommon.BuildInsertStatement(RawQueueSchema.TableName, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet).ToList();

            var command = DatabaseHelper.BuildSqlCommand(
                connection,
                sql,
                this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0,
                parameters);

            return command;
        }
    }
}
