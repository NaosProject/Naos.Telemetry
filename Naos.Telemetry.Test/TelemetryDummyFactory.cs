// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryDummyFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Recipes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FakeItEasy;
    using Naos.Diagnostics.Domain;
    using Naos.Telemetry;
    using Naos.Telemetry.Domain;
    using Naos.Telemetry.Test;
    using OBeautifulCode.AutoFakeItEasy;

    /// <summary>
    /// A dummy factory for Accounting Time types.
    /// </summary>
#if !NaosTelemetrySolution
    internal
#else
    public
#endif
    class TelemetryDummyFactory : DefaultTelemetryDummyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryDummyFactory"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is not excessively complex.  Dummy factories typically wire-up many types.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "This is not excessively complex.  Dummy factories typically wire-up many types.")]
        public TelemetryDummyFactory()
        {
            /* Add any overriding or custom registrations here. */

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new EventTelemetry(A.Dummy<DateTime>(), A.Dummy<string>(), A.Dummy<Dictionary<string, string>>(), A.Dummy<Dictionary<string, decimal?>>());

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new DiagnosticsTelemetry(A.Dummy<DateTime>(), A.Dummy<MachineDetails>(), A.Dummy<ProcessDetails>(), Some.Dummies<AssemblyDetails>().ToList());

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new StopwatchSnapshot(A.Dummy<string>(), A.Dummy<long>(), A.Dummy<bool>());

                    return result;
                });
        }
    }
}
