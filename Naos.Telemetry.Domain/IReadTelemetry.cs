// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadTelemetry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to define a reader.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Want an interface NOT an attribute.")]
    public interface IReadTelemetry
    {
        /// <summary>
        /// Get items in queue waiting to be processed.
        /// </summary>
        /// <returns>Collection of queued items.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Don't want a property for an active operation against a data source.")]
        Task<IReadOnlyCollection<RawQueueItem>> GetQueuedRawItemsAsync();
    }
}