using System.Collections.Generic;
using System.Data;

namespace Generator
{
    public class TableSchema
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public List<ColumnSchema> Columns { get; set; }

        public bool HasPrimaryKey { get { return PrimaryKey != null; } }

        public PrimaryKeySchema PrimaryKey { get; set; }

        public List<TableKeySchema> PrimaryKeys { get; set; }

        public List<ColumnSchema> NonPrimaryKeyColumns { get; set; }

        public List<TableKeySchema> ForeignKeys { get; set; }

        public TableSchema()
        {
            Columns = new List<ColumnSchema>();
            PrimaryKey = null; // new PrimaryKeySchema();
            PrimaryKeys = new List<TableKeySchema>();
            NonPrimaryKeyColumns = new List<ColumnSchema>();
            ForeignKeys = new List<TableKeySchema>();
        }
    }

    public class ColumnSchema
    {
        public string Name { get; set; }

        public bool AllowDBNull { get; set; }

        public DbType DataType { get; set; }

        public bool IsAutoNumber { get; set; }

        public bool IsForeignKeyMember { get; set; }

        public bool IsPrimaryKeyMember { get; set; }

        public int Size { get; set; }

        //

        public string DbDataType { get; set; }

        public string NetDataType { get; set; }
    }

    public class PrimaryKeySchema
    {
        public string FullName { get; set; }

        public List<MemberColumnSchema> MemberColumns { get; set; }

        public PrimaryKeySchema()
        {
            MemberColumns = new List<MemberColumnSchema>();
        }
    }

    public class MemberColumnSchema : ColumnSchema
    {
    }

    public class TableKeySchema
    {
        public TableSchema ForeignKeyTable { get; set; }

        public List<MemberColumnSchema> ForeignKeyMemberColumns { get; set; }

        public TableSchema PrimaryKeyTable { get; set; }

        public List<MemberColumnSchema> PrimaryKeyMemberColumns { get; set; }

        public TableKeySchema()
        {
            ForeignKeyTable = null;
            ForeignKeyMemberColumns = new List<MemberColumnSchema>();
            PrimaryKeyTable = null;
            PrimaryKeyMemberColumns = new List<MemberColumnSchema>();
        }
    }
}
