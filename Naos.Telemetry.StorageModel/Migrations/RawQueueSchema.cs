// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawQueueSchema.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
#pragma warning disable SA1203 // Constants should appear before fields
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class RawQueueSchema
    {
        public const string TableName = "RawQueue";

        public const string Id = "Id";

        public const string SampledUtc = "SampledUtc";

        public const string TelemetryObjectDescribedSerializationJson = "TelemetryObjectDescribedSerializationJson";

        public const string LogItemKindJson = "LogItemKindJson";

        public const string LogItemContextJson = "LogItemContextJson";

        public const string LogItemCorrelationsJson = "LogItemCorrelationsJson";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }
#pragma warning restore SA1203 // Constants should appear before fields
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}