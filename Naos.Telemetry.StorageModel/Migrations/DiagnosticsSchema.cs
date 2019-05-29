// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagnosticsSchema.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using static System.FormattableString;

#pragma warning disable SA1203 // Constants should appear before fields
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DiagnosticsSchema
    {
        public const string TableName = "Diagnostics";

        public const string Id = "Id";

        public const string SampledUtc = "SampledUtc";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }

    public static class MachineDetailsSchema
    {
        public const string TableName = "MachineDetails";

        public const string Id = "Id";

        public const string DiagnosticsId = "DiagnosticsId";

        public static readonly string ForeignKeyNameDiagnosticsId = Invariant($"FK__{TableName}_{DiagnosticsId}__{DiagnosticsSchema.TableName}_{DiagnosticsSchema.Id}");

        public const string MachineName = "MachineName";

        public const string MachineNameMapJson = "MachineNameMapJson";

        public const string ProcessorCount = "ProcessorCount";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Gb", Justification = "Spelling/name is correct.")]
        public const string PhysicalMemoryInGb = "PhysicalMemoryInGb";

        public const string MemoryMapJson = "MemoryMapJson";

        public const string OperatingSystemIs64Bit = "OperatingSystemIs64Bit";

        public const string OperatingSystemJson = "OperatingSystemJson";

        public const string ClrVersion = "ClrVersion";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }

    public static class ProcessDetailsSchema
    {
        public const string TableName = "ProcessDetails";

        public const string Id = "Id";

        public const string DiagnosticsId = "DiagnosticsId";

        public static readonly string ForeignKeyNameDiagnosticsId = Invariant($"FK__{TableName}_{DiagnosticsId}__{DiagnosticsSchema.TableName}_{DiagnosticsSchema.Id}");

        public const string Name = "Name";

        public const string FilePath = "FilePath";

        public const string FileVersion = "FileVersion";

        public const string ProductVersion = "ProductVersion";

        public const string RunningAsAdmin = "RunningAsAdmin";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }

    public static class AssemblyDetailsSchema
    {
        public const string TableName = "AssemblyDetails";

        public const string Id = "Id";

        public const string DiagnosticsId = "DiagnosticsId";

        public static readonly string ForeignKeyNameDiagnosticsId = Invariant($"FK__{TableName}_{DiagnosticsId}__{DiagnosticsSchema.TableName}_{DiagnosticsSchema.Id}");

        public const string Name = "Name";

        public const string VersionJson = "VersionJson";

        public const string FilePath = "FilePath";

        public const string FrameworkVersion = "FrameworkVersion";

        public const string RowCreatedUtc = "RowCreatedUtc";
    }
#pragma warning restore SA1203 // Constants should appear before fields
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}