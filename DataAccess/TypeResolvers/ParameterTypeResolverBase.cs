// <copyright file="ParameterTypeResolverBase.cs" company="Axa Direct Solutions">
// Axa Direct Solutions SAS S A Uproszczona Oddział w Polsce. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains base class for database types resolvers.</description>

namespace Ads.Lnow.Common.DataAccess.TypeResolvers
{
    using System;
    using System.Data;
    using System.Globalization;

    /// <summary>
    /// Base class for database types resolvers.
    /// </summary>
    /// <typeparam name="T">Framework type this instance resolves database types for.</typeparam>
    public abstract class ParameterTypeResolverBase<T> : IParameterTypeResolver
    {
        /// <summary>
        /// Gets framework type resolved.
        /// </summary>
        public Type SupportedType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Resolves database type for given value.
        /// </summary>
        /// <param name="value">Value to resolve type for.</param>
        /// <returns>Database type resolved, or null if none specific type should be selected.</returns>
        public DbType? ResolveType(object value)
        {
            var casted = Cast(value);
            return this.ResolveTypeInternal(casted);
        }

        /// <summary>
        /// Resolves database type for given value.
        /// </summary>
        /// <param name="value">Value to resolve type for.</param>
        /// <returns>Database type resolved, or null if none specific type should be selected.</returns>
        protected abstract DbType? ResolveTypeInternal(T value);

        /// <summary>
        /// Casts value to resolve database type for.
        /// </summary>
        /// <param name="value">Value to be casted.</param>
        /// <returns>Casted value.</returns>
        private static T Cast(object value)
        {
            if (value == null)
            {
                return default(T);
            }

            if (value is T)
            {
                return (T)value;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expected instance of '{0}', '{1}' found", typeof(T).FullName, value.GetType().FullName));
        }
    }
}
