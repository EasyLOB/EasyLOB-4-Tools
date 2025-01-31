using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using Generator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

// Recursively Copy folder contents to another in C#
// https://www.codeproject.com/Tips/278248/Recursively-Copy-folder-contents-to-another-in-Csh

// How to quickly check if folder is empty (.NET)?
// http://stackoverflow.com/questions/755574/how-to-quickly-check-if-folder-is-empty-net

namespace EasyLOB.Generator
{
    public static class Helper
    {
        #region Methods Config

        public static void ConfigWrite(
            string txtSolutionTemplateDirectory,
            string txtSolutionDirectory,
            string txtSolutionApplication,
            string txtFilesDirectory,
            string txtFilesConnectionString,
            string txtFilesApplication,
            string txtFilesNamespaceData,
            string txtFilesNamespacePresentation,
            string txtFilesNamespacePersistence
        )
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["SolutionTemplateDirectory"].Value = txtSolutionTemplateDirectory;
                config.AppSettings.Settings["SolutionDirectory"].Value = txtSolutionDirectory;
                config.AppSettings.Settings["SolutionApplication"].Value = txtSolutionApplication;

                config.AppSettings.Settings["FilesDirectory"].Value = txtFilesDirectory;
                config.AppSettings.Settings["FilesConnectionString"].Value = txtFilesConnectionString;
                config.AppSettings.Settings["FilesApplication"].Value = txtFilesApplication;
                config.AppSettings.Settings["FilesNamespaceData"].Value = txtFilesNamespaceData;
                config.AppSettings.Settings["FilesNamespacePresentation"].Value = txtFilesNamespacePresentation;
                config.AppSettings.Settings["FilesNamespacePersistence"].Value = txtFilesNamespacePersistence;

                config.Save(ConfigurationSaveMode.Modified);

                //ConfigurationManager.RefreshSection("AppSettings");
            }
            catch { }
        }

        #endregion Methods Config

        #region Methods Generate

        public static void GenerateSolution(string solutionTemplateDirectory,
            string solutionDirectory,
            string applicationOLD,
            string applicationNEW,
            Func<string, int> log)
        {
            string applicationDirectory = Path.Combine(solutionDirectory, applicationNEW);

            // 1
            if (!Directory.Exists(applicationDirectory))
            {
                log("Creating directory \"" + applicationDirectory + "\"...");
                Directory.CreateDirectory(applicationDirectory);
                //log("  Directory created");
            }

            // 2
            //log("Deleting directory \"" + applicationDirectory + "\" and sub-directories content...");
            //if (Helper.DeleteDirectoryContents(applicationDirectory, log)) // solutionName
            {
                //log("  Directory content deleted");

                // 3
                //log("Copying directory \"" + applicationDirectory + "\" content...");
                if (Helper.CopyDirectoryContents(solutionTemplateDirectory,
                    applicationDirectory,
                    log))
                {
                    //log("  Directory content copied");

                    // 4
                    log("Renaming directory \"" + applicationDirectory + "\" content...");
                    if (Helper.RenameDirectoryContents(applicationDirectory,
                        applicationOLD,
                        applicationNEW,
                        log))
                    {
                        //log("  Directory content renamed");

                        // 5
                        log("Replacing directory \"" + applicationDirectory + "\" content...");
                        Helper.ReplaceDirectoryContents(applicationDirectory,
                            applicationOLD,
                            applicationNEW,
                            log);
                        //log("  Directory content replaced");
                    }
                }
            }
        }

        public static void GenerateFiles(
            string filesDirectory,
            string connectionString,
            IEnumerable<string> selectedTables,
            string application,
            string namespaceData,
            string namespacePersistence,
            string namespaceService,
            string namespacePresentation,
            Archetypes archetype,
            Cultures culture,
            Syncfusions syncfusion,
            //
            bool isDataDataModel,
            bool isDataDTO,
            bool isDataResource,
            //
            bool isPersistenceConfiguration,
            bool isPersistenceDbContext,
            //
            bool isServiceController,
            //
            bool isPresentationController,
            bool isPresentationModelCollection,
            bool isPresentationModelItem,
            bool isPresentationModelView,
            bool isPresentationModelViewProfile,
            //
            bool isPresentationViewIndex,
            bool isPresentationViewSearch,
            bool isPresentationViewCRUD,
            bool isPresentationViewCollection,
            bool isPresentationViewItem,
            //
            bool isPresentationLookupView,
            bool isPresentationLookupController,
            //
            bool isPresentationMenu)
        {
            if (string.IsNullOrEmpty(namespaceData))
            {
                isDataDataModel = false;
                isDataDTO = false;
                isDataResource = false;
            }

            if (string.IsNullOrEmpty(namespacePersistence))
            {
                isPersistenceConfiguration = false;
                isPersistenceDbContext = false;
            }

            if (string.IsNullOrEmpty(namespacePresentation))
            {
                isPresentationController = false;
                isPresentationModelCollection = false;
                isPresentationModelItem = false;
                isPresentationModelView = false;
                isPresentationModelViewProfile = false;

                isPresentationViewIndex = false;
                isPresentationViewSearch = false;
                isPresentationViewCRUD = false;
                isPresentationViewCollection = false;
                isPresentationViewItem = false;

                isPresentationLookupView = false;
                isPresentationLookupController = false;

                isPresentationMenu = false;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                // .NET Framework
                IGeneratorManager generatorManager = new GeneratorManagerFramework();
                isPresentationLookupController = false;

                // .NET Core
                //IGeneratorManager generatorManager = new GeneratorManagerCore();

                DatabaseReader dbReader = new DatabaseReader(connection);
                DatabaseSchema dbSchema = dbReader.ReadAll();
                List<TableSchema> tablesAll = generatorManager.DatabaseReader2CodeSmith(dbSchema);
                tablesAll.RemoveAll(x => !x.HasPrimaryKey); // EasyLOB

                List<TableSchema> tables = new List<TableSchema>();
                foreach (TableSchema tableSchema in tablesAll)
                {
                    if (selectedTables.Contains(tableSchema.Name))
                    {
                        tables.Add(tableSchema);
                    }
                }

                if (tables.Count > 0)
                {
                    string filePath;
                    string output = filesDirectory;

                    if (isPersistenceDbContext)
                    {
                        filePath = generatorManager.GetFilePath(application, "", "DbContext.cs",
                            output,
                            namespacePersistence + "EntityFramework");
                        generatorManager.Persistence_EntityFramework_DbContext(application,
                            namespacePersistence,
                            culture,
                            tables,
                            filePath);
                    }

                    if (isPresentationMenu)
                    {
                        filePath = generatorManager.GetFilePath(application, "Menu.", ".json",
                            output,
                            namespacePresentation, "EasyLOB-Configuration", "Menu");
                        generatorManager.Presentation_MVC_Menu(application,
                            namespacePersistence,
                            culture,
                            tables,
                            filePath);
                    }

                    foreach (TableSchema table in tables)
                    {
                        string className = generatorManager.ClassName(table.FullName, culture);

                        #region Data

                        if (isDataDataModel)
                        {
                            filePath = generatorManager.GetFilePath(className, "", ".cs",
                                output,
                                namespaceData, "DataModels");
                            generatorManager.Data_DataModel(application,
                                namespaceData,
                                culture,
                                table,
                                filePath);
                        }

                        if (isDataDTO)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "DTO.cs",
                            output,
                            namespaceData, "DTOs");
                            generatorManager.Data_DTO(application,
                                namespaceData,
                                culture,
                                table,
                                filePath);
                        }

                        if (isDataResource)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "Resources.resx",
                            output,
                            namespaceData, "Resources");
                            generatorManager.Data_Resource(application,
                                namespaceData,
                                culture,
                                table,
                                filePath);
                        }

                        #endregion Data

                        #region Persistence

                        if (isPersistenceConfiguration)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "Configuration.cs",
                                output,
                                namespacePersistence + "EntityFramework", "Configurations");
                            generatorManager.Persistence_EntityFramework_Configuration(application,
                                namespacePersistence,
                                culture,
                                table,
                                filePath);
                        }

                        #endregion Persistence

                        #region Service

                        if (isServiceController)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "APIController.cs",
                                output,
                                namespaceService, "ControllersAPI", application);
                            generatorManager.Service_WebAPI_Controller(Archetypes.ApplicationDTO,
                                application,
                                namespaceService,
                                culture,
                                table,
                                filePath);
                        }

                        #endregion Service

                        #region Presentation Controller

                        if (isPresentationController)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "Controller.cs",
                                output,
                                namespacePresentation, "Controllers", application);
                            generatorManager.Presentation_MVC_Controller(archetype,
                                application,
                                namespacePresentation,
                                culture,
                                syncfusion,
                                table,
                                filePath);
                        }

                        #endregion Presentation Controller

                        #region Presentation Model

                        if (isPresentationModelCollection)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "CollectionModel.cs",
                                output,
                                namespacePresentation, "Models", application, className);
                            generatorManager.Presentation_MVC_Model_CollectionModel(application,
                                namespacePresentation,
                                culture,
                                table,
                                filePath);
                        }

                        if (isPresentationModelItem)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "ItemModel.cs",
                                output,
                                namespacePresentation, "Models", application, className);
                            generatorManager.Presentation_MVC_Model_ItemModel(application,
                                namespacePresentation,
                                culture,
                                table,
                                filePath);
                        }

                        if (isPresentationModelView)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "ViewModel.cs",
                                output,
                                namespacePresentation, "Models", application, className);
                            generatorManager.Presentation_MVC_Model_ViewModel_DataModel(application,
                                namespacePresentation,
                                culture,
                                table,
                                filePath);
                        }

                        if (isPresentationModelViewProfile)
                        {
                            filePath = generatorManager.GetFilePath(className, "", "Profile.cs",
                                output,
                                namespacePresentation, "Models", application + "-Profile");
                            generatorManager.Presentation_MVC_Model_ViewModel_Profile(application,
                                namespacePresentation,
                                culture,
                                table,
                                filePath);
                        }

                        #endregion Presentation Model

                        #region Presentation View

                        if (isPresentationViewIndex)
                        {
                            filePath = generatorManager.GetFilePath("Index", "", ".cshtml",
                                output,
                                namespacePresentation, "Views", application, className);
                            generatorManager.Presentation_MVC_View_Index(application,
                                namespacePresentation,
                                culture,
                                table,
                                filePath);
                        }

                        if (isPresentationViewSearch)
                        {
                            filePath = generatorManager.GetFilePath("Search", "", ".cshtml",
                                output,
                                namespacePresentation, "Views", application, className);
                            generatorManager.Presentation_MVC_View_Search(application,
                                namespacePresentation,
                                culture,
                                table,
                                filePath);
                        }

                        if (isPresentationViewCRUD)
                        {
                            filePath = generatorManager.GetFilePath("CRUD", "", ".cshtml",
                                output,
                                namespacePresentation, "Views", application, className);
                            generatorManager.Presentation_MVC_View_CRUD(application,
                                namespacePresentation,
                                culture,
                                table,
                                filePath);
                        }

                        if (isPresentationViewCollection)
                        {
                            filePath = generatorManager.GetFilePath(className, "_", "Collection.cshtml",
                                output,
                                namespacePresentation, "Views", application, className);
                            generatorManager.Presentation_MVC_PartialView_Collection(application,
                                namespacePresentation,
                                culture,
                                syncfusion,
                                table,
                                filePath);
                        }

                        if (isPresentationViewItem)
                        {
                            filePath = generatorManager.GetFilePath(className, "_", "Item.cshtml",
                                output,
                                namespacePresentation, "Views", application, className);
                            generatorManager.Presentation_MVC_PartialView_Item(application,
                                namespacePresentation,
                                culture,
                                syncfusion,
                                table,
                                filePath);
                        }

                        if (generatorManager.Framework == Frameworks.DotNetFramework)
                        {
                            if (isPresentationLookupView)
                            {
                                filePath = generatorManager.GetFilePath(className, "_", "Lookup.cshtml",
                                    output,
                                    namespacePresentation, "Views", application, className);
                                generatorManager.Presentation_MVC_PartialView_Lookup(application,
                                    namespacePresentation,
                                    culture,
                                    syncfusion,
                                    table,
                                    filePath);
                            }
                        }

                        #endregion Presentation View

                        #region Presentation Component

                        if (generatorManager.Framework == Frameworks.DotNetCore)
                        {
                            if (isPresentationLookupController)
                            {
                                filePath = generatorManager.GetFilePath(className, "", "LookupViewComponent.cs",
                                    output,
                                    namespacePresentation, "Components");
                                generatorManager.Presentation_MVC_Component_Lookup_Controller(application,
                                    namespacePresentation,
                                    culture,
                                    table,
                                    filePath);
                            }

                            if (isPresentationLookupView)
                            {
                                filePath = generatorManager.GetFilePath("", "", "Default.cshtml",
                                    output,
                                    namespacePresentation, "Views", application, "Components", className);
                                generatorManager.Presentation_MVC_Component_Lookup_View(application,
                                    namespacePresentation,
                                    culture,
                                    table,
                                    filePath);
                            }
                        }

                        #endregion Presentation Component
                    }
                }
            }
        }

        #endregion Methods Generate

        #region Methods IO

        public static bool CopyDirectoryContents(string sourceDirectory,
            string destinationDirectory,
            Func<string, int> log)
        {
            sourceDirectory = sourceDirectory.EndsWith(@"\") ? sourceDirectory : sourceDirectory + @"\";
            destinationDirectory = destinationDirectory.EndsWith(@"\") ? destinationDirectory : destinationDirectory + @"\";

            try
            {
                if (Directory.Exists(sourceDirectory) && !sourceDirectory.ToLower().Contains("node_modules"))
                {
                    log("Copying directory \"" + sourceDirectory + "\" content...");

                    if (Directory.Exists(destinationDirectory) == false)
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    foreach (string files in Directory.GetFiles(sourceDirectory))
                    {
                        FileInfo fileInfo = new FileInfo(files);
                        fileInfo.CopyTo(string.Format(@"{0}\{1}", destinationDirectory, fileInfo.Name), true);
                    }

                    foreach (string drs in Directory.GetDirectories(sourceDirectory))
                    {
                        // .git
                        // .vs
                        // bin
                        // obj
                        // packages
                        if (!drs.EndsWith(".git") && !drs.EndsWith(".vs") &&
                            !drs.EndsWith("obj") && !drs.EndsWith("bin") &&
                            !drs.EndsWith("packages"))
                        {
                            DirectoryInfo directoryInfo = new DirectoryInfo(drs);
                            if (CopyDirectoryContents(drs, destinationDirectory + directoryInfo.Name, log) == false)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch // (Exception exception)
            {
                return false;
            }
        }

        public static bool DeleteDirectoryContents(string directory,
            Func<string, int> log)
        {
            directory = directory.EndsWith(@"\") ? directory : directory + @"\";

            try
            {
                if (Directory.Exists(directory) && !directory.ToLower().Contains("node_modules"))
                {
                    log("Deleting directory \"" + directory + "\" content...");

                    foreach (string files in Directory.GetFiles(directory))
                    {
                        FileInfo fileInfo = new FileInfo(files);
                        fileInfo.Delete();
                    }

                    foreach (string drs in Directory.GetDirectories(directory))
                    {
                        if (DeleteDirectoryContents(drs, log) == false)
                        {
                            return false;
                        }
                        Directory.Delete(drs);
                    }
                }
                return true;
            }
            catch // (Exception exception)
            {
                return false;
            }
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static bool RenameDirectoryContents(string directory,
            string from,
            string to,
            Func<string, int> log)
        {
            directory = directory.EndsWith(@"\") ? directory : directory + @"\";

            try
            {
                if (Directory.Exists(directory) && !directory.ToLower().Contains("node_modules"))
                {
                    log("Renaming directory \"" + directory + "\" content...");

                    foreach (string files in Directory.GetFiles(directory))
                    {
                        FileInfo fileInfo = new FileInfo(files);
                        if (fileInfo.FullName.Contains(from))
                        {
                            File.Move(fileInfo.FullName, fileInfo.FullName.Replace(from, to));
                        }
                    }

                    foreach (string drs in Directory.GetDirectories(directory))
                    {
                        // .git
                        // .vs
                        // bin
                        // obj
                        // packages
                        if (!drs.EndsWith(".git") && !drs.EndsWith(".vs") &&
                            !drs.EndsWith("obj") && !drs.EndsWith("bin") &&
                            !drs.EndsWith("packages"))
                        {
                            string drsFromTo = drs;
                            if (drs.Contains(from))
                            {
                                drsFromTo = drs.Replace(from, to);
                                Directory.Move(drs, drsFromTo);
                            }
                            if (RenameDirectoryContents(drsFromTo, from, to, log) == false)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch // (Exception exception)
            {
                return false;
            }
        }

        public static bool ReplaceDirectoryContents(string directory,
            string from,
            string to,
            Func<string, int> log)
        {
            directory = directory.EndsWith(@"\") ? directory : directory + @"\";

            try
            {
                if (Directory.Exists(directory) && !directory.ToLower().Contains("node_modules"))
                {
                    log("Replacing directory \"" + directory + "\" content...");

                    foreach (string files in Directory.GetFiles(directory))
                    {
                        FileInfo fileInfo = new FileInfo(files);
                        if (".asax|.cs|.cshtml|.csproj|.config|.ini|.json|.resx|.sln".Contains(fileInfo.Extension))
                        {
                            string text = File.ReadAllText(fileInfo.FullName);
                            text = text.Replace(from, to);
                            File.WriteAllText(fileInfo.FullName, text);
                        }
                    }

                    foreach (string drs in Directory.GetDirectories(directory))
                    {
                        // .git
                        // .vs
                        // bin
                        // obj
                        // packages
                        if (!drs.EndsWith(".git") && !drs.EndsWith(".vs") &&
                            !drs.EndsWith("obj") && !drs.EndsWith("bin") &&
                            !drs.EndsWith("packages"))
                        {
                            if (ReplaceDirectoryContents(drs, from, to, log) == false)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch // (Exception exception)
            {
                return false;
            }
        }

        #endregion Methods IO
    }
}