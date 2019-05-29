// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryBsonConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using Naos.Diagnostics.Serialization.Bson;
    using Naos.Serialization.Bson;
    using Naos.Telemetry.Domain;

    /// <summary>
    /// Implementation for the <see cref="Telemetry" /> domain.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Telemetry", Justification = "Spelling/name is correct.")]
    public class TelemetryBsonConfiguration : BsonConfigurationBase
    {
        /// <inheritdoc />
        public override IReadOnlyCollection<Type> DependentConfigurationTypes => new[]
        {
            typeof(DiagnosticsBsonConfiguration),
        };

        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegister => new[]
        {
            typeof(DiagnosticsTelemetry),
            typeof(EventTelemetry),
        };
    }
}
