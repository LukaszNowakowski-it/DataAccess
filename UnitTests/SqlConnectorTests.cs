// <copyright file="SqlConnectorTests.cs" company="Axa Direct Solutions">
// Axa Direct Solutions SAS S A Uproszczona Oddział w Polsce. All rights reserved.
// </copyright>
// <creationDate>2016-04-08</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains unit tests for SqlConnector class.</description>

namespace Lnow.Libraries.DataAccess.UnitTests
{
    using System;
    using System.Configuration;
    using System.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="SqlConnector" /> class.
    /// </summary>
    [TestClass]
    public class SqlConnectorTests
    {
        /// <summary>
        /// Checks if new connection is created each time if no transaction is active.
        /// </summary>
        [TestMethod]
        public void ConnectionRetrievesNewConnectionEachTimeWhenNoTransactionActive()
        {
            using (var connector = CreateConnector())
            {
                var connection1 = connector.Connection;
                var connection2 = connector.Connection;
                Assert.AreNotSame(connection1, connection2, "Connections are the same");
            }
        }

        /// <summary>
        /// Checks if the same connection is returned each time if transaction is active.
        /// </summary>
        [TestMethod]
        public void ConnectionRetrievesTheSameConnectionWhenTransactionIsActive()
        {
            using (var connector = CreateConnector())
            {
                connector.BeginTransaction(IsolationLevel.ReadCommitted);
                var connection1 = connector.Connection;
                var connection2 = connector.Connection;
                Assert.AreSame(connection1, connection2, "Connections are different");
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.BeginTransaction" /> fails when transaction is already active.
        /// </summary>
        [TestMethod]
        public void BeginTransactionFailsIfTransactionIsActive()
        {
            using (var connector = CreateConnector())
            {
                connector.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    connector.BeginTransaction(IsolationLevel.ReadCommitted);
                    Assert.Fail("Operation should fail");
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> succeeds for active transaction.
        /// </summary>
        [TestMethod]
        public void CommitTransactionSucceedsForActiveTransaction()
        {
            using (var connector = CreateConnector())
            {
                connector.BeginTransaction(IsolationLevel.ReadCommitted);
                connector.CommitTransaction();
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> fails for inactive transaction.
        /// </summary>
        [TestMethod]
        public void CommitTransactionFailsForInactiveTransaction()
        {
            try
            {
                using (var connector = CreateConnector())
                {
                    connector.CommitTransaction();
                    Assert.Fail("Operation should throw exception");
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> succeeds for active transaction.
        /// </summary>
        [TestMethod]
        public void RollbackTransactionSucceedsForActiveTransaction()
        {
            using (var connector = CreateConnector())
            {
                connector.BeginTransaction(IsolationLevel.ReadCommitted);
                connector.RollbackTransaction();
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CommitTransaction" /> fails for inactive transaction.
        /// </summary>
        [TestMethod]
        public void RollbackTransactionFailsForInactiveTransaction()
        {
            try
            {
                using (var connector = CreateConnector())
                {
                    connector.RollbackTransaction();
                    Assert.Fail("Operation should throw exception");
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CreateCommand" /> creates new command with each call.
        /// </summary>
        [TestMethod]
        public void CreateCommandCreatesNewCommandWithEachCall()
        {
            using (var connector = CreateConnector())
            {
                var command1 = connector.CreateCommand();
                Assert.IsNotNull(command1, "Null command1 created");
                var command2 = connector.CreateCommand();
                Assert.IsNotNull(command2, "Null command2 created");
                Assert.AreNotSame(command1, command2, "Instances are the same");
            }
        }

        /// <summary>
        /// Checks if <see cref="SqlConnector.CreateCommand" /> creates new command with each call.
        /// </summary>
        [TestMethod]
        public void CreateParameterCreatesNewParameterWithEachCall()
        {
            using (var connector = CreateConnector())
            {
                var parameter1 = connector.CreateParameter();
                Assert.IsNotNull(parameter1, "Null parameter1 created");
                var parameter2 = connector.CreateParameter();
                Assert.IsNotNull(parameter2, "Null parameter2 created");
                Assert.AreNotSame(parameter1, parameter2, "Instances are the same");
            }
        }

        /// <summary>
        /// Creates <see cref="SqlConnector" /> instance for testing.
        /// </summary>
        /// <returns>Connector created.</returns>
        private static SqlConnector CreateConnector()
        {
            return new SqlConnector(ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString);
        }
    }
}
