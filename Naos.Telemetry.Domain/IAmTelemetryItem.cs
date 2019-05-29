// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAmTelemetryItem.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface to define that an object is a telemetry item.
    /// </summary>
    public interface IAmTelemetryItem
    {
        /// <summary>
        /// Gets the sampled date and time in UTC.
        /// </summary>
        DateTime SampledUtc { get; }
    }

    /// <summary>
    /// Null implementation of <see cref="IAmTelemetryItem" />.
    /// </summary>
    public class NullTelemetryItem : IAmTelemetryItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullTelemetryItem"/> class.
        /// </summary>
        /// <param name="sampledUtc">Sampled date and time in UTC.</param>
        public NullTelemetryItem(DateTime sampledUtc) => this.SampledUtc = sampledUtc;

        /// <inheritdoc />
        public DateTime SampledUtc { get; private set; }
    }

    /// <summary>
    /// Interface to define that an object is a telemetry item.
    /// </summary>
    public interface IAmAggregateTelemetryItem : IAmTelemetryItem
    {
        /// <summary>
        /// Gets the telemetry items.
        /// </summary>
        IReadOnlyCollection<IAmTelemetryItem> TelemetryItems { get; }
    }

    /// <summary>
    /// Concrete <see cref="IAmAggregateTelemetryItem" />.
    /// </summary>
    public class AggregateTelemetryItem : IAmAggregateTelemetryItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateTelemetryItem"/> class.
        /// </summary>
        /// <param name="sampledUtc">Sampled date and time in UTC.</param>
        /// <param name="telemetryItems">Telemetry items.</param>
        public AggregateTelemetryItem(DateTime sampledUtc, IReadOnlyCollection<IAmTelemetryItem> telemetryItems)
        {
            this.SampledUtc = sampledUtc;
            this.TelemetryItems = telemetryItems;
        }

        /// <inheritdoc />
        public DateTime SampledUtc { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<IAmTelemetryItem> TelemetryItems { get; private set; }
    }
}