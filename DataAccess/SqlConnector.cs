using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Lnow.Common.DataAccess
{
    public class SqlConnector : IDbConnector
    {
        private readonly string connectionString;

        private SqlTransaction transaction;

        public SqlConnector(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool IsTransactionInProgress
        {
            get
            {
                return this.transaction != null;
            }
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this.IsTransactionInProgress)
            {
                throw new InvalidOperationException("Transaction is already in progress");
            }

            var connection = this.CreateConnection();
            this.transaction = connection.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            if (!this.IsTransactionInProgress)
            {
                throw new InvalidOperationException("No transaction in progress");
            }

            this.transaction.Commit();
            this.FinalizeTransaction();
        }

        public void RollbackTransaction()
        {
            if (!this.IsTransactionInProgress)
            {
                throw new InvalidOperationException("No transaction in progress");
            }

            this.transaction.Rollback();
            this.FinalizeTransaction();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.transaction != null)
                {
                    this.FinalizeTransaction();
                    this.transaction.Dispose();
                    this.transaction = null;
                }
            }
        }

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

        public DbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public DbParameter CreateParameter()
        {
            throw new NotImplementedException();
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(this.connectionString);
        }

        private void FinalizeTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
