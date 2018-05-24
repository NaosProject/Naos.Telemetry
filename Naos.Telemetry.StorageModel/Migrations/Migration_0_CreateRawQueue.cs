// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration_0_CreateRawQueue.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using System;

    using FluentMigrator;

    /// <summary>
    /// The migration that adds the diagnostics tables.
    /// </summary>
    [Migration(MigrationVersion.CreateRawQueueSchema, TransactionBehavior.None)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Improves readability.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Spelling/name is correct.")]
    public class Migration_0_CreateRawQueue : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Create.Table(RawQueueSchema.TableName)
                .WithColumn(RawQueueSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(RawQueueSchema.SampledUtc).AsDateTime().NotNullable()
                .WithColumn(RawQueueSchema.TelemetryObjectJson).AsString().NotNullable()
                .WithColumn(RawQueueSchema.TelemetryObjectTypeDescriptionJson).AsString().NotNullable()
                .WithColumn(RawQueueSchema.LogItemKindJson).AsString().NotNullable()
                .WithColumn(RawQueueSchema.LogItemContextJson).AsString().NotNullable()
                .WithColumn(RawQueueSchema.LogItemCorrelationsJson).AsString().NotNullable()
                .WithColumn(RawQueueSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        /// <inheritdoc />
        public override void Down()
        {
            throw new NotImplementedException("Down migration not supported for this schema");
        }
    }
}