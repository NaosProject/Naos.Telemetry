// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryDatabase.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.StorageModel
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Spritely.Cqrs;

    /// <summary>
    /// Represents the Telemetry database.
    /// </summary>
    public class TelemetryDatabase : IDatabase
    {
        private DatabaseConnectionSettings connectionSettings;

        private string insecureConnectionString;

        /// <summary>
        /// Gets or sets the connection settings.
        /// </summary>
        public DatabaseConnectionSettings ConnectionSettings
        {
            get => this.connectionSettings;
            set
            {
                this.insecureConnectionString = value.ToInsecureConnectionString();
                this.connectionSettings = value;
            }
        }

        /// <summary>
        /// Create a connection.
        /// </summary>
        /// <returns>
        /// Returns an open database connection.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposing would defeat the purpose of this method - to create an open connection.")]
        public async Task<IDbConnection> CreateOpenConnectionAsync()
        {
            // note: not using ConnectionSettings.CreateSqlConnection because that method
            // causes a new connection pool to be generated for the same ConnectionSettings object.
            var connection = new SqlConnection(this.insecureConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}