using System.Collections.Generic;
using System.IO;

// Persistence_EntityFramework_DbContext
// Persistence_EntityFramework_Configuration

namespace Generator
{
    public partial class GeneratorManagerFramework
    {
        #region Persistence

        public void Persistence_EntityFramework_DbContext(string Application,
            string Namespace,
            Cultures Culture,
            List<TableSchema> SourceTables,
            string filePath)
        {
            using (StreamWriter file = CreateStreamWriter(filePath))
            {
                file.WriteLine($@"using {Namespace.Replace(".Persistence", ".Data")};
using System.Data.Entity;

namespace {Namespace}
{{
    public partial class {Application}DbContext : DbContext
    {{
        #region Properties

        //public DbSet<ModuleInfo> ModulesInfo {{ get; set; }}
");

                foreach (TableSchema table in SourceTables)
                {
                    file.WriteLine($@"        public DbSet<{ClassName(table.Name, Culture)}> {Plural(ClassName(table.Name, Culture), Culture)} {{ get; set; }}
");
                }

                file.WriteLine($@"        #endregion Properties

        #region Methods

        static {Application}DbContext()
        {{
            /*
            // Refer to <configuration><entityframework><contexts> section in Web.config or App.config
            //Database.SetInitializer<{Application}DbContext>(null);
            //Database.SetInitializer<{Application}DbContext>(new CreateDatabaseIfNotExists<{Application}DbContext>());
             */
        }}

        public {Application}DbContext()
            : base(""Name={Application}"")
        {{
            Setup();
        }}

        //public {Application}DbContext(string connectionString)
        //    : base(connectionString)
        //{{
        //    Setup();
        //}}

        //public {Application}DbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
        //    : base(objectContext, dbContextOwnsObjectContext)
        //{{
        //    Setup();
        //}}

        //public {Application}DbContext(DbConnection connection)
        //    : base(connection, false)
        //{{
        //    Setup();
        //}}

        private void Setup()
        {{
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            Database.Log = null;
            //Database.Log = Console.Write;
            //Database.Log = log => EntityFrameworkHelper.Log(log, ZLibrary.ZDatabaseLogger.File);
            //Database.Log = log => EntityFrameworkHelper.Log(log, ZLibrary.ZDatabaseLogger.NLog);
        }}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {{
            //modelBuilder.Entity<ModuleInfo>().Map(t =>
            //{{
            //    t.ToTable(""ModuleInfo"");
            //}});
");
                foreach (TableSchema table in SourceTables)
                {
                    file.WriteLine($@"            modelBuilder.Configurations.Add(new {ClassName(table.Name, Culture)}Configuration());");
                }

                file.WriteLine($@"        }}

        #endregion Methods
    }}
}}");
            }
        }

        public void Persistence_EntityFramework_Configuration(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath)
        {
            using (StreamWriter file = CreateStreamWriter(filePath))
            {
                string tableName = TableName(SourceTable.FullName);
                string className = ClassName(SourceTable.FullName, Culture);
                string objectName = ObjectName(SourceTable.FullName, Culture);

                // Associations

                Dictionary<string, int> associations123 = new Dictionary<string, int>();
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    Dictionary123(associations123, pkClassName);
                }

                file.WriteLine($@"using {Namespace.Replace(".Persistence", ".Data")};
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace {Namespace}
{{
    public class {className}Configuration : EntityTypeConfiguration<{className}>
    {{
        public {className}Configuration()
        {{
            #region Class

            this.ToTable(""{tableName}"");
");

                if (SourceTable.PrimaryKey.MemberColumns.Count == 1)
                {
                    file.WriteLine($@"            this.HasKey(x => x.{PropertyName(SourceTable.PrimaryKey.MemberColumns[0].Name)});");
                }
                else
                {
                    string columns = "";
                    string comma = "";
                    foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                    {
                        columns += comma + "x." + PropertyName(column.Name);
                        comma = ", ";
                    }

                    file.WriteLine($@"            this.HasKey(x => new {{ {columns} }});");
                }

                file.WriteLine($@"
            #endregion Class

            #region Properties");

                int order = 1;
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    bool propertyIsPrimaryKey = column.IsPrimaryKeyMember;
                    bool propertyIsIdentity = IsIdentity(SourceTable.PrimaryKey.MemberColumns[0]);
                    bool propertyIsNullable = column.AllowDBNull;
                    bool isNullable = column.AllowDBNull;

                    file.WriteLine($@"
            this.Property(x => x.{PropertyName(column.Name)})
                .HasColumnName(""{column.Name}"")");

                    if (propertyIsPrimaryKey && SourceTable.PrimaryKey.MemberColumns.Count > 1)
                    {
                        file.WriteLine($@"                .HasColumnOrder({ order++.ToString() })");
                    }

                    file.Write($@"                .HasColumnType(""{GetSqlType(column)}"")");

                    if (propertyIsPrimaryKey)
                    {
                        if (propertyIsIdentity)
                        {
                            file.Write($"\r\n                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)");
                        }
                        else
                        {
                            file.Write($"\r\n                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)");
                        }
                    }
                    if ((IsString(column.DataType) && !IsNText(column) && !IsText(column)) || IsBinary(column.DataType) || IsImage(column))
                    {
                        string maxLength = column.Size.ToString();
                        if (IsImage(column))
                        {
                            maxLength = 8192.ToString();
                        }
                        else if (column.Size == -1)
                        {
                            if (IsBinary(column.DataType))
                            {
                                maxLength = 65536.ToString();
                            }
                            else
                            {
                                maxLength = 1024.ToString();
                            }
                        }

                        file.Write($"\r\n                .HasMaxLength({maxLength})");
                    }
                    if (!isNullable)
                    {
                        file.Write($"\r\n                .IsRequired()");
                    }
                    file.WriteLine($";");
                }

                file.WriteLine($@"
            #endregion Properties");

                if (SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine($@"
            #region Associations (FK)");

                    Dictionary1(associations123);
                    foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                    {
                        string fkClassName = ClassName(fkTable.ForeignKeyTable.FullName, Culture);
                        ColumnSchema fkColumn = fkTable.ForeignKeyMemberColumns[0];
                        string fkPropertyName = PropertyName(fkColumn.Name);

                        string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);
                        ColumnSchema pkColumn = fkTable.PrimaryKeyMemberColumns[0];
                        string pkPropertyName = PropertyName(pkColumn.Name);
                        pkPropertyName = pkPropertyName == className ? pkPropertyName + fkPropertyName : pkPropertyName;

                        bool isNullable = fkColumn.AllowDBNull;
                        string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        string foreignKeys = "";
                        if (fkTable.ForeignKeyMemberColumns.Count == 1)
                        {
                            foreignKeys = "x." + PropertyName(fkTable.ForeignKeyMemberColumns[0].Name);
                        }
                        else
                        {
                            foreach (ColumnSchema column in fkTable.ForeignKeyMemberColumns)
                            {
                                foreignKeys += (string.IsNullOrEmpty(foreignKeys) ? "" : ", ") + "x." + PropertyName(column.Name);
                            }
                            foreignKeys = "new { " + foreignKeys + " }";
                        }

                        if (isNullable)
                        {
                            file.WriteLine($@"
            this.HasOptional(x => x.{pkClassName2}{x})");
                        }
                        else
                        {
                            file.WriteLine($@"
            this.HasRequired(x => x.{pkClassName2}{x})");
                        }

                        file.WriteLine($@"                .WithMany(x => x.{Plural(fkClassName, Culture)}{x})
                .HasForeignKey(x => {foreignKeys});");
                    }

                    file.WriteLine($@"
            #endregion Associations (FK)");
                }

                file.WriteLine($@"        }}
    }}
}}");
            }
        }

        #endregion Persistence
    }
}
