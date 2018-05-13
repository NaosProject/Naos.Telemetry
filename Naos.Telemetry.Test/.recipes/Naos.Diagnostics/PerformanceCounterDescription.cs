﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceCounterDescription.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if NaosDiagnosticsDomain
namespace Naos.Diagnostics.Domain
#else
namespace Naos.Diagnostics.Recipes
#endif
{
    using static System.FormattableString;

    /// <summary>
    /// Model that holds descrption of a Perfomance Counter.
    /// </summary>
#if NaosDiagnosticsRecipes
#pragma warning disable SA1649 // File name should match first type name
    public class RecipePerformanceCounterDescription
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecipePerformanceCounterDescription"/> class.
        /// </summary>
        /// <param name="categoryName">Category name.</param>
        /// <param name="counterName">Counter name.</param>
        /// <param name="instanceName">Instance name.</param>
        /// <param name="expectedMinValue">Optional expected mininum value.</param>
        /// <param name="expectedMaxValue">Optional expected maximum value.</param>
        public RecipePerformanceCounterDescription(string categoryName, string counterName, string instanceName = null, float? expectedMinValue = null, float? expectedMaxValue = null)
        {
            this.CategoryName = categoryName;
            this.CounterName = counterName;
            this.InstanceName = instanceName;
            this.ExpectedMinValue = expectedMinValue;
            this.ExpectedMaxValue = expectedMaxValue;
        }
#elif NaosDiagnosticsDomain
    public class PerformanceCounterDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterDescription"/> class.
        /// </summary>
        /// <param name="categoryName">Category name.</param>
        /// <param name="counterName">Counter name.</param>
        /// <param name="instanceName">Instance name.</param>
        /// <param name="expectedMinValue">Optional expected mininum value.</param>
        /// <param name="expectedMaxValue">Optional expected maximum value.</param>
        public PerformanceCounterDescription(string categoryName, string counterName, string instanceName = null, float? expectedMinValue = null, float? expectedMaxValue = null)
        {
            this.CategoryName = categoryName;
            this.CounterName = counterName;
            this.InstanceName = instanceName;
            this.ExpectedMinValue = expectedMinValue;
            this.ExpectedMaxValue = expectedMaxValue;
        }
#else
    [System.CodeDom.Compiler.GeneratedCode("Naos.Diagnostics", "See package version number")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class RecipePerformanceCounterDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecipePerformanceCounterDescription"/> class.
        /// </summary>
        /// <param name="categoryName">Category name.</param>
        /// <param name="counterName">Counter name.</param>
        /// <param name="instanceName">Instance name.</param>
        /// <param name="expectedMinValue">Optional expected mininum value.</param>
        /// <param name="expectedMaxValue">Optional expected maximum value.</param>
        public RecipePerformanceCounterDescription(string categoryName, string counterName, string instanceName = null, float? expectedMinValue = null, float? expectedMaxValue = null)
        {
            this.CategoryName = categoryName;
            this.CounterName = counterName;
            this.InstanceName = instanceName;
            this.ExpectedMinValue = expectedMinValue;
            this.ExpectedMaxValue = expectedMaxValue;
        }
#endif

        /// <summary>
        /// Gets the category name.
        /// </summary>
        public string CategoryName { get; private set; }

        /// <summary>
        /// Gets the counter name.
        /// </summary>
        public string CounterName { get; private set; }

        /// <summary>
        /// Gets the instance name.
        /// </summary>
        public string InstanceName { get; private set; }

        /// <summary>
        /// Gets the expected minimum value.
        /// </summary>
        public float? ExpectedMinValue { get; private set; }

        /// <summary>
        /// Gets the expected maximum value.
        /// </summary>
        public float? ExpectedMaxValue { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"PerformanceCounterDescription - {nameof(this.CategoryName)}: {this.CategoryName}; {nameof(this.CounterName)}: {this.CounterName}; {nameof(this.InstanceName)}: {this.InstanceName ?? "<null>"}.");
            return result;
        }
    }

    /// <summary>
    /// Model of a sampled performance counter.
    /// </summary>
#if NaosDiagnosticsRecipes
    public class RecipePerformanceCounterSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecipePerformanceCounterSample"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">The sampled value.</param>
        public RecipePerformanceCounterSample(RecipePerformanceCounterDescription description, float value)
        {
            this.Description = description;
            this.Value = value;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public RecipePerformanceCounterDescription Description { get; private set; }
#elif NaosDiagnosticsDomain
    public class PerformanceCounterSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterSample"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">The sampled value.</param>
        public PerformanceCounterSample(PerformanceCounterDescription description, float value)
        {
            this.Description = description;
            this.Value = value;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public PerformanceCounterDescription Description { get; private set; }
#else
    [System.CodeDom.Compiler.GeneratedCode("Naos.Diagnostics", "See package version number")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class RecipePerformanceCounterSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecipePerformanceCounterSample"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">The sampled value.</param>
        public RecipePerformanceCounterSample(RecipePerformanceCounterDescription description, float value)
        {
            this.Description = description;
            this.Value = value;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public RecipePerformanceCounterDescription Description { get; private set; }
#endif

        /// <summary>
        /// Gets the value.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the value was in range (if range provided).
        /// </summary>
        public bool? InRange => this.Description.ExpectedMinValue == null || this.Description.ExpectedMaxValue == null
                                    ? (bool?)null
                                    : this.Value < this.Description.ExpectedMaxValue && this.Value > this.Description.ExpectedMinValue;
    }
}