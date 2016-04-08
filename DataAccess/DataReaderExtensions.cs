// <copyright file="DataReaderExtensions.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains extension methods for DbDataReader.</description>

namespace Lnow.Libraries.DataAccess
{
    using System;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// Extension methods for <see cref="DbDataReader" /> class.
    /// </summary>
    /// <remarks>
    /// These methods are helpers for retrieving data from data reader's row.
    /// </remarks>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Reads an object from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static object ReadObject(this DbDataReader reader, string columnName)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader", "Reader must not be null");
            }

            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName", "Column name must not be empty");
            }

            if (reader.IsClosed)
            {
                throw new ArgumentException("Reader is closed", "reader");
            }

            return reader[columnName];
        }

        /// <summary>
        /// Reads an <see cref="Int16" /> value from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static short ReadInt16(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueType(columnName, Convert.ToInt16);
        }

        /// <summary>
        /// Reads an <see cref="Int16" /> value from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static short? ReadInt16Nullable(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueTypeNullable(columnName, Convert.ToInt16);
        }

        /// <summary>
        /// Reads an <see cref="Int32" /> value from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static int ReadInt32(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueType(columnName, Convert.ToInt32);
        }

        /// <summary>
        /// Reads an <see cref="Int32" /> value from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static int? ReadInt32Nullable(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueTypeNullable(columnName, Convert.ToInt32);
        }

        /// <summary>
        /// Reads an <see cref="Int64" /> value from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static long ReadInt64(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueType(columnName, Convert.ToInt64);
        }

        /// <summary>
        /// Reads an <see cref="Int64" /> value from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static long? ReadInt64Nullable(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueTypeNullable(columnName, Convert.ToInt64);
        }

        /// <summary>
        /// Reads a string from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static string ReadString(this DbDataReader reader, string columnName)
        {
            return reader.ReadReferenceType(columnName, Convert.ToString);
        }

        /// <summary>
        /// Reads a string from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static string ReadStringNullable(this DbDataReader reader, string columnName)
        {
            return reader.ReadReferenceTypeNullable(columnName, Convert.ToString);
        }

        /// <summary>
        /// Reads a <see cref="DateTime" /> from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static DateTime ReadDateTime(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueType(columnName, Convert.ToDateTime);
        }

        /// <summary>
        /// Reads a <see cref="DateTime" /> from reader.
        /// </summary>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <returns>Value retrieved.</returns>
        public static DateTime? ReadDateTimeNullable(this DbDataReader reader, string columnName)
        {
            return reader.ReadValueTypeNullable(columnName, Convert.ToDateTime);
        }

        /// <summary>
        /// Reads a value type value from reader.
        /// </summary>
        /// <typeparam name="T">Type of value to retrieve.</typeparam>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <param name="valueConverter">Value converter to use</param>
        /// <returns>Value retrieved.</returns>
        public static T ReadValueType<T>(this DbDataReader reader, string columnName, Func<object, T> valueConverter)
            where T : struct
        {
            if (valueConverter == null)
            {
                throw new ArgumentNullException("valueConverter", "Value converter must not be null");
            }

            var value = reader.ReadObject(columnName);
            if (value == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Attempted to read non-nullable value from null column '{0}'", columnName));
            }

            return valueConverter(value);
        }

        /// <summary>
        /// Reads a value type value from reader.
        /// </summary>
        /// <typeparam name="T">Type of value to retrieve.</typeparam>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <param name="valueConverter">Value converter to use</param>
        /// <returns>Value retrieved.</returns>
        public static T? ReadValueTypeNullable<T>(this DbDataReader reader, string columnName, Func<object, T> valueConverter)
            where T : struct
        {
            if (valueConverter == null)
            {
                throw new ArgumentNullException("valueConverter", "Value converter must not be null");
            }

            var value = reader.ReadObject(columnName);
            if (value == null)
            {
                return null;
            }

            return valueConverter(value);
        }

        /// <summary>
        /// Reads a reference value from reader.
        /// </summary>
        /// <typeparam name="T">Type of value to retrieve.</typeparam>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <param name="valueConverter">Value converter to use</param>
        /// <returns>Value retrieved.</returns>
        public static T ReadReferenceType<T>(this DbDataReader reader, string columnName, Func<object, T> valueConverter)
            where T : class
        {
            if (valueConverter == null)
            {
                throw new ArgumentNullException("valueConverter", "Value converter must not be null");
            }

            var value = reader.ReadObject(columnName);
            if (value == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Attempted to read non-nullable value from null column '{0}'", columnName));
            }

            return valueConverter(value);
        }

        /// <summary>
        /// Reads a reference value from reader.
        /// </summary>
        /// <typeparam name="T">Type of value to retrieve.</typeparam>
        /// <param name="reader">Reader to read column from.</param>
        /// <param name="columnName">Name of column to read.</param>
        /// <param name="valueConverter">Value converter to use</param>
        /// <returns>Value retrieved.</returns>
        public static T ReadReferenceTypeNullable<T>(this DbDataReader reader, string columnName, Func<object, T> valueConverter)
            where T : class
        {
            if (valueConverter == null)
            {
                throw new ArgumentNullException("valueConverter", "Value converter must not be null");
            }

            var value = reader.ReadObject(columnName);
            if (value == null)
            {
                return null;
            }

            return valueConverter(value);
        }
    }
}