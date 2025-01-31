using DatabaseSchemaReader.DataSchema;
using System.Collections.Generic;

namespace Generator
{
    public enum Frameworks
    {
        DotNetFramework,
        DotNetCore
    }

    public interface IGeneratorManager
    {
        Frameworks Framework { get; }

        string ClassName(string name, Cultures culture);

        List<TableSchema> DatabaseReader2CodeSmith(DatabaseSchema dbSchema);

        string GetFilePath(string name, string prefix, string suffix,
            string output,
            string directory1 = "", string directory2 = "", string directory3 = "", string directory4 = "", string directory5 = "");

        void Data_DataModel(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Data_DTO(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Data_Resource(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Persistence_EntityFramework_DbContext(string Application,
            string Namespace,
            Cultures Culture,
            List<TableSchema> SourceTables,
            string filePath);

        void Persistence_EntityFramework_Configuration(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_Controller(Archetypes archetype,
            string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions syncfusion,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_Model_CollectionModel(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);
        void Presentation_MVC_Model_ItemModel(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_Model_ViewModel_DataModel(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_Model_ViewModel_Profile(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);
        void Presentation_MVC_View_Index(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_View_Search(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_View_CRUD(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_PartialView_Collection(string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions syncfusion,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_PartialView_Item(string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions syncfusion,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_PartialView_Lookup(string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions syncfusion,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_Component_Lookup_Controller(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_Component_Lookup_View(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);

        void Presentation_MVC_Menu(string Application,
            string Namespace,
            Cultures Culture,
            List<TableSchema> SourceTables,
            string filePath);

        void Service_WebAPI_Controller(Archetypes archetype,
            string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath);
    }
}
