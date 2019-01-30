// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration_2_CreateEvent.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using System;
    using System.Collections.Generic;
    using FluentMigrator;

    using static System.FormattableString;

    /// <summary>
    /// The migration that adds the event tables.
    /// </summary>
    [Migration(MigrationVersion.CreateEventSchema, TransactionBehavior.None)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Improves readability.")]
    public class Migration_2_CreateEvent : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Create.Table(EventSourceSchema.TableName)
                .WithColumn(EventSourceSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(EventSourceSchema.MachineName).AsString(1024).NotNullable()
                .WithColumn(EventSourceSchema.ProcessName).AsString(1024).Nullable()
                .WithColumn(EventSourceSchema.ProcessFileVersion).AsString(1024).Nullable()
                .WithColumn(EventSourceSchema.CallingMethod).AsString(int.MaxValue).Nullable()
                .WithColumn(EventSourceSchema.StackTrace).AsString(int.MaxValue).Nullable()
                .WithColumn(EventSourceSchema.CallingTypeJson).AsString(int.MaxValue).Nullable()
                .WithColumn(EventSourceSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Insert.IntoTable(EventSourceSchema.TableName).Row(new Dictionary<string, object>
            {
                { EventSourceSchema.Id, Guid.Empty },
                { EventSourceSchema.MachineName, "Unknown" },
                { EventSourceSchema.ProcessName, "Unknown" },
                { EventSourceSchema.ProcessFileVersion, "Unknown" },
                { EventSourceSchema.CallingMethod, "Unknown" },
                { EventSourceSchema.StackTrace, "Unknown" },
                { EventSourceSchema.CallingTypeJson, "Unknown" },
            });

            this.Create.Table(EventSchema.TableName)
                .WithColumn(EventSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(EventSchema.EventSourceId).AsGuid().NotNullable().ForeignKey(EventSchema.ForeignKeyNameEventSourceId, EventSourceSchema.TableName, EventSourceSchema.Id)
                .WithColumn(EventSchema.Name).AsString(1024).NotNullable()
                .WithColumn(EventSchema.SampledUtc).AsDateTime().NotNullable().Indexed(Invariant($"IX_{EventSchema.TableName}_{EventSchema.SampledUtc}"))
                .WithColumn(EventSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(PropertySchema.TableName)
                .WithColumn(PropertySchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(PropertySchema.EventId).AsGuid().NotNullable().ForeignKey(PropertySchema.ForeignKeyNameEventId, EventSchema.TableName, EventSchema.Id)
                .WithColumn(PropertySchema.Name).AsString(1024).NotNullable()
                .WithColumn(PropertySchema.Value).AsString(int.MaxValue).Nullable()
                .WithColumn(PropertySchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Create.Table(MetricSchema.TableName)
                .WithColumn(MetricSchema.Id).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(MetricSchema.EventId).AsGuid().NotNullable().ForeignKey(MetricSchema.ForeignKeyNameEventId, EventSchema.TableName, EventSchema.Id)
                .WithColumn(MetricSchema.Name).AsString(1024).NotNullable()
                .WithColumn(MetricSchema.Value).AsDecimal().Nullable()
                .WithColumn(MetricSchema.RowCreatedUtc).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            this.Execute.Sql(EventSourceSchema.Sprocs.InsertEventTelemetrySourceAsNecessary.Script);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Execute.Sql("DROP PROCEDURE " + EventSourceSchema.Sprocs.InsertEventTelemetrySourceAsNecessary.Name);

            this.Delete.Table(MetricSchema.TableName);
            this.Delete.Table(PropertySchema.TableName);
            this.Delete.Table(EventSchema.TableName);
            this.Delete.Table(EventSourceSchema.TableName);
        }
    }
}