// <copyright file="IRowReader.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains delegate for reading object from a reader row.</description>

namespace Lnow.Libraries.DataAccess
{
    using System.Data.Common;

    /// <summary>
    /// Interface for classes that read object from a reader row.
    /// </summary>
    /// <typeparam name="TItem">Type of item to retrieve from reader.</typeparam>
    public interface IRowReader<out TItem>
        where TItem : class
    {
        /// <summary>
        /// Reads object from row.
        /// </summary>
        /// <param name="reader">Reader to read from.</param>
        /// <returns>Object read.</returns>
        TItem Read(DbDataReader reader);
    }
}