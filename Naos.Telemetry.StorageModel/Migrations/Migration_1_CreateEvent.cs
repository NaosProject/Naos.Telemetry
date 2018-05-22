// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration_1_CreateEvent.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using System;

    using FluentMigrator;

    using static System.FormattableString;

    /// <summary>
    /// The migration that adds the event tables.
    /// </summary>
    [Migration(MigrationVersion.CreateEventSchema, TransactionBehavior.None)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Improves readability.")]
    public class Migration_1_CreateEvent : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Create.Table(EventSchema.TableName)
                .WithColumn(EventSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(EventSchema.SampledUtc).AsDateTime().NotNullable().Indexed(Invariant($"IX_{EventSchema.TableName}_{EventSchema.SampledUtc}"))
                .WithColumn(EventSchema.Name).AsString().NotNullable()
                .WithColumn(EventSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(PropertySchema.TableName)
                .WithColumn(PropertySchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(PropertySchema.EventId).AsGuid().NotNullable().ForeignKey(PropertySchema.ForeignKeyNameEventId, EventSchema.TableName, EventSchema.Id)
                .WithColumn(PropertySchema.Name).AsString().NotNullable()
                .WithColumn(PropertySchema.Value).AsString().Nullable()
                .WithColumn(PropertySchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(MetricSchema.TableName)
                .WithColumn(MetricSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(MetricSchema.EventId).AsGuid().NotNullable().ForeignKey(PropertySchema.ForeignKeyNameEventId, EventSchema.TableName, EventSchema.Id)
                .WithColumn(MetricSchema.Name).AsString().NotNullable()
                .WithColumn(MetricSchema.Value).AsDecimal().Nullable()
                .WithColumn(MetricSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        /// <inheritdoc />
        public override void Down()
        {
            throw new NotImplementedException("Down migration not supported for this schema");
        }
    }
}