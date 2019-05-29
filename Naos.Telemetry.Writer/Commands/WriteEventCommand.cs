// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteEventCommand.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Writer
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.StorageModel;

    using OBeautifulCode.Database.Recipes;

    using Spritely.Cqrs;

    /// <summary>
    /// Query to get the queued items.
    /// </summary>
    public class WriteEventCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventCommand"/> class.
        /// </summary>
        /// <param name="source">Source of the item.</param>
        /// <param name="item">Item to add.</param>
        public WriteEventCommand(EventTelemetrySource source, EventTelemetry item)
        {
            this.Source = source;
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        /// <summary>
        /// Gets the source context of the item.
        /// </summary>
        public EventTelemetrySource Source { get; private set; }

        /// <summary>
        /// Gets the item to write.
        /// </summary>
        public EventTelemetry Item { get; private set; }
    }

    /// <summary>
    /// Handler for <see cref="WriteEventCommand" />.
    /// </summary>
    public class WriteEventCommandHandler : ICommandHandlerAsync<WriteEventCommand>
    {
        private readonly TelemetryDatabase telemetryDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventCommandHandler"/> class.
        /// </summary>
        /// <param name="telemetryDatabase">Telemetry database.</param>
        public WriteEventCommandHandler(TelemetryDatabase telemetryDatabase)
        {
            this.telemetryDatabase = telemetryDatabase ?? throw new ArgumentNullException(nameof(telemetryDatabase));
        }

        /// <inheritdoc />
        public async Task HandleAsync(WriteEventCommand command)
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
                var transaction = connection.BeginTransaction();
                try
                {
                    var callingTypeJson = TelemetryWriter.JsonSerializer.SerializeToString(command.Source.CallingType);
                    var eventSourceCommand = this.BuildEventSourceCommand(
                        connection,
                        transaction,
                        command.Source.MachineName,
                        command.Source.ProcessName,
                        command.Source.ProcessFileVersion,
                        command.Source.CallingMethod,
                        command.Source.StackTrace,
                        callingTypeJson);

                    var eventSourceIdRaw = eventSourceCommand.ExecuteScalar();
                    var eventSourceId = Guid.Parse(eventSourceIdRaw.ToString());

                    var eventId = Guid.NewGuid();
                    var eventCommand = this.BuildEventCommand(
                        connection,
                        transaction,
                        eventId,
                        eventSourceId,
                        command.Item.Name,
                        command.Item.SampledUtc);
                    eventCommand.ExecuteNonQuery();

                    foreach (var property in command.Item.PropertyNameToValueMap)
                    {
                        var propertyCommand = this.BuildPropertyCommand(
                            connection,
                            transaction,
                            eventId,
                            property.Key,
                            property.Value);
                        propertyCommand.ExecuteNonQuery();
                    }

                    foreach (var metric in command.Item.MetricNameToValueMap)
                    {
                        var metricCommand = this.BuildMetricCommand(
                            connection,
                            transaction,
                            eventId,
                            metric.Key,
                            metric.Value);
                        metricCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception commitException)
                {
                    if (transaction != null && transaction.Connection != null)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception rollbackException)
                        {
                            throw new AggregateException(rollbackException, commitException);
                        }
                    }

                    throw;
                }
            }
        }

        private IDbCommand BuildEventSourceCommand(
            IDbConnection connection,
            IDbTransaction transaction,
            string machineName,
            string processName,
            string processFileVersion,
            string callingMethod,
            string stackTrace,
            string callingTypeJson)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(EventSourceSchema.MachineName, DbType.String, machineName),
                                    new ColumnObject(EventSourceSchema.ProcessName, DbType.String, processName),
                                    new ColumnObject(EventSourceSchema.ProcessFileVersion, DbType.String, processFileVersion),
                                    new ColumnObject(EventSourceSchema.CallingMethod, DbType.String, callingMethod),
                                    new ColumnObject(EventSourceSchema.StackTrace, DbType.String, stackTrace),
                                    new ColumnObject(EventSourceSchema.CallingTypeJson, DbType.String, callingTypeJson),
                                };

            var sql = SqlCommon.BuildProcedureStatement(EventSourceSchema.Sprocs.InsertEventTelemetrySourceAsNecessary.Name, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet);

            var command = DatabaseHelper.BuildCommand(
                connection,
                sql,
                parameters,
                transaction: transaction,
                timeoutSeconds: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0);

            return command;
        }

        private IDbCommand BuildEventCommand(IDbConnection connection, IDbTransaction transaction, Guid eventId, Guid eventSourceId, string name, DateTime sampledUtc)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(EventSchema.Id, DbType.Guid, eventId),
                                    new ColumnObject(EventSchema.EventSourceId, DbType.Guid, eventSourceId),
                                    new ColumnObject(EventSchema.Name, DbType.String, name),
                                    new ColumnObject(EventSchema.SampledUtc, DbType.DateTime, sampledUtc),
                                };

            var sql = SqlCommon.BuildInsertStatement(EventSchema.TableName, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet);

            var command = DatabaseHelper.BuildCommand(
                connection,
                sql,
                parameters,
                transaction: transaction,
                timeoutSeconds: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0);

            return command;
        }

        private IDbCommand BuildPropertyCommand(IDbConnection connection, IDbTransaction transaction, Guid eventId, string name, string value)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(PropertySchema.Id, DbType.Guid, Guid.NewGuid()),
                                    new ColumnObject(PropertySchema.EventId, DbType.Guid, eventId),
                                    new ColumnObject(PropertySchema.Name, DbType.String, name),
                                    new ColumnObject(PropertySchema.Value, DbType.String, value),
                                };

            var sql = SqlCommon.BuildInsertStatement(PropertySchema.TableName, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet);

            var command = DatabaseHelper.BuildCommand(
                connection,
                sql,
                parameters,
                transaction: transaction,
                timeoutSeconds: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0);

            return command;
        }

        private IDbCommand BuildMetricCommand(IDbConnection connection, IDbTransaction transaction, Guid eventId, string name, decimal? value)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(MetricSchema.Id, DbType.Guid, Guid.NewGuid()),
                                    new ColumnObject(MetricSchema.EventId, DbType.Guid, eventId),
                                    new ColumnObject(MetricSchema.Name, DbType.String, name),
                                    new ColumnObject(MetricSchema.Value, DbType.Decimal, value),
                                };

            var sql = SqlCommon.BuildInsertStatement(MetricSchema.TableName, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet);

            var command = DatabaseHelper.BuildCommand(
                connection,
                sql,
                parameters,
                transaction: transaction,
                timeoutSeconds: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0);

            return command;
        }
    }
}