// <copyright file="IDbConnector.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains interface for types that provide access to specific database servers..</description>

namespace Lnow.Libraries.DataAccess
{
    using System;
    using System.Data.Common;

    /// <summary>
    /// Interface for types that provide access to specific database servers.
    /// </summary>
    public interface IDbConnector : ITransactionController, IDisposable
    {
        /// <summary>
        /// Gets underlying connection.
        /// </summary>
        DbConnection Connection
        {
            get;
        }

        /// <summary>
        /// Gets transaction, if one is in progress, otherwise null is returned.
        /// </summary>
        DbTransaction Transaction
        {
            get;
        }

        /// <summary>
        /// Creates empty command.
        /// </summary>
        /// <returns>Command created.</returns>
        DbCommand CreateCommand();

        /// <summary>
        /// Creates empty command parameter.
        /// </summary>
        /// <returns>Created parameter.</returns>
        DbParameter CreateParameter();
    }
}
