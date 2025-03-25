using CodeSmith.Engine;
using SchemaExplorer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
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

namespace XCodeSmith
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

    public class CodeSmithHelper : CodeTemplate
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
                    "CR",           // Contas a Receber
                    "CST",          // Código de Situação Tributária
                    "CTE",          // Conhecimento de Transporte Eletrônico
                    "DDD",          // Discagem Direta a Distância
                    "FGTS",         // Fundo de Garantia por Tempo de Serviço
                    "ICMS",         // Imposto sobre Circulação de Mercadorias e Serviços
                    "ICMSST",       // ICMS Substituição Tributária
                    "IE",           // Inscrição Estadual
                    "IERG",         // IE + RG
                    "IERGUF",       // IE + RG + UF
                    "INSS",         // INSS
                    "IPI",          // Imposto de Produtos Industralizados
                    "IR",           // Imposto de Renda
                    "IRRF",         // Imposto de Renda Retido na Fonte
                    "ISS",          // Imposto sobre Serviços
                    "JS",           // JavaScript
                    "LOB",          // EasyLOB
                    "MVA",          // MVA
                    "MVAICMSST",    // MVA ICMS Substituição Tributária
                    "NCM",          // Nomeclatura Comum do Mercosul
                    "NF",           // Nota Fiscal
                    "NSU",          // Numero Sequencial Único
                    "Pais",         // País
                    "PLR",          // PLR
                    "PIS",          // PIS
                    "PD",           // Pedido
                    "RG",           // Registro Geral
                    "SUFRAMA",      // SUFRAMA
                    "UF",           // Unidade da Federação
                    // ...
                    "AFE",
                    "AZ"
                };
            }
        }

        // Default output path
        private string DefaultOutput = "C:/CodeSmith";

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

        #region Generate

        protected void GenerateTable(string templateFileName, TableSchema table,
            string myApplication, string myNamespace, string fileName, Cultures culture, Syncfusions syncfusion = Syncfusions.EJ1)
        {
            //System.Diagnostics.Debugger.Break();

            CodeTemplateCompiler compiler = new CodeTemplateCompiler(templateFileName);
            compiler.Compile();

            if (compiler.Errors.Count == 0)
            {
                CodeTemplate template = compiler.CreateInstance();

                template.SetProperty("Application", myApplication);
                template.SetProperty("Culture", culture);
                template.SetProperty("Namespace", myNamespace);
                template.SetProperty("SourceTable", table);
                template.SetProperty("Syncfusion", syncfusion);

                template.RenderToFile(fileName, true);
            }
            else
            {
                for (int i = 0; i < compiler.Errors.Count; i++)
                {
                    Console.Error.WriteLine(compiler.Errors[i].ToString());
                }
            }
        }

        protected void GenerateTables(string templateFileName, TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string fileName, Cultures culture, Syncfusions syncfusion = Syncfusions.EJ1)
        {
            //System.Diagnostics.Debugger.Break();

            CodeTemplateCompiler compiler = new CodeTemplateCompiler(templateFileName);
            compiler.Compile();

            if (compiler.Errors.Count == 0)
            {
                CodeTemplate template = compiler.CreateInstance();

                template.SetProperty("Application", myApplication);
                template.SetProperty("Culture", culture);
                template.SetProperty("Namespace", myNamespace);
                template.SetProperty("SourceTables", sourceTables);
                template.SetProperty("Syncfusion", syncfusion);

                template.RenderToFile(fileName, true);
            }
            else
            {
                for (int i = 0; i < compiler.Errors.Count; i++)
                {
                    Console.Error.WriteLine(compiler.Errors[i].ToString());
                }
            }
        }

        #endregion

        #region Generate Presentation

        public void GeneratePresentationCollectionModel(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Models/" + myApplication;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                string outputC = output + "/" + className;
                CreateDirectory(outputC);

                GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Model.CollectionModel.cst",
                    table, myApplication, myNamespace, outputC + "/" + className + "CollectionModel.cs", culture);
            }
        }

        public void GeneratePresentationItemModel(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Models/" + myApplication;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                string outputC = output + "/" + className;
                CreateDirectory(outputC);

                GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Model.ItemModel.cst",
                    table, myApplication, myNamespace, outputC + "/" + className + "ItemModel.cs", culture);
            }
        }

        public void GeneratePresentationViewModel(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture, Archetypes archetype)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Models/" + myApplication;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                string outputC = output + "/" + className;
                CreateDirectory(outputC);

                if (archetype == Archetypes.ApplicationDTO)
                {
                    GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Model.ViewModel.DTO.cst",
                        table, myApplication, myNamespace, outputC + "/" + className + "ViewModel.cs", culture);
                }
                else
                {
                    GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Model.ViewModel.DataModel.cst",
                        table, myApplication, myNamespace, outputC + "/" + className + "ViewModel.cs", culture);
                }
            }
        }

        public void GeneratePresentationViewModelProfile(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Models/" + myApplication + "-Profile";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                string outputC = output;

                GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Model.ViewModel.Profile.cst",
                    table, myApplication, myNamespace, outputC + "/" + className + "Profile.cs", culture);
            }
        }

        #endregion

        #region Generate Presentation MVC

        public void GeneratePresentationMvcController(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture, Archetypes archetype, Syncfusions syncfusion)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Controllers" + "/" + myApplication;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                if (archetype == Archetypes.Persistence)
                {
                    GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Controller.Persistence.cst",
                        table, myApplication, myNamespace, output + "/" + className + "Controller.cs", culture, syncfusion);
                }
                else if (archetype == Archetypes.Application)
                {
                    GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Controller.Application.DataModel.cst",
                        table, myApplication, myNamespace, output + "/" + className + "Controller.cs", culture, syncfusion);
                }
                else // if (archetype == Archetypes.ApplicationDTO)
                {
                    GenerateTable(input + "/Presentation.MVC/Presentation.MVC.Controller.Application.DTO.cst",
                        table, myApplication, myNamespace, output + "/" + className + "Controller.cs", culture, syncfusion);
                }
            }
        }

        public void GeneratePresentationMvcMenu(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/EasyLOB-Configuration/Menu";
            CreateDirectory(output);

            GenerateTables(input + "/Presentation.MVC/Presentation.MVC.Menu.cst",
                sourceTables, myApplication, myNamespace, output + "/Menu." + myApplication + ".json", culture);
        }

        public void GeneratePresentationMvcPartialViewLookup(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture, Syncfusions syncfusion)
        {
            GeneratePresentationMvcPartialView(sourceTables,
                myApplication, myNamespace, output, culture, syncfusion,
                "Presentation.MVC.PartialView._Lookup.cst", "Lookup.cshtml");
        }

        public void GeneratePresentationMvcPartialView(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture, Syncfusions syncfusion,
            string cst, string cshtml)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            string ej = "EJ1";
            if (syncfusion == XCodeSmith.Syncfusions.EJ2) {
                ej = "EJ2";
            }

            output = output + "/" + myNamespace + "/Views";
            CreateDirectory(output);

            output = output + "/" + myApplication;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                string outputC = output + "/" + className;
                CreateDirectory(outputC);

                GenerateTable(input + "/Presentation.MVC/" + ej + "/" + cst, table, myApplication, myNamespace, outputC + "/_" + className + cshtml, culture);

                //GenerateTable(input + "/Presentation.MVC/Presentation.MVC.PartialView._Collection.cst",
                //    table, myApplication, myNamespace, outputC + "/_" + className + "Collection.cshtml", culture);
                //GenerateTable(input + "/Presentation.MVC/Presentation.MVC.PartialView._Item.cst",
                //    table, myApplication, myNamespace, outputC + "/_" + className + "Item.cshtml", culture);
                //GenerateTable(input + "/Presentation.MVC/Presentation.MVC.PartialView._Lookup.cst",
                //    table, myApplication, myNamespace, outputC + "/_" + className + "Lookup.cshtml", culture);
            }
        }

        public void GeneratePresentationMvcView(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture, Syncfusions syncfusion)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Views";
            CreateDirectory(output);

            output = output + "/" + myApplication;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                string outputC = output + "/" + className;
                CreateDirectory(outputC);

                GenerateTable(input + "/Presentation.MVC/Presentation.MVC.View.CRUD.cst",
                    table, myApplication, myNamespace, outputC + "/" + "CRUD.cshtml", culture, syncfusion);
                GenerateTable(input + "/Presentation.MVC/Presentation.MVC.View.Index.cst",
                    table, myApplication, myNamespace, outputC + "/" + "Index.cshtml", culture, syncfusion);
                GenerateTable(input + "/Presentation.MVC/Presentation.MVC.View.Search.cst",
                    table, myApplication, myNamespace, outputC + "/" + "Search.cshtml", culture, syncfusion);
            }
        }

        #endregion

        #region Generate Service OData

        public void GenerateServiceODataController(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture, Archetypes archetype)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Controllers" + "/" + myApplication;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                if (archetype == Archetypes.Persistence)
                {
                    GenerateTable(input + "/Service.OData/Service.OData.Controller.Persistence.cst",
                        table, myApplication, myNamespace, output + "/" + className + "Controller.cs", culture);
                }
                else // Archetypes.Application || Archetypes.ApplicationDTO
                {
                    GenerateTable(input + "/Service.OData/Service.OData.Controller.Application.cst",
                        table, myApplication, myNamespace, output + "/" + className + "Controller.cs", culture);
                }
            }
        }

        #endregion

        #region Generate Service Web API

        public void GenerateServiceWebApiController(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture, Archetypes archetype)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            string output1 = output + "/" + myNamespace + "/ControllersAPI" + "/" + myApplication;
            CreateDirectory(output1);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);
                
                GenerateTable(input + "/Service.WebAPI/Service.WebAPI.Controller.Application.DTO.cst",
                    table, myApplication, myNamespace, output1 + "/" + className + "APIController.cs", culture);
                /*
                if (archetype == Archetypes.Persistence)
                {
                    GenerateTable(input + "/Service.WebAPI/Service.WebAPI.Controller.Persistence.cst",
                        table, myApplication, myNamespace, output1 + "/" + className + "APIController.cs", culture);
                }
                else if (archetype == Archetypes.Application)
                {
                    GenerateTable(input + "/Service.WebAPI/Service.WebAPI.Controller.Application.Data.cst",
                        table, myApplication, myNamespace, output1 + "/" + className + "APIController.cs", culture);
                }
                else // Archetypes.ApplicationDTO
                {
                    GenerateTable(input + "/Service.WebAPI/Service.WebAPI.Controller.Application.DTO.cst",
                        table, myApplication, myNamespace, output1 + "/" + className + "APIController.cs", culture);
                }
                */
            }
            /*
            string output2 = output + "/" + myNamespace + "/Views" + "/" + myApplication + "-Custom" + "/" + myApplication + "Tasks";
            CreateDirectory(output2);

            GenerateTables(input + "/Service.WebAPI/Service.WebAPI.View.API.cst",
                sourceTables, myApplication, myNamespace, output2 + "/" + myApplication + "API.cshtml", culture);
            */
            }

        #endregion

        #region Generate Application

        public void GenerateApplication(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            string output1 = output + "/" + myApplication;
            CreateDirectory(output);
            string outputI = output1 + "/Interfaces/Application";
            CreateDirectory(outputI);

            GenerateTables(input + "/Application/Application.IGenericApplication.cst",
                        sourceTables, myApplication, myNamespace, outputI + "/I" + myApplication + "GenericApplication.cs", culture);
            GenerateTables(input + "/Application/Application.IGenericApplicationDTO.cst",
                        sourceTables, myApplication, myNamespace, outputI + "/I" + myApplication + "GenericApplicationDTO.cs", culture);

            string output2 = output + "/" + myNamespace;
            CreateDirectory(output);

            GenerateTables(input + "/Application/Application.GenericApplication.cst",
                sourceTables, myApplication, myNamespace, output2 + "/" + myApplication + "GenericApplication.cs", culture);
            GenerateTables(input + "/Application/Application.GenericApplicationDTO.cst",
                sourceTables, myApplication, myNamespace, output2 + "/" + myApplication + "GenericApplicationDTO.cs", culture);
        }

        #endregion

        #region Generate Persistence

        public void GeneratePersistence(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myApplication;
            CreateDirectory(output);
            string outputI = output + "/Interfaces/Persistence";
            CreateDirectory(outputI);

            GenerateTables(input + "/Persistence/Persistence.IGenericRepository.cst",
                sourceTables, myApplication, myNamespace, outputI + "/I" + myApplication + "GenericRepository.cs", culture);
            GenerateTables(input + "/Persistence/Persistence.IGenericRepositoryDTO.cst",
                sourceTables, myApplication, myNamespace, outputI + "/I" + myApplication + "GenericRepositoryDTO.cs", culture);
            GenerateTables(input + "/Persistence/Persistence.IUnitOfWork.cst",
                sourceTables, myApplication, myNamespace, outputI + "/I" + myApplication + "UnitOfWork.cs", culture);
            GenerateTables(input + "/Persistence/Persistence.IUnitOfWorkDTO.cst",
                sourceTables, myApplication, myNamespace, outputI + "/I" + myApplication + "UnitOfWorkDTO.cs", culture);
        }

        #endregion

        #region Generate Persistence Entity Framework

        public void GeneratePersistenceEntityFrameworkConfiguration(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "EntityFramework" + "/Configurations";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceEntityFramework/PersistenceEntityFramework.Configuration.cst",
                    table, myApplication, myNamespace, output + "/" + className + "Configuration.cs", culture);
            }
        }

        public void GeneratePersistenceEntityFrameworkDbContext(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "EntityFramework";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceEntityFramework/PersistenceEntityFramework.DbContext.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "DbContext.cs", culture);
        }

        public void GeneratePersistenceEntityFrameworkGenericRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "EntityFramework" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceEntityFramework/PersistenceEntityFramework.GenericRepository.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "GenericRepositoryEF.cs", culture);
        }

        public void GeneratePersistenceEntityFrameworkUnitOfWork(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "EntityFramework" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceEntityFramework/PersistenceEntityFramework.UnitOfWork.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "UnitOfWorkEF.cs", culture);
        }

        #endregion

        #region Generate Persistence LINQ2DB

        public void GeneratePersistenceLINQ2DBDataConnection(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "LINQ2DB";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceLINQ2DB/PersistenceLINQ2DB.DataConnection.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "LINQ2DB.cs", culture);
        }

        public void GeneratePersistenceLINQ2DBGenericRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "LINQ2DB" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceLINQ2DB/PersistenceLINQ2DB.GenericRepository.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "GenericRepositoryLINQ2DB.cs", culture);
        }

        public void GeneratePersistenceLINQ2DBMap(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "LINQ2DB" + "/Maps";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceLINQ2DB/PersistenceLINQ2DB.Map.cst",
                    table, myApplication, myNamespace, output + "/" + className + "Map.cs", culture);
            }
        }

        public void GeneratePersistenceLINQ2DBRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "LINQ2DB" + "/Repositories";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceLINQ2DB/PersistenceLINQ2DB.Repository.cst",
                    table, myApplication, myNamespace, output + "/" + myApplication + className + "RepositoryLINQ2DB.cs", culture);
            }
        }

        public void GeneratePersistenceLINQ2DBUnitOfWork(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "LINQ2DB" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceLINQ2DB/PersistenceLINQ2DB.UnitOfWork.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "UnitOfWorkLINQ2DB.cs", culture);
        }

        #endregion

        #region Generate Persistence MongoDB

        public void GeneratePersistenceMongoDBMap(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "MongoDB" + "/Maps";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceMongoDB/PersistenceMongoDB.Map.cst",
                    table, myApplication, myNamespace, output + "/" + className + "Map.cs", culture);
            }
        }

        public void GeneratePersistenceMongoDBGenericRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "MongoDB" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceMongoDB/PersistenceMongoDB.GenericRepository.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "GenericRepositoryMongoDB.cs", culture);
        }

        public void GeneratePersistenceMongoDBRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "MongoDB" + "/Repositories";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceMongoDB/PersistenceMongoDB.Repository.cst",
                    table, myApplication, myNamespace, output + "/" + myApplication + className + "RepositoryMongoDB.cs", culture);
            }
        }

        public void GeneratePersistenceMongoDBUnitOfWork(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "MongoDB" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceMongoDB/PersistenceMongoDB.UnitOfWork.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "UnitOfWorkMongoDB.cs", culture);
        }

        #endregion

        #region Generate Persistence NHibernate

        public void GeneratePersistenceNHibernateMap(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "NHibernate" + "/Maps";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceNHibernate/PersistenceNHibernate.Map.cst",
                    table, myApplication, myNamespace, output + "/" + className + "Map.cs", culture);
            }
        }

        public void GeneratePersistenceNHibernateFactory(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "NHibernate";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceNHibernate/PersistenceNHibernate.Factory.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "Factory.cs", culture);
        }

        public void GeneratePersistenceNHibernateGenericRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "NHibernate" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceNHibernate/PersistenceNHibernate.GenericRepository.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "GenericRepositoryNH.cs", culture);
        }

        public void GeneratePersistenceNHibernateUnitOfWork(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "NHibernate" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceNHibernate/PersistenceNHibernate.UnitOfWork.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "UnitOfWorkNH.cs", culture);
        }

        #endregion

        #region Generate Persistence OData

        public void GeneratePersistenceODataDTO(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "OData" + "/DTOs";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceOData/PersistenceOData.DTO.cst",
                    table, myApplication, myNamespace, output + "/" + className + "DTO.cs", culture);
            }
        }

        public void GeneratePersistenceODataGenericRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "OData" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceOData/PersistenceOData.GenericRepository.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "GenericRepositoryOData.cs", culture);
        }

        public void GeneratePersistenceODataUnitOfWork(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "OData" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceOData/PersistenceOData.UnitOfWork.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "UnitOfWorkOData.cs", culture);
        }

        #endregion

        #region Generate Persistence RavenDB

        public void GeneratePersistenceRavenDBGenericRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "RavenDB" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceRavenDB/PersistenceRavenDB.GenericRepository.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "GenericRepositoryRavenDB.cs", culture);
        }

        public void GeneratePersistenceRavenDBUnitOfWork(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "RavenDB" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceRavenDB/PersistenceRavenDB.UnitOfWork.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "UnitOfWorkRavenDB.cs", culture);
        }

        #endregion

        #region Generate Persistence Redis

        public void GeneratePersistenceRedisGenericRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "Redis" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceRedis/PersistenceRedis.GenericRepository.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "GenericRepositoryRedis.cs", culture);
        }

        public void GeneratePersistenceRedisRepository(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "Redis" + "/Repositories";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/PersistenceRedis/PersistenceRedis.Repository.cst",
                    table, myApplication, myNamespace, output + "/" + myApplication + className + "RepositoryRedis.cs", culture);
            }
        }

        public void GeneratePersistenceRedisUnitOfWork(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "Redis" + "/UnitOfWork";
            CreateDirectory(output);

            GenerateTables(input + "/PersistenceRedis/PersistenceRedis.UnitOfWork.cst",
                sourceTables, myApplication, myNamespace, output + "/" + myApplication + "UnitOfWorkRedis.cs", culture);
        }

        #endregion

        #region Generate Data

        public void GenerateDataDataModel(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/DataModels";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/Data/Data.DataModel.cst",
                    table, myApplication, myNamespace, output + "/" + className + ".cs", culture);
            }
        }

        public void GenerateDataDTO(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/DTOs";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/Data/Data.DTO.cst",
                    table, myApplication, myNamespace, output + "/" + className + "DTO.cs", culture);
            }
        }

        public void GenerateDataResource(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace + "/Resources";
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/Data/Data.Resource.cst",
                    table, myApplication, myNamespace, output + "/" + className + "Resources.resx", culture);
            }
        }

        #endregion

        #region Generate Knockout

        public void GenerateKnockoutCollection(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/Knockout/Knockout.Collection.cst",
                    table, myApplication, myNamespace, output + "/Collection-" + className + ".js", culture);
            }
        }

        public void GenerateKnockoutItem(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/Knockout/Knockout.Item.cst",
                    table, myApplication, myNamespace, output + "/Item-" + className + ".js", culture);
            }
        }

        public void GenerateKnockoutViewModel(TableSchemaCollection sourceTables,
            string myApplication, string myNamespace, string output, Cultures culture)
        {
            string input = this.CodeTemplateInfo.DirectoryName;

            if (output.Trim() == "")
            {
                output = DefaultOutput;
            }

            if (IsNullOrEmpty(myApplication))
            {
                myApplication = myNamespace;
            }

            output = output + "/" + myNamespace;
            CreateDirectory(output);

            foreach (TableSchema table in sourceTables)
            {
                string className = ClassName(table.FullName, culture);

                GenerateTable(input + "/Knockout/Knockout.ViewModel.cst",
                    table, myApplication, myNamespace, output + "/" + className + ".js", culture);
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
                return column.NativeType;
            }
            /*
            switch (column.DataType)
            {
                case DbType.AnsiString: return column.NativeType; // return "varchar";
                case DbType.AnsiStringFixedLength: return column.NativeType; // return "varchar";
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
                case DbType.Object:
                    if (column.NativeType == "geography") {
                        return "geography";
                    } else {
                        return "binary";
                    }
                case DbType.SByte: return "tinyint";
                case DbType.Single: return "real";
                case DbType.String: return column.NativeType; // return "varchar";
                case DbType.StringFixedLength: return column.NativeType; // return "varchar";
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
                case DbType.Object:return "byte[]";
                //case DbType.SByte: return "sbyte"; // siegmar
                case DbType.SByte: return "byte" + (allowDBNull ? "?" : "");
                case DbType.Single: return "float" + (allowDBNull ? "?" : "");
                case DbType.String:
                    if (column.NativeType == "varbinary") {
                        return "byte[]";
                    } else {
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

        //public string PKColumnsSQL(MemberColumnSchemaCollection columns) // 2.6
        public string PKColumnsSQL(ColumnSchemaCollection columns)
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

        //public string PKColumnsSQLParameters(MemberColumnSchemaCollection columns) // 2.6
        public string PKColumnsSQLParameters(ColumnSchemaCollection columns)
        {
            string result = "";

            for (int i = 0;i < columns.Count;i++)
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

            for (int i = 0;i < table.ForeignKeyMemberColumns.Count;i++)
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

        public int FKIndexOf(TableKeySchemaCollection foreignKeys, string fkColumn) // 2.6
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
            bool result = false;

            if (column.ExtendedProperties["CS_IsIdentity"] != null) // 2.6
            {
                result = (bool)column.ExtendedProperties["CS_IsIdentity"].Value;
            }

            return result;
        }

        public bool IsImage(ColumnSchema column)
        {
            bool result = false;

            if (column.ExtendedProperties["CS_SystemType"] != null) // 2.6
            {
                result = (((string)(column.ExtendedProperties["CS_SystemType"].Value)).ToLower() == "image");
            }

            return result;
        }

        public bool IsNText(ColumnSchema column)
        {
            bool result = false;

            if (column.ExtendedProperties["CS_SystemType"] != null) // 2.6
            {
                result = (((string)(column.ExtendedProperties["CS_SystemType"].Value)).ToLower() == "ntext");
            }

            return result;
        }

        public bool IsText(ColumnSchema column)
        {
            bool result = false;

            if (column.ExtendedProperties["CS_SystemType"] != null) // 2.6
            {
                result = (((string)(column.ExtendedProperties["CS_SystemType"].Value)).ToLower() == "text");
            }

            return result;
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

        public string KnockoutWidth(ColumnSchema column)
        {
            int result;

            if (IsString(column.DataType))
            {
                if (column.Size == -1) // char(max) | varchar(max)
                {
                    result = 20;
                }
                else if (column.Size <= 5)
                {
                    result = 5;
                }
                else if (column.Size <= 10)
                {
                    result = 10;
                }
                else if (column.Size <= 15)
                {
                    result = 15;
                }
                else
                //else if (column.Size <= 20)
                {
                    result = 20;
                }
                //else
                //{
                //    result = 250;
                //}
            }
            else if (IsDate(column.DataType))
            {
                result = 10;
            }
            else if (IsDateTime(column.DataType))
            {
                result = 20;
            }
            else if (IsDecimal(column.DataType) || IsFloat(column.DataType))
            {
                result = 10;
            }
            else if (IsInteger(column.DataType))
            {
                result = 5;
            }
            else
            {
                result = 10;
            }

            result = result <= 0 ? 10 : result;

            return result.ToString();
        }

        #endregion
    }
}

