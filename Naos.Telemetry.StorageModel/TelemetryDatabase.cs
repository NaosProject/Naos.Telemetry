// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryDatabase.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using System.Data.SqlClient;

    using Spritely.Cqrs;

    /// <summary>
    /// Represents the database that stores events in the telemetry system.
    /// </summary>
    public class TelemetryDatabase : IDatabase
    {
        /// <summary>
        /// Gets or sets the connection settings.
        /// </summary>
        public DatabaseConnectionSettings ConnectionSettings { get; set; }

        /// <summary>
        /// Create a connection.
        /// </summary>
        /// <returns>
        /// Returns a database connection.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposing would defeat the purpose of this method - to create an open connection.")]
        public SqlConnection CreateConnection()
        {
            // note: not using ConnectionSettings.CreateSqlConnection because that method
            // causes a new connection pool to be generated for the same ConnectionSettings object.
            // see: http://stackoverflow.com/questions/33106463/ado-net-is-not-closing-tcp-connections-fast-enough/33115406#33115406
            var connection = new SqlConnection(this.ConnectionSettings.ToInsecureConnectionString());
            return connection;
        }
    }
}