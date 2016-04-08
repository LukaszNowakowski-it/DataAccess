
using System.Configuration;
using System.Data;

namespace Lnow.Libraries.DataAccess.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SqlConnectorTests
    {
        /// <summary>
        /// Checks if new connection is created each time if no transaction is active.
        /// </summary>
        [TestMethod]
        public void ConnectionRetrievesNewConnectionEachTimeWhenNoTransactionActive()
        {
            var connector = CreateConnector();
            var connection1 = connector.Connection;
            var connection2 = connector.Connection;
            Assert.AreNotSame(connection1, connection2, "Connections are the same");
        }

        /// <summary>
        /// Checks if the same connection is returned each time if transaction is active.
        /// </summary>
        [TestMethod]
        public void ConnectionRetrievesTheSameConnectionWhenTransactionIsActive()
        {
            var connector = CreateConnector();
            connector.BeginTransaction(IsolationLevel.ReadCommitted);
            var connection1 = connector.Connection;
            var connection2 = connector.Connection;
            Assert.AreSame(connection1, connection2, "Connections are different");
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> succeedes for active transaction.
        /// </summary>
        [TestMethod]
        public void CommitTransactionSucceedsForActiveTransaction()
        {
            var connector = CreateConnector();
            connector.BeginTransaction(IsolationLevel.ReadCommitted);
            connector.CommitTransaction();
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> fails for inactive transaction.
        /// </summary>
        [TestMethod]
        public void CommitTransactionFailsForInactiveTransaction()
        {
            try
            {
                var connector = CreateConnector();
                connector.CommitTransaction();
                Assert.Fail("Operation should throw exception");
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> succeedes for active transaction.
        /// </summary>
        [TestMethod]
        public void RollbackTransactionSucceedsForActiveTransaction()
        {
            var connector = CreateConnector();
            connector.BeginTransaction(IsolationLevel.ReadCommitted);
            connector.RollbackTransaction();
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> fails for inactive transaction.
        /// </summary>
        [TestMethod]
        public void RollbackTransactionFailsForInactiveTransaction()
        {
            try
            {
                var connector = CreateConnector();
                connector.RollbackTransaction();
                Assert.Fail("Operation should throw exception");
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Creates <see cref="SqlConnector" /> instance for testing.
        /// </summary>
        /// <returns></returns>
        private static SqlConnector CreateConnector()
        {
            return new SqlConnector(ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString);
        }
    }
}
