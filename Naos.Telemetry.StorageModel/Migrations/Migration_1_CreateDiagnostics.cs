// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration_1_CreateDiagnostics.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using System;

    using FluentMigrator;

    using static System.FormattableString;

    /// <summary>
    /// The migration that adds the diagnostics tables.
    /// </summary>
    [Migration(MigrationVersion.CreateDiagnosticsSchema, TransactionBehavior.None)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Improves readability.")]
    public class Migration_1_CreateDiagnostics : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Create.Table(DiagnosticsSchema.TableName)
                .WithColumn(DiagnosticsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(DiagnosticsSchema.SampledUtc).AsDateTime().NotNullable().Indexed(Invariant($"IX_{DiagnosticsSchema.TableName}_{DiagnosticsSchema.SampledUtc}"))
                .WithColumn(DiagnosticsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(MachineDetailsSchema.TableName)
                .WithColumn(MachineDetailsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(MachineDetailsSchema.DiagnosticsId).AsGuid().NotNullable().ForeignKey(MachineDetailsSchema.ForeignKeyNameDiagnosticsId, DiagnosticsSchema.TableName, DiagnosticsSchema.Id)
                .WithColumn(MachineDetailsSchema.MachineName).AsString().NotNullable()
                .WithColumn(MachineDetailsSchema.MachineNameMapJson).AsString(int.MaxValue).NotNullable()
                .WithColumn(MachineDetailsSchema.ProcessorCount).AsInt32().NotNullable()
                .WithColumn(MachineDetailsSchema.PhysicalMemoryInGb).AsDecimal().NotNullable()
                .WithColumn(MachineDetailsSchema.MemoryMapJson).AsString(int.MaxValue).NotNullable()
                .WithColumn(MachineDetailsSchema.OperatingSystemIs64Bit).AsBoolean().NotNullable()
                .WithColumn(MachineDetailsSchema.OperatingSystemJson).AsString(1024).NotNullable()
                .WithColumn(MachineDetailsSchema.ClrVersion).AsString(1024).NotNullable()
                .WithColumn(MachineDetailsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(ProcessDetailsSchema.TableName)
                .WithColumn(ProcessDetailsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(ProcessDetailsSchema.DiagnosticsId).AsGuid().NotNullable().ForeignKey(ProcessDetailsSchema.ForeignKeyNameDiagnosticsId, DiagnosticsSchema.TableName, DiagnosticsSchema.Id)
                .WithColumn(ProcessDetailsSchema.Name).AsString(1024).NotNullable()
                .WithColumn(ProcessDetailsSchema.FilePath).AsString(int.MaxValue).NotNullable()
                .WithColumn(ProcessDetailsSchema.FileVersion).AsString(1024).NotNullable()
                .WithColumn(ProcessDetailsSchema.ProductVersion).AsString(1024).NotNullable()
                .WithColumn(ProcessDetailsSchema.RunningAsAdmin).AsBoolean().NotNullable()
                .WithColumn(ProcessDetailsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(AssemblyDetailsSchema.TableName)
                .WithColumn(AssemblyDetailsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(AssemblyDetailsSchema.DiagnosticsId).AsGuid().NotNullable().ForeignKey(AssemblyDetailsSchema.ForeignKeyNameDiagnosticsId, DiagnosticsSchema.TableName, DiagnosticsSchema.Id)
                .WithColumn(AssemblyDetailsSchema.Name).AsString(1024).NotNullable()
                .WithColumn(AssemblyDetailsSchema.VersionJson).AsString(1024).NotNullable()
                .WithColumn(AssemblyDetailsSchema.FilePath).AsString(int.MaxValue).NotNullable()
                .WithColumn(AssemblyDetailsSchema.FrameworkVersion).AsString(1024).NotNullable()
                .WithColumn(AssemblyDetailsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        /// <inheritdoc />
        public override void Down()
        {
            throw new NotImplementedException("Down migration not supported for this schema");
        }
    }
}
