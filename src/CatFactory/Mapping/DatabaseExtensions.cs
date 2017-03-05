﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CatFactory.Mapping
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<DbObject> GetTables(this Database db)
            => db.DbObjects.Where(item => item.Type == "USER_TABLE");

        public static IEnumerable<DbObject> GetViews(this Database db)
            => db.DbObjects.Where(item => item.Type == "VIEW");

        public static IEnumerable<DbObject> GetProcedures(this Database db)
            => db.DbObjects.Where(item => item.Type == "SQL_STORED_PROCEDURE");

        public static IEnumerable<DbObject> GetScalarFunctions(this Database db)
            => db.DbObjects.Where(item => item.Type == "SQL_SCALAR_FUNCTION");

        public static IEnumerable<DbObject> GetTableFunctions(this Database db)
            => db.DbObjects.Where(item => item.Type == "SQL_TABLE_VALUED_FUNCTION");

        public static void AddPrimaryKeyToTables(this Database database)
        {
            foreach (var table in database.Tables)
            {
                if (table.PrimaryKey == null && table.Columns.Count > 0)
                {
                    table.PrimaryKey = new PrimaryKey(table.Columns[0].Name);
                }
            }
        }

        public static void AddColumnsForAllTables(this Database database, Column[] columns, params String[] exclusions)
        {
            foreach (var table in database.Tables)
            {
                if (exclusions != null && exclusions.Contains(table.FullName))
                {
                    continue;
                }

                foreach (var column in columns)
                {
                    if (!table.Columns.Contains(column))
                    {
                        table.Columns.Add(column);
                    }
                }
            }
        }

        public static void AddColumnForAllTables(this Database database, Column column, params String[] exclusions)
        {
            AddColumnsForAllTables(database, new Column[] { column }, exclusions);
        }

        public static void LinkTables(this Database db)
        {
            foreach (var table in db.Tables)
            {
                foreach (var column in table.Columns)
                {
                    if (table.PrimaryKey != null && table.PrimaryKey.Key.Count == 1 && table.PrimaryKey.Key.Contains(column.Name))
                    {
                        continue;
                    }

                    foreach (var parentTable in db.Tables)
                    {
                        if (table.FullName == parentTable.FullName)
                        {
                            continue;
                        }

                        if (parentTable.PrimaryKey != null && parentTable.PrimaryKey.Key.Contains(column.Name))
                        {
                            table.ForeignKeys.Add(new ForeignKey(column.Name)
                            {
                                ConstraintName = db.NamingConvention.GetForeignKeyConstraintName(table.Name, new String[] { column.Name }, parentTable.Name),
                                References = parentTable.FullName,
                                Child = table.FullName
                            });

                        }
                    }
                }
            }
        }
    }
}
