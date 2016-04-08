// <copyright file="ConnectorBase.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains base class for types interacting with database.</description>

namespace Lnow.Libraries.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Base class for types interacting with database.
    /// </summary>
    public abstract class ConnectorBase : ITransactionController
    {
        /// <summary>
        /// Instance of <see cref="IDbConnector" /> implementation to use.
        /// </summary>
        private readonly IDbConnector connector;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorBase" /> class.
        /// </summary>
        /// <param name="connector">Implementation of <see cref="IDbConnector" /> to use.</param>
        protected ConnectorBase(IDbConnector connector)
        {
            this.connector = connector;
        }

        /// <summary>
        /// Gets a value indicating whether there is a transaction in progress.
        /// </summary>
        public bool IsTransactionInProgress
        {
            get
            {
                return this.connector.IsTransactionInProgress;
            }
        }

        /// <summary>
        /// Begins new transaction.
        /// </summary>
        /// <param name="isolationLevel">Isolation level to use for transaction.</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.connector.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Commits transaction.
        /// </summary>
        public void CommitTransaction()
        {
            this.connector.CommitTransaction();
        }

        /// <summary>
        /// Rollbacks transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            this.connector.RollbackTransaction();
        }

        /// <summary>
        /// Executes command, that returns rows from database.
        /// </summary>
        /// <typeparam name="TItem">Type of item returned in a row.</typeparam>
        /// <param name="commandText">Command text to execute.</param>
        /// <param name="parameters">Command parameters to pass.</param>
        /// <param name="commandModifier">Delegate, that allows modification of command just before execution.</param>
        /// <param name="rowReader">Delegate for reading single row from database.</param>
        /// <returns>Collection of rows found.</returns>
        protected Collection<TItem> ExecuteQuery<TItem>(
            string commandText,
            Dictionary<string, object> parameters,
            Action<DbCommand> commandModifier,
            RowReader<TItem> rowReader)
            where TItem : class
        {
            if (rowReader == null)
            {
                throw new ArgumentNullException("rowReader", "Row reader is required");
            }

            return this.Execute(
                commandText,
                parameters,
                commandModifier,
                command =>
                {
                    var reader = command.ExecuteReader();
                    var result = new Collection<TItem>();
                    while (reader.Read())
                    {
                        result.Add(item: rowReader(reader));
                    }

                    return result;
                });
        }

        /// <summary>
        /// Executes command, that returns rows from database.
        /// </summary>
        /// <typeparam name="TItem">Type of item returned in a row.</typeparam>
        /// <param name="commandText">Command text to execute.</param>
        /// <param name="parameters">Command parameters to pass.</param>
        /// <param name="rowReader">Delegate for reading single row from database.</param>
        /// <returns>Collection of rows found.</returns>
        protected Collection<TItem> ExecuteQuery<TItem>(
            string commandText,
            Dictionary<string, object> parameters,
            RowReader<TItem> rowReader)
            where TItem : class
        {
            return ExecuteQuery(commandText, parameters, null, rowReader);
        }

        /// <summary>
        /// Executes command, that returns it's result as return value
        /// </summary>
        /// <typeparam name="TResult">Type of result returned.</typeparam>
        /// <param name="commandText">Command text to execute.</param>
        /// <param name="parameters">Command parameters to pass.</param>
        /// <param name="commandModifier">Delegate, that allows modification of command just before execution.</param>
        /// <param name="resultConverter">Delegate for converting result to specific type.</param>
        /// <returns>Result received from database.</returns>
        protected TResult ExecuteCommand<TResult>(
            string commandText,
            Dictionary<string, object> parameters,
            Action<DbCommand> commandModifier,
            Func<object, TResult> resultConverter)
        {
            if (resultConverter == null)
            {
                throw new ArgumentNullException("resultConverter", "Row reader is required");
            }

            return this.Execute(
                commandText,
                parameters,
                commandModifier,
                command =>
                {
                    var returnValueKey = this.AttachReturnValue(command, parameters);
                    command.ExecuteNonQuery();
                    var parameter = command.Parameters[returnValueKey];
                    if (parameter != null)
                    {
                        return resultConverter(parameter.Value);
                    }

                    return default(TResult);
                });
        }

        /// <summary>
        /// Executes command, that returns it's result as return value
        /// </summary>
        /// <typeparam name="TResult">Type of result returned.</typeparam>
        /// <param name="commandText">Command text to execute.</param>
        /// <param name="parameters">Command parameters to pass.</param>
        /// <param name="resultConverter">Delegate for converting result to specific type.</param>
        /// <returns>Result received from database.</returns>
        protected TResult ExecuteCommand<TResult>(
            string commandText,
            Dictionary<string, object> parameters,
            Func<object, TResult> resultConverter)
        {
            return ExecuteCommand(commandText, parameters, null, resultConverter);
        }

        /// <summary>
        /// Executes command against a database.
        /// </summary>
        /// <typeparam name="T">Type of database result.</typeparam>
        /// <param name="commandText">Command text to execute.</param>
        /// <param name="parameters">Command parameters to pass.</param>
        /// <param name="commandModifier">Delegate, that allows modification of command just before execution.</param>
        /// <param name="commandExecution">Delegate executing logic for retrieving data from command.</param>
        /// <returns>Result received from database.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Command text is used always as a stored procedure name")]
        private T Execute<T>(
            string commandText,
            Dictionary<string, object> parameters,
            Action<DbCommand> commandModifier,
            Func<DbCommand, T> commandExecution)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText", "Command text must not be empty");
            }

            using (var command = this.connector.CreateCommand())
            {
                command.CommandText = commandText;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    this.AttachInputParameters(command, parameters);
                }

                command.Connection = this.connector.Connection;
                if (commandModifier != null)
                {
                    commandModifier(command);
                }

                return commandExecution(command);
            }
        }

        /// <summary>
        /// Creates instance of parameter.
        /// </summary>
        /// <param name="parameter">Parameter data to use for creation of database parameter.</param>
        /// <returns>Parameter created.</returns>
        private DbParameter CreateParameter(KeyValuePair<string, object> parameter)
        {
            var result = this.connector.CreateParameter();
            result.ParameterName = parameter.Key;
            result.Value = parameter.Value;
            var resolvedType = ParameterTypeResolvers.ResolveType(parameter.Value);
            if (resolvedType.HasValue)
            {
                result.DbType = resolvedType.Value;
            }

            return result;
        }

        /// <summary>
        /// Attaches input parameters to database command.
        /// </summary>
        /// <param name="command">Command to attach parameters to.</param>
        /// <param name="parameters">Parameters to be attached.</param>
        private void AttachInputParameters(DbCommand command, Dictionary<string, object> parameters)
        {
            foreach (var current in parameters.Select(this.CreateParameter))
            {
                current.Direction = ParameterDirection.Input;
                command.Parameters.Add(current);
            }
        }

        /// <summary>
        /// Attaches unique parameter for retrieving return value from command.
        /// </summary>
        /// <param name="command">Command to attach parameter to.</param>
        /// <param name="parameters">Parameters already attached.</param>
        /// <returns>Name of parameter added.</returns>
        private string AttachReturnValue(DbCommand command, Dictionary<string, object> parameters)
        {
            var parameterName = "ReturnValue";
            if (parameters.ContainsKey(parameterName))
            {
                const string ParameterNameFormat = "ReturnValue{0}";
                var currentIndex = 1;
                do
                {
                    parameterName = string.Format(CultureInfo.InvariantCulture, ParameterNameFormat, currentIndex);
                    currentIndex++;
                }
                while (parameters.ContainsKey(parameterName));
            }

            var parameter = this.connector.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(parameter);
            return parameterName;
        }
    }
}
