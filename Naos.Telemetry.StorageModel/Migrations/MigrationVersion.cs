// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationVersion.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    /// <summary>
    /// Database migration versions.
    /// </summary>
    public static class MigrationVersion
    {
        /// <summary>
        /// Version of the migration that creates the table for raw items to be processed.
        /// </summary>
        public const long CreateRawQueueSchema = 0;

        /// <summary>
        /// Version of the migration that creates the table for <see cref="Domain.DiagnosticsTelemetry" />
        /// </summary>
        public const long CreateDiagnosticsSchema = 1;

        /// <summary>
        /// Version of the migration that creates the table for <see cref="Domain.EventTelemetry"/>
        /// </summary>
        public const long CreateEventSchema = 2;
    }
}