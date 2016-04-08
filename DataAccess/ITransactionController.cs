// <copyright file="ITransactionController.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains interface for types, that can control transaction.</description>

namespace Lnow.Libraries.DataAccess
{
    using System.Data;

    /// <summary>
    /// Interface for types, that can control transactions.
    /// </summary>
    public interface ITransactionController
    {
        /// <summary>
        /// Gets a value indicating whether there is a transaction in progress.
        /// </summary>
        bool IsTransactionInProgress
        {
            get;
        }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        /// <param name="isolationLevel">Isolation level to use for created transaction.</param>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Commits transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollbacks transaction.
        /// </summary>
        void RollbackTransaction();
    }
}
