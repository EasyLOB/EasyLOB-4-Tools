using System.Collections.Generic;
using System.IO;

// Presentation_MVC_Controller_Application_DataModel

// Presentation_MVC_Model_CollectionModel
// Presentation_MVC_Model_ItemModel
// Presentation_MVC_Model_ViewModel_DataModel
// Presentation_MVC_Model_ViewModel_Profile

// Presentation_MVC_View_Index
// Presentation_MVC_View_Search
// Presentation_MVC_View_CRUD
// Presentation_MVC_PartialView_Collection
//     Presentation_MVC_PartialView_Collection_EJ1
//     Presentation_MVC_PartialView_Collection_EJ2
// Presentation_MVC_PartialView_Item
//     Presentation_MVC_PartialView_Item_EJ1
//     Presentation_MVC_PartialView_Item_EJ2
// Presentation_MVC_PartialView_Lookup
//     Presentation_MVC_PartialView_Lookup_EJ1
//     Presentation_MVC_PartialView_Lookup_EJ2

// Presentation_MVC_Component_Lookup_Controller
// Presentation_MVC_Component_Lookup_View

// Presentation_MVC_Menu

namespace Generator
{
    public partial class GeneratorManagerFramework
    {
        #region Presentation Controller

        public void Presentation_MVC_Controller(Archetypes archetype,
            string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions Syncfusion,
            TableSchema SourceTable,
            string filePath)
        {
            if (archetype == Archetypes.Application)
            {
                Presentation_MVC_Controller_Application_DataModel(Application,
                    Namespace,
                    Culture,
                    Syncfusion,
                    SourceTable,
                    filePath);
            }
        }

        private void Presentation_MVC_Controller_Application_DataModel(string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions Syncfusion,
            TableSchema SourceTable,
            string filePath)
        {
            using (StreamWriter file = CreateStreamWriter(filePath))
            {
                string tableName = TableName(SourceTable.FullName);
                string className = ClassName(SourceTable.FullName, Culture);
                string objectName = ObjectName(SourceTable.FullName, Culture);

                // DataSource(...)
                // orderBy = IsNullOrEmpty(orderBy) ? "<= pkOrderBy >" : orderBy;

                string pkParameters = ""; // int? parameter1, string parameter2, ...
                string pkParametersArray = ""; // new object[] { parameter1, parameter2, ... };
                string pkParametersIsNull = ""; // parameter1 == null || parameter2 == null || ...
                string pkPropertiesArray = ""; // new object[] { ClassItemModel.Class.Property1, ClassItemModel.Class.Property2, ... };
                                               // Url.Content(string.Format("~/< %= className % >/Update< %= pkUrl1 % >", < %= pkUrl2 % >)));
                string pkUrl1 = ""; // ?Key1={0}&Key2={1}& ...
                string pkUrl2 = ""; //  , objectItemModel.Key1, objectItemModel.Key2, ...
                                    // Url.Action("Update", "< %= className % >", new { < %= pkObject % > }, Request.Url.Scheme));
                string pkObject = ""; // Key1 = objectItemModel.Key1, Key2 objectItemModel.Key2, ...
                int urlIndex = 0;
                //string pkPropertiesOrderBy = ""; // Property1, Property2, ...
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    pkParameters += (pkParameters == "" ? "" : ", ") + GetType(column, false) + " " + LocalName(column.Name);
                    pkParametersArray += (pkParametersArray == "" ? "" : ", ") + LocalName(column.Name);
                    pkPropertiesArray += (pkPropertiesArray == "" ? "" : ", ") + objectName + "ItemModel." + className + "." + PropertyName(column.Name);
                    pkParametersIsNull += (pkParametersIsNull == "" ? "" : " || ") + LocalName(PropertyName(column.Name)) + " == null";
                    //pkPropertiesOrderBy += (pkPropertiesOrderBy == "" ? "" : ", ") + PropertyName(column.Name);
                    pkUrl1 += (pkUrl1 == "" ? "" : "&") + PropertyName(column.Name) + "={" + urlIndex++.ToString() + "}";
                    pkUrl2 += (pkUrl2 == "" ? "" : ", ") + objectName + "." + PropertyName(column.Name);
                    pkObject += (pkObject == "" ? "" : ", ") + PropertyName(column.Name) + " = " + objectName + "." + PropertyName(column.Name);
                }
                //if (SourceTable.PrimaryKey.MemberColumns.Count > 1) {
                pkParametersArray = "new object[] { " + pkParametersArray + " }";
                pkPropertiesArray = "new object[] { " + pkPropertiesArray + " }";
                //}
                //else
                //{
                //    ColumnSchema column = SourceTable.PrimaryKey.MemberColumns[0];
                //    pkParametersArray = "(" + GetType(column) + ")" + LocalName(PropertyName(column.Name));
                //    pkPropertiesArray = objectName + "ItemModel." + className + "." + PropertyName(column.Name);
                //}
                pkUrl1 = (pkUrl1 == "" ? "" : "?") + pkUrl1;
                pkUrl2 = (pkUrl2 == "" ? "" : ", ") + pkUrl2;

                file.WriteLine($@"using {Application}.Application;
using {Application}.Data;
using {Application}.Data.Resources;
using EasyLOB;
using EasyLOB.Data;
using EasyLOB.Mvc;
using Newtonsoft.Json;");

                if (Syncfusion == Syncfusions.EJ1)
                {
                    file.WriteLine("using Syncfusion.JavaScript;");
                }
                else
                {
                    file.WriteLine("using Syncfusion.EJ2.Base;");
                }

                file.WriteLine($@"using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace {Namespace}
{{
    public partial class {className}Controller : BaseMvcControllerSCRUDApplication<{className}>
    {{
        #region Methods

        public {className}Controller(I{Application}GenericApplication<{className}> application)
            : base(application.AuthorizationManager)
        {{
            Application = application;
        }}

        #endregion Methods

        #region Methods SCRUD

        // GET: {className}
        // GET: {className}/Index
        [HttpGet]
        public ActionResult Index(string operation = null)
        {{
            {className}CollectionModel {objectName}CollectionModel = new {className}CollectionModel(ActivityOperations, ""Index"", null, null, null, operation);

            try
            {{
                if (IsIndex({objectName}CollectionModel.OperationResult))
                {{
                    return ZView({objectName}CollectionModel);
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}CollectionModel.OperationResult.ParseException(exception);
            }}

            return ZViewOperationResult({objectName}CollectionModel.OperationResult);
        }}

        // GET & POST: {className}/Search
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Search(string masterControllerAction = null, string masterEntity = null, string masterKey = null)
        {{
            {className}CollectionModel {objectName}CollectionModel = new {className}CollectionModel(ActivityOperations, ""Search"", masterControllerAction, masterEntity, masterKey);

            try
            {{
                if (IsOperation({objectName}CollectionModel.OperationResult))
                {{
                    return ZPartialView({objectName}CollectionModel);
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}CollectionModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}CollectionModel.OperationResult);
        }}

        // GET & POST: {className}/Lookup
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Lookup(string text, string valueId, bool? required = false, List<LookupModelElement> elements = null, string query = null)
        {{
            LookupModel lookupModel = new LookupModel(ActivityOperations, text, valueId, required, elements, query);

            try
            {{
                if (IsSearch(lookupModel.OperationResult))
                {{
                    return ZPartialView(""_{className}Lookup"", lookupModel);
                }}
            }}
            catch (Exception exception)
            {{
                lookupModel.OperationResult.ParseException(exception);
            }}

            return null;
        }}

        // GET: {className}/Create
        [HttpGet]
        public ActionResult Create(string masterEntity = null, string masterKey = null)
        {{
            {className}ItemModel {objectName}ItemModel = new {className}ItemModel(ActivityOperations, ""Create"", masterEntity, masterKey);

            try
            {{
                if (IsCreate({objectName}ItemModel.OperationResult))
                {{
                    return ZPartialView(""CRUD"", {objectName}ItemModel);
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}ItemModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}ItemModel.OperationResult);
        }}

        // POST: {className}/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create({className}ItemModel {objectName}ItemModel)
        {{
            try
            {{
                if (IsCreate({objectName}ItemModel.OperationResult))
                {{
                    if (IsValid({objectName}ItemModel.OperationResult, {objectName}ItemModel.{className}))
                    {{
                        {className} {objectName} = ({className}){objectName}ItemModel.{className}.ToData();
                        if (Application.Create({objectName}ItemModel.OperationResult, {objectName}))
                        {{
                            if ({objectName}ItemModel.IsSave)
                            {{
                                Create2Update({objectName}ItemModel.OperationResult);
                                return JsonResultSuccess({objectName}ItemModel.OperationResult,
                                    Url.Action(""Update"", ""{className}"", new {{ {pkObject} }}, Request.Url.Scheme));
                            }}
                            else
                            {{
                                return JsonResultSuccess({objectName}ItemModel.OperationResult);
                            }}
                        }}
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}ItemModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}ItemModel.OperationResult);
        }}

        // GET: {className}/Read/1
        [HttpGet]
        public ActionResult Read({pkParameters}, string masterEntity = null, string masterKey = null)
        {{
            {className}ItemModel {objectName}ItemModel = new {className}ItemModel(ActivityOperations, ""Read"", masterEntity, masterKey);

            try
            {{
                if (IsRead({objectName}ItemModel.OperationResult))
                {{
                    {className} {objectName} = Application.GetById({objectName}ItemModel.OperationResult, {pkParametersArray}, false);
                    if ({objectName} != null)
                    {{
                        {objectName}ItemModel.{className} = new {className}ViewModel({objectName});

                        return ZPartialView(""CRUD"", {objectName}ItemModel);
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}ItemModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}ItemModel.OperationResult);
        }}

        // GET: {className}/Update/1
        [HttpGet]
        public ActionResult Update({pkParameters}, string masterEntity = null, string masterKey = null)
        {{
            {className}ItemModel {objectName}ItemModel = new {className}ItemModel(ActivityOperations, ""Update"", masterEntity, masterKey);

            try
            {{
                if (IsUpdate({objectName}ItemModel.OperationResult))
                {{
                    {className} {objectName} = Application.GetById({objectName}ItemModel.OperationResult, {pkParametersArray}, false);
                    if ({objectName} != null)
                    {{
                        {objectName}ItemModel.{className} = new {className}ViewModel({objectName});

                        return ZPartialView(""CRUD"", {objectName}ItemModel);
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}ItemModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}ItemModel.OperationResult);
        }}

        // POST: {className}/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update({className}ItemModel {objectName}ItemModel)
        {{
            try
            {{
                if (IsUpdate({objectName}ItemModel.OperationResult))
                {{
                    if (IsValid({objectName}ItemModel.OperationResult, {objectName}ItemModel.{className}))
                    {{
                        {className} {objectName} = ({className}){objectName}ItemModel.{className}.ToData();
                        if (Application.Update({objectName}ItemModel.OperationResult, {objectName}))
                        {{
                            if ({objectName}ItemModel.IsSave)
                            {{
                                return JsonResultSuccess({objectName}ItemModel.OperationResult,
                                    Url.Action(""Update"", ""{className}"", new {{ {pkObject} }}, Request.Url.Scheme));
                            }}
                            else
                            {{
                                return JsonResultSuccess({objectName}ItemModel.OperationResult);
                            }}
                        }}
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}ItemModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}ItemModel.OperationResult);
        }}

        // GET: {className}/Delete/1
        [HttpGet]
        public ActionResult Delete({pkParameters}, string masterEntity = null, string masterKey = null)
        {{
            {className}ItemModel {objectName}ItemModel = new {className}ItemModel(ActivityOperations, ""Delete"", masterEntity, masterKey);

            try
            {{
                if (IsDelete({objectName}ItemModel.OperationResult))
                {{
                    {className} {objectName} = Application.GetById({objectName}ItemModel.OperationResult, {pkParametersArray}, false);
                    if ({objectName} != null)
                    {{
                        {objectName}ItemModel.{className} = new {className}ViewModel({objectName});

                        return ZPartialView(""CRUD"", {objectName}ItemModel);
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}ItemModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}ItemModel.OperationResult);
        }}

        // POST: {className}/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete({className}ItemModel {objectName}ItemModel)
        {{
            try
            {{
                if (IsDelete({objectName}ItemModel.OperationResult))
                {{
                    if (Application.Delete({objectName}ItemModel.OperationResult, ({className}){objectName}ItemModel.{className}.ToData()))
                    {{
                        return JsonResultSuccess({objectName}ItemModel.OperationResult);
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                {objectName}ItemModel.OperationResult.ParseException(exception);
            }}

            return JsonResultOperationResult({objectName}ItemModel.OperationResult);
        }}

        #endregion Methods SCRUD

        #region Methods Syncfusion

        // POST: {className}/DataSource
        [HttpPost]");

                if (Syncfusion == Syncfusions.EJ1)
                {
                    file.WriteLine("        public ActionResult DataSource(DataManager dataManager)");
                }
                else
                {
                    file.WriteLine("        public ActionResult DataSource(DataManagerRequest dataManager)");
                }

                //public ActionResult DataSource(DataManager dataManager)

                file.WriteLine($@"        {{
            SyncfusionDataResult dataResult = new SyncfusionDataResult
            {{
                result = new List<{className}ViewModel>()
            }};

            ZOperationResult operationResult = new ZOperationResult();

            if (IsSearch(operationResult))
            {{
                try
                {{
                    SyncfusionGrid syncfusionGrid = new SyncfusionGrid(typeof({className}), Application.UnitOfWork.DBMS);
                    ArrayList args = new ArrayList();
                    string where = syncfusionGrid.ToLinqWhere(dataManager.Search, dataManager.Where, args);
                    string orderBy = syncfusionGrid.ToLinqOrderBy(dataManager.Sorted);
                    int take = (dataManager.Skip == 0 && dataManager.Take == 0) ? AppDefaults.SyncfusionRecordsBySearch : dataManager.Take; // Excel Filtering
                    dataResult.result = ZViewModelHelper<{className}ViewModel, {className}>.ToViewList(Application.Search(operationResult, where, args.ToArray(), orderBy, dataManager.Skip, take));

                    if (dataManager.RequiresCounts)
                    {{
                        dataResult.count = Application.Count(operationResult, where, args.ToArray());
                    }}
                }}
                catch (Exception exception)
                {{
                    operationResult.ParseException(exception);
                }}
            }}

            if (!operationResult.Ok)
            {{
                operationResult.ThrowException();
            }}

            return Json(JsonConvert.SerializeObject(dataResult), JsonRequestBehavior.AllowGet);
        }}

        // POST: {className}/ExportToExcel
        [HttpPost]
        public void ExportToExcel(string gridModel)
        {{
            if (IsExport())
            {{
                ExportToExcel(gridModel, {className}Resources.EntitySingular + "".xlsx"");
            }}
        }}

        // POST: {className}/ExportToPdf
        [HttpPost]
        public void ExportToPdf(string gridModel)
        {{
            if (IsExport())
            {{
                ExportToPdf(gridModel, {className}Resources.EntitySingular + "".pdf"");
            }}
        }}

        // POST: {className}/ExportToWord
        [HttpPost]
        public void ExportToWord(string gridModel)
        {{
            if (IsExport())
            {{
                ExportToWord(gridModel, {className}Resources.EntitySingular + "".docx"");
            }}
        }}

        #endregion Methods Syncfusion
    }}
}}");
            }
        }

        #endregion Presentation Controller

        #region Presentation Model

        public void Presentation_MVC_Model_CollectionModel(string Application,
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

                file.WriteLine($@"using EasyLOB;
using EasyLOB.Mvc;

namespace {Namespace}
{{
    public partial class {className}CollectionModel : CollectionModel
    {{
        #region Methods

        public {className}CollectionModel()
            : base()
        {{
            Entity = ""{className}"";

            OnConstructor();
        }}

        public {className}CollectionModel(ZActivityOperations activityOperations, string controllerAction, string masterControllerAction = null, string masterEntity = null, string masterKey = null, string operation = null)
            : this()
        {{
            ActivityOperations = activityOperations;
            ControllerAction = controllerAction;
            MasterControllerAction = masterControllerAction;
            MasterEntity = masterEntity;
            MasterKey = masterKey;
            Operation = operation;
        }}

        #endregion Methods
    }}
}}");
            }
        }

        public void Presentation_MVC_Model_ItemModel(string Application,
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

                file.WriteLine($@"using {Application}.Data;
using EasyLOB;
using EasyLOB.Mvc;

namespace {Namespace}
{{
    public partial class {className}ItemModel : ItemModel
    {{
        #region Properties

        public {className}ViewModel {className} {{ get; set; }}

        #endregion Properties
        
        #region Methods

        public {className}ItemModel()
            : base()
        {{
            Entity = ""{className}"";
            {className} = new {className}ViewModel();

            OnConstructor();
        }}

        public {className}ItemModel(ZActivityOperations activityOperations, string controllerAction, string masterEntity = null, string masterKey = null, {className}ViewModel {objectName} = null)
            : this()
        {{
            ActivityOperations = activityOperations;
            ControllerAction = controllerAction;
            MasterEntity = masterEntity;
            MasterKey = masterKey;
            {className} = {objectName} ?? {className};
        }}
        
        #endregion Methods
    }}
}}");
            }
        }

        public void Presentation_MVC_Model_ViewModel_DataModel(string Application,
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

                bool isDataAnnotations = true; // Visual Studio Scaffolding or HTML Helpers

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

                // Associations

                Dictionary<string, int> associations123 = new Dictionary<string, int>();
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    Dictionary123(associations123, pkClassName);
                }

                file.WriteLine($@"using {Application}.Data.Resources;
using EasyLOB;
using EasyLOB.Data;
using EasyLOB.Library;
using EasyLOB.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace {Application}.Data
{{
    public partial class {className}ViewModel : ZViewModel<{className}ViewModel, {className}>
    {{
        #region Properties");

                int pkIndex = 1;
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    //bool isPrimaryKey = column.IsPrimaryKeyMember;
                    //bool isIdentity = IsIdentity(SourceTable.PrimaryKey.MemberColumns[0]);
                    bool isNullable = column.AllowDBNull;

                    file.WriteLine();

                    if (isDataAnnotations)
                    {
                        file.WriteLine($@"        [Display(Name = ""Property{PropertyName(column.Name)}"", ResourceType = typeof({className}Resources))]");
                    }
                    if (isDataAnnotations)
                    {
                        if (IsDate(column.DataType) || IsDateTime(column.DataType))
                        {
                            file.WriteLine($@"        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = ""{{0:d}}"", ApplyFormatInEditMode = true)]");
                        } else if (IsDateTime(column.DataType)) {
                            // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                            file.WriteLine($@"        //[DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = ""{{0:G}}"", ApplyFormatInEditMode = true)]");
                        }
                        else if (IsDecimal(column.DataType) || IsFloat(column.DataType)) {
                            file.WriteLine($@"        [DisplayFormat(DataFormatString = ""{{0:f2}}"", ApplyFormatInEditMode = true)]");
                        } else if (IsInteger(column.DataType)) {
                            file.WriteLine($@"        [DisplayFormat(DataFormatString = ""{{0:d}}"", ApplyFormatInEditMode = true)]");
                        }
                    }
                    if (isDataAnnotations && column.IsPrimaryKeyMember)
                    {
                        file.WriteLine($@"        //[Key]");
                    }
                    if (isDataAnnotations && column.IsPrimaryKeyMember && SourceTable.PrimaryKey.MemberColumns.Count > 1)
                    {
                        file.WriteLine($@"        //[Column(Order={pkIndex++.ToString()})]");
                    }
                    if (isDataAnnotations && column.IsForeignKeyMember && IsInteger(column.DataType) && !isNullable)
                    {
                        file.WriteLine($@"        [Range(1, System.Int32.MaxValue, ErrorMessageResourceName = ""Range"", ErrorMessageResourceType = typeof(DataAnnotationResources))]");
                    }
                    if (isDataAnnotations && !isNullable)
                    {
                        file.WriteLine($@"        [Required]");
                    }
                    if (isDataAnnotations && IsString(column.DataType))
                    {
                        string stringLength = column.Size.ToString();
                        if (column.Size == -1 || IsNText(column) || IsText(column))
                        {
                            stringLength = 1024.ToString();
                        }
                        else if (IsImage(column))
                        {
                            stringLength = 8192.ToString();
                        };

                        file.WriteLine($@"        [StringLength({stringLength})]");
                    }

                    file.WriteLine($@"        public virtual {GetType(column)} {PropertyName(column.Name)} {{ get; set; }}");
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
        
        public {className}ViewModel()
        {{
            OnConstructor();
        }}

        public {className}ViewModel(IZDataModel dataModel)
        {{
            FromData(dataModel);
        }}

        #endregion Methods
    }}
}}");
            }
        }

        public void Presentation_MVC_Model_ViewModel_Profile(string Application,
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
                Dictionary1(associations123);

                // Collections

                Dictionary<string, int> collections123 = new Dictionary<string, int>();
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                    Dictionary123(collections123, pkClassName);
                }
                Dictionary1(collections123);

                file.WriteLine($@"using EasyLOB;

namespace {Application}.Data
{{
    public partial class {className}ViewModel
    {{
        #region Methods

        public static void OnSetupProfile(IZProfile profile)
        {{");
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                    string x = "";
                    if (collections123.ContainsKey(pkClassName))
                    {
                        x = (++collections123[pkClassName]).ToString();
                    }

                    file.WriteLine($@"            //profile.Collections[""{Plural(pkClassName, Culture)}{x}""] = false;");
                }

                if (SourceTable.PrimaryKeys.Count > 0 && SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine();
                }

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

                    file.WriteLine($@"            //profile.SetProfileProperty(""{pkClassName2}{x}LookupText"", isGridVisible: true);");
                }

                file.WriteLine($@"        }}

        #endregion Methods
    }}
}}");
            }
        }

        #endregion Presentation Model

        #region Presentation View

        public void Presentation_MVC_View_Index(string Application,
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

                file.WriteLine($@"@model {className}CollectionModel

@{{
    string CSHTML = ""{className} - Index.cshtml"";
}}

<div id=""ZAjax""></div>

<script>
    $(function () {{
        try {{
            zUrlDictionaryClear();

            $(""#ZAjax"").load(""@(Url.Action(Model.OperationAction, ""{className}"", null, Request.Url.Scheme))"", function(responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception.message));
        }}
    }});
</script>");
            }
        }

        public void Presentation_MVC_View_Search(string Application,
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

                file.WriteLine($@"@model {className}CollectionModel

@{{
    string CSHTML = ""{className} - Search.cshtml"";

    string controllerAction = Model.ControllerAction.ToLower();
    string controllerActionResource = PresentationResources.ResourceManager.GetString(Model.ControllerAction);

    string documentTitle, pageTitle;
    AppHelper.Title(out documentTitle, out pageTitle,
        {className}Resources.EntitySingular, {className}Resources.EntityPlural,
        controllerAction, controllerActionResource,
        Model.IsMasterDetail);
}}

<h4>@pageTitle</h4>

<div class=""form-inline"" style=""display: none"">
    @{{ Html.RenderPartial(""_{className}Collection"", Model); }}
</div>

<script>
    $(function () {{
        try {{
            var model = @Html.Raw(JsonConvert.SerializeObject(Model));

            var url = zUrlDictionaryRead(""{className}""); // Master-Detail
            if (!url) {{
                zUrlDictionaryWrite(""{className}"", ""@Context.Request.Url.AbsoluteUri"");
            }}

            if (!model.IsMasterDetail) {{
                $(document).prop(""title"", ""@Html.Raw(documentTitle)"");
            }}
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception.message));
        }}
    }});
</script>");
            }
        }

        public void Presentation_MVC_View_CRUD(string Application,
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

                file.WriteLine($@"@model {className}ItemModel

@{{
    string CSHTML = ""{className} - CRUD.cshtml"";

    string controllerAction = Model.ControllerAction.ToLower();
    string controllerActionResource = PresentationResources.ResourceManager.GetString(Model.ControllerAction);

    string documentTitle, pageTitle;
    AppHelper.Title(out documentTitle, out pageTitle,
        {className}Resources.EntitySingular, {className}Resources.EntityPlural,
        controllerAction, controllerActionResource,
        Model.IsMasterDetail);
}}

@using (Ajax.BeginForm(Model.ControllerAction, ""{className}"", AppHelper.AjaxOptions, new {{ id = ""Form_{className}"", style = ""display: none"" }}))
{{
    @Html.AntiForgeryToken()

    <h4>@pageTitle</h4>

    <div class=""@AppDefaults.CSSClassForm"">
        @{{ Html.RenderPartial(""_{className}Item"", Model); }}
    </div>

    <div class=""@AppDefaults.CSSClassFormButtons"">
        @Ajax.ZImageLink(""Button_Cancel"", Url.Action(""Search"", ""{className}""), ""ZAjax"", ""btn z-buttonCancel"", PresentationResources.Cancel)
        @Html.ZImageInput(""Button_Save"", ""btn z-buttonSave"", PresentationResources.Save, ""$('#{className}_Item_IsSave').val('True');"")
        @Html.ZImageInput(""Button_OK"", ""btn z-buttonOk"", controllerActionResource)
    </div>
}}

<script>
    $(function () {{
        try {{
            var model = @Html.Raw(JsonConvert.SerializeObject(Model));

            var url = zUrlDictionaryRead(""{className}"");
            if (url) {{
                $(""#Button_Cancel"").attr(""href"", url);
            }}

            $(document).prop(""title"", ""@Html.Raw(documentTitle)"");

            zOnCRUDView(model);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception.message));
        }}
    }});
</script>");
            }
        }

        public void Presentation_MVC_PartialView_Collection(string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions syncfusion,
            TableSchema SourceTable,
            string filePath)
        {
            if (syncfusion == Syncfusions.EJ1)
            {
                Presentation_MVC_PartialView_Collection_EJ1(Application,
                    Namespace,
                    Culture,
                    SourceTable,
                    filePath);
            }
            else
            {
                Presentation_MVC_PartialView_Collection_EJ2(Application,
                    Namespace,
                    Culture,
                    SourceTable,
                    filePath);
            }
        }

        private void Presentation_MVC_PartialView_Collection_EJ1(string Application,
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

                string pkUrl = ""; // Property1=String(data.Property1), Property2=String(data.Property2), ...
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    if (IsDate(column.DataType) || IsDateTime(column.DataType))
                    {
                        pkUrl += (pkUrl == "" ? "" : " + \"&") + PropertyName(column.Name) + "=\" + data." + PropertyName(column.Name) + ".toISOString()";
                    }
                    else
                    {
                        pkUrl += (pkUrl == "" ? "" : " + \"&") + PropertyName(column.Name) + "=\" + String(data." + PropertyName(column.Name) + ")";
                    }
                }

                // Associations

                Dictionary<string, int> associations123 = new Dictionary<string, int>();
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    Dictionary123(associations123, pkClassName);
                }

                file.WriteLine($@"@model {className}CollectionModel

@{{
    string CSHTML = ""_{className}Collection.cshtml"";

    IZProfile profile = DataHelper.GetProfile(typeof({className}));
    // Associations (FK)
    string query = """";");

                if (SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine($@"    switch (Model.MasterEntity)
    {{");
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

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"        case ""{pkClassName}{x}"":");

                        if (IsString(fkColumn.DataType))
                        {
                            file.WriteLine($@"            query = string.Format(""ej.Query().where('{fkPropertyName}', ej.FilterOperators.equal, '{{0}}')"", Model.MasterKey);");
                        }
                        else
                        {
                            file.WriteLine($@"            query = string.Format(""ej.Query().where('{fkPropertyName}', ej.FilterOperators.equal, {{0}})"", Model.MasterKey);");
                        }

                        if (x != "")
                        {
                            file.WriteLine($@"            Model.Suffix = ""{x}"";");
                        }

                        file.WriteLine($@"            break;");
                    }

                    file.WriteLine($@"    }}");
                }

                file.WriteLine($@"}}

<div id=""Collection_@(Model.Id)"">
    @(Html.EJ().Grid<{className}>(""Grid_"" + Model.Id)
        .Query(query)
        .AllowFiltering()
        .AllowGrouping()
        .AllowPaging()
        .AllowReordering()
        //.AllowResizeToFit()
        .AllowResizing()
        //.AllowScrolling()
        .AllowSearching()
        .AllowSorting()
        .AllowTextWrap()
        .TextWrapSettings(wrap =>
        {{
            wrap.WrapMode(WrapMode.Both);
        }})
        .ClientSideEvents(clientEvent => clientEvent
            .ActionBegin(""actionBegin_Grid_"" + Model.Id)
            .ActionFailure(""actionFailure_Grid_"" + Model.Id)
            .Load(""load_Grid_"" + Model.Id)
            .ToolbarClick(""toolbarClick_Grid_"" + Model.Id)
        )
        .Columns(column =>
        {{");

                //int visibles = 0;
                //string visible;
                Dictionary1(associations123);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    bool columnIsPrimaryKey = column.IsPrimaryKeyMember;
                    bool columnIsForeignKey = column.IsForeignKeyMember;
                    bool columnIsIdentity = IsIdentity(column);
                    bool columnIsNullable = column.AllowDBNull;

                    //if (columnIsPrimaryKey)
                    //{
                    //    if (SourceTable.PrimaryKey.MemberColumns.Count == SourceTable.Columns.Count)
                    //    {
                    //        visible = "true";
                    //    }
                    //    else
                    //    {
                    //        visible = "false";
                    //    }
                    //}
                    //else if (columnIsForeignKey)
                    //{
                    //    visible = "false";
                    //}
                    //else if (visibles >= 1)
                    //{
                    //    visible = "false";
                    //}
                    //else
                    //{
                    //    visibles++;
                    //    visible = "true";
                    //}

                    file.WriteLine($@"            column.Field(""{PropertyName(column.Name)}"")");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                .AllowEditing(false)");
                    }
                    if (IsBoolean(column.DataType))
                    {
                        file.WriteLine($@"                .Type(""boolean"")
                .EditType(EditingType.BooleanEdit)");
                    }
                    else if (IsDate(column.DataType) || IsTime(column.DataType) || IsDateTime(column.DataType))
                    {
                        file.WriteLine($@"                .Type(""date"")
                .EditType(EditingType.Datepicker)
                .Format(SyncfusionPatternResources.GridFormat_Date)");
                    }
                    else if (IsTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"                .Type(""datetime"")
                .EditType(EditingType.DateTimePicker)
                .Format(SyncfusionPatternResources.GridFormat_Time)");
                    }
                    else if (IsDateTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"                .Type(""datetime"")
                .EditType(EditingType.DateTimePicker)
                .Format(SyncfusionPatternResources.GridFormat_DateTime)");
                    }
                    else if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                .Type(""number"")
                .EditType(EditingType.NumericEdit)");
                        if (IsDecimal(column.DataType) || IsFloat(column.DataType))
                        {
                            file.WriteLine($@"                .Format(SyncfusionPatternResources.GridFormat_Float)");
                        }
                        else
                        {
                            file.WriteLine($@"                .Format(SyncfusionPatternResources.GridFormat_Integer)");
                        }
                    }
                    else
                    {
                        file.WriteLine($@"                .Type(""string"")
                .EditType(EditingType.StringEdit)");
                    }

                    file.WriteLine($@"                .HeaderText({className}Resources.Property{PropertyName(column.Name)})");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                .IsIdentity(true)");
                    }
                    if (columnIsPrimaryKey)
                    {
                        file.WriteLine($@"                .IsPrimaryKey(true)");
                    }
                    if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                .TextAlign(TextAlign.Right)");
                    }

                    file.WriteLine($@"                .Visible(profile.IsGridVisibleFor(""{PropertyName(column.Name)}""))
                .Width(profile.GridWidthFor(""{PropertyName(column.Name)}""))
                .Add();");

                    if (columnIsForeignKey)
                    {
                        foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                        {
                            if (fkTable.ForeignKeyMemberColumns[0].Name == column.Name)
                            {
                                string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                                string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                                string x = "";
                                if (associations123.ContainsKey(pkClassName))
                                {
                                    x = (++associations123[pkClassName]).ToString();
                                }

                                file.WriteLine($@"            column.Field(""{pkClassName2}{x}LookupText"")
                .AllowEditing(false)
                .Type(""string"")
                .HeaderText({ClassName(fkTable.PrimaryKeyTable.FullName, Culture)}Resources.EntitySingular)
                .Visible(profile.IsGridVisibleFor(""{pkClassName2}{x}LookupText""))
                .Width(profile.GridWidthFor(""{pkClassName2}{x}LookupText""))
                .Add();");
                            }
                        }
                    }
                }

                file.WriteLine($@"        }})
        .EditSettings(edit => edit
            .AllowAdding()
            .AllowDeleting()
            .AllowEditing()
            .AllowEditOnDblClick(false)
        )
        //.EnablePersistence()
        .EnableTouch(false)
        //.IsResponsive(true)
        .FilterSettings(filter => filter
            .EnableCaseSensitivity(false)
            .FilterType(FilterType.Excel)
            .MaxFilterChoices(AppDefaults.SyncfusionRecordsForFiltering)
        )
        //.Mappers(map => map
        //    .ExportToExcelAction(Url.Content(""~/{className}/ExportToExcel""))
        //    .ExportToPdfAction(Url.Content(""~/{className}/ExportToPdf""))
        //    .ExportToWordAction(Url.Content(""~/{className}/ExportToWord""))
        //)
        .PageSettings(page => page
            .PageSize(AppDefaults.SyncfusionRecordsByPage)
        )
        .ShowColumnChooser()
        .ToolbarSettings(toolbar => toolbar
            .CustomToolbarItems(new List<object>() {{
                new Syncfusion.JavaScript.Models.CustomToolbarItem() {{ TemplateID = ""#Grid_"" + Model.Id + ""_Toolbar"", Tooltip = """" }}
            }})
            .ShowToolbar()
            .ToolbarItems(items =>
            {{
                items.AddTool(ToolBarItems.Search);
                items.AddTool(ToolBarItems.Add);
                items.AddTool(ToolBarItems.Edit);
                items.AddTool(ToolBarItems.Delete);
                //items.AddTool(ToolBarItems.ExcelExport);
                //items.AddTool(ToolBarItems.PdfExport);
                //items.AddTool(ToolBarItems.WordExport);
            }})
        )
    )
</div>

<script type=""text/x-jsrender"" id=""Grid_@(Model.Id)_Toolbar"">
    <div id=""Grid_@(Model.Id)_Toolbar_Read"" class=""e-toolbaricons e-icon e-document"" title=""@PresentationResources.Read""></div>
    <div id=""Grid_@(Model.Id)_Toolbar_Refresh"" class=""e-toolbaricons e-icon e-reload"" title=""@PresentationResources.Refresh""></div>
</script>

<script>
    zSyncfusionCollection(""Collection_@(Model.Id)"");

    $(function () {{
        try {{
            zSyncfusionCollectionReady(""Collection_@(Model.Id)"");

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));

            zOnCollectionView(model, profile, ""@Url.Action(""DataSource"", ""{className}"")"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception));
        }}
    }});

    function actionBegin_Grid_@(Model.Id)(args) {{
        try {{
            var model = @Html.Raw(JsonConvert.SerializeObject(Model));

            // Associations (FK)
            var url = """";");

                if (SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine($@"            switch (model.MasterEntity) {{");

                    Dictionary1(associations123);
                    foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                    {
                        string fkClassName = ClassName(fkTable.ForeignKeyTable.FullName, Culture);
                        string fkPropertyName = PropertyName(fkTable.ForeignKeyMemberColumns[0].Name);
                        string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);
                        string pkPropertyName = PropertyName(fkTable.PrimaryKeyMemberColumns[0].Name);

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"                case ""{pkClassName}{x}"":
                    url = url + (url == """" ? """" : ""&"") + ""MasterEntity={pkClassName}{x}&MasterKey="" + model.MasterKey;
                    break;");
                    }
                    file.WriteLine($@"            }}");
                }

                file.WriteLine($@"
            var scrud = zOnCollectionViewActionBeginSCRUD(model);
            switch (args.requestType) {{
                case ""searching"":
                    // Search
                    if (scrud.isSearch) {{
                        zSearchDictionaryWrite(""{className}"", args.keyValue);
                    }}
                    break;
                case ""add"":
                    // Create
                    if (model.ActivityOperations.IsCreate && scrud.isCreate) {{
                        $(""#ZAjax"").load(""@(Url.Action(""Create"", ""{className}"", null, Request.Url.Scheme))"" +
                            (url == """" ? """" : ""?"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                    }}
                    break;
                case ""read"":
                    // Read
                    if (model.ActivityOperations.IsRead && scrud.isRead) {{
                        var data = args.data;
                        if (!ej.isNullOrUndefined(data)) {{
                            $(""#ZAjax"").load(""@(Url.Action(""Read"", ""{className}"", null, Request.Url.Scheme))?{pkUrl} +
                                (url == """" ? """" : ""&"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                        }}
                    }}
                    break;
                case ""beginedit"":
                    // Update
                    if (model.ActivityOperations.IsUpdate && scrud.isUpdate) {{
                        var data = this.model.currentViewData[args.rowIndex];
                        if (!ej.isNullOrUndefined(data)) {{
                            $(""#ZAjax"").load(""@(Url.Action(""Update"", ""{className}"", null, Request.Url.Scheme))?{pkUrl} +
                                (url == """" ? """" : ""&"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                        }}
                    }}
                    break;
                case ""delete"":
                    // Delete
                    if (model.ActivityOperations.IsDelete && scrud.isDelete) {{
                        var data = args.data;
                        if (!ej.isNullOrUndefined(data)) {{
                            $(""#ZAjax"").load(""@(Url.Action(""Delete"", ""{className}"", null, Request.Url.Scheme))?{pkUrl} +
                                (url == """" ? """" : ""&"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                        }}
                    }}
                    break;
            }}

            zOnCollectionViewActionBegin(model, args);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""actionBegin_Grid_@(Model.Id)"", exception));
        }}
    }}

    function actionFailure_Grid_@(Model.Id)(args) {{
        try {{
            zAlert(args.error.responseText);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""actionFailure_Grid_@(Model.Id)"", exception));
        }}
    }}

    function load_Grid_@(Model.Id)(args) {{
        try {{
            var culture = ""@System.Globalization.CultureInfo.CurrentCulture.Name"";
            this.model.locale = culture;
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""load_Grid_@(Model.Id)"", exception));
        }}
    }}

    function toolbarClick_Grid_@(Model.Id)(sender) {{
        try {{
            var toolbar = $(sender.target);

            if (toolbar.prop(""id"") == ""Grid_@(Model.Id)_Toolbar_Read"") {{
                var records = this.getSelectedRecords();
                if (records.length == 1) {{
                    var args = {{ requestType: ""read"", data: records[0] }};
                    actionBegin_Grid_@(Model.Id)(args);
                }}
            }}
            else if (toolbar.prop(""id"") == ""Grid_@(Model.Id)_Toolbar_Refresh"") {{
                this.refreshContent();
            }}
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""toolbarClick_Grid_@(Model.Id)"", exception));
        }}
    }}
</script>");
            }
        }

        private void Presentation_MVC_PartialView_Collection_EJ2(string Application,
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

                string pkUrl = ""; // Property1=String(data.Property1), Property2=String(data.Property2), ...
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    if (IsDate(column.DataType) || IsDateTime(column.DataType))
                    {
                        pkUrl += (pkUrl == "" ? "" : " + \"&") + PropertyName(column.Name) + "=\" + data." + PropertyName(column.Name) + ".toISOString()";
                    }
                    else
                    {
                        pkUrl += (pkUrl == "" ? "" : " + \"&") + PropertyName(column.Name) + "=\" + String(data." + PropertyName(column.Name) + ")";
                    }
                }

                // Associations

                Dictionary<string, int> associations123 = new Dictionary<string, int>();
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    Dictionary123(associations123, pkClassName);
                }

                file.WriteLine($@"@model {className}CollectionModel

@{{
    string CSHTML = ""_{className}Collection.cshtml"";

    IZProfile profile = DataHelper.GetProfile(typeof({className}));
    // Associations (FK)
    string query = ""null"";");

                if (SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine($@"    switch (Model.MasterEntity)
    {{");
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

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"        case ""{pkClassName}{x}"":");

                        if (IsString(fkColumn.DataType))
                        {
                            file.WriteLine($@"            query = string.Format(""new ej.data.Query().where(new ej.data.Predicate('{fkPropertyName}', 'equal', '{{0}}'))"", Model.MasterKey);");
                        }
                        else
                        {
                            file.WriteLine($@"            query = string.Format(""new ej.data.Query().where(new ej.data.Predicate('{fkPropertyName}', 'equal', {{0}}))"", Model.MasterKey);");
                        }

                        if (x != "")
                        {
                            file.WriteLine($@"            Model.Suffix = ""{x}"";");
                        }

                        file.WriteLine($@"            break;");
                    }

                    file.WriteLine($@"    }}");
                }

                file.WriteLine($@"
    List<object> toolbar = new List<object>();
    toolbar.Add(""ColumnChooser"");
    toolbar.Add(""Search"");
    toolbar.Add(""Add"");
    toolbar.Add(new {{ id = ""Grid_"" + Model.Id + ""_Read"", text = """", tooltipText = PresentationResources.Read, prefixIcon = ""e-easylob-read"" }});
    toolbar.Add(""Edit"");
    toolbar.Add(""Delete"");
    toolbar.Add(new {{ id = ""Grid_"" + Model.Id + ""_Refresh"", text = """", tooltipText = PresentationResources.Refresh, prefixIcon = ""e-easylob-refresh"" }});
}}

<div id=""Collection_@(Model.Id)"">
    @(Html.EJS().Grid(""Grid_"" + Model.Id)
        .AllowFiltering()
        .AllowGrouping(false)
        .AllowPaging()
        .AllowReordering()
        .AllowResizing()
        .AllowSorting()
        .AllowTextWrap()
        .Locale(AppHelper.CultureLanguage)
        .Query(query)
        .ShowColumnChooser(true)
        .Toolbar(toolbar)
        .ActionBegin(""actionBegin_Grid_"" + Model.Id)
        .ActionFailure(""actionFailure_Grid_"" + Model.Id)
        .ToolbarClick(""toolbarClick_Grid_"" + Model.Id)
        .Columns(column =>
        {{");

                //int visibles = 0;
                //string visible;
                Dictionary1(associations123);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    bool columnIsPrimaryKey = column.IsPrimaryKeyMember;
                    bool columnIsForeignKey = column.IsForeignKeyMember;
                    bool columnIsIdentity = IsIdentity(column);
                    bool columnIsNullable = column.AllowDBNull;

                    //if (columnIsPrimaryKey)
                    //{
                    //    if (SourceTable.PrimaryKey.MemberColumns.Count == SourceTable.Columns.Count)
                    //    {
                    //        visible = "true";
                    //    }
                    //    else
                    //    {
                    //        visible = "false";
                    //    }
                    //}
                    //else if (columnIsForeignKey)
                    //{
                    //    visible = "false";
                    //}
                    //else if (visibles >= 1)
                    //{
                    //    visible = "false";
                    //}
                    //else
                    //{
                    //    visibles++;
                    //    visible = "true";
                    //}

                    file.WriteLine($@"            column.Field(""{PropertyName(column.Name)}"")");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                .AllowEditing(false)");
                    }
                    if (IsBoolean(column.DataType))
                    {
                        file.WriteLine($@"                .Type(""boolean"")
                .EditType(""booleanEdit"")");
                    }
                    else if (IsDate(column.DataType) || IsTime(column.DataType) || IsDateTime(column.DataType))
                    {
                        file.WriteLine($@"                .Type(""date"")
                .EditType(""datePickerEdit"")
                .Format(SyncfusionPatternResources.GridFormat_Date)");
                    }
                    else if (IsTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"                .Type(""datetime"")
                .EditType(""dateTimePickerEdit"")
                .Format(SyncfusionPatternResources.GridFormat_Time)");
                    }
                    else if (IsDateTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"                .Type(""datetime"")
                .EditType(""dateTimePickerEdit"")
                .Format(SyncfusionPatternResources.GridFormat_DateTime)");
                    }
                    else if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                .Type(""number"")
                .EditType(""numericEdit"")");
                        if (IsDecimal(column.DataType) || IsFloat(column.DataType))
                        {
                            file.WriteLine($@"                .Format(SyncfusionPatternResources.GridFormat_Float)");
                        }
                        else
                        {
                            file.WriteLine($@"                .Format(SyncfusionPatternResources.GridFormat_Integer)");
                        }
                    }
                    else
                    {
                        file.WriteLine($@"                .Type(""string"")
                .EditType(""stringEdit"")");
                    }

                    file.WriteLine($@"                .HeaderText({className}Resources.Property{PropertyName(column.Name)})");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                .IsIdentity(true)");
                    }
                    if (columnIsPrimaryKey)
                    {
                        file.WriteLine($@"                .IsPrimaryKey(true)");
                    }
                    if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                .TextAlign(TextAlign.Right)");
                    }

                    file.WriteLine($@"                .Visible(profile.IsGridVisibleFor(""{PropertyName(column.Name)}""))
                .Width(profile.GridWidthFor(""{PropertyName(column.Name)}""))
                .Add();");

                    if (columnIsForeignKey)
                    {
                        foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                        {
                            if (fkTable.ForeignKeyMemberColumns[0].Name == column.Name)
                            {
                                string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                                string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                                string x = "";
                                if (associations123.ContainsKey(pkClassName))
                                {
                                    x = (++associations123[pkClassName]).ToString();
                                }

                                file.WriteLine($@"            column.Field(""{pkClassName2}{x}LookupText"")
                .AllowEditing(false)
                .Type(""string"")
                .HeaderText({ClassName(fkTable.PrimaryKeyTable.FullName, Culture)}Resources.EntitySingular)
                .Visible(profile.IsGridVisibleFor(""{pkClassName2}{x}LookupText""))
                .Width(profile.GridWidthFor(""{pkClassName2}{x}LookupText""))
                .Add();");
                            }
                        }
                    }
                }

                file.WriteLine($@"        }})
        .EditSettings(edit => edit
            .AllowAdding(true)
            .AllowDeleting(true)
            .AllowEditing(true)
            .AllowEditOnDblClick(false)
        )
        .FilterSettings(filter => filter
            .EnableCaseSensitivity(false)
            .Type(FilterType.Excel)
        )
        .PageSettings(page => page
            .PageSize(AppDefaults.SyncfusionRecordsByPage)
        )
        .TextWrapSettings(wrap => wrap
            .WrapMode(WrapMode.Content)
        )
        .Render()
    )
</div>

<script>
    zSyncfusionCollection(""Collection_@(Model.Id)"");

    $(function () {{
        try {{
            zSyncfusionCollectionReady(""Collection_@(Model.Id)"");

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));

            zOnCollectionView(model, profile, ""@Url.Action(""DataSource"", ""{className}"")"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception));
        }}
    }});

    function actionBegin_Grid_@(Model.Id)(args) {{
        try {{
            var model = @Html.Raw(JsonConvert.SerializeObject(Model));

            // Associations (FK)
            var url = """";");

                if (SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine($@"            switch (model.MasterEntity) {{");

                    Dictionary1(associations123);
                    foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                    {
                        string fkClassName = ClassName(fkTable.ForeignKeyTable.FullName, Culture);
                        string fkPropertyName = PropertyName(fkTable.ForeignKeyMemberColumns[0].Name);
                        string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);
                        string pkPropertyName = PropertyName(fkTable.PrimaryKeyMemberColumns[0].Name);

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"                case ""{pkClassName}{x}"":
                    url = url + (url == """" ? """" : ""&"") + ""MasterEntity={pkClassName}{x}&MasterKey="" + model.MasterKey;
                    break;");
                    }
                    file.WriteLine($@"            }}");
                }

                file.WriteLine($@"
            var scrud = zOnCollectionViewActionBeginSCRUD(model);
            switch (args.requestType) {{
                case ""searching"":
                    // Search
                    if (scrud.isSearch) {{
                        zSearchDictionaryWrite(""{className}"", args.searchString);
                    }}
                    break;
                case ""add"":
                    // Create
                    if (model.ActivityOperations.IsCreate && scrud.isCreate) {{
                        $(""#ZAjax"").load(""@(Url.Action(""Create"", ""{className}"", null, Request.Url.Scheme))"" +
                            (url == """" ? """" : ""?"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                    }}
                    break;
                case ""read"":
                    // Read
                    if (model.ActivityOperations.IsRead && scrud.isRead) {{
                        var data = args.data;
                        if (!ej.base.isNullOrUndefined(data)) {{
                            $(""#ZAjax"").load(""@(Url.Action(""Read"", ""{className}"", null, Request.Url.Scheme))?{pkUrl} +
                                (url == """" ? """" : ""&"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                        }}
                    }}
                    break;
                case ""beginEdit"":
                    // Update
                    if (model.ActivityOperations.IsUpdate && scrud.isUpdate) {{
                        var data = args.rowData;
                        if (!ej.base.isNullOrUndefined(data)) {{
                            $(""#ZAjax"").load(""@(Url.Action(""Update"", ""{className}"", null, Request.Url.Scheme))?{pkUrl} +
                                (url == """" ? """" : ""&"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                        }}
                    }}
                    break;
                case ""delete"":
                    // Delete
                    if (model.ActivityOperations.IsDelete && scrud.isDelete) {{
                        var data = args.data[0];
                        if (!ej.base.isNullOrUndefined(data)) {{
                            $(""#ZAjax"").load(""@(Url.Action(""Delete"", ""{className}"", null, Request.Url.Scheme))?{pkUrl} +
                                (url == """" ? """" : ""&"") + url, function (responseText, textStatus, jqXHR) {{ zAjaxLoadComplete(responseText, textStatus, jqXHR); }});
                        }}
                    }}
                    break;
                case ""filterchoicerequest"":
                    args.filterChoiceCount = @AppDefaults.SyncfusionRecordsForFiltering; 
                    break;
            }}

            zOnCollectionViewActionBegin(model, args);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""actionBegin_Grid_@(Model.Id)"", exception));
        }}
    }}

    function actionFailure_Grid_@(Model.Id)(args) {{
        try {{
            zAlert(args.error.error.responseText);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""actionFailure_Grid_@(Model.Id)"", exception));
        }}
    }}

    function toolbarClick_Grid_@(Model.Id)(args) {{
        try {{
            var operation = args.item.id.replace(""Grid_@(Model.Id)_"", """").toLowerCase();

            if (operation == ""read"") {{
                var records = this.getSelectedRecords();
                if (records.length == 1) {{
                    var args = {{ requestType: ""read"", data: records[0] }};
                    actionBegin_Grid_@(Model.Id)(args);
                }}
            }} else if (operation == ""refresh"") {{
                this.refresh();
            }} else {{
                var isExport = @JsonConvert.SerializeObject(Model.ActivityOperations.IsExport);
                if (isExport) {{
                    if (operation == ""excelexport"") {{
                        this.excelExport();
                    }} else if (operation == ""pdfexport"") {{
                        this.pdfExport();
                    }}
                }}
            }}
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""toolbarClick_Grid_@(Model.Id)"", exception));
        }}
    }}
</script>

@Html.EJS().ScriptManager()");
            }
        }

        public void Presentation_MVC_PartialView_Item(string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions syncfusion,
            TableSchema SourceTable,
            string filePath)
        {
            if (syncfusion == Syncfusions.EJ1)
            {
                Presentation_MVC_PartialView_Item_EJ1(Application,
                    Namespace,
                    Culture,
                    SourceTable,
                    filePath);
            }
            else
            {
                Presentation_MVC_PartialView_Item_EJ2(Application,
                    Namespace,
                    Culture,
                    SourceTable,
                    filePath);
            }
        }

        private void Presentation_MVC_PartialView_Item_EJ1(string Application,
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

                // !?! Master-Detail Views using relationships with more then 1 property are RARE, so I will use just 1 property !?!
                ColumnSchema keyColumn = SourceTable.PrimaryKey.MemberColumns[0];
                string keyProperty = PropertyName(keyColumn.Name);

                // Associations

                Dictionary<string, int> associations123 = new Dictionary<string, int>();
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    Dictionary123(associations123, pkClassName);
                }

                // Collections

                Dictionary<string, int> collections123 = new Dictionary<string, int>();
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                    Dictionary123(collections123, pkClassName);
                }

                file.WriteLine($@"@model {className}ItemModel

@{{
    string CSHTML = ""_{className}Item.cshtml"";

    IZProfile profile = DataHelper.GetProfile(typeof({className}));
    // Associations (FK)");

                Dictionary1(associations123);
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                    string x = "";
                    if (associations123.ContainsKey(pkClassName))
                    {
                        x = (++associations123[pkClassName]).ToString();
                    }

                    file.WriteLine($@"    string {ObjectName(pkClassName2, Culture)}{x}Text = Model.{className} == null ? """" : (Model.{className}.{pkClassName2}{x}LookupText ?? """");");
                }

                int image = 0;
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    if (IsImage(column))
                    {
                        string propertyName = PropertyName(column.Name);
                        if (image++ > 0)
                        {
                            file.WriteLine();
                        }

                        file.WriteLine($@"    string {objectName}_{propertyName}_Base64 = Convert.ToBase64String(new byte[] {{}});
    if (Model.{className} != null && Model.{className}.{propertyName} != null)
    {{
        {objectName}_{propertyName}_Base64 = Convert.ToBase64String(Model.{className}.{propertyName});
    }}
    string {objectName}_{propertyName}_Source = string.Format(""data:image/jpg;base64,{{0}}"", {objectName}_{propertyName}_Base64);");
                    }
                }

                file.WriteLine($@"}}

<div id=""Item_{className}"">
    @Html.ValidationSummary(false, """", new {{ @class = ""text-danger"" }})

    @Html.HiddenFor(model => model.ControllerAction, new {{ id = ""{className}_Item_ControllerAction"" }})
    @Html.HiddenFor(model => model.MasterEntity, new {{ id = ""{className}_Item_MasterEntity"" }})
    @Html.HiddenFor(model => model.MasterKey, new {{ id = ""{className}_Item_MasterKey"" }})
    @Html.ZHiddenFor(model => model.IsReadOnly, ""{className}_Item_IsReadOnly"")
    @Html.ZHiddenFor(model => model.IsSave, ""{className}_Item_IsSave"")

    @{{Html.EJ().Tab(""Tab_{className}"")
        .ClientSideEvents(clientEvent => clientEvent
            .ItemActive(""itemActive_Tab_{className}"")
        )
        //.EnablePersistence()
        .Items(data =>
        {{
            data.Add().ID(""TabSheet_{className}_{className}"").Text({className}Resources.EntitySingular).ContentTemplate(@<div class=""@AppDefaults.CSSClassPanel"">");

                Dictionary1(associations123);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    string propertyName = PropertyName(column.Name);
                    string cssClassLabel = column.AllowDBNull ? "CssClassLabel" : "CssClassLabelRequired";
                    if (column.IsForeignKeyMember)
                    {
                        string fkClassName = ClassName(FKTableName(SourceTable, column), Culture);

                        //string fkClassName2 = fkClassName;
                        //string fkClassName2 = fkClassName == className ? fkClassName + fkPropertyName : fkClassName;
                        string fkClassName2 = fkClassName == className ? fkClassName + fkClassName : fkClassName;

                        string x = "";
                        if (associations123.ContainsKey(fkClassName))
                        {
                            x = (++associations123[fkClassName]).ToString();
                        }

                        file.WriteLine($@"
                <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{fkClassName2}{x}LookupText"")"">
                    @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                    @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }})
                    @{{
                        Html.RenderAction(""Lookup"", ""{fkClassName}"", new
                        {{
                            Text = {ObjectName(fkClassName2, Culture)}{x}Text,
                            ValueId = ""{className}_{propertyName}"",
                            Required = profile.IsRequiredView(""{propertyName}"")
                        }});
                    }}");
                    }
                    else if (IsDate(column.DataType) || IsDateTime(column.DataType))
                    {
                        file.WriteLine($@"
                <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                    @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                    @* @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }}) *@
                    @Html.EJ().DatePickerFor(model => model.{className}.{propertyName}, AppHelper.DateModel, new {{ @class = profile.EditCSSEditorDateFor(""{propertyName}""), id = ""{className}_{propertyName}"" }})");
                    }
                    else if (IsDateTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"
                <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                    @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                    @* @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }}) *@
                    @Html.EJ().DateTimePickerFor(model => model.{className}.{propertyName}, AppHelper.DateTimeModel, new {{ @class = profile.EditCSSEditorDateTimeFor(""{propertyName}""), id = ""{className}_{propertyName}"" }})");
                    }
                    else if (IsImage(column))
                    {
                        file.WriteLine($@"
                <div id=""Group_{className}_{propertyName}"" class=""@AppDefaults.CSSClassGroupImage col-md-2"">
                    @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                    <img src=""@{objectName}_{propertyName}_Source"" class=""z-image"" />");
                    }
                    else if (IsBoolean(column.DataType))
                    {
                        file.WriteLine($@"
                <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                    @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                    @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }})");
                    }
                    else
                    {
                        file.WriteLine($@"
                <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                    @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                    @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }})");
                    }

                    file.WriteLine($@"                    @* @Html.ValidationMessageFor(model => model.{className}.{propertyName}, """", new {{ @class = AppDefaults.CSSClassValidator }}) *@
                </div>");

                }

                file.WriteLine($@"
            </div>);");

                Dictionary1(collections123);
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);
                    int index = 0;
                    foreach (ColumnSchema column in primaryKey.PrimaryKeyMemberColumns)
                    {
                        string fkPropertyName = PropertyName(column.Name);
                        string pkPropertyName = PropertyName(primaryKey.ForeignKeyMemberColumns[index].Name);

                        string x = "";
                        if (collections123.ContainsKey(pkClassName))
                        {
                            x = (++collections123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"            data.Add().ID(""TabSheet_{className}_{Plural(pkClassName, Culture)}{x}"").Text({pkClassName}Resources.EntityPlural).ContentTemplate(@<div class=""@AppDefaults.CSSClassPanel"">
                <div id=""Ajax_{className}_{Plural(pkClassName, Culture)}{x}""></div>
            </div>);");

                        index++;
                    }
                }

                file.WriteLine($@"        }})
        .Render();
    }}
</div>

<script>
    zSyncfusionItem(""Item_{className}"");

    $(function () {{
        try {{
            zSyncfusionItemReady(""Item_{className}"");

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));
            var controllerAction = model.ControllerAction == null ? """" : model.ControllerAction.toLowerCase();

            // Associations (FK)");

                if (SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine($@"            switch (model.MasterEntity) {{");

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

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"                case ""{pkClassName}{x}"":
                    $(""#{fkClassName}_{fkPropertyName}"").val(model.MasterKey);
                    $(""#Group_{fkClassName}_{fkPropertyName}"").hide();
                    break;");
                    }

                    file.WriteLine($@"            }}");
                }

                file.WriteLine($@"
            // Collections (PK)");

                Dictionary1(collections123);
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);
                    int index = 0;
                    foreach (ColumnSchema column in primaryKey.PrimaryKeyMemberColumns)
                    {
                        string fkPropertyName = PropertyName(column.Name);
                        string pkPropertyName = PropertyName(primaryKey.ForeignKeyMemberColumns[index].Name);

                        string x = "";
                        if (collections123.ContainsKey(pkClassName))
                        {
                            x = (++collections123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"            if (controllerAction != ""create"" && zContains(profile.EditCollections, ""{Plural(pkClassName, Culture)}{x}"")) {{
                zUrlDictionaryWrite(""{pkClassName}"", ""@Context.Request.Url.AbsoluteUri"");
                var ajaxUrl = ""@(Html.Raw(Url.Action(""Search"", ""{pkClassName}"", new {{ MasterControllerAction = Model.ControllerAction, MasterEntity = ""{className}{x}"", MasterKey = Model.{className}.{fkPropertyName} }})))"";
                zAjaxLoadSync(""Ajax_{className}_{Plural(pkClassName, Culture)}{x}"", ajaxUrl);
            }}");

                        index++;
                    }
                }

                file.WriteLine($@"
            zOnItemView(model, profile);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception));
        }}
    }});

    function itemActive_Tab_{className}(args) {{
        zTabDictionaryWrite(""{className}"", args.model.selectedItemIndex);

        try {{");

                if (SourceTable.PrimaryKeys.Count > 0)
                {
                    file.WriteLine($@"            var tabId = this.contentPanels[args.model.selectedItemIndex].id;
            switch (tabId) {{");

                    Dictionary1(collections123);
                    foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                    {
                        string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                        string x = "";
                        if (collections123.ContainsKey(pkClassName))
                        {
                            x = (++collections123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"                case ""TabSheet_{className}_{Plural(pkClassName, Culture)}{x}"":
                    zGridDataSource(""Grid_{pkClassName}{x}"", ""@Url.Action(""DataSource"", ""{pkClassName}"")"");
                    break;");
                    }

                    file.WriteLine($@"            }}");
                }

                file.WriteLine($@"        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""itemActive_Tab_{className}"", exception));
        }}
    }}
</script>");
            }
        }

        private void Presentation_MVC_PartialView_Item_EJ2(string Application,
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

                // !?! Master-Detail Views using relationships with more then 1 property are RARE, so I will use just 1 property !?!
                ColumnSchema keyColumn = SourceTable.PrimaryKey.MemberColumns[0];
                string keyProperty = PropertyName(keyColumn.Name);

                // Associations

                Dictionary<string, int> associations123 = new Dictionary<string, int>();
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    Dictionary123(associations123, pkClassName);
                }

                // Collections

                Dictionary<string, int> collections123 = new Dictionary<string, int>();
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                    Dictionary123(collections123, pkClassName);
                }

                file.WriteLine($@"@model {className}ItemModel

@{{
    string CSHTML = ""_{className}Item.cshtml"";

    IZProfile profile = DataHelper.GetProfile(typeof({className}));
    // Associations (FK)");

                Dictionary1(associations123);
                foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                {
                    string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                    string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                    string x = "";
                    if (associations123.ContainsKey(pkClassName))
                    {
                        x = (++associations123[pkClassName]).ToString();
                    }

                    file.WriteLine($@"    string {ObjectName(pkClassName2, Culture)}{x}Text = Model.{className} == null ? """" : (Model.{className}.{pkClassName2}{x}LookupText ?? """");");
                }

                int image = 0;
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    if (IsImage(column))
                    {
                        string propertyName = PropertyName(column.Name);
                        if (image++ > 0)
                        {
                            file.WriteLine();
                        }

                        file.WriteLine($@"    string {objectName}_{propertyName}_Base64 = Convert.ToBase64String(new byte[] {{}});
    if (Model.{className} != null && Model.{className}.{propertyName} != null)
    {{
        {objectName}_{propertyName}_Base64 = Convert.ToBase64String(Model.{className}.{propertyName});
    }}
    string {objectName}_{propertyName}_Source = string.Format(""data:image/jpg;base64,{{0}}"", {objectName}_{propertyName}_Base64);");
                    }
                }

                file.WriteLine($@"}}

<div id=""Item_{className}"">
    @Html.ValidationSummary(false, """", new {{ @class = ""text-danger"" }})

    @Html.HiddenFor(model => model.ControllerAction, new {{ id = ""{className}_Item_ControllerAction"" }})
    @Html.HiddenFor(model => model.MasterEntity, new {{ id = ""{className}_Item_MasterEntity"" }})
    @Html.HiddenFor(model => model.MasterKey, new {{ id = ""{className}_Item_MasterKey"" }})
    @Html.ZHiddenFor(model => model.IsReadOnly, ""{className}_Item_IsReadOnly"")
    @Html.ZHiddenFor(model => model.IsSave, ""{className}_Item_IsSave"")

    <div id=""Tab_{className}"" class=""@AppDefaults.CSSClassTab"">
        <div class=""e-tab-header"">
            <div>@{className}Resources.EntitySingular</div>");

                Dictionary1(collections123);
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);
                    int index = 0;
                    foreach (ColumnSchema column in primaryKey.PrimaryKeyMemberColumns)
                    {
                        string fkPropertyName = PropertyName(column.Name);
                        string pkPropertyName = PropertyName(primaryKey.ForeignKeyMemberColumns[index].Name);

                        string x = "";
                        if (collections123.ContainsKey(pkClassName))
                        {
                            if (++collections123[pkClassName] > 1)
                            {
                                x = "@*";
                            }
                        }
                        if (x == "@*")
                        {
                            file.WriteLine("        @*");
                        }
                        file.WriteLine($"        <div>@{pkClassName}Resources.EntityPlural</div>");
                        if (x == "@*")
                        {
                            file.WriteLine("        *@");
                        }

                        index++;
                    }
                }

                file.WriteLine($@"        </div>
        <div class=""e-content"">
            <div data-easylob-id=""TabItem_{className}"">
                <div class=""@AppDefaults.CSSClassTabItem"">");

                Dictionary1(associations123);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    string propertyName = PropertyName(column.Name);
                    string cssClassLabel = column.AllowDBNull ? "CssClassLabel" : "CssClassLabelRequired";
                    if (column.IsForeignKeyMember)
                    {
                        string fkClassName = ClassName(FKTableName(SourceTable, column), Culture);

                        //string fkClassName2 = fkClassName;
                        //string fkClassName2 = fkClassName == className ? fkClassName + fkPropertyName : fkClassName;
                        string fkClassName2 = fkClassName == className ? fkClassName + fkClassName : fkClassName;

                        string x = "";
                        if (associations123.ContainsKey(fkClassName))
                        {
                            x = (++associations123[fkClassName]).ToString();
                        }

                        file.WriteLine($@"
                    <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{fkClassName2}LookupText"")"">
                        @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                        @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }})
                        @{{
                            Html.RenderAction(""Lookup"", ""{fkClassName}"", new
                            {{
                                Text = {ObjectName(fkClassName2, Culture)}{x}Text,
                                ValueId = ""{className}_{propertyName}"",
                                Required = profile.IsRequiredView(""{propertyName}"")
                            }});
                        }}");
                    }
                    else if (IsDate(column.DataType) || IsDateTime(column.DataType))
                    {
                        file.WriteLine($@"
                    <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                        @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                        @* @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }}) *@
                        @(Html.EJS().DatePickerFor(model => model.{className}.{propertyName})
                             id = ""{className}_{propertyName}"" }}).Render()
                            .CssClass(profile.EditCSSEditorDateTimeFor(""{propertyName}""))
                            .Format(PatternResources.Format_Date)
                            .HtmlAttributes(new Dictionary<string, object> {{ {{ ""id"", ""{className}_{propertyName}"" }} }})
                            .Locale(AppHelper.CultureLanguage)
                            .Render()
                        )");
                    }
                    else if (IsDateTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"
                    <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                        @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                        @* @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }}) *@
                        @(Html.EJS().DatePickerFor(model => model.{className}.{propertyName})
                             id = ""{className}_{propertyName}"" }}).Render()
                            .CssClass(profile.EditCSSEditorDateTimeFor(""{propertyName}""))
                            .Format(PatternResources.Format_DateTime)
                            .HtmlAttributes(new Dictionary<string, object> {{ {{ ""id"", ""{className}_{propertyName}"" }} }})
                            .Locale(AppHelper.CultureLanguage)
                            .Render()
                        )");
                    }
                    else if (IsImage(column))
                    {
                        file.WriteLine($@"
                    <div id=""Group_{className}_{propertyName}"" class=""@AppDefaults.CSSClassGroupImage col-md-2"">
                        @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                        <img src=""@{objectName}_{propertyName}_Source"" class=""z-image"" />");
                    }
                    else if (IsBoolean(column.DataType))
                    {
                        file.WriteLine($@"
                    <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                        @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                        @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }})");
                    }
                    else
                    {
                        file.WriteLine($@"
                    <div id=""Group_{className}_{propertyName}"" class=""@profile.EditCSSGroupFor(""{propertyName}"")"">
                        @Html.LabelFor(model => model.{className}.{propertyName}, new {{ @class = profile.EditCSSLabelFor(""{propertyName}"") }})
                        @Html.EditorFor(model => model.{className}.{propertyName}, new {{ htmlAttributes = new {{ @class = profile.EditCSSEditorFor(""{propertyName}""), id = ""{className}_{propertyName}"" }} }})");
                    }

                    file.WriteLine($@"                        @* @Html.ValidationMessageFor(model => model.{className}.{propertyName}, """", new {{ @class = AppDefaults.CSSClassValidator }}) *@
                    </div>");

                }

                file.WriteLine($@"
                </div>
            </div>");

                Dictionary1(collections123);
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);
                    int index = 0;
                    foreach (ColumnSchema column in primaryKey.PrimaryKeyMemberColumns)
                    {
                        string fkPropertyName = PropertyName(column.Name);
                        string pkPropertyName = PropertyName(primaryKey.ForeignKeyMemberColumns[index].Name);

                        string x = "";
                        if (collections123.ContainsKey(pkClassName))
                        {
                            x = (++collections123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"            <div data-easylob-id=""TabItem_{className}_{Plural(pkClassName, Culture)}{x}"">
                <div class=""@AppDefaults.CSSClassTabItem"">
                    <div id=""Ajax_{className}_{Plural(pkClassName, Culture)}{x}""></div>
                </div>
            </div>");

                        index++;
                    }
                }

                file.WriteLine($@"        </div>
    </div>
</div>

<script>
    zSyncfusionItem(""Item_{className}"");

    $(function () {{
        try {{
            zSyncfusionItemReady(""Item_{className}"");

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));
            var controllerAction = model.ControllerAction == null ? """" : model.ControllerAction.toLowerCase();

            var ejTab = new ej.navigations.Tab({{
                selected: function (args) {{ selected_Tab_{className}(args); }}
            }});
            ejTab.appendTo(""#Tab_{className}"");

            // Associations (FK)");

                if (SourceTable.ForeignKeys.Count > 0)
                {
                    file.WriteLine($@"            switch (model.MasterEntity) {{");

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

                        string x = "";
                        if (associations123.ContainsKey(pkClassName))
                        {
                            x = (++associations123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"                case ""{pkClassName}{x}"":
                    $(""#{fkClassName}_{fkPropertyName}"").val(model.MasterKey);
                    $(""#Group_{fkClassName}_{fkPropertyName}"").hide();
                    break;");
                    }

                    file.WriteLine($@"            }}");
                }

                file.WriteLine($@"
            // Collections (PK)");

                Dictionary1(collections123);
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);
                    int index = 0;
                    foreach (ColumnSchema column in primaryKey.PrimaryKeyMemberColumns)
                    {
                        string fkPropertyName = PropertyName(column.Name);
                        string pkPropertyName = PropertyName(primaryKey.ForeignKeyMemberColumns[index].Name);

                        string x = "";
                        if (collections123.ContainsKey(pkClassName))
                        {
                            x = (++collections123[pkClassName]).ToString();
                        }

                        file.WriteLine($@"            if (controllerAction != ""create"" && zContains(profile.EditCollections, ""{Plural(pkClassName, Culture)}{x}"")) {{
                zUrlDictionaryWrite(""{pkClassName}"", ""@Context.Request.Url.AbsoluteUri"");
                var ajaxUrl = ""@(Html.Raw(Url.Action(""Search"", ""{pkClassName}"", new {{ MasterControllerAction = Model.ControllerAction, MasterEntity = ""{className}{x}"", MasterKey = Model.{className}.{fkPropertyName} }})))"";
                zAjaxLoadSync(""Ajax_{className}_{Plural(pkClassName, Culture)}{x}"", ajaxUrl);
            }}");

                        index++;
                    }
                }

                file.WriteLine($@"
            zOnItemView(model, profile);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception));
        }}
    }});

    function selected_Tab_{className}(args) {{
        zTabDictionaryWrite(""{className}"", args.selectedIndex);

        try {{
            var tabId = $(args.selectedContent).attr(""data-easylob-id"").replace(""TabItem_{className}_"", """");
            switch (tabId) {{");

                Dictionary1(collections123);
                foreach (TableKeySchema primaryKey in SourceTable.PrimaryKeys)
                {
                    string pkClassName = ClassName(primaryKey.ForeignKeyTable.Name, Culture);

                    string x = "";
                    if (collections123.ContainsKey(pkClassName))
                    {
                        x = (++collections123[pkClassName]).ToString();
                    }

                    file.WriteLine($@"                case ""{Plural(pkClassName, Culture)}{x}"":
                    zGridDataSource(""Grid_{pkClassName}{x}"", ""@Url.Action(""DataSource"", ""{pkClassName}"")"");
                    break;");
                }

                file.WriteLine($@"            }}
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""selected_Tab_{className}"", exception));
        }}
    }}
</script>

@Html.EJS().ScriptManager()");
            }
        }

        public void Presentation_MVC_PartialView_Lookup(string Application,
            string Namespace,
            Cultures Culture,
            Syncfusions syncfusion,
            TableSchema SourceTable,
            string filePath)
        {
            if (syncfusion == Syncfusions.EJ1)
            {
                Presentation_MVC_PartialView_Lookup_EJ1(Application,
                    Namespace,
                    Culture,
                    SourceTable,
                    filePath);
            }
            else
            {
                Presentation_MVC_PartialView_Lookup_EJ2(Application,
                    Namespace,
                    Culture,
                    SourceTable,
                    filePath);
            }
        }

        private void Presentation_MVC_PartialView_Lookup_EJ1(string Application,
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

                string pkUrl = ""; // Property1=String(data.Property1), Property2=String(data.Property2), ...
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    pkUrl += (pkUrl == "" ? "" : " + \"&") + PropertyName(column.Name) + "=\" + String(data." + PropertyName(column.Name) + ")";
                }

                file.WriteLine($@"@model LookupModel

@{{
    string CSHTML = ""_{className}Lookup.cshtml"";

    IZProfile profile = DataHelper.GetProfile(typeof({className}));
}}

<div class=""@AppDefaults.CSSClassLookup"">
    <span class=""input-group-addon z-lookupButton""><img id=""@(Model.ValueId)_LookupButton"" class=""btn z-buttonLookup"" /></span>
    @Html.TextBox(Model.ValueId + ""_LookupText"", Model.Text, new {{ @class = profile.EditCSSLookupEditor(Model.Required) }})
</div>

<div id=""@(Model.ValueId)_LookupModal"" aria-labelledby=""@(Model.ValueId)_LookupLabel"" class=""modal fade col-md-10"" role=""dialog"" tabindex=""-1"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <button aria-label=""@PresentationResources.Cancel"" class=""close"" data-dismiss=""modal"" type=""button""><span aria-hidden=""true"">&times;</span></button>
                <h4 id=""@(Model.ValueId)_LookupLabel"" class=""modal-title"">@{className}Resources.EntityPlural</h4>
            </div>
            <div class=""modal-body"">

                @(Html.EJ().Grid<{className}>(Model.ValueId + ""_LookupGrid"")
                    .Query(Model.Query)
                    .AllowFiltering()
                    .AllowPaging()
                    .AllowReordering()
                    //.AllowResizeToFit()
                    .AllowResizing()
                    //.AllowScrolling()
                    .AllowSearching()
                    .AllowSorting()
                    .AllowTextWrap()
                    .TextWrapSettings(wrap =>
                    {{
                        wrap.WrapMode(WrapMode.Both);
                    }})
                    .ClientSideEvents(clientEvent => clientEvent
                        .ActionFailure(""actionFailure_"" + Model.ValueId + ""_LookupGrid"")
                        .Load(""load_"" + Model.ValueId + ""_LookupGrid"")
                        .RowSelected(""rowSelected_"" + Model.ValueId + ""_LookupGrid"")
                        .ToolbarClick(""toolbarClick_"" + Model.ValueId + ""_LookupGrid"")
                    )
                    .Columns(column =>
                    {{");

                //int visibles = 0;
                //string visible;
                Dictionary1(associations123);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    bool columnIsPrimaryKey = column.IsPrimaryKeyMember;
                    bool columnIsForeignKey = column.IsForeignKeyMember;
                    bool columnIsIdentity = IsIdentity(column);
                    bool columnIsNullable = column.AllowDBNull;

                    //if (columnIsPrimaryKey)
                    //{
                    //    if (SourceTable.PrimaryKey.MemberColumns.Count == SourceTable.Columns.Count)
                    //    {
                    //        visible = "true";
                    //    }
                    //    else
                    //    {
                    //        visible = "false";
                    //    }
                    //}
                    //else if (columnIsForeignKey)
                    //{
                    //    visible = "false";
                    //}
                    //else if (visibles >= 1)
                    //{
                    //    visible = "false";
                    //}
                    //else
                    //{
                    //    visibles++;
                    //    visible = "true";
                    //}

                    file.WriteLine($@"                        column.Field(""{PropertyName(column.Name)}"")");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                            .AllowEditing(false)");
                    }
                    if (IsBoolean(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""boolean"")
                            .EditType(EditingType.BooleanEdit)");
                    }
                    else if (IsDate(column.DataType) || IsTime(column.DataType) || IsDateTime(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""date"")
                            .EditType(EditingType.Datepicker)
                            .Format(SyncfusionPatternResources.GridFormat_Date)");
                    }
                    else if (IsTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"                            .Type(""datetime"")
                            .EditType(EditingType.DateTimePicker)
                            .Format(SyncfusionPatternResources.GridFormat_Time)");
                    }
                    else if (IsDateTime(column.DataType))
                    {
                        // DatabaseSchemaReader does not know the difference between "date" and "datetime"
                        file.WriteLine($@"                            .Type(""datetime"")
                            .EditType(EditingType.DateTimePicker)
                            .Format(SyncfusionPatternResources.GridFormat_DateTime)");
                    }
                    else if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""number"")
                            .EditType(EditingType.NumericEdit)");
                        if (IsDecimal(column.DataType) || IsFloat(column.DataType))
                        {
                            file.WriteLine($@"                            .Format(SyncfusionPatternResources.GridFormat_Float)");
                        }
                        else
                        {
                            file.WriteLine($@"                            .Format(SyncfusionPatternResources.GridFormat_Integer)");
                        }
                    }
                    else
                    {
                        file.WriteLine($@"                            .Type(""string"")
                            .EditType(EditingType.StringEdit)");
                    }

                    file.WriteLine($@"                            .HeaderText({className}Resources.Property{PropertyName(column.Name)})");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                            .IsIdentity(true)");
                    }
                    if (columnIsPrimaryKey)
                    {
                        file.WriteLine($@"                            .IsPrimaryKey(true)");
                    }
                    if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                            .TextAlign(TextAlign.Right)");
                    }

                    file.WriteLine($@"                            .Visible(profile.IsGridVisibleFor(""{PropertyName(column.Name)}""))
                            .Width(profile.GridWidthFor(""{PropertyName(column.Name)}""))
                            .Add();");

                    if (columnIsForeignKey)
                    {
                        foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                        {
                            if (fkTable.ForeignKeyMemberColumns[0].Name == column.Name)
                            {
                                string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                                string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                                string x = "";
                                if (associations123.ContainsKey(pkClassName))
                                {
                                    x = (++associations123[pkClassName]).ToString();
                                }

                                file.WriteLine($@"                        column.Field(""{pkClassName2}{x}LookupText"")
                            .AllowEditing(false)
                            .Type(""string"")
                            .HeaderText({ClassName(fkTable.PrimaryKeyTable.FullName, Culture)}Resources.EntitySingular)
                            .Visible(profile.IsGridVisibleFor(""{pkClassName2}{x}LookupText""))
                            .Width(profile.GridWidthFor(""{pkClassName2}{x}LookupText""))
                            .Add();");
                            }
                        }
                    }
                }

                file.WriteLine($@"                    }})
                    //.EnablePersistence()
                    .EnableTouch(false)
                    //.IsResponsive(true)
                    .FilterSettings(filter => filter
                        .EnableCaseSensitivity(false)
                        .FilterType(FilterType.Excel)
                        .MaxFilterChoices(AppDefaults.SyncfusionRecordsForFiltering)
                    )
                    .PageSettings(page => page
                        .PageSize(AppDefaults.SyncfusionRecordsByPage)
                    )
                    .ShowColumnChooser()
                    .ToolbarSettings(toolbar => toolbar
                        .CustomToolbarItems(new List<object>() {{
                            new Syncfusion.JavaScript.Models.CustomToolbarItem() {{ TemplateID = ""#"" + Model.ValueId + ""_LookupGrid_Toolbar"", Tooltip = """" }}
                        }})
                        .ShowToolbar()
                        .ToolbarItems(items =>
                        {{
                            items.AddTool(ToolBarItems.Search);
                        }})
                    )
                )

                <div class=""z-lookupButtons"">
                    @Html.ZImage(""Button_Clear"", ""btn z-buttonClear"", PresentationResources.Clear, ""clear_"" + Model.ValueId + ""()"")
                    @Html.ZImage(""Button_Create"", ""btn z-buttonCreate"", PresentationResources.Create, ""create_"" + Model.ValueId + ""()"")
                </div>

            </div>
        </div>
    </div>
</div>

<script type=""text/x-jsrender"" id=""@(Model.ValueId)_LookupGrid_Toolbar"">
    <div id=""@(Model.ValueId)_LookupGrid_Toolbar_Refresh"" class=""e-toolbaricons e-icon e-reload"" title=""@PresentationResources.Refresh""></div>
</script>

<script>
    zSyncfusionLookup(""@(Model.ValueId)_LookupModal"");

    $(function () {{
        try {{
            zSyncfusionLookupReady(""@(Model.ValueId)_LookupModal"");

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));

            $(""#@(Model.ValueId)"").hide();

            $(""#@(Model.ValueId)_LookupButton"").click(function () {{
                zGridDataSource(""@(Model.ValueId)_LookupGrid"", ""@Url.Action(""DataSource"", ""{className}"")"");

                $(""#@(Model.ValueId)_LookupGrid_Toolbar"").removeAttr(""data-content"");

                $(""#@(Model.ValueId)_LookupModal"").modal(""show"");
            }});

            $(""#@(Model.ValueId)_LookupText"").prop(""readonly"", true);
            $(""#@(Model.ValueId)_LookupText"").val(model.Text.toLocaleString(""@CultureInfo.CurrentCulture.Name""));

            $(""#@(Model.ValueId)_LookupModal"").modal(""hide"");

            $(""#@(Model.ValueId)_LookupGrid_Toolbar"").removeAttr(""data-content"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception));
        }}
    }});

    function actionFailure_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            zAlert(args.error.responseText);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""actionFailure_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}

    function clear_@(Model.ValueId)() {{
        try {{
            $(""#@(Model.ValueId)"").val("""").change();

            $(""#@(Model.ValueId)_LookupText"").val("""").change();

            $(""#@(Model.ValueId)_LookupModal"").modal(""hide"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""clear_@(Model.ValueId)"", exception));
        }}
    }}

    function create_@(Model.ValueId)() {{
        try {{
            window.open(""@(Url.Action(""Index"", ""{className}"", new {{ Operation = ""create"" }}, Request.Url.Scheme))"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""create_@(Model.ValueId)"", exception));
        }}
    }}

    function load_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            var culture = ""@System.Globalization.CultureInfo.CurrentCulture.Name"";
            this.model.locale = culture;

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));
            zOnLookupView(model, profile);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""load_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}

    function rowSelected_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            var data = this.model.currentViewData[args.rowIndex];

            $(""#@(Model.ValueId)"").val(data.{PropertyName(SourceTable.PrimaryKey.MemberColumns[0].Name)}).change();

            $(""#@(Model.ValueId)_LookupText"").val(data.LookupText).change();

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var culture = ""@System.Globalization.CultureInfo.CurrentCulture.Name"";
            zLookupElements(data, model.Elements, culture);

            $(""#@(Model.ValueId)_LookupModal"").modal(""hide"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""rowSelected_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}

    function toolbarClick_@(Model.ValueId)_LookupGrid(sender) {{
        try {{
            var toolbar = $(sender.target);
            var ejGrid = zGrid(""@(Model.ValueId)_LookupGrid"");

            if (toolbar.prop(""id"") == ""@(Model.ValueId)_LookupGrid_Toolbar_Refresh"") {{
                ejGrid.refreshContent();
            }}
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""toolbarClick_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}
</script>");
            }
        }

        private void Presentation_MVC_PartialView_Lookup_EJ2(string Application,
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

                string pkUrl = ""; // Property1=String(data.Property1), Property2=String(data.Property2), ...
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    pkUrl += (pkUrl == "" ? "" : " + \"&") + PropertyName(column.Name) + "=\" + String(data." + PropertyName(column.Name) + ")";
                }

                file.WriteLine($@"@model LookupModel

@{{
    string CSHTML = ""_{className}Lookup.cshtml"";

    IZProfile profile = DataHelper.GetProfile(typeof({className}));

    string query = string.IsNullOrEmpty(Model.Query) ? ""null"" : Model.Query;

    List<object> toolbar = new List<object>();
    toolbar.Add(new {{ id = Model.ValueId + ""_LookupGrid_Refresh"", text = """", tooltipText = PresentationResources.Refresh, prefixIcon = ""e-easylob-refresh"" }});
}}

<div class=""@AppDefaults.CSSClassLookup"">
    <span class=""input-group-prepend z-lookupButton"">
        <span class=""input-group-text"">
            <img id=""@(Model.ValueId)_LookupButton"" class=""btn z-buttonLookup"" />
        </span>
    </span>
    @Html.TextBox(Model.ValueId + ""_LookupText"", Model.Text, new {{ @class = profile.EditCSSLookupEditor(Model.Required) }})
</div>

<div id=""@(Model.ValueId)_LookupModal"" aria-labelledby=""@(Model.ValueId)_LookupLabel"" class=""modal fade"" data-backdrop=""false"" role=""dialog"" tabindex=""-1"">
    <div class=""modal-dialog modal-lg"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">

                <h4 id=""@(Model.ValueId)_LookupLabel"" class=""modal-title"">@{className}Resources.EntityPlural</h4>
                <button aria-label=""@PresentationResources.Cancel"" class=""close"" data-dismiss=""modal"" type=""button""><span aria-hidden=""true"">&times;</span></button>

            </div>
            <div class=""modal-body"">

                @(Html.EJS().Grid(Model.ValueId + ""_LookupGrid"")
                    .AllowFiltering()
                    .AllowPaging()
                    .AllowReordering()
                    .AllowResizing()
                    .AllowSorting()
                    .AllowTextWrap()
                    .Locale(AppHelper.CultureLanguage)
                    .Query(query)
                    .ShowColumnChooser(true)
                    .ActionBegin(""actionBegin_"" + Model.ValueId + ""_LookupGrid"")
                    .ActionFailure(""actionFailure_"" + Model.ValueId + ""_LookupGrid"")
                    .Load(""load_"" + Model.ValueId + ""_LookupGrid"")
                    .RowSelected(""rowSelected_"" + Model.ValueId + ""_LookupGrid"")
                    .ToolbarClick(""toolbarClick_"" + Model.ValueId + ""_LookupGrid"")
                    .Columns(column =>
                    {{");

                //int visibles = 0;
                //string visible;
                Dictionary1(associations123);
                foreach (ColumnSchema column in SourceTable.Columns)
                {
                    bool columnIsPrimaryKey = column.IsPrimaryKeyMember;
                    bool columnIsForeignKey = column.IsForeignKeyMember;
                    bool columnIsIdentity = IsIdentity(column);
                    bool columnIsNullable = column.AllowDBNull;

                    //if (columnIsPrimaryKey)
                    //{
                    //    if (SourceTable.PrimaryKey.MemberColumns.Count == SourceTable.Columns.Count)
                    //    {
                    //        visible = "true";
                    //    }
                    //    else
                    //    {
                    //        visible = "false";
                    //    }
                    //}
                    //else if (columnIsForeignKey)
                    //{
                    //    visible = "false";
                    //}
                    //else if (visibles >= 1)
                    //{
                    //    visible = "false";
                    //}
                    //else
                    //{
                    //    visibles++;
                    //    visible = "true";
                    //}

                    file.WriteLine($@"                        column.Field(""{PropertyName(column.Name)}"")");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                            .AllowEditing(false)");
                    }
                    if (IsBoolean(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""boolean"")
                            .EditType(""booleanEdit"")");
                    }
                    else if (IsDate(column.DataType) || IsTime(column.DataType) || IsDateTime(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""date"")
                            .EditType(""datePickerEdit"")
                            .Format(SyncfusionPatternResources.GridFormat_Date)");
                    }
                    else if (IsTime(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""datetime"")
                            .EditType(""dateTimePickerEdit"")
                            .Format(SyncfusionPatternResources.GridFormat_Time)");
                    }
                    else if (IsDateTime(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""datetime"")
                            .EditType(""dateTimePickerEdit"")
                            .Format(SyncfusionPatternResources.GridFormat_DateTime)");
                    }
                    else if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                            .Type(""number"")
                            .EditType(""numericEdit"")");
                        if (IsDecimal(column.DataType) || IsFloat(column.DataType))
                        {
                            file.WriteLine($@"                            .Format(SyncfusionPatternResources.GridFormat_Float)");
                        }
                        else
                        {
                            file.WriteLine($@"                            .Format(SyncfusionPatternResources.GridFormat_Integer)");
                        }
                    }
                    else
                    {
                        file.WriteLine($@"                            .Type(""string"")
                            .EditType(""stringEdit"")");
                    }

                    file.WriteLine($@"                            .HeaderText({className}Resources.Property{PropertyName(column.Name)})");

                    if (columnIsIdentity)
                    {
                        file.WriteLine($@"                            .IsIdentity(true)");
                    }
                    if (columnIsPrimaryKey)
                    {
                        file.WriteLine($@"                            .IsPrimaryKey(true)");
                    }
                    if (IsDecimal(column.DataType) || IsFloat(column.DataType) || IsInteger(column.DataType))
                    {
                        file.WriteLine($@"                            .TextAlign(TextAlign.Right)");
                    }

                    file.WriteLine($@"                            .Visible(profile.IsGridVisibleFor(""{PropertyName(column.Name)}""))
                            .Width(profile.GridWidthFor(""{PropertyName(column.Name)}""))
                            .Add();");

                    if (columnIsForeignKey)
                    {
                        foreach (TableKeySchema fkTable in SourceTable.ForeignKeys)
                        {
                            if (fkTable.ForeignKeyMemberColumns[0].Name == column.Name)
                            {
                                string pkClassName = ClassName(fkTable.PrimaryKeyTable.FullName, Culture);

                                string pkClassName2 = pkClassName == className ? pkClassName + "X" : pkClassName;

                                string x = "";
                                if (associations123.ContainsKey(pkClassName))
                                {
                                    x = (++associations123[pkClassName]).ToString();
                                }

                                file.WriteLine($@"                        column.Field(""{pkClassName2}{x}LookupText"")
                            .AllowEditing(false)
                            .Type(""string"")
                            .HeaderText({ClassName(fkTable.PrimaryKeyTable.FullName, Culture)}Resources.EntitySingular)
                            .Visible(profile.IsGridVisibleFor(""{pkClassName2}{x}LookupText""))
                            .Width(profile.GridWidthFor(""{pkClassName2}{x}LookupText""))
                            .Add();");
                            }
                        }
                    }
                }

                file.WriteLine($@"                    }})
                    .FilterSettings(filter => filter
                        .EnableCaseSensitivity(false)
                        .Type(FilterType.Excel)
                    )
                    .PageSettings(page => page
                        .PageSize(AppDefaults.SyncfusionRecordsByPage)
                    )
                    .TextWrapSettings(wrap => wrap
                        .WrapMode(WrapMode.Content)
                    )
                    .Render()
                )

                <div class=""z-lookupButtons"">
                    @Html.ZImage(""Button_Clear"", ""btn z-buttonClear"", PresentationResources.Clear, ""clear_"" + Model.ValueId + ""()"")
                    @Html.ZImage(""Button_Create"", ""btn z-buttonCreate"", PresentationResources.Create, ""create_"" + Model.ValueId + ""()"")
                </div>

            </div>
        </div>
    </div>
</div>

<script>
    zSyncfusionLookup(""@(Model.ValueId)_LookupModal"");

    $(function () {{
        try {{
            zSyncfusionLookupReady(""@(Model.ValueId)_LookupModal"");

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));

            $(""#@(Model.ValueId)"").hide();

            $(""#@(Model.ValueId)_LookupButton"").click(function () {{
                zGridDataSource(""@(Model.ValueId)_LookupGrid"", ""@Url.Action(""DataSource"", ""{className}"")"");

                $(""#@(Model.ValueId)_LookupGrid_Toolbar"").removeAttr(""data-content"");

                $(""#@(Model.ValueId)_LookupModal"").modal(""show"");
            }});

            $(""#@(Model.ValueId)_LookupText"").prop(""readonly"", true);
            $(""#@(Model.ValueId)_LookupText"").val(model.Text.toLocaleString(""@CultureInfo.CurrentCulture.Name""));

            $(""#@(Model.ValueId)_LookupModal"").modal(""hide"");

            $(""#@(Model.ValueId)_LookupGrid_Toolbar"").removeAttr(""data-content"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""function"", exception));
        }}
    }});

    function actionBegin_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            switch (args.requestType) {{
                case ""filterchoicerequest"":
                    args.filterChoiceCount = @AppDefaults.SyncfusionRecordsForFiltering; 
                    break;
            }}
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""actionBegin_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}

    function actionFailure_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            zAlert(args.error.error.responseText);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""actionFailure_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}

    function clear_@(Model.ValueId)() {{
        try {{
            $(""#@(Model.ValueId)"").val("""").change();

            $(""#@(Model.ValueId)_LookupText"").val("""").change();

            $(""#@(Model.ValueId)_LookupModal"").modal(""hide"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""clear_@(Model.ValueId)"", exception));
        }}
    }}

    function create_@(Model.ValueId)() {{
        try {{
            window.open(""@(Url.Action(""Index"", ""{className}"", new {{ Operation = ""create"" }}, Request.Url.Scheme))"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""create_@(Model.ValueId)"", exception));
        }}
    }}

    function load_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var profile = @Html.Raw(JsonConvert.SerializeObject(profile));
            zOnLookupView(model, profile);
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""load_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}

    function rowSelected_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            var data = this.model.currentViewData[args.rowIndex];

            $(""#@(Model.ValueId)"").val(data.{PropertyName(SourceTable.PrimaryKey.MemberColumns[0].Name)}).change();

            $(""#@(Model.ValueId)_LookupText"").val(data.LookupText).change();

            var model = @Html.Raw(JsonConvert.SerializeObject(Model));
            var culture = ""@System.Globalization.CultureInfo.CurrentCulture.Name"";
            zLookupElements(data, model.Elements, culture);

            $(""#@(Model.ValueId)_LookupModal"").modal(""hide"");
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""rowSelected_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}

    function toolbarClick_@(Model.ValueId)_LookupGrid(args) {{
        try {{
            var operation = args.item.id.replace(@Model.ValueId + ""_LookupGrid"", """").toLowerCase();

            if (operation == ""refresh"") {{
                this.refresh();
            }}
        }} catch (exception) {{
            zAlert(zExceptionMessage(""@CSHTML"", ""toolbarClick_@(Model.ValueId)_LookupGrid"", exception));
        }}
    }}
</script>

@Html.EJS().ScriptManager()");
            }
        }

        #endregion Presentation View

        #region Presentation Component .NET Core

        public void Presentation_MVC_Component_Lookup_Controller(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath)
        {
        }

        public void Presentation_MVC_Component_Lookup_View(string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath)
        {
        }

        #endregion Presentation Component .NET Core

        public void Presentation_MVC_Menu(string Application,
            string Namespace,
            Cultures Culture,
            List<TableSchema> SourceTables,
            string filePath)
        {
            using (StreamWriter file = CreateStreamWriter(filePath))
            {
                file.WriteLine(@"[
  {
    ""Text"": ""MenuResources.Application"",
    ""Url"": ""/Home""
  },
  {
    ""Text"": ""MenuResources.Application"",
    ""SubMenus"": [");

                int tables = SourceTables.Count;
                int t = 0;
                foreach (TableSchema table in SourceTables)
                {
                    t++;

                    file.WriteLine(@"      {
        ""Text"": """ + ClassName(table.Name, Culture) + @"Resources.EntityPlural"",
        ""Url"": ""/" + ClassName(table.Name, Culture) + @"""
      }" + (t < tables ? "," : ""));
                }

                file.WriteLine(@"    ]
  },
  {
    ""Text"": ""MenuResources.Administration"",
    ""SubMenus"": [
      {
        ""Text"": ""SecurityResources.Security"",
        ""SubMenus"": [
          {
            ""Text"": ""SecurityResources.Authentication"",
            ""SubMenus"": [
              {
                ""Text"": ""RoleResources.EntityPlural"",
                ""Url"": ""/Role""
              },
              {
                ""Text"": ""UserResources.EntityPlural"",
                ""Url"": ""/User""
              },
              {
                ""Text"": ""UserRoleResources.EntityPlural"",
                ""Url"": ""/UserRole""
              }
            ]
          },
          {
            ""Text"": ""SecurityResources.Authorization"",
            ""SubMenus"": [
              {
                ""Text"": ""ActivityResources.EntityPlural"",
                ""Url"": ""/Activity""
              },
              {
                ""Text"": ""ActivityRoleResources.EntityPlural"",
                ""Url"": ""/ActivityRole""
              }
            ]
          },
          {
            ""Text"": ""AuditTrailResources.AuditTrail"",
            ""SubMenus"": [
              {
                ""Text"": ""AuditTrailConfigurationResources.EntityPlural"",
                ""Url"": ""/AuditTrailConfiguration""
              },
              {
                ""Text"": ""AuditTrailLogResources.EntityPlural"",
                ""Url"": ""/AuditTrailLog""
              }
            ]
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskExportSecurity"",
            ""Url"": ""/Tasks/ExportSecurity""
          }
        ]
      }
    ]
  },
  {
    ""Text"": ""MenuResources.System"",
    ""SubMenus"": [
      {
        ""Text"": ""PresentationResources.Language"",
        ""SubMenus"": [
          {
            ""Text"": ""English {en-US}"",
            ""Url"": ""/Globalization/Culture?language=en&locale=US""
          },
          {
            ""Text"": ""Português {pt-BR}"",
            ""Url"": ""/Globalization/Culture?language=pt&locale=BR""
          }
        ]
      },
      {
        ""Text"": ""PresentationResources.Tasks"",
        ""SubMenus"": [
          {
            ""Text"": ""MenuResources.Application"",
            ""SubMenus"": [
              {
                ""Text"": ""EasyLOBPresentationResources.TaskApplicationAPI"",
                ""Url"": ""/" + Application + @"Tasks/API""
              },
              {
                ""Text"": ""EasyLOBPresentationResources.TaskApplicationHelp"",
                ""Url"": ""/" + Application + @"Tasks/Help""
              },
              {
                ""Text"": ""EasyLOBPresentationResources.TaskApplicationStatus"",
                ""Url"": ""/" + Application + @"Tasks/Status""
              }
            ]
          },
          {
            ""Text"": ""EasyLOB"",
            ""SubMenus"": [
              {
                ""Text"": ""EasyLOBPresentationResources.TaskGlobalization"",
                ""Url"": ""/Tasks/Globalization""
              },
              {
                ""Text"": ""Operation Result FALSE"",
                ""Url"": ""/Tasks/OperationResultFalse""
              },
              {
                ""Text"": ""Operation Result TRUE"",
                ""Url"": ""/Tasks/OperationResultTrue""
              }
            ]
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskAPI"",
            ""Url"": ""/Tasks/API""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskCleanExportImport"",
            ""Url"": ""/Tasks/CleanExportImport""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskCleanLocalStorage"",
            ""Url"": ""/Tasks/CleanLocalStorage""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskExportAutoMapper"",
            ""Url"": ""/Tasks/ExportAutoMapper""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskExportProfile"",
            ""Url"": ""/Tasks/ExportProfile""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskHelp"",
            ""Url"": ""/Tasks/Help""
          },
          {
            ""Text"": ""NLog"",
            ""Url"": ""/Tasks/NLog""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskProfileViewer"",
            ""Url"": ""/Tasks/ProfileViewer""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskStatus"",
            ""Url"": ""/Tasks/Status""
          },
          {
            ""Text"": ""EasyLOBPresentationResources.TaskUrlDictionary"",
            ""Url"": ""/Tasks/UrlDictionary""
          }
        ]
      },
      {
        ""Text"": ""EasyLOBSecurityResources.ChangePassword"",
        ""Url"": ""/Security/ChangePassword""
      },
      {
        ""Text"": ""EasyLOBSecurityResources.LogOut"",
        ""Url"": ""/Security/Logout""
      }
    ]
  }
]");
            }
        }
    }
}
