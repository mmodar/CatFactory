﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.ObjectRelationalMapping
{
    /// <summary>
    /// Represents a database
    /// TODO refactor to work with case sensitive database and/or collations as indicated by the data
    /// <![CDATA[  https://www.webucator.com/how-to/how-check-case-sensitivity-sql-server.cfm  ]]>
    /// </summary>
    [DebuggerDisplay("Name={Name}, DbObjects={DbObjects.Count}, Tables={Tables.Count}, Views={Views.Count}")]
    public class Database
    {
        #region [ Fields ]

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<DbObject> m_dbObjects;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private IDatabaseNamingConvention m_namingConvention;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<DatabaseTypeMap> m_databaseTypeMaps;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<ExtendedProperty> m_extendedProperties;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<Table> m_tables;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<View> m_views;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<ScalarFunction> m_scalarFunctions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<TableFunction> m_tableFunctions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<StoredProcedure> m_storedProcedures;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private List<Sequence> m_sequences;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of <see cref="Database"/> class
        /// </summary>
        public Database()
        {
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the data source
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets the catalog (Database name)
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or sets the name for database
        /// </summary>
        [Obsolete("Prefer Catalog over Name")]
        public string Name
        {
            get => Catalog;
            set => Catalog = value;
        }

        /// <summary>
        /// Gets or sets the default schema for database
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// Gets or sets if database supports transactions
        /// </summary>
        public bool SupportTransactions { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [Obsolete("Save description as extended property")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Db objects
        /// </summary>
        public List<DbObject> DbObjects
        {
            get => m_dbObjects ?? (m_dbObjects = new List<DbObject>());
            set => m_dbObjects = value;
        }

        /// <summary>
        /// Gets or sets naming convention for database
        /// </summary>
        [XmlIgnore]
        public IDatabaseNamingConvention NamingConvention
        {
            get => m_namingConvention ?? (m_namingConvention = new DatabaseNamingConvention());
            set => m_namingConvention = value;
        }

        /// <summary>
        /// Gets or sets database type maps (data type equivalents)
        /// </summary>
        public List<DatabaseTypeMap> DatabaseTypeMaps
        {
            get => m_databaseTypeMaps ?? (m_databaseTypeMaps = new List<DatabaseTypeMap>());
            set => m_databaseTypeMaps = value;
        }

        /// <summary>
        /// Gets or sets the extended properties
        /// </summary>
        public List<ExtendedProperty> ExtendedProperties
        {
            get => m_extendedProperties ?? (m_extendedProperties = new List<ExtendedProperty>());
            set => m_extendedProperties = value;
        }

        /// <summary>
        /// Gets or sets the tables
        /// </summary>
        public List<Table> Tables
        {
            get => m_tables ?? (m_tables = new List<Table>());
            set => m_tables = value;
        }

        /// <summary>
        /// Gets or sets the views
        /// </summary>
        public List<View> Views
        {
            get => m_views ?? (m_views = new List<View>());
            set => m_views = value;
        }

        /// <summary>
        /// Gets or sets the scalar functions
        /// </summary>
        public List<ScalarFunction> ScalarFunctions
        {
            get => m_scalarFunctions ?? (m_scalarFunctions = new List<ScalarFunction>());
            set => m_scalarFunctions = value;
        }

        /// <summary>
        /// Gets or sets the table functions
        /// </summary>
        public List<TableFunction> TableFunctions
        {
            get => m_tableFunctions ?? (m_tableFunctions = new List<TableFunction>());
            set => m_tableFunctions = value;
        }

        /// <summary>
        /// Gets or sets the store procedures
        /// </summary>
        public List<StoredProcedure> StoredProcedures
        {
            get => m_storedProcedures ?? (m_storedProcedures = new List<StoredProcedure>());
            set => m_storedProcedures = value;
        }

        /// <summary>
        /// Gets or sets the sequences
        /// </summary>
        public List<Sequence> Sequences
        {
            get => m_sequences ?? (m_sequences = new List<Sequence>());
            set => m_sequences = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets database objects by schema
        /// </summary>
        /// <param name="schema">Schema name</param>
        /// <returns>A <see cref="List{DbObject}"/></returns>
        public virtual List<DbObject> GetDbObjectsBySchema(string schema)
            => DbObjects.Where(item => item.Schema == schema).ToList();

        /// <summary>
        /// Gets a table from table list by name
        /// </summary>
        /// <param name="name">Name for table</param>
        /// <returns>An instance of <see cref="Table"/> class</returns>
        /// <remarks>
        /// TODO refactor to work with case sensitive database and/or collations as indicated by the data
        /// <![CDATA[  https://www.webucator.com/how-to/how-check-case-sensitivity-sql-server.cfm  ]]>
        /// </remarks>
        public virtual Table FindTable(string name)
        {
            string Join4(Table item)
                => string.Join(".", new string[] { item.DataSource, item.Catalog, item.Schema, item.Name });

            string Join3(Table item)
                => string.Join(".", new string[] { item.Catalog, item.Schema, item.Name });

            string Join2(Table item)
                => string.Join(".", new string[] { item.Schema, item.Name });

            var table = Tables.FirstOrDefault(item => string.Equals(Join4(item: item), name, StringComparison.OrdinalIgnoreCase)
                || string.Equals(Join3(item: item), name, StringComparison.OrdinalIgnoreCase)
                || string.Equals(Join2(item: item), name, StringComparison.OrdinalIgnoreCase));

            if (table == null)
                table = Tables.FirstOrDefault(item => string.Join(".", new string[] { Catalog, item.Schema, item.Name }) == name);

            return table;
        }

        /// <summary>
        /// Gets tables from table list by schema
        /// </summary>
        /// <param name="schema">Schema</param>
        /// <returns>A <see cref="IEnumerable{Table}"/></returns>
        public virtual IEnumerable<Table> FindTablesBySchema(string schema)
            => Tables.Where(item => item.Schema == schema);

        /// <summary>
        /// Gets tables from table list by name
        /// </summary>
        /// <param name="name">Name for tables</param>
        /// <returns>A <see cref="IEnumerable{Table}"/></returns>
        public virtual IEnumerable<Table> FindTablesByName(string name)
            => Tables.Where(item => item.Name == name);

        /// <summary>
        /// Gets view from view list by name
        /// </summary>
        /// <param name="name">Name for view</param>
        /// <returns>A <see cref="View"/></returns>
        public virtual View FindView(string name)
        {
            var view = Views.FirstOrDefault(item => string.Join(".", new string[] { item.Schema, item.Name }) == name);

            if (view == null)
                view = Views.FirstOrDefault(item => string.Join(".", new string[] { Catalog, item.Schema, item.Name }) == name);

            return view;
        }

        /// <summary>
        /// Gets views from views list by name
        /// </summary>
        /// <param name="schema">Schema name</param>
        /// <returns>A <see cref="IEnumerable{View}"/></returns>
        public virtual IEnumerable<View> FindViewsBySchema(string schema)
            => Views.Where(item => item.Schema == schema);

        /// <summary>
        /// Gets a view from view list by name
        /// </summary>
        /// <param name="name">Name for views</param>
        /// <returns>A <see cref="IEnumerable{View}"/></returns>
        public virtual IEnumerable<View> FindViewsByName(string name)
            => Views.Where(item => item.Name == name);

        #endregion
    }
}
