// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryJsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using Naos.Diagnostics.Serialization.Json;
    using Naos.Telemetry.Domain;
    using OBeautifulCode.Serialization.Json;

    /// <summary>
    /// Implementation for the <see cref="Telemetry" /> domain.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Telemetry", Justification = "Spelling/name is correct.")]
    public class TelemetryJsonSerializationConfiguration : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => new[]
                                                                                               {
                                                                                                   typeof(DiagnosticsTelemetry).Namespace,
                                                                                               };

        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new[]
                                                                                 {
                                                                                     typeof(DiagnosticsJsonSerializationConfiguration).ToJsonSerializationConfigurationType(),
                                                                                 };

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
                                                                        {
                                                                            typeof(DiagnosticsTelemetry).ToTypeToRegisterForJson(),
                                                                            typeof(EventTelemetry).ToTypeToRegisterForJson(),
                                                                            typeof(StopwatchSnapshot).ToTypeToRegisterForJson(),
                                                                        };
    }
}