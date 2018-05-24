// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadTelemetry.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
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
        Task<IReadOnlyCollection<RawQueueItem>> GetQueuedRawItemsAsync();
    }
}