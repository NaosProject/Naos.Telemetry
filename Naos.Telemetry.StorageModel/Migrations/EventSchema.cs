// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSchema.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using static System.FormattableString;

#pragma warning disable SA1203 // Constants should appear before fields
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class EventSchema
    {
        public const string TableName = "Event";

        public const string Id = "Id";

        public const string EventSourceId = "EventSourceId";

        public static readonly string ForeignKeyNameEventSourceId = Invariant($"FK__{TableName}_{EventSourceId}__{EventSourceSchema.TableName}_{EventSourceSchema.Id}");

        public const string Name = "Name";

        public const string SampledUtc = "SampledUtc";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }

    public static class EventSourceSchema
    {
        public const string TableName = "EventSource";

        public const string Id = "Id";

        public const string MachineName = "MachineName";

        public const string ProcessName = "ProcessName";

        public const string ProcessFileVersion = "ProcessFileVersion";

        public const string CallingMethod = "CallingMethod";

        public const string StackTrace = "StackTrace";

        public const string CallingTypeJson = "CallingTypeJson";

        public const string RowCreatedUtc = "RowCreatedUtc";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sprocs", Justification = "Spelling/name is correct.")]
        public static class Sprocs
        {
            public static class InsertEventTelemetrySourceAsNecessary
            {
                public const string Name = "InsertEventTelemetrySourceAsNecessary";

                public const string Script = @"
                            CREATE PROCEDURE InsertEventTelemetrySourceAsNecessary 
                            (
	                            @MachineName nvarchar(2048),
	                            @ProcessName nvarchar(2048),
	                            @ProcessFileVersion nvarchar(2048),
	                            @CallingMethod nvarchar(max),
	                            @StackTrace nvarchar(max),
	                            @CallingTypeJson nvarchar(max)
                            )
                            AS
                            BEGIN
                              IF @MachineName = 'null' OR @MachineName IS NULL
                              BEGIN
                                 SET @MachineName = 'Unknown'
                              END
                              IF @ProcessName = 'null'
                              BEGIN
                                 SET @ProcessName = NULL
                              END
                              IF @ProcessFileVersion = 'null'
                              BEGIN
                                 SET @ProcessFileVersion = NULL
                              END
                              IF @CallingMethod = 'null'
                              BEGIN
                                 SET @CallingMethod = NULL
                              END
                              IF @Stacktrace = 'null'
                              BEGIN
                                 SET @Stacktrace = NULL
                              END
                              IF @CallingTypeJson = 'null'
                              BEGIN
                                 SET @CallingTypeJson = NULL
                              END

                              DECLARE @ret uniqueidentifier
                              SELECT @ret = Id 
                              FROM [EventSource]
                              WHERE MachineName = @MachineName
                              AND   ISNULL(ProcessName, 'null') = ISNULL(@ProcessName, 'null')
                              AND   ISNULL(ProcessFileVersion, 'null') = ISNULL(@ProcessFileVersion, 'null')
                              AND   ISNULL(CallingMethod , 'null')= ISNULL(@CallingMethod, 'null')
                              AND   ISNULL(Stacktrace, 'null') = ISNULL(@StackTrace, 'null')
                              AND   ISNULL(CallingTypeJson, 'null') = ISNULL(@CallingTypeJson, 'null')

                              IF @ret IS NULL
                              BEGIN
                                 SELECT @ret = NEWID()
                                 INSERT INTO EventSource (ID, MachineName, ProcessName, ProcessFileVersion, CallingMethod, StackTrace, CallingTypeJson)
	                             VALUES (@ret, @MachineName, @ProcessName, @ProcessFileVersion, @CallingMethod, @StackTrace, @CallingTypeJson)
                              END
                              SELECT @ret
                            END
                            ";
            }
        }
    }

    public static class PropertySchema
    {
        public const string TableName = "Property";

        public const string Id = "Id";

        public const string EventId = "EventId";

        public static readonly string ForeignKeyNameEventId = Invariant($"FK__{TableName}_{EventId}__{EventSchema.TableName}_{EventSchema.Id}");

        public const string Name = "Name";

        public const string Value = "Value";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }

    public static class MetricSchema
    {
        public const string TableName = "Metric";

        public const string Id = "Id";

        public const string EventId = "EventId";

        public static readonly string ForeignKeyNameEventId = Invariant($"FK__{TableName}_{EventId}__{EventSchema.TableName}_{EventSchema.Id}");

        public const string Name = "Name";

        public const string Value = "Value";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }
#pragma warning restore SA1203 // Constants should appear before fields
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}