// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration_0_CreateDiagnostics.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
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
    public class Migration_0_CreateDiagnostics : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Create.Table(MachineDetailsSchema.TableName)
                .WithColumn(MachineDetailsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(MachineDetailsSchema.MachineName).AsString().NotNullable()
                .WithColumn(MachineDetailsSchema.MachineNameMapJson).AsString().NotNullable()
                .WithColumn(MachineDetailsSchema.ProcessorCount).AsInt16().NotNullable()
                .WithColumn(MachineDetailsSchema.PhysicalMemoryInGb).AsInt16().NotNullable()
                .WithColumn(MachineDetailsSchema.MemoryMapJson).AsString().NotNullable()
                .WithColumn(MachineDetailsSchema.OperatingSystemIs64Bit).AsBoolean().NotNullable()
                .WithColumn(MachineDetailsSchema.OperatingSystem).AsString().NotNullable()
                .WithColumn(MachineDetailsSchema.ClrVersion).AsString().NotNullable()
                .WithColumn(MachineDetailsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(ProcessDetailsSchema.TableName)
                .WithColumn(ProcessDetailsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(ProcessDetailsSchema.Name).AsString().NotNullable()
                .WithColumn(ProcessDetailsSchema.FilePath).AsString().NotNullable()
                .WithColumn(ProcessDetailsSchema.FileVersion).AsString().NotNullable()
                .WithColumn(ProcessDetailsSchema.ProductVersion).AsString().NotNullable()
                .WithColumn(ProcessDetailsSchema.RunningAsAdmin).AsBoolean().NotNullable()
                .WithColumn(ProcessDetailsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(AssemblyDetailsSchema.TableName)
                .WithColumn(AssemblyDetailsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(AssemblyDetailsSchema.Name).AsString().NotNullable()
                .WithColumn(AssemblyDetailsSchema.Version).AsString().NotNullable()
                .WithColumn(AssemblyDetailsSchema.FilePath).AsString().NotNullable()
                .WithColumn(AssemblyDetailsSchema.FrameworkVersion).AsString().NotNullable()
                .WithColumn(AssemblyDetailsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(DiagnosticsSchema.TableName)
                .WithColumn(DiagnosticsSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(DiagnosticsSchema.SampledUtc).AsDateTime().NotNullable().Indexed(Invariant($"IX_{DiagnosticsSchema.TableName}_{DiagnosticsSchema.SampledUtc}"))
                .WithColumn(DiagnosticsSchema.MachineDetailsId).AsGuid().NotNullable().ForeignKey(DiagnosticsSchema.ForeignKeyNameMachineDetailsId, MachineDetailsSchema.TableName, MachineDetailsSchema.Id)
                .WithColumn(DiagnosticsSchema.ProcessDetailsId).AsGuid().NotNullable().ForeignKey(DiagnosticsSchema.ForeignKeyNameProcessDetailsId, ProcessDetailsSchema.TableName, ProcessDetailsSchema.Id)
                .WithColumn(DiagnosticsSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(DiagnosticsAssemblyCrossReferenceSchema.TableName)
                .WithColumn(DiagnosticsAssemblyCrossReferenceSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(DiagnosticsAssemblyCrossReferenceSchema.DiagnosticsId).AsGuid().NotNullable().ForeignKey(DiagnosticsAssemblyCrossReferenceSchema.ForeignKeyNameDiagnosticsId, DiagnosticsSchema.TableName, DiagnosticsSchema.Id)
                .WithColumn(DiagnosticsAssemblyCrossReferenceSchema.AssemblyDetailsId).AsGuid().NotNullable().ForeignKey(DiagnosticsAssemblyCrossReferenceSchema.ForeignKeyNameAssemblyDetailsId, AssemblyDetailsSchema.TableName, AssemblyDetailsSchema.Id)
                .WithColumn(DiagnosticsAssemblyCrossReferenceSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        /// <inheritdoc />
        public override void Down()
        {
            throw new NotImplementedException("Down migration not supported for this schema");
        }
    }
}