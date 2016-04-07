// <copyright file="RowReader.cs" company="Axa Direct Solutions">
// Axa Direct Solutions SAS S A Uproszczona Oddział w Polsce. All rights reserved.
// </copyright>
// <creationDate>2016-04-07</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains delegate for reading object from a reader row.</description>

namespace Ads.Lnow.Common.DataAccess
{
    using System.Data.Common;

    /// <summary>
    /// Delegate for reading object from a reader row.
    /// </summary>
    /// <typeparam name="TItem">Type of item to retrieve from reader.</typeparam>
    /// <param name="reader">Reader to retrieve item from.</param>
    /// <returns>Item retrieved.</returns>
    public delegate TItem RowReader<out TItem>(DbDataReader reader)
        where TItem : class;
}