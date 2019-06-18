// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopwatchSnapshotTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using FakeItEasy;
    using FluentAssertions;
    using Naos.Serialization.Bson;
    using Naos.Serialization.Json;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.Serialization.Bson;
    using Naos.Telemetry.Serialization.Json;
    using OBeautifulCode.AutoFakeItEasy;
    using Xunit;

    using static System.FormattableString;

    public static class StopwatchSnapshotTests
    {
        [Fact]
        public static void Constructor__Should_throw_ArgumentNullException___When_parameter_name_is_null()
        {
            // Arrange
            Action action = () => new StopwatchSnapshot(null, A.Dummy<decimal>(), A.Dummy<bool>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Value cannot be null.\r\nParameter name: name");
        }

        [Fact]
        public static void Constructor__Should_throw_ArgumentException___When_parameter_name_is_empty_string()
        {
            // Arrange
            Action action = () => new StopwatchSnapshot(string.Empty, A.Dummy<decimal>(), A.Dummy<bool>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Value cannot be null.\r\nParameter name: name");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "whitespace", Justification = "Name/spelling is correct.")]
        [Fact]
        public static void Constructor__Should_throw_ArgumentException___When_parameter_name_is_whitespace()
        {
            // Arrange
            Action action = () => new StopwatchSnapshot("\t", A.Dummy<decimal>(), A.Dummy<bool>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Value cannot be null.\r\nParameter name: name");
        }

        [Fact]
        public static void StopwatchSnapshot_properties__Should_return_same_StopwatchSnapshot_passed_to_constructor___When_getting()
        {
            // Arrange
            var name = A.Dummy<string>();
            var elapsedMilliseconds = A.Dummy<decimal>();
            var isRunning = A.Dummy<bool>();
            var systemUnderTest = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning);

            // Act
            var actualName = systemUnderTest.Name;
            var actualElapsedMilliseconds = systemUnderTest.ElapsedMilliseconds;
            var actualIsRunning = systemUnderTest.IsRunning;

            // Assert
            actualName.Should().BeSameAs(name);
            actualElapsedMilliseconds.Should().Be(elapsedMilliseconds);
            actualIsRunning.Should().Be(isRunning);
        }

        [Fact]
        public static void EqualityLogic___Should_be_valid___When_different_data()
        {
            // Arrange
            var name = A.Dummy<string>();
            var elapsedMilliseconds = A.Dummy<decimal>();
            var isRunning = A.Dummy<bool>();

            var notEqualTests = new[]
                                    {
                                        new
                                            {
                                                First = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning),
                                                Second = new StopwatchSnapshot(A.Dummy<string>(), elapsedMilliseconds, isRunning),
                                            },
                                        new
                                            {
                                                First = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning),
                                                Second = new StopwatchSnapshot(name, A.Dummy<decimal>(), isRunning),
                                            },
                                        new
                                            {
                                                First = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning),
                                                Second = new StopwatchSnapshot(name, elapsedMilliseconds, !isRunning),
                                            },
                                        new
                                            {
                                                First = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning),
                                                Second = (StopwatchSnapshot)null,
                                            },
                                        new
                                            {
                                                First = (StopwatchSnapshot)null,
                                                Second = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning),
                                            },
                                    }.ToList();

            // Act & Assert
            notEqualTests.ForEach(
                _ =>
                {
                    if (_.First != null && _.Second != null)
                    {
                        (_.First.GetHashCode() == _.Second.GetHashCode()).Should().BeFalse(Invariant($"First: {_.First}; Second: {_.Second}"));
                        _.First.Equals(_.Second).Should().BeFalse(Invariant($"First: {_.First}; Second: {_.Second}"));
                        _.First.Equals((object)_.Second).Should().BeFalse(Invariant($"First: {_.First}; Second: {_.Second}"));
                    }

                    (_.First == _.Second).Should().BeFalse(Invariant($"First: {_.First}; Second: {_.Second}"));
                    (_.First != _.Second).Should().BeTrue(Invariant($"First: {_.First}; Second: {_.Second}"));
                });
        }

        [Fact]
        public static void EqualityLogic___Should_be_valid___When_same_data()
        {
            // Arrange
            var name = A.Dummy<string>();
            var elapsedMilliseconds = A.Dummy<decimal>();
            var isRunning = A.Dummy<bool>();
            var notEqualTests = new[]
                                    {
                                        new
                                            {
                                                First = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning),
                                                Second = new StopwatchSnapshot(name, elapsedMilliseconds, isRunning),
                                            },
                                        new
                                            {
                                                First = (StopwatchSnapshot)null,
                                                Second = (StopwatchSnapshot)null,
                                            },
                                    }.ToList();

            // Act & Assert
            notEqualTests.ForEach(
                _ =>
                {
                    if (_.First != null && _.Second != null)
                    {
                        _.First.Equals(_.Second).Should().BeTrue(Invariant($"First: {_.First}; Second: {_.Second}"));
                        _.First.Equals((object)_.Second).Should().BeTrue(Invariant($"First: {_.First}; Second: {_.Second}"));
                        (_.First.GetHashCode() == _.Second.GetHashCode()).Should().BeTrue(Invariant($"First: {_.First}; Second: {_.Second}"));
                    }

                    (_.First == _.Second).Should().BeTrue(Invariant($"First: {_.First}; Second: {_.Second}"));
                    (_.First != _.Second).Should().BeFalse(Invariant($"First: {_.First}; Second: {_.Second}"));
                });
        }

        [Fact]
        public static void ToEventTelemetry___When_passed_null_snapshots___Should_throw()
        {
            // Arrange
            Action action = () => ((IReadOnlyCollection<StopwatchSnapshot>)null).ToEventTelemetry(A.Dummy<string>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Value cannot be null.\r\nParameter name: stopwatchSnapshots");
        }

        [Fact]
        public static void ToEventTelemetry___When_passed_valid_snapshots___Should_produce_EventTelemetry()
        {
            // Arrange
            var eventName = A.Dummy<string>();
            var timestamp = DateTime.UtcNow;
            var snapshots = Some.ReadOnlyDummies<StopwatchSnapshot>().Distinct().ToList();
            var metadata = new Dictionary<string, string> { { "somePropKey", "somePropValue" } };

            // Act
            var eventTelemetry = snapshots.ToEventTelemetry(eventName, timestamp, metadata);

            // Assert
            eventTelemetry.Should().NotBeNull();
            eventTelemetry.Name.Should().Be(eventName);
            eventTelemetry.PropertyNameToValueMap.Count.Should().Be(snapshots.Count + metadata.Count);
            eventTelemetry.PropertyNameToValueMap[metadata.Single().Key].Should().Be(metadata.Single().Value);
            foreach (var snapshot in snapshots)
            {
                var property = eventTelemetry.PropertyNameToValueMap[snapshot.Name];
                if (snapshot.IsRunning)
                {
                    property.Should().Be("Running");
                }
                else
                {
                    property.Should().Be("NotRunning");
                }

                eventTelemetry.MetricNameToValueMap[snapshot.Name].Should().Be(snapshot.ElapsedMilliseconds);
            }
        }
    }
}
