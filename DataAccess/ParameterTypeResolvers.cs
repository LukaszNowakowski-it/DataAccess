// <copyright file="ParameterTypeResolvers.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains class for interacting with parameter type resolvers.</description>

namespace Lnow.Libraries.DataAccess
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Class for interacting with parameter type resolvers.
    /// </summary>
    internal static class ParameterTypeResolvers
    {
        /// <summary>
        /// Active resolvers.
        /// </summary>
        private static Collection<IParameterTypeResolver> resolvers;

        /// <summary>
        /// Gets active resolvers.
        /// </summary>
        public static Collection<IParameterTypeResolver> Resolvers
        {
            get
            {
                if (resolvers == null)
                {
                    resolvers = new Collection<IParameterTypeResolver>(FindResolvers().ToList());
                }

                return resolvers;
            }
        }

        /// <summary>
        /// Resolves database type for given value.
        /// </summary>
        /// <param name="value">Value to resolve type for.</param>
        /// <returns>Database type resolved, or null if none specific type should be selected.</returns>
        public static DbType? ResolveType(object value)
        {
            if (value == null)
            {
                return null;
            }

            var resolver = Resolvers.FirstOrDefault(r => r.SupportedType == value.GetType());
            if (resolver == null)
            {
                return null;
            }

            return resolver.ResolveType(value);
        }

        /// <summary>
        /// Finds resolvers for current application.
        /// </summary>
        /// <returns>Enumeration of parameter type resolvers.</returns>
        private static IEnumerable<IParameterTypeResolver> FindResolvers()
        {
            yield return new TypeResolvers.DateTimeResolver();
        }
    }
}
