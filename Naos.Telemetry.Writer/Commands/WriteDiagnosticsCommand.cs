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

            using (var connection = await this.telemetryDatabase.CreateOpenConnectionAsync())
            {
                var transaction = connection.BeginTransaction();
                try
                {
                    var machineDetailsId = Guid.NewGuid();
                    var machineDetailsCommand = this.BuildMachineDetailsCommand(
                        connection,
                        transaction,
                        machineDetailsId,
                        command.Item.MachineDetails);
                    machineDetailsCommand.ExecuteNonQuery();

                    var processDetailsId = Guid.NewGuid();
                    var processDetailsCommand = this.BuildProcessDetailsCommand(
                        connection,
                        transaction,
                        processDetailsId,
                        command.Item.ProcessDetails);
                    processDetailsCommand.ExecuteNonQuery();

                    var diagnosticsId = Guid.NewGuid();
                    var diagnosticsCommand = this.BuildDiagnosticsCommand(
                        connection,
                        transaction,
                        command.Item.SampledUtc,
                        diagnosticsId,
                        machineDetailsId,
                        processDetailsId);
                    diagnosticsCommand.ExecuteNonQuery();

                    var committedAssemblyIds = new List<Guid>();
                    foreach (var assembly in command.Item.ProcessSiblingAssemblies)
                    {
                        var assemblyId = Guid.NewGuid();
                        var assemblyCommand = this.BuildAssemblyDetailsCommand(
                            connection,
                            transaction,
                            assemblyId,
                            assembly);
                        assemblyCommand.ExecuteNonQuery();
                        committedAssemblyIds.Add(assemblyId);
                    }

                    foreach (var assemblyId in committedAssemblyIds)
                    {
                        var assemblyDiagnosticsCrossReferenceCommand = this.BuildAssemblyDiagnosticsCrossReferenceCommand(
                            connection,
                            transaction,
                            diagnosticsId,
                            assemblyId);
                        assemblyDiagnosticsCrossReferenceCommand.ExecuteNonQuery();
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

        private IDbCommand BuildMachineDetailsCommand(IDbConnection connection, IDbTransaction transaction, Guid machineDetailsId, MachineDetails machineDetails)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(MachineDetailsSchema.Id, DbType.Guid, machineDetailsId),
                                    new ColumnObject(MachineDetailsSchema.MachineName, DbType.String, machineDetails.GetTypedMachineNameKindToNameMap().GetMachineName()),
                                    new ColumnObject(MachineDetailsSchema.MachineNameMapJson, DbType.String, machineDetails.MachineNameKindToNameMap),
                                    new ColumnObject(MachineDetailsSchema.ProcessorCount, DbType.Int16, machineDetails.ProcessorCount),
                                    new ColumnObject(MachineDetailsSchema.PhysicalMemoryInGb, DbType.Int16, machineDetails.GetTypedMemoryKindToValueInGbMap()[MachineMemoryKind.TotalPhysical]),
                                    new ColumnObject(MachineDetailsSchema.MemoryMapJson, DbType.String, machineDetails.MemoryKindToValueInGbMap),
                                    new ColumnObject(MachineDetailsSchema.OperatingSystemIs64Bit, DbType.Boolean, machineDetails.OperatingSystemIs64Bit),
                                    new ColumnObject(MachineDetailsSchema.OperatingSystem, DbType.String, machineDetails.OperatingSystem),
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

        private IDbCommand BuildProcessDetailsCommand(IDbConnection connection, IDbTransaction transaction, Guid processDetailsId, ProcessDetails processDetails)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(ProcessDetailsSchema.Id, DbType.Guid, processDetailsId),
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

        private IDbCommand BuildDiagnosticsCommand(IDbConnection connection, IDbTransaction transaction, DateTime sampledUtc, Guid diagnosticsId, Guid machineDetailsId, Guid processDetailsId)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(DiagnosticsSchema.Id, DbType.Guid, diagnosticsId),
                                    new ColumnObject(DiagnosticsSchema.MachineDetailsId, DbType.Guid, machineDetailsId),
                                    new ColumnObject(DiagnosticsSchema.ProcessDetailsId, DbType.Guid, processDetailsId),
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

        private IDbCommand BuildAssemblyDetailsCommand(IDbConnection connection, IDbTransaction transaction, Guid assemblyDetailsId, AssemblyDetails assemblyDetails)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(AssemblyDetailsSchema.Id, DbType.Guid, assemblyDetailsId),
                                    new ColumnObject(AssemblyDetailsSchema.Name, DbType.String, assemblyDetails.Name),
                                    new ColumnObject(AssemblyDetailsSchema.Version, DbType.String, assemblyDetails.Version),
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

        private IDbCommand BuildAssemblyDiagnosticsCrossReferenceCommand(IDbConnection connection, IDbTransaction transaction, Guid diagnosticsId, Guid assemblyDetailsId)
        {
            var columnSet = new[]
                                {
                                    new ColumnObject(DiagnosticsAssemblyCrossReferenceSchema.Id, DbType.Guid, Guid.NewGuid()),
                                    new ColumnObject(DiagnosticsAssemblyCrossReferenceSchema.DiagnosticsId, DbType.Guid, diagnosticsId),
                                    new ColumnObject(DiagnosticsAssemblyCrossReferenceSchema.AssemblyDetailsId, DbType.Guid, assemblyDetailsId),
                                };

            var sql = SqlCommon.BuildInsertStatement(DiagnosticsAssemblyCrossReferenceSchema.TableName, columnSet);

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