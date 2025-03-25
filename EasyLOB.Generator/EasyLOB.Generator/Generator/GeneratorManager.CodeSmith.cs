using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

/*
<%  Dictionary1(associations123);
    foreach (KeyValuePair<string, int> entry in associations123)
    { %>
    <%= entry.Key %> = <%= entry.Value.ToString() %>
<%  } %>

<%  Dictionary1(collections123);
    foreach (KeyValuePair<string, int> entry in collections123)
    { %>
    <%= entry.Key %> = <%= entry.Value.ToString() %>
<%  } %>
*/

namespace Generator
{
    public enum Archetypes
    {
        Application,
        ApplicationDTO,
        Persistence
    }

    public enum Cultures
    {
        en_US, // English
        pt_BR // Brazilian Portuguese
    }

    public enum Syncfusions
    {
        EJ1,
        EJ2
    }

    public partial class GeneratorManager
    {

        #region Properties

        // Acronyms that should not be renamed in Classes, Objects and Properties Names
        // Array.IndexOf(Acronyms, name) < 0
        private string[] Acronyms
        {
            get
            {
                return new string[]
                {
                    // en-US
                    "BI",
                    "CRM",          // Customer Relationship Management
                    "CVC",          // Credit Card Verification Code
                    "ECommerce",
                    "EMail",
                    "ERP",          // Enterprise Resource Planning
                    "KPI",          // Key Performance Indicator
                    "PBX",
                    "TID",          // Transaction ID
                    "URL",          // URL
                    "YTD",          // Year to date
                    "YTM",          // Year to month
                    "ZIP",          // Zone Improvement Plan
                    // pt-BR
                    "AFEC",         //
                    "AFEM",         //
                    "CEP",          // Código de Endereçamento Postal
                    "CFOP",         // Código Fiscal de Operação e Prestação
                    "CNPJ",         // Cadastro Nacional de Pessoas Jurídicas    
                    "CNPJCPF",      // CNPJ + CPF
                    "COFINS",       // COFINS
                    "CPF",          // Cadastro de Pessoas Físicas
                    "CST",          // Código de Situação Tributária
                    "CTE",          // Conhecimento de Transporte Eletrônico
                    "DDD",          // Discagem Direta a Distância
                    "FGTS",         // 
                    "ICMS",         // Imposto sobre Circulação de Mercadorias e Serviços
                    "ICMSST",       // ICMS Substituição Tributária
                    "IE",           // Inscrição Estadual
                    "IERG",         // IE + RG
                    "IERGUF",       // IE + RG + UF
                    "INSS",         // 
                    "IPI",          // Imposto de Produtos Industralizados
                    "IR",           // Imposto de Renda
                    "IRRF",         // Imposto de Renda Retido na Fonte
                    "ISS",          // Imposto sobre Serviços
                    "LOB",          // EasyLOB
                    "MVA",          // MVA
                    "MVAICMSST",    // MVA ICMS Substituição Tributária
                    "NCM",          // Nomeclatura Comum do Mercosul
                    "NF",           // Nota Fiscal
                    "NSU",          // Numero Sequencial Único
                    "Pais",         // País
                    "PIS",          // PIS
                    "PLR",          // 
                    "PD",           // Pedido
                    "RG",           // Registro Geral
                    "SUFRAMA",      // SUFRAMA
                    "UF",           // Unidade da Federação
                    "ZFM",          // Zona Franca de Manaus
                    "ZFMALC",       // ZFM/ALC
                    "ALC",          // Área de Lívre Comércio
                    // ...
                    "AFE",
                    "AZ"
                };
            }
        }

        // Expressions that should removed in Classes and Properties Names
        // Array.IndexOf(Expressions, name) < 0
        private string[] Expressions
        {
            get
            {
                return new string[]
                {
                    "aspnet_",
                    "AspNet",
                    "EasyLOB",
                    "TB_"
                };
            }
        }

        #endregion

        #region Methods

        public void CreateDirectory(string directory)
        {
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void Dictionary123(Dictionary<string, int> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, 1);
            }
            else
            {
                int value = dictionary[key];
                dictionary[key] = value + 1;
            }
        }

        public static void Dictionary1(Dictionary<string, int> dictionary)
        {
            foreach (KeyValuePair<string, int> entry in dictionary.ToList())
            {
                if (entry.Value < 2)
                {
                    dictionary.Remove(entry.Key);
                }
                else
                {
                    dictionary[entry.Key] = 0;
                }
            }
        }

        public static bool IsNullOrEmpty(string s) // 2.6
        {
            return (s == null || s.Trim() == "");
        }

        public string Plural(string s, Cultures culture)
        {
            // Case Sensitive
            //if (Array.IndexOf(Acronyms, s) >= 0) // Is an Acronym
            // Case Insensitive
            if (Array.FindIndex(Acronyms, x => x.Equals(s, StringComparison.InvariantCultureIgnoreCase)) >= 0) // Is an Acronym
            {
                return s;
            }
            else if (culture == Cultures.pt_BR)
            {
                return Plural_pt_BR(s);
            }
            else
            {
                return Plural_en_US(s);
            }
        }

        public string Plural_en_US(string s)
        {
            string result = "";

            s = s.Trim();

            if (s.EndsWith("ss"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "ss$", "sses"); // ss => sses
            }
            else if (s.EndsWith("s"))
            {
                result = s;
            }
            else if (s.EndsWith("y"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "y$", "ies"); // y => ies
            }
            else
            {
                result = s + "s"; // ? => s
            }

            return result;
        }

        public string Plural_pt_BR(string s)
        {
            string result = "";

            s = s.Trim();

            if (s.EndsWith("ao"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "ao$", "oes"); // ao => oes
            }
            else if (s.EndsWith("il"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "l$", "ls"); // il => ils
            }
            else if (s.EndsWith("l"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "l$", "is"); // l => is
            }
            else if (s.EndsWith("m"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "m$", "ns"); // m => ns
            }
            else if (s.EndsWith("r"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "r$", "res"); // r => res
            }
            else if (s.EndsWith("s"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "s$", "ses"); // s => ses
            }
            else
            {
                result = s + "s";
            }

            return result;
        }

        public string Singular(string s, Cultures culture)
        {
            // Case Sensitive
            //if (Array.IndexOf(Acronyms, s) >= 0) // Is an Acronym
            // Case Insensitive
            if (Array.FindIndex(Acronyms, x => x.Equals(s, StringComparison.InvariantCultureIgnoreCase)) >= 0) // Is an Acronym
            {
                return s;
            }
            else if (culture == Cultures.pt_BR)
            {
                return Singular_pt_BR(s);
            }
            else
            {
                return Singular_en_US(s);
            }
        }

        public string Singular_en_US(string s)
        {
            string result = "";

            s = s.Trim();

            if (s.EndsWith("ss"))
            {
                result = s;
            }
            else if (s.EndsWith("ies"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "ies$", "y"); // y => ies
            }
            else if (s.EndsWith("sses"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "sses$", "ss"); // ss => sses
            }
            else if (s.EndsWith("s"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "s$", ""); // ? => s
            }
            else
            {
                result = s;
            }

            return result;
        }

        public string Singular_pt_BR(string s)
        {
            string result = "";

            s = s.Trim();

            if (s.EndsWith("oes"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "oes$", "ao"); // ao => oes
            }
            else if (s.EndsWith("is") && s.Length > 4)
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "is$", "l"); // l => is
            }
            else if (s.EndsWith("ns"))
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "ns$", "m"); // m => ns
            }
            else if (s.EndsWith("res") && s.Length > 5)
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "res$", "r"); // r => res
            }
            else if (s.EndsWith("Mes"))
            {
                result = s;
            }
            else if (s.EndsWith("s") && s.Length > 4)
            {
                result = System.Text.RegularExpressions.Regex.Replace(s, "s$", ""); // ? => s
            }
            else
            {
                result = s;
            }

            return result;
        }

        public string StringSplitPascalCase(string s)
        {
            Regex regex = new Regex("(?<=[a-z])(?<x>[A-Z|0-9|#])|(?<=.)(?<x>[A-Z|0-9|#])(?=[a-z])");

            return regex.Replace(s, " ${x}").Replace("_", " "); // "_" => " " !?!
        }

        public string StringToLowerFirstLetter(string s)
        {
            if (s.Length > 1)
            {
                return Char.ToLower(s[0]) + s.Substring(1); // 2.6
                //return Char.ToLowerInvariant(s[0]) + s.Substring(1); // 2.6
            }
            else
            {
                return s.ToLower();
            }
        }

        public string StringToUpperFirstLetter(string s)
        {
            if (s.Length > 1)
            {
                return Char.ToUpper(s[0]) + s.Substring(1);
                //return Char.ToUpperInvariant(s[0]) + s.Substring(1); // 2.6
            }
            else
            {
                return s.ToUpper();
            }
        }

        #endregion

        #region Tables & Columns

        // SQL

        public string TableName(string name)
        {
            return name.Replace("dbo.", "");
        }

        public string TableAlias(string name)
        {
            return TableName(name).Replace(".", "_");
        }

        public string ColumnName(string name)
        {
            return name;
        }

        // Class

        public string ClassLabel(string name)
        {
            return ClassWords(UnderscoreToPascalCase(name));
        }

        public string ClassName(string name, Cultures culture)
        {
            string result = ClassWords(UnderscoreToPascalCase(name));

            return Singular(result.Replace(" ", ""), culture); // Singular
        }

        public string ObjectName(string name, Cultures culture)
        {
            string result = ClassWords(UnderscoreToPascalCase(name), true);

            return Singular(result.Replace(" ", ""), culture); // Singular
        }

        public string ClassWords(string name, bool isLower = false)
        {
            string result;
            string[] words;

            foreach (string expression in Expressions)
            {
                name = name.Replace(expression, "");
            }

            // Schema.Table => Table
            words = name.Split('.');
            if (words.Length > 1)
            {
                name = words[1];
            }

            words = StringSplitPascalCase(name).Split(' ');

            result = "";
            int index = 0;
            foreach (string word in words)
            {
                if (index == 0 && isLower)
                {
                    result += (!IsNullOrEmpty(result) ? " " : "") + word.ToLower();
                }
                // Case Sensitive
                //if (Array.IndexOf(Acronyms, word) >= 0) // Is an Acronym
                // Case Insensitive
                else if (Array.FindIndex(Acronyms, x => x.Equals(word, StringComparison.InvariantCultureIgnoreCase)) >= 0) // Is an Acronym
                {
                    result += (!IsNullOrEmpty(result) ? " " : "") + word;
                }
                else
                {
                    result += (!IsNullOrEmpty(result) ? " " : "") + StringToUpperFirstLetter(word.ToLower());
                }
                index++;
            }

            return result;
        }

        // Property

        public string PropertyLabel(string name)
        {
            return PropertyWords(UnderscoreToPascalCase(name));
        }

        public string PropertyName(string name)
        {
            string result = PropertyWords(UnderscoreToPascalCase(name));

            return result.Replace(" ", "");
        }

        public string LocalName(string name)
        {
            string result = PropertyWords(UnderscoreToPascalCase(name));

            return result.Replace(" ", "");
        }

        public string PropertyWords(string name)
        {
            string result;
            string[] words;

            foreach (string expression in Expressions)
            {
                name = name.Replace(expression, "");
            }

            words = StringSplitPascalCase(name).Split(' ');

            result = "";
            int index = 0;
            foreach (string word in words)
            {
                // Case Sensitive
                //if (Array.IndexOf(Acronyms, word) >= 0) // Is an Acronym
                // Case Insensitive
                if (Array.FindIndex(Acronyms, x => x.Equals(word, StringComparison.InvariantCultureIgnoreCase)) >= 0) // Is an Acronym
                {
                    result += (!IsNullOrEmpty(result) ? " " : "") + word;
                }
                else
                {
                    result += (!IsNullOrEmpty(result) ? " " : "") + StringToUpperFirstLetter(word.ToLower());
                }

                index++;
            }

            return result;
        }

        public string UnderscoreToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input) || !input.Contains("_"))
                return input;

            string[] words = input.Split('_');
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = textInfo.ToTitleCase(words[i]);
            }

            return string.Join("", words);
        }

        #endregion

        #region Type

        public bool IsBinary(DbType dbType)
        {
            return (dbType == DbType.Binary
                || dbType == DbType.Object);
        }

        public bool IsBoolean(DbType dbType)
        {
            return (dbType == DbType.Boolean);
        }

        public bool IsDate(DbType dbType)
        {
            return (dbType == DbType.Date);
        }

        public bool IsDateTime(DbType dbType)
        {
            return (dbType == DbType.DateTime);
            //    || dbType == DbType.DateTime2 // 2.6
            //    || dbType == DbType.DateTimeOffset) // 2.6
        }

        public bool IsTime(DbType dbType)
        {
            return (dbType == DbType.Time);
        }

        public bool IsDecimal(DbType dbType)
        {
            return (dbType == DbType.Currency
                || dbType == DbType.Decimal);
        }

        public bool IsFloat(DbType dbType)
        {
            return (dbType == DbType.Double
                || dbType == DbType.Single);
        }
        /*
        public bool IsDouble(DbType dbType)
        {
            return (dbType == DbType.Double);
        }

        public bool IsSingle(DbType dbType)
        {
            return (dbType == DbType.Single);
        }
         */
        public bool IsGuid(DbType dbType)
        {
            return (dbType == DbType.Guid);
        }

        public bool IsInteger(DbType dbType)
        {
            return (dbType == DbType.Byte
                || dbType == DbType.SByte
                || dbType == DbType.Int16
                || dbType == DbType.Int32
                || dbType == DbType.Int64
                || dbType == DbType.UInt16
                || dbType == DbType.UInt32
                || dbType == DbType.UInt64);
        }
        /*
        public bool IsInteger8(DbType dbType) // sbyte
        {
            return (dbType == DbType.Byte
                || dbType == DbType.SByte);
        }

        public bool IsInteger16(DbType dbType) // short
        {
            return (dbType == DbType.Int16
                || dbType == DbType.UInt16);
        }

        public bool IsInteger32(DbType dbType) // int
        {
            return (dbType == DbType.Int32
                || dbType == DbType.UInt32);
        }

        public bool IsInteger64(DbType dbType) // long
        {
            return (dbType == DbType.Int64
                || dbType == DbType.UInt64);
        }
        
        public bool IsObject(DbType dbType)
        {
            return (dbType == DbType.Object);
        }
         */
        public bool IsString(DbType dbType)
        {
            return (dbType == DbType.AnsiString
                || dbType == DbType.AnsiStringFixedLength
                || dbType == DbType.String
                || dbType == DbType.StringFixedLength);
            //|| dbType == DbType.Xml); // 2.6
        }

        public string GetDefault(DbType dbType)
        {
            string result = "";

            if (IsDate(dbType))
                result = "Default_Date";
            else if (IsDateTime(dbType))
                result = "Default_DateTime";
            else if (IsDecimal(dbType))
                result = "Default_Float";
            else if (IsFloat(dbType))
                result = "Default_Float";
            else if (IsInteger(dbType))
                result = "Default_Integer";
            else if (IsString(dbType))
                result = "Default_String";
            else
                result = "Default_String";

            return result;
        }

        public string GetFormat(DbType dbType)
        {
            string result = "";

            if (IsDate(dbType))
                result = "Format_Date";
            else if (IsDateTime(dbType))
                result = "Format_DateTime";
            else if (IsDecimal(dbType))
                result = "Format_Float";
            else if (IsFloat(dbType))
                result = "Format_Float";
            else if (IsInteger(dbType))
                result = "Format_Integer";
            else if (IsString(dbType))
                result = "Format_String";
            else
                result = "Format_String";

            return result;
        }

        public string GetDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return "String";
                case DbType.AnsiStringFixedLength: return "String";
                case DbType.Binary: return "Binary";
                case DbType.Boolean: return "Boolean";
                case DbType.Byte: return "Byte";
                //case DbType.Currency: return "Currency";
                case DbType.Currency: return "Decimal";
                case DbType.Date: return "DateTime";
                case DbType.DateTime: return "DateTime";
                //case DbType.DateTime2: return ""; // 2.6
                //case DbType.DateTimeOffset: return ""; // 2.6
                case DbType.Decimal: return "Decimal";
                case DbType.Double: return "Double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "Int16";
                case DbType.Int32: return "Int32";
                case DbType.Int64: return "Int64";
                //case DbType.Object: return "Object";
                case DbType.Object: return "Binary";
                //case DbType.SByte: return "SByte"; // siegmar
                case DbType.SByte: return "Byte";
                case DbType.Single: return "Single";
                case DbType.String: return "String";
                case DbType.StringFixedLength: return "String";
                case DbType.Time: return "TimeSpan";
                //case DbType.UInt16: return "UInt16"; // siegmar
                case DbType.UInt16: return "Int16";
                //case DbType.UInt32: return "UInt32"; // siegmar
                case DbType.UInt32: return "Int32";
                //case DbType.UInt64: return "UInt64"; // siegmar
                case DbType.UInt64: return "Int64";
                case DbType.VarNumeric: return "VarNumeric";
                //case DbType.Xml: return "Xml";
                //case DbType.Xml: return "String"; // 2.6
                //default: return "_" + column.NativeType + "_";
                default: return "String";
            }
        }

        public string GetLINQ2DBType(ColumnSchema column)
        {
            switch (column.DataType)
            {
                case DbType.AnsiString: return "VarChar";
                case DbType.AnsiStringFixedLength: return "VarChar";
                case DbType.Binary: return "Binary";
                case DbType.Boolean: return "Boolean";
                case DbType.Byte: return "Byte";
                //case DbType.Currency: return "Currency";
                case DbType.Currency: return "Decimal";
                case DbType.Date: return "DateTime";
                case DbType.DateTime: return "DateTime";
                //case DbType.DateTime2: return ""; // 2.6
                //case DbType.DateTimeOffset: return ""; // 2.6
                case DbType.Decimal: return "Decimal";
                case DbType.Double: return "Double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "Int16";
                case DbType.Int32: return "Int32";
                case DbType.Int64: return "Int64";
                //case DbType.Object: return "Object";
                case DbType.Object: return "Binary";
                //case DbType.SByte: return "SByte"; // siegmar
                case DbType.SByte: return "Byte";
                case DbType.Single: return "Single";
                case DbType.String: return "VarChar";
                case DbType.StringFixedLength: return "VarChar";
                case DbType.Time: return "Time";
                //case DbType.UInt16: return "UInt16"; // siegmar
                case DbType.UInt16: return "Int16";
                //case DbType.UInt32: return "UInt32"; // siegmar
                case DbType.UInt32: return "Int32";
                //case DbType.UInt64: return "UInt64"; // siegmar
                case DbType.UInt64: return "Int64";
                case DbType.VarNumeric: return "VarNumeric";
                //case DbType.Xml: return "Xml";
                //case DbType.Xml: return "String"; // 2.6
                //default: return "_" + column.NativeType + "_";
                default: return "String";
            }
        }

        public string GetSqlType(ColumnSchema column)
        {
            if (IsImage(column))
            {
                return "image";
            }
            else if (IsNText(column))
            {
                return "ntext";
            }
            else if(IsText(column))
            {
                return "text";
            }
            else
            {
                return column.DbDataType;
            }
            /*
            switch (column.DataType)
            {
                case DbType.AnsiString: return column.DbDataType; // return "varchar";
                case DbType.AnsiStringFixedLength: return column.DbDataType; // return "varchar";
                case DbType.Binary: return "varbinary";
                case DbType.Boolean: return "bit";
                case DbType.Byte: return "tinyint";
                case DbType.Currency: return "money";
                case DbType.Date: return "date";
                case DbType.DateTime: return "datetime";
                case DbType.Decimal: return "decimal";
                case DbType.Double: return "float";
                case DbType.Guid: return "uniqueidentifier";
                case DbType.Int16: return "smallint";
                case DbType.Int32: return "int";
                case DbType.Int64: return "bigint";
                case DbType.Object: return "binary";
                case DbType.SByte: return "tinyint";
                case DbType.Single: return "real";
                case DbType.String: return column.DbDataType; // return "varchar";
                case DbType.StringFixedLength: return column.DbDataType; // return "varchar";
                case DbType.Time: return "time"; // TimeSpan => time
                case DbType.UInt16: return "smallint";
                case DbType.UInt32: return "int";
                case DbType.UInt64: return "bigint";
                case DbType.VarNumeric: return "decimal";
                default: return "varchar";
            }
            */
        }

        public string GetType(ColumnSchema column, bool? isNullable = null)
        {
            bool allowDBNull = isNullable ?? column.AllowDBNull;

            switch (column.DataType)
            {
                case DbType.AnsiString: return "string";
                case DbType.AnsiStringFixedLength: return "string";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool" + (allowDBNull ? "?" : "");
                case DbType.Byte: return "byte" + (allowDBNull ? "?" : "");
                case DbType.Currency: return "decimal" + (allowDBNull ? "?" : "");
                case DbType.Date: return "DateTime" + (allowDBNull ? "?" : "");
                case DbType.DateTime: return "DateTime" + (allowDBNull ? "?" : "");
                //case DbType.DateTime2: return ""; // 2.6
                //case DbType.DateTimeOffset: return ""; // 2.6
                case DbType.Decimal: return "decimal" + (allowDBNull ? "?" : "");
                case DbType.Double: return "double" + (allowDBNull ? "?" : "");
                case DbType.Guid: return "Guid" + (allowDBNull ? "?" : "");
                case DbType.Int16: return "short" + (allowDBNull ? "?" : "");
                case DbType.Int32: return "int" + (allowDBNull ? "?" : "");
                case DbType.Int64: return "long" + (allowDBNull ? "?" : "");
                //case DbType.Object: return "object";
                case DbType.Object: return "byte[]";
                //case DbType.SByte: return "sbyte"; // siegmar
                case DbType.SByte: return "byte" + (allowDBNull ? "?" : "");
                case DbType.Single: return "float" + (allowDBNull ? "?" : "");
                case DbType.String:
                    if (column.DbDataType == "geography" || column.DbDataType == "varbinary")
                    {
                        return "byte[]";
                    }
                    else
                    {
                        return "string";
                    }
                case DbType.StringFixedLength: return "string";
                case DbType.Time: return "TimeSpan" + (allowDBNull ? "?" : "");
                //case DbType.UInt16: return "ushort" + (allowDBNull ? "?" : "");
                case DbType.UInt16: return "short" + (allowDBNull ? "?" : "");
                //case DbType.UInt32: return "uint" + (allowDBNull ? "?" : "");
                case DbType.UInt32: return "int" + (allowDBNull ? "?" : "");
                //case DbType.UInt64: return "ulong" + (allowDBNull ? "?" : "");
                case DbType.UInt64: return "long" + (allowDBNull ? "?" : "");
                case DbType.VarNumeric: return "decimal" + (allowDBNull ? "?" : "");
                //case DbType.Xml: return "Xml" + (allowDBNull ? "?" : "");
                //case DbType.Xml: return "string"; // 2.6
                //default: return "_" + column.NativeType + "_";
                //default: return "_" + dbType.ToString() + "_";
                default: return "string";
            }
        }
        /*
        public string GetType(DbType dbType, bool isNullable)
        {
            if (isNullable)
            {
                return GetTypeNullable(dbType);
            }
            else
            {
                return GetType(dbType);
            }
        }

        public string GetType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return "string";
                case DbType.AnsiStringFixedLength: return "string";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool";
                case DbType.Byte: return "byte";
                case DbType.Currency: return "decimal";
                case DbType.Date: return "DateTime";
                case DbType.DateTime: return "DateTime";
                //case DbType.DateTime2: return ""; // 2.6
                //case DbType.DateTimeOffset: return ""; // 2.6
                case DbType.Decimal: return "decimal";
                case DbType.Double: return "double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "short";
                case DbType.Int32: return "int";
                case DbType.Int64: return "long";
                //case DbType.Object: return "object";
                case DbType.Object: return "byte[]";
                //case DbType.SByte: return "sbyte"; // siegmar
                case DbType.SByte: return "byte";
                case DbType.Single: return "float";
                case DbType.String: return "string";
                case DbType.StringFixedLength: return "string";
                case DbType.Time: return "TimeSpan";
                //case DbType.UInt16: return "ushort"; // siegmar
                case DbType.UInt16: return "short";
                //case DbType.UInt32: return "uint"; // siegmar
                case DbType.UInt32: return "int";
                //case DbType.UInt64: return "ulong"; // siegmar
                case DbType.UInt64: return "long";
                case DbType.VarNumeric: return "decimal";
                //case DbType.Xml: return "xml";
                //case DbType.Xml: return "string"; // 2.6
                //default: return "_" + column.NativeType + "_";
                default: return "string";
            }
        }

        public string GetTypeNullable(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return "string";
                case DbType.AnsiStringFixedLength: return "string";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool?";
                case DbType.Byte: return "byte?";
                case DbType.Currency: return "decimal?";
                case DbType.Date: return "DateTime?";
                case DbType.DateTime: return "DateTime?";
                //case DbType.DateTime2: return ""; // 2.6
                //case DbType.DateTimeOffset: return ""; // 2.6
                case DbType.Decimal: return "decimal?";
                case DbType.Double: return "double?";
                case DbType.Guid: return "Guid?";
                case DbType.Int16: return "short?";
                case DbType.Int32: return "int?";
                case DbType.Int64: return "long?";
                //case DbType.Object: return "object";
                case DbType.Object: return "byte[]";
                //case DbType.SByte: return "sbyte"; // siegmar
                case DbType.SByte: return "byte?";
                case DbType.Single: return "float?";
                case DbType.String: return "string";
                case DbType.StringFixedLength: return "string";
                case DbType.Time: return "TimeSpan?";
                //case DbType.UInt16: return "ushort?";
                case DbType.UInt16: return "short?";
                //case DbType.UInt32: return "uint?";
                case DbType.UInt32: return "int?";
                //case DbType.UInt64: return "ulong?";
                case DbType.UInt64: return "long?";
                case DbType.VarNumeric: return "decimal?";
                //case DbType.Xml: return "Xml?";
                //case DbType.Xml: return "string"; // 2.6
                //default: return "_" + column.NativeType + "_";
                //default: return "_" + dbType.ToString() + "_";
                default: return "string";
            }
        }
        */
        #endregion

        #region Schema

        // PK

        public string PKColumnsSQL(List<ColumnSchema> columns)
        {
            string result = "";

            //for (int i = 0;i < columns.Count;i++)
            foreach (ColumnSchema column in columns)
            {
                if (!result.Equals(""))
                {
                    result += ",";
                }
                //result += columns[i].Name;    
                result += column.Name;
            }

            return result;
        }

        public string GetDataToString(ColumnSchema column, string prefix, bool isProperty)
        {
            string name = isProperty ? PropertyName(column.Name) : LocalName(column.Name);
            string result = "";

            //if (IsSysColumn(column))
            //{
            //    result = "LibraryHelper.DataToString(" + prefix + name + ", ResourceHelper.Format_SysColumn)";
            //}
            if (IsDate(column.DataType))
            {
                result = "LibraryHelper.DataToString(" + prefix + name + ", ResourceHelper.Format_Date)";
            }
            else if (IsDateTime(column.DataType))
            {
                result = "LibraryHelper.DataToString(" + prefix + name + ", ResourceHelper.Format_DateTime)";
            }
            else if (IsBinary(column.DataType) || IsBoolean(column.DataType) || IsGuid(column.DataType) || IsString(column.DataType))
            {
                result = "LibraryHelper.DataToString(" + prefix + name + ")";
            }
            else
            {
                result = "LibraryHelper.DataToString(" + prefix + name + ", ResourceHelper." + (column.DataType) + ")";
            }

            return result;
        }

        public string PKColumnsSQLParameters(List<ColumnSchema> columns)
        {
            string result = "";

            for (int i = 0; i < columns.Count; i++)
            {
                if (!result.Equals(""))
                {
                    result += ",";
                }
                result += "#" + columns[i].Name;
            }

            return result;
        }

        // FK

        public string FKColumnsSQL(TableKeySchema table, string suffix)
        {
            string result = "";

            for (int i = 0; i < table.ForeignKeyMemberColumns.Count; i++)
            {
                if (!result.Equals(""))
                {
                    result += " AND ";
                }
                result += TableName(table.PrimaryKeyTable.Name) + suffix + "." + table.PrimaryKeyMemberColumns[i].Name +
                    " = " + TableName(table.ForeignKeyTable.Name) + "." + table.ForeignKeyMemberColumns[i].Name;
            }

            return result;
        }

        public int FKIndexOf(List<TableKeySchema> foreignKeys, string fkColumn) // 2.6
        {
            int i = 0, index = -1;

            foreach (TableKeySchema fkTable in foreignKeys)
            {
                if (fkTable.ForeignKeyMemberColumns[0].Name == fkColumn)
                {
                    index = i;
                }
                i++;
            }

            return index;
        }

        public string FKTableName(TableSchema table, ColumnSchema column)
        {
            string fkTableName = "";

            foreach (TableKeySchema tableX in table.ForeignKeys)
            {
                foreach (ColumnSchema columnX in tableX.ForeignKeyMemberColumns)
                {
                    if (columnX.Name == column.Name)
                    {
                        fkTableName = tableX.PrimaryKeyTable.FullName;
                        break;
                    }
                }
            }

            return fkTableName;
        }

        // 

        public string ColumnFK(ColumnSchema column)
        {
            if (column.IsForeignKeyMember)
            {
                return "FK";
            }
            else
            {
                return "--";
            }
        }

        public string ColumnPK(ColumnSchema column)
        {
            if (column.IsPrimaryKeyMember)
            {
                return "PK";
            }
            else
            {
                return "--";
            }
        }

        public string ColumnNULL(ColumnSchema column)
        {
            if (column.AllowDBNull)
            {
                return "NULL";
            }
            else
            {
                return "----";
            }
        }

        #endregion

        #region Schema Extended Properties

        public bool IsIdentity(ColumnSchema column)
        {
            return column.IsAutoNumber;
        }

        public bool IsImage(ColumnSchema column)
        {
            return column.DbDataType.ToLower() == "image";
        }

        public bool IsNText(ColumnSchema column)
        {
            return column.DbDataType.ToLower() == "ntext";
        }

        public bool IsText(ColumnSchema column)
        {
            return column.DbDataType.ToLower() == "text";
        }

        #endregion

        #region Width
        /*
        public int TextBoxWidth(int width)
        {
            if (width <= 1)
                return 10;
            else if (width <= 2)
                return 20;
            else if (width <= 3)
                return 30;
            else if (width <= 4)
                return 40;
            else if (width <= 5)
                return 50;
            else if (width <= 10)
                return 80;
            else if (width <= 20)
                return 160;
            else if (width <= 30)
                return 240;
            else if (width <= 40)
                return 320;
            else
                return 400;
        }
         */
        public string BootstrapWidth(ColumnSchema column)
        {
            string result;

            if (IsBoolean(column.DataType))
            {
                result = "col-md-1";
            }
            else if (IsString(column.DataType))
            {
                if (column.Size == -1) // char(max) | varchar(max)
                {
                    result = "col-md-4";
                }
                else if (column.Size <= 10) // 9
                {
                    result = "col-md-1";
                }
                else if (column.Size <= 20) // 22
                {
                    result = "col-md-2";
                }
                else if (column.Size <= 30) // 35
                {
                    result = "col-md-3";
                }
                else if (column.Size <= 50) // 48
                {
                    result = "col-md-4";
                }
                else
                {
                    result = "col-md-4";
                }
            }
            else if (IsDate(column.DataType))
            {
                result = "col-md-2";
            }
            else if (IsDateTime(column.DataType))
            {
                result = "col-md-2";
            }
            else if (IsDecimal(column.DataType) || IsFloat(column.DataType))
            {
                result = "col-md-1";
            }
            else if (IsInteger(column.DataType))
            {
                result = "col-md-1";
            }
            else
            {
                result = "col-md-2";
            }

            return result;
        }

        public string GridWidth(ColumnSchema column)
        {
            int result;

            if (IsString(column.DataType))
            {
                if (column.Size == -1) // char(max) | varchar(max)
                {
                    result = 200;
                }
                else if (column.Size <= 5)
                {
                    result = 50;
                }
                else if (column.Size <= 10)
                {
                    result = 100;
                }
                else if (column.Size <= 15)
                {
                    result = 150;
                }
                else
                //else if (column.Size <= 20)
                {
                    result = 200;
                }
                //else
                //{
                //    result = 250;
                //}
            }
            else if (IsDate(column.DataType))
            {
                result = 100;
            }
            else if (IsDateTime(column.DataType))
            {
                result = 200;
            }
            else if (IsDecimal(column.DataType) || IsFloat(column.DataType))
            {
                result = 100;
            }
            else if (IsInteger(column.DataType))
            {
                result = 50;
            }
            else
            {
                result = 100;
            }

            result = result <= 0 ? 100 : result;

            return result.ToString() + "px";
        }

        #endregion
    }
}

