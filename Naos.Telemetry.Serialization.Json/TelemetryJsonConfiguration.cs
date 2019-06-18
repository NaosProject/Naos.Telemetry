// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryJsonConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using Naos.Diagnostics.Serialization.Json;
    using Naos.Serialization.Json;
    using Naos.Telemetry.Domain;

    /// <summary>
    /// Implementation for the <see cref="Telemetry" /> domain.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Telemetry", Justification = "Spelling/name is correct.")]
    public class TelemetryJsonConfiguration : JsonConfigurationBase
    {
        /// <inheritdoc />
        public override IReadOnlyCollection<Type> DependentConfigurationTypes => new[]
        {
            typeof(DiagnosticsJsonConfiguration),
        };

        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegister => new[]
        {
            typeof(DiagnosticsTelemetry),
            typeof(EventTelemetry),
            typeof(StopwatchSnapshot),
        };
    }
}
