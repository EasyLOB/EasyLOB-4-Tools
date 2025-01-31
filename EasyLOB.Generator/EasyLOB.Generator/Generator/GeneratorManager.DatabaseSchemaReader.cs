using DatabaseSchemaReader.DataSchema;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Generator
{
    public partial class GeneratorManager
    {
        #region Methods DatabaseSchemaReader

        public List<TableSchema> DatabaseReader2CodeSmith(DatabaseSchema dbSchema)
        {
            List<TableSchema> tables = new List<TableSchema>();

            foreach (DatabaseTable table in dbSchema.Tables)
            {

                // Table

                string schema = string.IsNullOrEmpty(table.SchemaOwner) ? "" : table.SchemaOwner + ".";
                TableSchema tableSchema = new TableSchema
                {
                    Name = table.Name,
                    FullName = schema + table.Name
                };

                // Columns

                foreach (DatabaseColumn column in table.Columns)
                {
                    //if (table.Name == "?" && column.Name == "?")
                    //{
                    //    int providerDbType = column.DataType.ProviderDbType;
                    //    SqlDbType sqlDbType = (System.Data.SqlDbType)providerDbType;
                    //}

                    int length = column.Length ?? 0;
                    if (column.DataType is null)
                    {
                        length = 256;
                        ColumnSchema columnSchema = new ColumnSchema
                        {
                            Name = column.Name,
                            AllowDBNull = column.Nullable,
                            DataType = DbType.String,
                            IsAutoNumber = column.IsAutoNumber,
                            IsForeignKeyMember = column.IsForeignKey,
                            IsPrimaryKeyMember = column.IsPrimaryKey,
                            Size = length,

                            DbDataType = "nvarchar",
                            NetDataType = "System.String"
                        };

                        tableSchema.Columns.Add(columnSchema);
                    }
                    else
                    {
                        length = column.DataType.IsString ? (length == 0 ? 1024 : length) : 0;
                        ColumnSchema columnSchema = new ColumnSchema
                        {
                            Name = column.Name,
                            AllowDBNull = column.Nullable,
                            DataType = DataType2DbType(column.DataType),
                            IsAutoNumber = column.IsAutoNumber,
                            IsForeignKeyMember = column.IsForeignKey,
                            IsPrimaryKeyMember = column.IsPrimaryKey,
                            Size = length,

                            DbDataType = column.DbDataType, // "image"
                            NetDataType = column.DataType.NetDataType // "System.Byte[]"
                        };

                        tableSchema.Columns.Add(columnSchema);
                    }
                }

                // PrimaryKey

                if (table.PrimaryKey != null && table.PrimaryKey.Columns.Count > 0)
                {
                    tableSchema.PrimaryKey = new PrimaryKeySchema();
                    foreach (string constraintColumn in table.PrimaryKey.Columns)
                    {
                        ColumnSchema columnSchema = GetColumnSchema(tableSchema, constraintColumn);
                        tableSchema.PrimaryKey.MemberColumns.Add(ColumnSchema2MemberColumnSchema(columnSchema));
                    }
                }

                // ForeignKeys

                if (table.ForeignKeys.Count > 0)
                {
                    foreach (DatabaseConstraint constraint in table.ForeignKeys)
                    {
                        TableKeySchema tableKeySchema = new TableKeySchema();

                        // PrimaryKey <-

                        string pkSchema = string.IsNullOrEmpty(constraint.RefersToSchema) ? "" : constraint.RefersToSchema + ".";
                        tableKeySchema.PrimaryKeyTable = new TableSchema
                        {
                            Name = constraint.RefersToTable,
                            FullName = pkSchema + constraint.RefersToTable
                        };

                        foreach (string column in constraint.ReferencedColumns(table.DatabaseSchema))
                        {
                            MemberColumnSchema columnSchema = new MemberColumnSchema
                            {
                                Name = column
                            };

                            tableKeySchema.PrimaryKeyMemberColumns.Add(columnSchema);
                        }

                        // ForeignKey ->

                        tableKeySchema.ForeignKeyTable = tableSchema;
                        foreach (string column in constraint.Columns)
                        {
                            MemberColumnSchema columnSchema = new MemberColumnSchema
                            {
                                Name = column
                            };

                            tableKeySchema.ForeignKeyMemberColumns.Add(columnSchema);
                        }

                        tableSchema.ForeignKeys.Add(tableKeySchema);
                    }
                }

                tables.Add(tableSchema);
            }

            foreach (TableSchema table in tables)
            {

                // NonPrimaryKeyColumns

                foreach (ColumnSchema column in table.Columns)
                {
                    if (!column.IsPrimaryKeyMember)
                    {
                        table.NonPrimaryKeyColumns.Add(column);
                    }
                }

                // ForeignKeys

                //int index = 0;
                foreach (TableKeySchema tableKey in table.ForeignKeys)
                {

                    // Primary Key

                    TableSchema tablePK = GetTableSchema(tables, tableKey.PrimaryKeyTable.FullName);
                    if (tablePK != null)
                    {
                        //table.ForeignKeys[index].PrimaryKeyTable = tablePK;

                        List<MemberColumnSchema> columns = new List<MemberColumnSchema>();

                        foreach (MemberColumnSchema memberColumn in tableKey.PrimaryKeyMemberColumns)
                        {
                            ColumnSchema column = GetColumnSchema(tablePK, memberColumn.Name);
                            if (column != null)
                            {
                                columns.Add(ColumnSchema2MemberColumnSchema(column));
                            }
                            else
                            {
                                columns.Add(null);
                            }
                        }

                        int indexPK = 0;
                        foreach (MemberColumnSchema memberColumn in columns)
                        {
                            if (memberColumn != null)
                            {
                                tableKey.PrimaryKeyMemberColumns[indexPK] = memberColumn;
                            }

                            indexPK++;
                        }
                    }

                    // Foreign Key

                    TableSchema tableFK = GetTableSchema(tables, tableKey.ForeignKeyTable.FullName);
                    if (tableFK != null)
                    {
                        //table.ForeignKeys[index].ForeignKeyTable = tableFK;

                        List<MemberColumnSchema> columns = new List<MemberColumnSchema>();

                        foreach (MemberColumnSchema memberColumn in tableKey.ForeignKeyMemberColumns)
                        {
                            ColumnSchema column = GetColumnSchema(tableFK, memberColumn.Name);
                            if (column != null)
                            {
                                columns.Add(ColumnSchema2MemberColumnSchema(column));
                            }
                            else
                            {
                                columns.Add(null);
                            }
                        }

                        int indexPK = 0;
                        foreach (MemberColumnSchema memberColumn in columns)
                        {
                            if (memberColumn != null)
                            {
                                tableKey.ForeignKeyMemberColumns[indexPK] = memberColumn;
                            }

                            indexPK++;
                        }
                    }
                }
            }

            // PrimaryKeys

            foreach (TableSchema table in tables)
            {
                foreach (TableKeySchema tableKey in table.ForeignKeys)
                {
                    TableSchema pkTable = GetTableSchema(tables, tableKey.PrimaryKeyTable.FullName);
                    if (pkTable != null)
                    {
                        pkTable.PrimaryKeys.Add(tableKey);
                    }
                }
            }

            // Alphabetic Order
            foreach (TableSchema table in tables)
            {
                List<TableKeySchema> pks = table.PrimaryKeys.OrderBy(x => x.ForeignKeyTable.Name).ToList();
                table.PrimaryKeys = pks;

                List<TableKeySchema> fks = table.ForeignKeys.OrderBy(x => x.PrimaryKeyTable.Name).ToList();
                table.ForeignKeys = fks;
            }

            return tables;
        }

        protected MemberColumnSchema ColumnSchema2MemberColumnSchema(ColumnSchema columnSchema)
        {
            return new MemberColumnSchema
            {
                Name = columnSchema.Name,
                AllowDBNull = columnSchema.AllowDBNull,
                DataType = columnSchema.DataType,
                IsAutoNumber = columnSchema.IsAutoNumber,
                IsForeignKeyMember = columnSchema.IsForeignKeyMember,
                IsPrimaryKeyMember = columnSchema.IsPrimaryKeyMember,
                Size = columnSchema.Size
            };
        }

        protected DbType DataType2DbType(DataType dataType)
        {
            string netDataType = dataType.NetDataType.Replace("System.", "").Replace("[]", "");
            switch (netDataType)
            {
                case "AnsiString": return DbType.AnsiString;
                case "AnsiStringFixedLength": return DbType.AnsiStringFixedLength;
                case "Binary": return DbType.Binary;
                case "Boolean": return DbType.Boolean;
                case "Byte":
                    if (dataType.NetDataType.Replace("System.", "") == "Byte[]")
                        return DbType.Binary;
                    else
                        return DbType.Byte;
                case "Currency": return DbType.Currency;
                case "Decimal":
                    if (dataType.TypeName == "money")
                        return DbType.Currency;
                    else
                        return DbType.Decimal;
                case "Date": return DbType.Date;
                case "DateTime": return DbType.DateTime;
                case "DateTime2": return DbType.DateTime2;
                case "DateTimeOffset": return DbType.DateTimeOffset;
                case "Double": return DbType.Double;
                case "Guid": return DbType.Guid;
                case "Int16": return DbType.Int16;
                case "Int32": return DbType.Int32;
                case "Int64": return DbType.Int64;
                case "Object": return DbType.Object;
                case "SByte": return DbType.SByte;
                case "Single": return DbType.Single;
                case "String": return DbType.String;
                case "StringFixedLength": return DbType.StringFixedLength;
                case "Time": return DbType.Time;
                case "UInt16": return DbType.UInt16;
                case "UInt32": return DbType.UInt32;
                case "UInt64": return DbType.UInt64;
                case "VarNumeric": return DbType.VarNumeric;
                case "Xml": return DbType.Xml;
                default: return DbType.String;
            }
        }

        protected ColumnSchema GetColumnSchema(TableSchema table, string columnName)
        {
            ColumnSchema columnSchema = null;

            foreach (ColumnSchema column in table.Columns)
            {
                if (column.Name == columnName)
                {
                    columnSchema = column;
                    break;
                }
            }

            return columnSchema;
        }

        protected TableSchema GetTableSchema(List<TableSchema> tables, string tableFullName)
        {
            TableSchema tableSchema = null;

            foreach (TableSchema table in tables)
            {
                if (table.FullName == tableFullName)
                {
                    tableSchema = table;
                    break;
                }
            }

            return tableSchema;
        }

        #endregion Methods DatabaseSchemaReader
    }
}
