// <copyright file="CommandParameter.cs" company="LukaszNowakowski.it">
// LukaszNowakowski.it Łukasz Nowakowski. All rights reserved.
// </copyright>
// <creationDate>2016--5-24</creationDate>
// <author>Łukasz Nowakowski</author>
// <description>Contains information about command parameter.</description>

namespace Lnow.Libraries.DataAccess
{
    using System.Data;

    /// <summary>
    /// Information about command parameter.
    /// </summary>
    public class CommandParameter
    {
        /// <summary>
        /// Name of parameter.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Value of parameter.
        /// </summary>
        private readonly object value;

        /// <summary>
        /// Type of parameter.
        /// </summary>
        private readonly DbType? databaseType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParameter" /> class.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        /// <param name="databaseType">Type of parameter.</param>
        public CommandParameter(string name, object value, DbType? databaseType)
        {
            this.name = name;
            this.value = value;
            this.databaseType = databaseType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParameter" /> class.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        public CommandParameter(string name, object value)
            : this(name, value, null)
        {
        }

        /// <summary>
        /// Gets name of parameter.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets value of parameter.
        /// </summary>
        public object Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Gets type of parameter.
        /// </summary>
        public DbType? DatabaseType
        {
            get
            {
                return this.databaseType;
            }
        }
    }
}