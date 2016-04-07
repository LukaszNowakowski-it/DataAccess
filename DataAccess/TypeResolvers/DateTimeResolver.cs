// <copyright file="DateTimeResolver.cs" company="Axa Direct Solutions">
// Axa Direct Solutions SAS S A Uproszczona Oddział w Polsce. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains database type resolver for DateTime.</description>

namespace Ads.Lnow.Common.DataAccess.TypeResolvers
{
    using System;
    using System.Data;

    /// <summary>
    /// Database type resolver for <see cref="DateTime" />.
    /// </summary>
    internal class DateTimeResolver : ParameterTypeResolverBase<DateTime>
    {
        /// <summary>
        /// Resolves database type for given value.
        /// </summary>
        /// <param name="value">Value to resolve type for.</param>
        /// <returns>Database type resolved, or null if none specific type should be selected.</returns>
        protected override DbType? ResolveTypeInternal(DateTime value)
        {
            if (value < new DateTime(1753, 1, 1))
            {
                return DbType.DateTime2;
            }

            return null;
        }
    }
}
