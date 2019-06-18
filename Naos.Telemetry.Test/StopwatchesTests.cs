// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopwatchesTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Test
{
    using System;
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
    using Xunit;

    public static class StopwatchesTests
    {
        [Fact]
        public static void Stopwatches___Should_work()
        {
            // Arrange
            var shouldNotBeRunning = "ShouldNotBeRunning";
            var shouldBeRunning = "ShouldBeRunning";

            // Act
            var stopwatch = Stopwatch.StartNew();
            Stopwatches.Start(shouldNotBeRunning);
            Stopwatches.Start(shouldBeRunning);
            Thread.Sleep(TimeSpan.FromMilliseconds(5));
            Stopwatches.Stop(shouldNotBeRunning);
            var resultsBeforeClear = Stopwatches.BuildSummaryReport();
            Stopwatches.Clear();
            var resultsAfterClear = Stopwatches.BuildSummaryReport();
            stopwatch.Stop();

            // Assert
            resultsBeforeClear.Count.Should().Be(2);
            resultsAfterClear.Should().BeEmpty();
            var shouldNotBeRunningResult = resultsBeforeClear.Single(_ => _.Name == shouldNotBeRunning);
            var shouldBeRunningResult = resultsBeforeClear.Single(_ => _.Name == shouldBeRunning);
            shouldNotBeRunningResult.ElapsedMilliseconds.Should().BeLessThan(shouldBeRunningResult.ElapsedMilliseconds);
            shouldNotBeRunningResult.ElapsedMilliseconds.Should().BeLessThan(stopwatch.ElapsedMilliseconds);
            shouldBeRunningResult.ElapsedMilliseconds.Should().BeLessThan(stopwatch.ElapsedMilliseconds);
            shouldNotBeRunningResult.ElapsedMilliseconds.Should().BeGreaterThan(0);
            shouldBeRunningResult.ElapsedMilliseconds.Should().BeGreaterThan(0);
            shouldNotBeRunningResult.IsRunning.Should().BeFalse();
            shouldBeRunningResult.IsRunning.Should().BeTrue();
        }
    }
}
