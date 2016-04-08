// <copyright file="SqlConnector.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains implementation of IDbConnector for SQL Server.</description>

namespace Lnow.Libraries.DataAccess
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    /// Implementation of <see cref="IDbConnector" /> for SQL Server.
    /// </summary>
    public class SqlConnector : IDbConnector
    {
        /// <summary>
        /// Connection string for this instance.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Connection for transaction being active.
        /// </summary>
        private SqlConnection connection;

        /// <summary>
        /// Currently active transaction, or null if no transaction.
        /// </summary>
        private SqlTransaction transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnector" /> class.
        /// </summary>
        /// <param name="connectionString">Connection string for this instance.</param>
        public SqlConnector(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SqlConnector" /> class.
        /// </summary>
        ~SqlConnector()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether there is a transaction in progress.
        /// </summary>
        public bool IsTransactionInProgress
        {
            get
            {
                return this.transaction != null;
            }
        }

        /// <summary>
        /// Gets connection provided by this instance.
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (this.IsTransactionInProgress)
                {
                    return this.transaction.Connection;
                }

                return this.CreateConnection();
            }
        }

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <param name="isolationLevel">Isolation level for a transaction.</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this.IsTransactionInProgress)
            {
                throw new InvalidOperationException("Transaction is already in progress");
            }

            this.connection = this.CreateConnection();
            this.transaction = this.connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Commits a transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (!this.IsTransactionInProgress)
            {
                throw new InvalidOperationException("No transaction in progress");
            }

            this.transaction.Commit();
            this.FinalizeTransaction();
        }

        /// <summary>
        /// Rollbacks a transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (!this.IsTransactionInProgress)
            {
                throw new InvalidOperationException("No transaction in progress");
            }

            this.transaction.Rollback();
            this.FinalizeTransaction();
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a command.
        /// </summary>
        /// <returns>Command created.</returns>
        public DbCommand CreateCommand()
        {
            SqlCommand result = null;
            try
            {
                result = new SqlCommand();
                return result;
            }
            catch
            {
                if (result != null)
                {
                    result.Dispose();
                }

                throw;
            }
        }

        /// <summary>
        /// Creates a parameter.
        /// </summary>
        /// <returns>Parameter created.</returns>
        public DbParameter CreateParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        /// <param name="disposing">True to dispose managed resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.transaction != null)
                {
                    this.FinalizeTransaction();
                    this.transaction = null;
                }
            }
        }

        /// <summary>
        /// Creates a new connection.
        /// </summary>
        /// <returns>Connection created.</returns>
        private SqlConnection CreateConnection()
        {
            SqlConnection result = null;
            try
            {
                result = new SqlConnection(this.connectionString);
                result.Open();
                return result;
            }
            catch
            {
                if (result != null)
                {
                    result.Dispose();
                }

                throw;
            }
        }

        /// <summary>
        /// Finalizes transaction.
        /// </summary>
        private void FinalizeTransaction()
        {
            if (this.transaction != null)
            {
                this.transaction.Dispose();
                this.transaction = null;
            }

            if (this.connection != null)
            {
                this.connection.Dispose();
                this.connection = null;
            }
        }
    }
}