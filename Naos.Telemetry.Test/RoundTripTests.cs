// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundTripTests.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using Naos.Compression.Domain;
    using Naos.Diagnostics.Domain;
    using Naos.Diagnostics.Recipes;
    using Naos.Serialization.Domain;
    using Naos.Serialization.Domain.Extensions;
    using Naos.Serialization.Json;
    using Naos.Telemetry.Domain;

    using OBeautifulCode.Reflection.Recipes;

    using Xunit;

    public static class RoundtripTests
    {
        [Fact]
        public static void DiagnosticsTelemetry___can_roundtrip_DescribedSerialization_to_json()
        {
            // Arrange
            var machineDetails = DomainFactory.CreateMachineDetails();
            var processDetails = DomainFactory.CreateProcessDetails();
            var assemblyFilePath = typeof(IAmTelemetryItem).Assembly.GetCodeBaseAsPathInsteadOfUri();
            var assemblies = DiagnosticsTelemetry.GetSiblingAssemblyFilePaths(assemblyFilePath).Select(_ => AssemblyDetails.CreateFromFile(_)).ToList();

            var diagnosticsTelemetry = new DiagnosticsTelemetry(machineDetails, processDetails, assemblies);
            var serializationDescription = new SerializationDescription(SerializationFormat.Json, SerializationRepresentation.String);

            // Act
            var serialized = diagnosticsTelemetry.ToDescribedSerializationUsingSpecificFactory(
                serializationDescription,
                JsonSerializerFactory.Instance,
                CompressorFactory.Instance);
            var rehydrated = serialized.DeserializePayloadUsingSpecificFactory<DiagnosticsTelemetry>(JsonSerializerFactory.Instance, CompressorFactory.Instance);

            // Assert
            rehydrated.Should().NotBeNull();
        }
    }
}
