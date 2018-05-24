// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagnosticsTelemetry.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Telemetry.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Naos.Diagnostics.Domain;

    /// <summary>
    /// Diagnostics data.
    /// </summary>
    public class DiagnosticsTelemetry : IAmTelemetryItem
    {
        /// <summary>
        /// Gets the file paths of all assemblies in the directory of the assembly provided.
        /// </summary>
        /// <param name="assemblyFilePath">Assembly path.</param>
        /// <returns>Assembly files in the directory of execution.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Not a direct parameter.")]
        public static IReadOnlyCollection<string> GetSiblingAssemblyFilePaths(string assemblyFilePath)
        {
            if (string.IsNullOrWhiteSpace(assemblyFilePath))
            {
                throw new ArgumentNullException(nameof(assemblyFilePath));
            }

            var directoryPath = Path.GetDirectoryName(assemblyFilePath);
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentNullException(nameof(Path.GetDirectoryName));
            }

            var allFiles = Directory.GetFiles(directoryPath);
            var allAssemblyFilePaths = allFiles.Where(
                _ =>
                    {
                        var extension = Path.GetExtension(_).ToUpperInvariant();
                        return extension == ".DLL" || extension == ".EXE";
                    }).ToList();

            return allAssemblyFilePaths;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsTelemetry"/> class.
        /// </summary>
        /// <param name="sampledUtc">Sampled date and time in UTC.</param>
        /// <param name="machineDetails">Machine details.</param>
        /// <param name="processDetails">Process details.</param>
        /// <param name="processSiblingAssemblies">Assemblies of the process.</param>
        public DiagnosticsTelemetry(DateTime sampledUtc, MachineDetails machineDetails, ProcessDetails processDetails, IReadOnlyCollection<AssemblyDetails> processSiblingAssemblies)
        {
            this.SampledUtc = sampledUtc;
            this.MachineDetails = machineDetails;
            this.ProcessDetails = processDetails;
            this.ProcessSiblingAssemblies = processSiblingAssemblies;
        }

        /// <inheritdoc />
        public DateTime SampledUtc { get; private set; }

        /// <summary>
        /// Gets the machine details.
        /// </summary>
        public MachineDetails MachineDetails { get; private set; }

        /// <summary>
        /// Gets the process details.
        /// </summary>
        public ProcessDetails ProcessDetails { get; private set; }

        /// <summary>
        /// Gets the assembly details of the processes assemblies.
        /// </summary>
        public IReadOnlyCollection<AssemblyDetails> ProcessSiblingAssemblies { get; private set; }
    }
}