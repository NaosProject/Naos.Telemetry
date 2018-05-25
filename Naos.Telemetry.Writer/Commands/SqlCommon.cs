// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlCommon.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Writer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    using OBeautifulCode.Database.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Common methods.
    /// </summary>
    public static class SqlCommon
    {
        /// <summary>
        /// Builds an insert statement with parameterized inputs.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="columns">Column descriptions.</param>
        /// <returns>Parameterized insert SQL statement.</returns>
        public static string BuildInsertStatement(string tableName, IReadOnlyCollection<ColumnObject> columns)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            var builder = new StringBuilder();
            builder.Append(Invariant($"INSERT INTO {tableName} ("));

            var idx = 0;
            foreach (var column in columns)
            {
                builder.Append(column.Name);

                if (idx != columns.Count - 1)
                {
                    builder.Append(",");
                    idx++;
                }
            }

            builder.Append(") VALUES (");

            idx = 0;
            foreach (var column in columns)
            {
                builder.Append(Invariant($"@{column.Name}"));

                if (idx != columns.Count - 1)
                {
                    builder.Append(",");
                    idx++;
                }
            }

            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Builds parameters to use with an insert statement.
        /// </summary>
        /// <param name="columns">Columns descriptions.</param>
        /// <returns>Parameters to use with an insert statement.</returns>
        public static IReadOnlyCollection<SqlParameter> BuildParameters(IReadOnlyCollection<ColumnObject> columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            var result = columns.Select(
                column =>
                    {
                        var preppedValue = column.Value;
                        if (column.DbType == DbType.String && preppedValue != null && !(preppedValue is string))
                        {
                            preppedValue = TelemetryWriter.JsonSerializer.SerializeToString(preppedValue);
                        }

                        var parameter = DatabaseHelper.CreateParameter<SqlParameter>("@" + column.Name, column.DbType, preppedValue);
                        return parameter;
                    }).ToList();

            return result;
        }
    }

    /// <summary>
    /// Column description.
    /// </summary>
    public class ColumnObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnObject"/> class.
        /// </summary>
        /// <param name="name">Name of column.</param>
        /// <param name="dbType">Type of column.</param>
        /// <param name="value">Value for column.</param>
        public ColumnObject(string name, DbType dbType, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.DbType = dbType;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public DbType DbType { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public object Value { get; private set; }
    }
}