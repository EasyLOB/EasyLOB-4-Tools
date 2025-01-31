using System.IO;
using System.Text;

// 3 C   EasyLOB

// = =   Data_DataModel
// = =   Data_DTO
// = =   Data_Resource

// # # X Persistence_EntityFramework_DbContext
// # # X Persistence_EntityFramework_Configuration

//       Service_WebAPI_Controller_Application_DTO

// # # X Presentation_MVC_Controller_Application_DataModel

// = =   Presentation_MVC_Model_CollectionModel
// = =   Presentation_MVC_Model_ItemModel
// = =   Presentation_MVC_Model_ViewModel_DataModel
// = =   Presentation_MVC_Model_ViewModel_Profile

// # # X Presentation_MVC_View_Index
// # # X Presentation_MVC_View_Search
// # # X Presentation_MVC_View_CRUD
// # #   Presentation_MVC_PartialView_Collection
// # # X Presentation_MVC_PartialView_Item

//   # X Presentation_MVC_Component_Lookup
// # #   Presentation_MVC_Component_Lookup_View

// = =   Presentation_MVC_Menu

namespace Generator
{
    public partial class GeneratorManager
    {
        #region Methods

        protected StreamWriter CreateStreamWriter(string filePath,
            FileMode fileMode = FileMode.Create,
            FileAccess fileAccess = FileAccess.ReadWrite)
        {
            return new StreamWriter(new FileStream(filePath, fileMode, fileAccess), Encoding.UTF8);
        }

        public string GetFilePath(string name, string prefix, string suffix,
            string output,
            string directory1 = "", string directory2 = "", string directory3 = "", string directory4 = "", string directory5 = "")
        {
            string directory = output;
            CreateDirectory(directory);

            if (!string.IsNullOrEmpty(directory1))
            {
                directory = Path.Combine(directory, directory1);
                CreateDirectory(directory);

                if (!string.IsNullOrEmpty(directory2))
                {
                    directory = Path.Combine(directory, directory2);
                    CreateDirectory(directory);

                    if (!string.IsNullOrEmpty(directory3))
                    {
                        directory = Path.Combine(directory, directory3);
                        CreateDirectory(directory);

                        if (!string.IsNullOrEmpty(directory4))
                        {
                            directory = Path.Combine(directory, directory4);
                            CreateDirectory(directory);

                            if (!string.IsNullOrEmpty(directory5))
                            {
                                directory = Path.Combine(directory, directory5);
                                CreateDirectory(directory);
                            }
                        }
                    }
                }
            }

            string filePath = Path.Combine(directory, prefix + name + suffix);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return filePath;
        }

        #endregion Methods
    }
}
