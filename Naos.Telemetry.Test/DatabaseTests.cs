// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseTests.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.VisualBasic.Logging;

    using Naos.Compression.Domain;
    using Naos.Database.Migrator;
    using Naos.Diagnostics.Domain;
    using Naos.Diagnostics.Recipes;
    using Naos.Serialization.Domain;
    using Naos.Serialization.Domain.Extensions;
    using Naos.Serialization.Json;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.Reader;
    using Naos.Telemetry.StorageModel;
    using Naos.Telemetry.Writer;

    using OBeautifulCode.Reflection.Recipes;

    using Spritely.Cqrs;

    using Xunit;

    public static class DatabaseTests
    {
        [Fact(Skip = "For local testing")]
        public static void Create_schema_on_localhost()
        {
            // Arrange
            var database =
                new TelemetryDatabase { ConnectionSettings = new DatabaseConnectionSettings { Server = "localhost", Database = "Telemetry" } };
            var connectionString = database.ConnectionSettings.ToInsecureConnectionString();
            var announcements = new List<string>();

            // Act
            MigrationExecutor.Up(
                typeof(MigrationVersion).Assembly,
                connectionString,
                database.ConnectionSettings.Database,
                MigrationVersion.CreateEventSchema,
                msg => announcements.Add(msg),
                TimeSpan.FromSeconds(database.ConnectionSettings.DefaultCommandTimeoutInSeconds ?? 30));

            // Assert
            announcements.Should().NotBeEmpty();
        }

        [Fact(Skip = "For local testing")]
        public static async Task Write_and_process_items()
        {
            // Arrange
            var serializationDescription = new SerializationDescription(SerializationFormat.Json, SerializationRepresentation.String);
            var database =
                new TelemetryDatabase { ConnectionSettings = new DatabaseConnectionSettings { Server = "localhost", Database = "Telemetry" } };
            var writer = TelemetryWriterBuilder.Build(database);
            var reader = TelemetryReaderBuilder.Build(database);

            var sampledUtc = DateTime.UtcNow;

            var machineDetails = DomainFactory.CreateMachineDetails();
            var processDetails = DomainFactory.CreateProcessDetails();
            var assemblyFilePath = typeof(IAmTelemetryItem).Assembly.GetCodeBaseAsPathInsteadOfUri();
            var assemblies = DiagnosticsTelemetry.GetSiblingAssemblyFilePaths(assemblyFilePath).Select(_ => AssemblyDetails.CreateFromFile(_)).ToList();
            var diagnosticsTelemetry = new DiagnosticsTelemetry(sampledUtc, machineDetails, processDetails, assemblies);

            var eventTelemetry = new EventTelemetry(
                sampledUtc,
                "Event",
                new Dictionary<string, string> { { "PropertyKey1", "PropertyValue1" }, { "PropertyKey2", null }, },
                new Dictionary<string, decimal?> { { "MetricKey1", null }, { "MetricKey2", 33m } });

            var rawDiagnostics = new RawQueueItem(
                Guid.NewGuid(),
                sampledUtc,
                TelemetryWriter.JsonSerializer.SerializeToString(
                    diagnosticsTelemetry.ToDescribedSerializationUsingSpecificFactory(
                        serializationDescription,
                        JsonSerializerFactory.Instance,
                        CompressorFactory.Instance)),
                "{}",
                "{}",
                "{}");

            var rawEvent = new RawQueueItem(
                Guid.NewGuid(),
                sampledUtc,
                TelemetryWriter.JsonSerializer.SerializeToString(
                    eventTelemetry.ToDescribedSerializationUsingSpecificFactory(
                        serializationDescription,
                        JsonSerializerFactory.Instance,
                        CompressorFactory.Instance)),
                "{}",
                "{}",
                "{}");

            // Act & Assert
            await writer.WriteRawItemToQueueAsync(new[] { rawDiagnostics });
            await writer.WriteRawItemToQueueAsync(new[] { rawEvent });

            var actualRaw = await reader.GetQueuedRawItemsAsync();
            actualRaw.Count.Should().Be(2);

            var diagnosticsRaw = actualRaw.Single(_ => _.TelemetryObjectDescribedSerializationJson.Contains(nameof(DiagnosticsTelemetry)));
            var readDiagnostics = (DiagnosticsTelemetry)TelemetryWriter.JsonSerializer
                .Deserialize<DescribedSerialization>(diagnosticsRaw.TelemetryObjectDescribedSerializationJson)
                .DeserializePayloadUsingSpecificFactory(JsonSerializerFactory.Instance, CompressorFactory.Instance);

            var eventRaw = actualRaw.Single(_ => _.TelemetryObjectDescribedSerializationJson.Contains(nameof(EventTelemetry)));
            var readEvent = (EventTelemetry)TelemetryWriter.JsonSerializer
                .Deserialize<DescribedSerialization>(eventRaw.TelemetryObjectDescribedSerializationJson)
                .DeserializePayloadUsingSpecificFactory(JsonSerializerFactory.Instance, CompressorFactory.Instance);

            await writer.WriteDiagnosticsTelemetryAsync(new[] { readDiagnostics });
            await writer.WriteEventTelemetryAsync(new[] { readEvent });

            await writer.RemoveItemsFromQueueAsync(new[] { rawDiagnostics.Id, rawEvent.Id });
        }
    }
}
