using System.Collections.Generic;
using System.IO;

// Data_DataModel
// Data_DTO
// Data_Resource

namespace Generator
{
    public partial class GeneratorManagerFramework
    {
        #region Data

        public void Data_DataModel(string Application,
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

                // LookupProperty
                //   2nd property, or 1st if there is no 2nd property
                //   1st string property

                string lookupProperty = PropertyName((SourceTable.Columns.Count >= 2) ? SourceTable.Columns[1].Name : SourceTable.Columns[0].Name);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    // First String non-primary-key column
                    if (!column.IsPrimaryKeyMember && IsString(column.DataType))
                    {
                        // First 
                        lookupProperty = PropertyName(column.Name);
                        break;
                    }
                }

                // Associations

                string associations = "";
                string associationKeys = "";
                Dictionary<string, int> associations123 = new Dictionary<string, int>();
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string fkClassName = ClassName(fkTable.ForeignKeyTable.FullName, Culture);
                    ColumnSchema fkColumn = fkTable.ForeignKeyMemberColumns[0];
                    string fkPropertyName = PropertyName(fkColumn.Name);

                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);
                    ColumnSchema pkColumn = fkTable.PrimaryKeyMemberColumns[0];
                    string pkPropertyName = PropertyName(pkColumn.Name);

                    string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                    associations += (associations == "" ? " " : ", ") + "\"" + pkClassName2 + "\"";

                    string associationKey = "";
                    foreach (ColumnSchema column in fkTable.PrimaryKeyMemberColumns)
                    {
                        associationKey += (associationKey == "" ? "" : ", ") + "\"" + PropertyName(column.Name) + "\"";
                    }
                    associationKeys += (associationKeys == "" ? " " : ", ") + "new string[] { " + associationKey + " }";

                    Dictionary123(associations123, pkClassName);
                }

                // Collections

                string collections = "";
                string collectionKeys = "";
                Dictionary<string, int> collections123 = new Dictionary<string, int>();
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);
                    collections += (collections == "" ? " " : ", ") + "\"" + Plural(pkClassName, Culture) + "\"";

                    string collectionKey = "";
                    foreach (ColumnSchema column in primaryKey.PrimaryKeyMemberColumns)
                    {
                        collectionKey += (collectionKey == "" ? " " : ", ") + "\"" + column.Name + "\"";
                    }
                    collectionKeys += (collectionKeys == "" ? " " : ", ") + "new string[] { " + collectionKey + " }";

                    Dictionary123(collections123, pkClassName);
                }

                // KeyProperties

                string keys = "";
                string keyParameters = "";
                string keyLinq = "";
                string keyCommas = "";
                string keyIds = "";
                int linq = 0;
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    keys += (keys == "" ? "" : ", ") + "\"" + PropertyName(column.Name) + "\"";
                    keyParameters += (keyParameters == "" ? "" : ", ") + GetType(column) + " " + ObjectName(column.Name, Culture);
                    keyCommas += (keyCommas == "" ? "" : ", ") + PropertyName(column.Name);
                    keyLinq += (keyLinq == "" ? "" : " && ") + PropertyName(column.Name) + " == @" + linq.ToString();
                    keyIds += (keyIds == "" ? "" : ", ") + "(" + GetType(column, false) + ")ids[" + linq.ToString() + "]";
                    linq++;
                }

                // OrderByExpression
                //   2nd property, or 1st if there is no 2nd property
                //   1st string property

                string orderByExpression = PropertyName((SourceTable.Columns.Count >= 2) ? SourceTable.Columns[1].Name : SourceTable.Columns[0].Name);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    // First String non-primary-key column
                    if (!column.IsPrimaryKeyMember && IsString(column.DataType))
                    {
                        // First 
                        orderByExpression = PropertyName(column.Name);
                        break;
                    }
                }

                int commaParameters = 0;
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    commaParameters++;
                }
                foreach (ColumnSchema column in SourceTable.NonPrimaryKeyColumns)
                {
                    if (!column.AllowDBNull)
                    {
                        commaParameters++;
                    }
                }
                foreach (ColumnSchema column in SourceTable.NonPrimaryKeyColumns)
                {
                    if (column.AllowDBNull)
                    {
                        commaParameters++;
                    }
                }

                file.WriteLine($@"using EasyLOB.Data;
using EasyLOB.Library;
using System;
using System.Collections.Generic;

namespace {Namespace}
{{
    public partial class {className} : ZDataModel
    {{
        #region Properties");

                Dictionary1(associations123);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    bool columnIsPrimaryKey = column.IsPrimaryKeyMember;
                    bool columnIsIdentity = IsIdentity(column);
                    bool columnIsNullable = column.AllowDBNull;
                    bool isNullable = column.AllowDBNull;

                    file.WriteLine();

                    if (!column.IsForeignKeyMember)
                    {
                        if (column.IsPrimaryKeyMember)
                        {
                            file.WriteLine($@"        [ZKey{(columnIsIdentity ? "(true)" : "")}]");
                        }

                        file.WriteLine($@"        public virtual {GetType(column)} {PropertyName(column.Name)} {{ get; set; }}");
                    }
                    else
                    {
                        string fkClassName = "";
                        string fkPropertyName = "";
                        string pkClassName = "";
                        string pkPropertyName = "";
                        string pkClassName2 = "";
                        foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                        {
                            fkClassName = ClassName(fkTable.ForeignKeyTable.FullName, Culture);
                            ColumnSchema fkColumn = fkTable.ForeignKeyMemberColumns[0];
                            fkPropertyName = PropertyName(fkColumn.Name);

                            pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);
                            ColumnSchema pkColumn = fkTable.PrimaryKeyMemberColumns[0];
                            pkPropertyName = PropertyName(pkColumn.Name);

                            pkClassName2 = pkClassName == fkClassName ? pkClassName + "X" : pkClassName;

                            if (fkColumn.Name == column.Name)
                            {
                                break;
                            }
                        }
                        string value = isNullable ? $"value ?? LibraryDefaults.Default_{GetDbType(column.DataType)}" : "value";

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"        private {GetType(column)} _{ObjectName(column.Name, Culture)};
");
                        if (column.IsPrimaryKeyMember)
                        {
                            file.WriteLine($@"        [ZKey(true)]");
                        }

                        file.WriteLine($@"        public virtual {GetType(column)} {PropertyName(column.Name)}
        {{
            get {{ return this.{pkClassName2}{x} == null ? _{ObjectName(column.Name, Culture)} : this.{pkClassName2}{x}.{pkPropertyName}; }}
            set
            {{
                _{ObjectName(column.Name, Culture)} = value;
                {pkClassName2}{x} = null;
            }}
        }}");
                    }
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

                        string pkClassName2 = pkClassName == fkClassName ? pkClassName + "X" : pkClassName;

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"
        public virtual {pkClassName} {pkClassName2}{x} {{ get; set; }} // {fkPropertyName}");
                    }

                    file.WriteLine($@"
        #endregion Associations (FK)");
                }

                if (SourceTable.PrimaryKeys.Count > 0)
                {
                    file.WriteLine($@"
        #region Collections (PK)");

                    Dictionary1(collections123);
                    foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                    {
                        string fkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                        string x = "";
                        if (collections123.ContainsKey(fkClassName))
                        {
                            x = (++collections123[fkClassName]).ToString();
                        }

                        file.WriteLine($@"
        public virtual IList<{fkClassName}> {Plural(fkClassName, Culture)}{x} {{ get; }}");
                    }

                    file.WriteLine($@"
        #endregion Collections (PK)");
                }

                file.WriteLine($@"
        #region Methods

        public {className}()
        {{");

                if (SourceTable.PrimaryKeys.Count > 0)
                {
                    Dictionary1(collections123);
                    foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                    {
                        string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                        string x = "";
                        if (collections123.ContainsKey(pkClassName))
                        {
                            x = (++collections123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"            {Plural(pkClassName, Culture)}{x} = new List<{pkClassName}>();");
                    }

                    file.WriteLine();
                }

                file.WriteLine($@"            OnConstructor();
        }}

        public override object[] GetId()
        {{
            return new object[] {{ {keyCommas} }};
        }}

        public override void SetId(object[] ids)
        {{");
                int index = 0;
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    file.WriteLine($@"            if (ids != null && ids[{index.ToString()}] != null)
            {{
                {PropertyName(column.Name)} = DataHelper.IdTo{GetDbType(column.DataType)}(ids[{index.ToString()}]);
            }}");

                    index++;
                }

                file.WriteLine($@"        }}

        #endregion Methods
    }}
}}");
            }
        }

        public void Data_DTO(string Application,
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

                file.WriteLine($@"using EasyLOB;
using EasyLOB.Data;
using EasyLOB.Library;
using System;
using System.Collections.Generic;

namespace {Namespace}
{{
    public partial class {className}DTO : ZDTOModel<{className}DTO, {className}>
    {{
        #region Properties");

                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    bool isNullable = column.AllowDBNull;

                    file.WriteLine($@"
        public virtual {GetType(column)} {PropertyName(column.Name)} {{ get; set; }}");
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

                        string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"
        public virtual string {pkClassName2}{x}LookupText {{ get; set; }} // {fkPropertyName}");
                    }

                    file.WriteLine($@"
        #endregion Associations (FK)");
                }

                file.WriteLine($@"
        #region Methods

        public {className}DTO()
        {{
            OnConstructor();
        }}

        public {className}DTO(IZDataModel dataModel)
        {{
            FromData(dataModel);
        }}

        #endregion Methods
    }}
}}");
            }
        }

        public void Data_Resource(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath)
        {
            using (StreamWriter file = CreateStreamWriter(filePath))
            {

                // Custom Tool = PublicResXFileCodeGenerator

                string tableName = TableName(SourceTable.FullName);
                string className = ClassName(SourceTable.FullName, Culture);
                string objectName = ObjectName(SourceTable.FullName, Culture);

                file.WriteLine($@"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <!--
    Microsoft ResX Schema

    Version 2.0

    The primary goals of this format is to allow a simple XML format
    that is mostly human readable. The generation and parsing of the
    various data types are done through the TypeConverter classes
    associated with the data types.

    Example:

    ... ado.net/XML headers & schema ...
    <resheader name=""resmimetype"">text/microsoft-resx</resheader>
    <resheader name=""version"">2.0</resheader>
    <resheader name=""reader"">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name=""writer"">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name=""Name1""><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name=""Color1"" type=""System.Drawing.Color, System.Drawing"">Blue</data>
    <data name=""Bitmap1"" mimetype=""application/x-microsoft.net.object.binary.base64"">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name=""Icon1"" type=""System.Drawing.Icon, System.Drawing"" mimetype=""application/x-microsoft.net.object.bytearray.base64"">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>

    There are any number of ""resheader"" rows that contain simple
    name/value pairs.

    Each data row contains a name, and value. The row also contains a
    type or mimetype. Type corresponds to a .NET class that support
    text/value conversion through the TypeConverter architecture.
    Classes that don't support this are serialized and stored with the
    mimetype set.

    The mimetype is used for serialized objects, and tells the
    ResXResourceReader how to depersist the object. This is currently not
    extensible. For a given mimetype the value must be set accordingly:

    Note - application/x-microsoft.net.object.binary.base64 is the format
    that the ResXResourceWriter will generate, however the reader can
    read any of the formats listed below.

    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:import namespace=""http://www.w3.org/XML/1998/namespace"" />
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""metadata"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" use=""required"" type=""xsd:string"" />
              <xsd:attribute name=""type"" type=""xsd:string"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""assembly"">
            <xsd:complexType>
              <xsd:attribute name=""alias"" type=""xsd:string"" />
              <xsd:attribute name=""name"" type=""xsd:string"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>2.0</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name=""EntitySingular"" xml:space= ""preserve"">
    <value>{ClassLabel(Singular(className, Culture))}</value>
  </data>
  <data name=""EntityPlural"" xml:space= ""preserve"">
    <value>{ClassLabel(Plural(className, Culture))}</value>
  </data>");

                foreach (ColumnSchema column in SourceTable.Columns) {
                    file.WriteLine($@"  <data name=""Property{PropertyName(column.Name)}"" xml:space=""preserve"">
    <value>{PropertyLabel(PropertyName(column.Name))}</value>
  </data>");
                }

                file.WriteLine($@"</root>");
            }
        }

        #endregion Data
    }
}
