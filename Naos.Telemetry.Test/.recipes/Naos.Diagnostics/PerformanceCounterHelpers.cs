﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceCounterHelpers.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Diagnostics.Recipes
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Various helper methods related to a <see cref="PerformanceCounter" />'s.
    /// </summary>
#if NaosDiagnosticsRecipes
    public
#else
    [System.CodeDom.Compiler.GeneratedCode("Naos.Diagnostics", "See package version number")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal
#endif
    static class PerformanceCounterHelpers
    {
        /// <summary>
        /// Gets the next value of a performance counter.
        /// </summary>
        /// <param name="categoryName">Category name.</param>
        /// <param name="counterName">Counter name.</param>
        /// <param name="instanceName">Instance name.</param>
        /// <returns>Next value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Want a default parameter here because it communicates intention with the constructor choice.")]
        public static float SampleNextValueOnPerformanceCounter(string categoryName, string counterName, string instanceName = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException(nameof(categoryName));
            }

            if (string.IsNullOrWhiteSpace(counterName))
            {
                throw new ArgumentNullException(nameof(counterName));
            }

            using (var perfCounter = instanceName == null ? new PerformanceCounter(categoryName, counterName) : new PerformanceCounter(categoryName, counterName, instanceName, true))
            {
                var result = perfCounter.NextValue();

                return result;
            }
        }

        /// <summary>
        /// Samples the provided performance counter.
        /// </summary>
        /// <param name="description">Description to query with.</param>
        /// <returns>Description and sample in one object.</returns>
        public static RecipePerformanceCounterSample Sample(this RecipePerformanceCounterDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            var nextValue = SampleNextValueOnPerformanceCounter(description.CategoryName, description.CounterName, description.InstanceName);
            var result = new RecipePerformanceCounterSample(description, nextValue);
            return result;
        }
    }
}
