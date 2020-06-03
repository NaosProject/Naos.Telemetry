// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Test
{
    using FakeItEasy;
    using FluentAssertions;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.Serialization.Bson;
    using Naos.Telemetry.Serialization.Json;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using Xunit;

    public static class SerializationTests
    {
        private static readonly ObcBsonSerializer BsonSerializer = new ObcBsonSerializer(typeof(TelemetryBsonSerializationConfiguration).ToBsonSerializationConfigurationType());
        private static readonly ObcJsonSerializer JsonSerializer = new ObcJsonSerializer(typeof(TelemetryJsonSerializationConfiguration).ToJsonSerializationConfigurationType());

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
            var bsonSerializer = new ObcBsonSerializer(typeof(TelemetryBsonSerializationConfiguration).ToBsonSerializationConfigurationType());
            var jsonSerializer = new ObcJsonSerializer(typeof(TelemetryJsonSerializationConfiguration).ToJsonSerializationConfigurationType());

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
            var bsonSerializer = new ObcBsonSerializer(typeof(TelemetryBsonSerializationConfiguration).ToBsonSerializationConfigurationType());
            var jsonSerializer = new ObcJsonSerializer(typeof(TelemetryJsonSerializationConfiguration).ToJsonSerializationConfigurationType());

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
