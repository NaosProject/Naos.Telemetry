// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationVersion.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
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
        /// Version of the migration that creates the table for <see cref="Domain.DiagnosticsTelemetry" />.
        /// </summary>
        public const long CreateDiagnosticsSchema = 1;

        /// <summary>
        /// Version of the migration that creates the table for <see cref="Domain.EventTelemetry"/>.
        /// </summary>
        public const long CreateEventSchema = 2;
    }
}
