// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAmTelemetryItem.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface to define that an object is a telemetry item.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Want an interface NOT an attribute.")]
    public interface IAmTelemetryItem
    {
    }

    /// <summary>
    /// Null implementation of <see cref="IAmTelemetryItem" />.
    /// </summary>
    public class NullTelemetryItem : IAmTelemetryItem
    {
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
        /// <param name="telemetryItems">Telemetry items.</param>
        public AggregateTelemetryItem(IReadOnlyCollection<IAmTelemetryItem> telemetryItems) => this.TelemetryItems = telemetryItems;

        /// <inheritdoc />
        public IReadOnlyCollection<IAmTelemetryItem> TelemetryItems { get; private set; }
    }
}