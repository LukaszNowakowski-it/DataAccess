// <copyright file="DataReaderExtensionsTests.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-08</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains unit tests for DataReaderExtensions class.</description>

using System.Collections.Generic;
using System.Data.Common;
using Moq;

namespace Lnow.Libraries.DataAccess.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="DataReaderExtensions" /> class.
    /// </summary>
    [TestClass]
    public class DataReaderExtensionsTests
    {
        /// <summary>
        /// Checks if <see cref="DataReaderExtensions.ReadObject" /> retrieves proper value.
        /// </summary>
        [TestMethod]
        public void ReadObjectRetrievesValue()
        {
            object testValue = "123456";
            var reader = CreateReaderMock("ColumnName", testValue);
            var retrieved = reader.ReadObject("ColumnName");
            Assert.AreSame(testValue, retrieved, "Instances are different");
        }

        /// <summary>
        /// Checks if <see cref="DataReaderExtensions.ReadValueType{T}" /> retrieves proper value.
        /// </summary>
        [TestMethod]
        public void ReadValueTypeRetrievesValue()
        {
            const int value = 123;
            var reader = CreateReaderMock("123", value);
            var retrieved = reader.ReadValueType("123", Convert.ToInt32);
            Assert.AreEqual(value, retrieved, "Invalid value retrieved");
        }

        /// <summary>
        /// Checks if <see cref="DataReaderExtensions.ReadValueType{T}" /> retrieves proper value.
        /// </summary>
        [TestMethod]
        public void ReadValueTypeNullableRetrievesValue()
        {
            int? value = 123;
            var reader = CreateReaderMock("123", value);
            var retrieved = reader.ReadValueTypeNullable("123", Convert.ToInt32);
            Assert.AreEqual(value, retrieved, "Invalid value retrieved");
        }

        /// <summary>
        /// Creates mock of <see cref="DbDataReader" /> with a single column.
        /// </summary>
        /// <param name="column">Name of column to read.</param>
        /// <param name="value">Value in the column.</param>
        /// <returns>Mock created.</returns>
        private static DbDataReader CreateReaderMock(string column, object value)
        {
            return CreateReaderMock(new Dictionary<string, object>() {{column, value}});
        }

        /// <summary>
        /// Creates mock of <see cref="DbDataReader" /> with multiple columns.
        /// </summary>
        /// <param name="columns">Definition of columns to use.</param>
        /// <returns>Mock created.</returns>
        private static DbDataReader CreateReaderMock(Dictionary<string, object> columns)
        {
            var mock = new Mock<DbDataReader>();
            foreach (var column in columns)
            {
                var columnName = column.Key;
                var value = column.Value;
                mock.SetupGet(r => r[columnName]).Returns(value);
            }

            return mock.Object;
        }
    }
}
