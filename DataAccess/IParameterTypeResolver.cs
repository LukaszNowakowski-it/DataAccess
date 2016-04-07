// <copyright file="IParameterTypeResolver.cs" company="Axa Direct Solutions">
// Axa Direct Solutions SAS S A Uproszczona Oddział w Polsce. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains interface for types resolving database type of parameter.</description>

namespace Ads.Lnow.Common.DataAccess
{
    using System;
    using System.Data;

    /// <summary>
    /// Interface for types resolving database type of parameter.
    /// </summary>
    internal interface IParameterTypeResolver
    {
        /// <summary>
        /// Gets type supported by this instance.
        /// </summary>
        Type SupportedType
        {
            get;
        }

        /// <summary>
        /// Resolves database type for given value.
        /// </summary>
        /// <param name="value">Value to resolve type for.</param>
        /// <returns>Database type resolved, or null if none specific type should be selected.</returns>
        DbType? ResolveType(object value);
    }
}
