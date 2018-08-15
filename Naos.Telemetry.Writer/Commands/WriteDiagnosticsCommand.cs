// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteDiagnosticsCommand.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Writer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Threading.Tasks;

    using Naos.Diagnostics.Domain;
    using Naos.Diagnostics.Recipes;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.StorageModel;

    using OBeautifulCode.Database.Recipes;

    using Spritely.Cqrs;

    using static System.FormattableString;

    /// <summary>
    /// Query to get the queued items.
    /// </summary>
    public class WriteDiagnosticsCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteDiagnosticsCommand"/> class.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public WriteDiagnosticsCommand(DiagnosticsTelemetry item)
        {
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        /// <summary>
        /// Gets the item to write.
        /// </summary>
        public DiagnosticsTelemetry Item { get; private set; }
    }

    /// <summary>
    /// Handler for <see cref="WriteDiagnosticsCommand" />.
    /// </summary>
    public class WriteDiagnosticsCommandHandler : ICommandHandlerAsync<WriteDiagnosticsCommand>
    {
        private readonly TelemetryDatabase telemetryDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteDiagnosticsCommandHandler"/> class.
        /// </summary>
        /// <param name="telemetryDatabase">Telemetry database.</param>
        public WriteDiagnosticsCommandHandler(TelemetryDatabase telemetryDatabase)
        {
            this.telemetryDatabase = telemetryDatabase ?? throw new ArgumentNullException(nameof(telemetryDatabase));
        }

        /// <inheritdoc />
        public async Task HandleAsync(WriteDiagnosticsCommand command)
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

            using (var connection = this.telemetryDatabase.CreateConnection())
            {
                var transaction = connection.BeginTransaction();
                try
                {
                    var diagnosticsId = Guid.NewGuid();
                    var diagnosticsCommand = this.BuildDiagnosticsCommand(
                        connection,
                        transaction,
                        command.Item.SampledUtc,
                        diagnosticsId);
                    diagnosticsCommand.ExecuteNonQuery();

                    var machineDetailsCommand = this.BuildMachineDetailsCommand(
                        connection,
                        transaction,
                        diagnosticsId,
                        command.Item.MachineDetails);
                    machineDetailsCommand.ExecuteNonQuery();

                    var processDetailsCommand = this.BuildProcessDetailsCommand(
                        connection,
                        transaction,
                        diagnosticsId,
                        command.Item.ProcessDetails);
                    processDetailsCommand.ExecuteNonQuery();

                    foreach (var assembly in command.Item.ProcessSiblingAssemblies)
                    {
                        var assemblyCommand = this.BuildAssemblyDetailsCommand(
                            connection,
                            transaction,
                            diagnosticsId,
                            assembly);
                        assemblyCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception commitException)
                {
                    if (transaction != null)
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

        private IDbCommand BuildDiagnosticsCommand(IDbConnection connection, IDbTransaction transaction, DateTime sampledUtc, Guid diagnosticsId)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(DiagnosticsSchema.Id, DbType.Guid, diagnosticsId),
                                    new ColumnObject(DiagnosticsSchema.SampledUtc, DbType.DateTime, sampledUtc),
                                };

            var sql = SqlCommon.BuildInsertStatement(DiagnosticsSchema.TableName, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet);

            var command = DatabaseHelper.BuildCommand(
                connection,
                sql,
                parameters,
                transaction: transaction,
                timeoutSeconds: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0);

            return command;
        }

        private IDbCommand BuildMachineDetailsCommand(IDbConnection connection, IDbTransaction transaction, Guid diagnosticsId, MachineDetails machineDetails)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(MachineDetailsSchema.Id, DbType.Guid, Guid.NewGuid()),
                                    new ColumnObject(MachineDetailsSchema.DiagnosticsId, DbType.Guid, diagnosticsId),
                                    new ColumnObject(MachineDetailsSchema.MachineName, DbType.String, machineDetails.GetTypedMachineNameKindToNameMap().GetMachineName()),
                                    new ColumnObject(MachineDetailsSchema.MachineNameMapJson, DbType.String, machineDetails.MachineNameKindToNameMap),
                                    new ColumnObject(MachineDetailsSchema.ProcessorCount, DbType.Int32, machineDetails.ProcessorCount),
                                    new ColumnObject(MachineDetailsSchema.PhysicalMemoryInGb, DbType.Decimal, machineDetails.GetTypedMemoryKindToValueInGbMap()[MachineMemoryKind.TotalPhysical]),
                                    new ColumnObject(MachineDetailsSchema.MemoryMapJson, DbType.String, machineDetails.MemoryKindToValueInGbMap),
                                    new ColumnObject(MachineDetailsSchema.OperatingSystemIs64Bit, DbType.Boolean, machineDetails.OperatingSystemIs64Bit),
                                    new ColumnObject(MachineDetailsSchema.OperatingSystemJson, DbType.String, machineDetails.OperatingSystem),
                                    new ColumnObject(MachineDetailsSchema.ClrVersion, DbType.String, machineDetails.ClrVersion),
                                };

            var sql = SqlCommon.BuildInsertStatement(MachineDetailsSchema.TableName, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet);

            var command = DatabaseHelper.BuildCommand(
                connection,
                sql,
                parameters,
                transaction: transaction,
                timeoutSeconds: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0);

            return command;
        }

        private IDbCommand BuildProcessDetailsCommand(IDbConnection connection, IDbTransaction transaction, Guid diagnosticsId, ProcessDetails processDetails)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(ProcessDetailsSchema.Id, DbType.Guid, Guid.NewGuid()),
                                    new ColumnObject(ProcessDetailsSchema.DiagnosticsId, DbType.Guid, diagnosticsId),
                                    new ColumnObject(ProcessDetailsSchema.Name, DbType.String, processDetails.Name),
                                    new ColumnObject(ProcessDetailsSchema.FilePath, DbType.String, processDetails.FilePath),
                                    new ColumnObject(ProcessDetailsSchema.FileVersion, DbType.String, processDetails.FileVersion),
                                    new ColumnObject(ProcessDetailsSchema.ProductVersion, DbType.String, processDetails.ProductVersion),
                                    new ColumnObject(ProcessDetailsSchema.RunningAsAdmin, DbType.Boolean, processDetails.RunningAsAdmin),
                                };

            var sql = SqlCommon.BuildInsertStatement(ProcessDetailsSchema.TableName, columnSet);

            var parameters = SqlCommon.BuildParameters(columnSet);

            var command = DatabaseHelper.BuildCommand(
                connection,
                sql,
                parameters,
                transaction: transaction,
                timeoutSeconds: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 0);

            return command;
        }

        private IDbCommand BuildAssemblyDetailsCommand(IDbConnection connection, IDbTransaction transaction, Guid diagnosticsId, AssemblyDetails assemblyDetails)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(AssemblyDetailsSchema.Id, DbType.Guid, Guid.NewGuid()),
                                    new ColumnObject(AssemblyDetailsSchema.DiagnosticsId, DbType.Guid, diagnosticsId),
                                    new ColumnObject(AssemblyDetailsSchema.Name, DbType.String, assemblyDetails.Name),
                                    new ColumnObject(AssemblyDetailsSchema.VersionJson, DbType.String, assemblyDetails.Version),
                                    new ColumnObject(AssemblyDetailsSchema.FilePath, DbType.String, assemblyDetails.FilePath),
                                    new ColumnObject(AssemblyDetailsSchema.FrameworkVersion, DbType.String, assemblyDetails.FrameworkVersion),
                                };

            var sql = SqlCommon.BuildInsertStatement(AssemblyDetailsSchema.TableName, columnSet);

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