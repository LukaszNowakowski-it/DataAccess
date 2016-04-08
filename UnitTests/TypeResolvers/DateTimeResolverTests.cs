// <copyright file="DateTimeResolverTests.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016-04-08</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains test methods for DateTimeResolver class.</description>

namespace Lnow.Libraries.DataAccess.UnitTests.TypeResolvers
{
    using System;
    using System.Data;
    using DataAccess.TypeResolvers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test methods for <see cref="DateTimeResolver" /> class.
    /// </summary>
    [TestClass]
    public class DateTimeResolverTests
    {
        /// <summary>
        /// Tests if for current date null value is resolved.
        /// </summary>
        [TestMethod]
        public void ResolvesNullForCurrentDate()
        {
            var value = DateTime.Now;
            var resolver = new DateTimeResolver();
            var databaseType = resolver.ResolveType(value);
            Assert.IsNull(databaseType, "Null value should be retrieved");
        }

        /// <summary>
        /// Tests if type resolved can store minimal value of <see cref="DateTime" />.
        /// </summary>
        [TestMethod]
        public void ResolvesDateTime2ForMinValue()
        {
            var value = DateTime.MinValue;
            var resolver = new DateTimeResolver();
            var databaseType = resolver.ResolveType(value);
            Assert.IsNotNull(databaseType, "Not null value should be calculated");
            Assert.AreEqual(DbType.DateTime2, databaseType.Value, "Database type should be DateTime2");
        }
    }
}
