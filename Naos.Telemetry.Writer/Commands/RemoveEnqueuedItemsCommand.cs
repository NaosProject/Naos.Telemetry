// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveEnqueuedItemsCommand.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Writer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Naos.Telemetry.StorageModel;

    using Spritely.Cqrs;

    using static System.FormattableString;

    /// <summary>
    /// Query to get the queued items.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Naming",
        "CA1704:IdentifiersShouldBeSpelledCorrectly",
        MessageId = "Enqueued",
        Justification = "Spelling/name is correct.")]
    public class RemoveEnqueuedItemsCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveEnqueuedItemsCommand"/> class.
        /// </summary>
        /// <param name="idsToRemove">ID's to remove.</param>
        public RemoveEnqueuedItemsCommand(
            IReadOnlyCollection<Guid> idsToRemove)
        {
            this.IdsToRemove = idsToRemove ?? throw new ArgumentNullException(nameof(idsToRemove));
        }

        /// <summary>
        /// Gets the ID's to remove from the queue.
        /// </summary>
        public IReadOnlyCollection<Guid> IdsToRemove { get; private set; }
    }

    /// <summary>
    /// Handler for <see cref="RemoveEnqueuedItemsCommand" />.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Naming",
        "CA1704:IdentifiersShouldBeSpelledCorrectly",
        MessageId = "Enqueued",
        Justification = "Spelling/name is correct.")]
    public class RemoveEnqueuedItemsCommandHandler : ICommandHandlerAsync<RemoveEnqueuedItemsCommand>
    {
        private readonly TelemetryDatabase telemetryDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveEnqueuedItemsCommandHandler"/> class.
        /// </summary>
        /// <param name="telemetryDatabase">Telemetry database.</param>
        public RemoveEnqueuedItemsCommandHandler(
            TelemetryDatabase telemetryDatabase)
        {
            this.telemetryDatabase = telemetryDatabase ?? throw new ArgumentNullException(nameof(telemetryDatabase));
        }

        /// <inheritdoc />
        public async Task HandleAsync(
            RemoveEnqueuedItemsCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var sql = Invariant(
                $"DELETE FROM {RawQueueSchema.TableName} WHERE {RawQueueSchema.Id} IN ({string.Join(",", command.IdsToRemove.Select(_ => Invariant($"'{_}'")))})");

            using (var connection = this.telemetryDatabase.CreateOpenedConnection())
            {
                await connection.ExecuteAsync(sql, commandTimeout: this.telemetryDatabase.ConnectionSettings.DefaultCommandTimeoutInSeconds);
            }
        }
    }
}
