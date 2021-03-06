﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryBsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using Naos.Diagnostics.Serialization.Bson;
    using Naos.Telemetry.Domain;
    using OBeautifulCode.Serialization.Bson;

    /// <summary>
    /// Implementation for the <see cref="Telemetry" /> domain.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Telemetry", Justification = "Spelling/name is correct.")]
    public class TelemetryBsonSerializationConfiguration : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => new[]
                                                                                               {
                                                                                                   typeof(DiagnosticsTelemetry).Namespace,
                                                                                               };

        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[]
                                                                                 {
                                                                                     typeof(DiagnosticsBsonSerializationConfiguration).ToBsonSerializationConfigurationType(),
                                                                                 };

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
                                                                        {
                                                                            typeof(DiagnosticsTelemetry).ToTypeToRegisterForBson(),
                                                                            typeof(EventTelemetry).ToTypeToRegisterForBson(),
                                                                            typeof(StopwatchSnapshot).ToTypeToRegisterForBson(),
                                                                        };
    }
}