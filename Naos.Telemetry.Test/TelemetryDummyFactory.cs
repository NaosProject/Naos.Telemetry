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
    using OBeautifulCode.AutoFakeItEasy;

    /// <summary>
    /// A dummy factory for Accounting Time types.
    /// </summary>
#if !NaosTelemetryRecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Telemetry", "See package version number")]
#endif
    public class TelemetryDummyFactory : IDummyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryDummyFactory"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is not excessively complex.  Dummy factories typically wire-up many types.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "This is not excessively complex.  Dummy factories typically wire-up many types.")]
        public TelemetryDummyFactory()
        {
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
        }

        /// <inheritdoc />
        public Priority Priority => new FakeItEasy.Priority(1);

        /// <inheritdoc />
        public bool CanCreate(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public object Create(Type type)
        {
            return null;
        }
    }
}
