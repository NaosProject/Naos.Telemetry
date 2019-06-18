// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Test
{
    using FakeItEasy;
    using FluentAssertions;
    using Naos.Serialization.Bson;
    using Naos.Serialization.Json;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.Serialization.Bson;
    using Naos.Telemetry.Serialization.Json;
    using Xunit;

    public static class SerializationTests
    {
        private static readonly NaosBsonSerializer BsonSerializer = new NaosBsonSerializer(typeof(TelemetryBsonConfiguration));
        private static readonly NaosJsonSerializer JsonSerializer = new NaosJsonSerializer(typeof(TelemetryJsonConfiguration));

        [Fact]
        public static void EventTelemetry_Roundtrips()
        {
            // Arrange
            var expected = A.Dummy<EventTelemetry>();

            // Act
            var actualBsonString = BsonSerializer.SerializeToString(expected);
            var actualBson = BsonSerializer.Deserialize<EventTelemetry>(actualBsonString);

            var actualJsonString = JsonSerializer.SerializeToString(expected);
            var actualJson = JsonSerializer.Deserialize<EventTelemetry>(actualJsonString);

            // Assert
            actualBson.Should().Be(expected);
            actualJson.Should().Be(expected);
        }

        [Fact]
        public static void DiagnosticsTelemetry_Roundtrips()
        {
            // Arrange
            var expected = A.Dummy<DiagnosticsTelemetry>();
            var bsonSerializer = new NaosBsonSerializer(typeof(TelemetryBsonConfiguration));
            var jsonSerializer = new NaosJsonSerializer(typeof(TelemetryJsonConfiguration));

            // Act
            var actualBsonString = bsonSerializer.SerializeToString(expected);
            var actualBson = bsonSerializer.Deserialize<DiagnosticsTelemetry>(actualBsonString);

            var actualJsonString = jsonSerializer.SerializeToString(expected);
            var actualJson = jsonSerializer.Deserialize<DiagnosticsTelemetry>(actualJsonString);

            // Assert
            actualBson.Should().Be(expected);
            actualJson.Should().Be(expected);
        }

        [Fact]
        public static void StopwatchSnapshot_Roundtrips()
        {
            // Arrange
            var expected = A.Dummy<StopwatchSnapshot>();
            var bsonSerializer = new NaosBsonSerializer(typeof(TelemetryBsonConfiguration));
            var jsonSerializer = new NaosJsonSerializer(typeof(TelemetryJsonConfiguration));

            // Act
            var actualBsonString = bsonSerializer.SerializeToString(expected);
            var actualBson = bsonSerializer.Deserialize<StopwatchSnapshot>(actualBsonString);

            var actualJsonString = jsonSerializer.SerializeToString(expected);
            var actualJson = jsonSerializer.Deserialize<StopwatchSnapshot>(actualJsonString);

            // Assert
            actualBson.Should().Be(expected);
            actualJson.Should().Be(expected);
        }
    }
}
