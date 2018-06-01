﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidation.Strings.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Validation source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Validation.Recipes
{
    /// <summary>
    /// Contains all validations that can be applied to a <see cref="Parameter"/>.
    /// </summary>
#if !OBeautifulCodeValidationRecipesProject
    internal
#else
    public
#endif
        static partial class ParameterValidation
    {
#pragma warning disable 1591
#pragma warning disable SA1600

        public const string ImproperUseOfFrameworkExceptionMessage = "The parameter validation framework is being used improperly; see: https://github.com/OBeautifulCode/OBeautifulCode.Validation for documentation on proper usage.";

        public const string AnyReferenceTypeName = "Any Reference Type";

        public const string EnumerableOfAnyReferenceTypeName = "IEnumerable<Any Reference Type>";

        public const string NullableGenericTypeName = "Nullable<T>";

        public const string EnumerableOfNullableGenericTypeName = "IEnumerable<Nullable<T>>";

        public const string ComparableGenericTypeName = "IComparable<T>";

        public const string NullValueToString = "<null>";

        public const string BeNullExceptionMessageSuffix = "is not null";

        public const string NotBeNullExceptionMessageSuffix = "is null";

        public const string BeTrueExceptionMessageSuffix = "is not true";

        public const string NotBeTrueExceptionMessageSuffix = "is true";

        public const string BeFalseExceptionMessageSuffix = "is not false";

        public const string NotBeFalseExceptionMessageSuffix = "is false";

        public const string NotBeNullNorWhiteSpaceExceptionMessageSuffix = "is white space";

        public const string BeEmptyGuidExceptionMessageSuffix = "is not an empty guid";

        public const string NotBeEmptyGuidExceptionMessageSuffix = "is an empty guid";

        public const string BeEmptyStringExceptionMessageSuffix = "is not an empty string";

        public const string NotBeEmptyStringExceptionMessageSuffix = "is an empty string";

        public const string BeEmptyEnumerableExceptionMessageSuffix = "is not an empty enumerable";

        public const string NotBeEmptyEnumerableExceptionMessageSuffix = "is an empty enumerable";

        public const string ContainSomeNullsExceptionMessageSuffix = "contains no null elements";

        public const string NotContainAnyNullsExceptionMessageSuffix = "contains at least one null element";

        public const string BeDefaultExceptionMessageSuffix = "is not equal to default(T) using EqualityComparer<T>.Default";

        public const string NotBeDefaultExceptionMessageSuffix = "is equal to default(T) using EqualityComparer<T>.Default";

        public const string BeLessThanExceptionMessageSuffix = "is not less than the comparison value using Comparer<T>.Default";

        public const string NotBeLessThanExceptionMessageSuffix = "is less than the comparison value using Comparer<T>.Default";

        public const string BeGreaterThanExceptionMessageSuffix = "is not greater than the comparison value using Comparer<T>.Default";

        public const string NotBeGreaterThanExceptionMessageSuffix = "is greater than the comparison value using Comparer<T>.Default";

        public const string BeLessThanOrEqualToExceptionMessageSuffix = "is not less than or equal to the comparison value using Comparer<T>.Default";

        public const string NotBeLessThanOrEqualToExceptionMessageSuffix = "is less than or equal to the comparison value using Comparer<T>.Default";

        public const string BeGreaterThanOrEqualToExceptionMessageSuffix = "is not greater than or equal to the comparison value using Comparer<T>.Default";

        public const string NotBeGreaterThanOrEqualToExceptionMessageSuffix = "is greater than or equal to the comparison value using Comparer<T>.Default";

        public const string BeEqualToExceptionMessageSuffix = "is not equal to the comparison value using EqualityComparer<T>.Default";

        public const string NotBeEqualToExceptionMessageSuffix = "is equal to the comparison value using EqualityComparer<T>.Default";

        public const string BeInRangeExceptionMessageSuffix = "is not within the specified range using Comparer<T>.Default";

        public const string NotBeInRangeExceptionMessageSuffix = "is within the specified range using Comparer<T>.Default";

        public const string ContainExceptionMessageSuffix = "does not contain the item to search for using EqualityComparer<T>.Default";

        public const string NotContainExceptionMessageSuffix = "contains the item to search for using EqualityComparer<T>.Default";

        public const string MalformedRangeExceptionMessage = "The specified range is invalid because '{1}' is less than '{0}'.  Specified '{0}' is '{2}'.  Specified '{1}' is '{3}'.";

#pragma warning restore SA1600
#pragma warning restore 1591
    }
}