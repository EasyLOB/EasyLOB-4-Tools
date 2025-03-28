﻿using System.Collections.Generic;
using System.IO;

// Service_WebAPI_Controller_Application_DTO

namespace Generator
{
    public partial class GeneratorManagerFramework
    {
        #region Presentation Controller

        public void Service_WebAPI_Controller(Archetypes archetype,
            string Application,
            string Namespace,
            Cultures Culture,
            TableSchema SourceTable,
            string filePath)
        {
            if (archetype == Archetypes.ApplicationDTO)
            {
                Service_WebAPI_Controller_Application_DTO(Application,
                    Namespace,
                    Culture,
                    SourceTable,
                    filePath);
            }
        }

        private void Service_WebAPI_Controller_Application_DTO(string Application,
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

                string classNamePlural = Plural(className, Culture);
                string objectNamePlural = Plural(objectName, Culture);

                string pkParameters = ""; // int? parameter1, string parameter2, ...
                string pkParametersArray = ""; // parameter1, parameter2, ...
                string pkParametersUrl = ""; // {parameter1}/{parameter2}/ ...
                string pkParametersNotEqual = ""; // parameters1 != Class.Property1 || parameters2 != Class.Property2 || ...
                string pkPropertiesArray = ""; // new object[] { object.Class.Property1, object.Property2, ... };
                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    pkParameters += (pkParameters == "" ? "" : ", ") + "[FromUri] " + GetType(column, false) + " " + LocalName(column.Name);
                    pkParametersArray += (pkParametersArray == "" ? "" : ", ") + LocalName(column.Name);
                    pkParametersUrl += (pkParametersUrl == "" ? "" : "/") + "{" + LocalName(column.Name) + "}";
                    pkParametersNotEqual += (pkParametersNotEqual == "" ? "" : " || ") + LocalName(PropertyName(column.Name)) + " == null";
                    pkPropertiesArray += (pkPropertiesArray == "" ? "" : ", ") + objectName + "DTO." + PropertyName(column.Name);
                }

                // int categoryId
                // categoryId
                // {categoryId}
                // categoryId == null
                // categoryDTO.CategoryId

                // string customerId, string customerTypeId
                // customerId, customerTypeId
                // {customerId}/{customerTypeId}
                // customerId == null || customerTypeId == null
                // customerCustomerDemoDTO.CustomerId, customerCustomerDemoDTO.CustomerTypeId

                file.WriteLine($@"using {Application}.Application;
using {Application}.Data;
using {Application}.Data.Resources;
using EasyLOB;
using EasyLOB.WebApi;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace {Namespace}
{{
    [RoutePrefix(""api/{className}"")]
    public class {className}APIController : BaseApiControllerApplicationDTO<{className}DTO, {className}>
    {{
        #region Methods

        public {className}APIController(I{Application}GenericApplicationDTO<{className}DTO, {className}> application,
            IAuthorizationManager authorizationManager)
            : base(authorizationManager)
        {{
            Application = application;
        }}

        #endregion Methods

        #region Methods CRUD

        /// <summary>
        /// DELETE: api/{className}/1 
        /// </summary>");

                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    file.WriteLine($@"        /// <param name=""{LocalName(column.Name)}"">{LocalName(column.Name)}</param>");
                }

                file.WriteLine($@"        /// <returns>object[] Ids</returns>
        [Route(""{pkParametersUrl}"")]
        public IHttpActionResult Delete{className}({pkParameters})
        {{
            ZOperationResult operationResult = new ZOperationResult();

            try
            {{
                if (IsDelete(operationResult))
                {{
                    object[] ids = new object[] {{ {pkParametersArray} }};
                    {className}DTO {objectName}DTO = Application.GetById(operationResult, ids);
                    if (operationResult.Ok)
                    {{
                        if ({objectName}DTO != null)
                        {{
                            if (Application.Delete(operationResult, {objectName}DTO))
                            {{
                                return Ok(ids);
                            }}
                        }}
                        else
                        {{
                            for (int i = 0; i < ids.Length; i++)
                            {{
                                ids[i] = null;
                            }}

                            return Ok(ids);
                        }}
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                operationResult.ParseException(exception);
            }}

            return ActionResultOperationResult(operationResult);
        }}

        /// <summary>
        /// GET: api/{className}/1
        /// </summary>");

                foreach (ColumnSchema column in SourceTable.PrimaryKey.MemberColumns)
                {
                    file.WriteLine($@"        /// <param name=""{LocalName(column.Name)}"">{LocalName(column.Name)}</param>");
                }

                file.WriteLine($@"        /// <returns>{className}DTO</returns>
        [Route(""{pkParametersUrl}"")]
        public IHttpActionResult Get{className}({pkParameters})
        {{
            ZOperationResult operationResult = new ZOperationResult();

            try
            {{
                if (IsSearch(operationResult))
                {{
                    object[] ids = new object[] {{ {pkParametersArray} }};
                    {className}DTO {objectName}DTO = Application.GetById(operationResult, ids);
                    if (operationResult.Ok)
                    {{
                        if ({objectName}DTO != null)
                        {{
                            return Ok({objectName}DTO);
                        }}
                        else
                        {{
                            return Ok((object)null);
                        }}
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                operationResult.ParseException(exception);
            }}

            return ActionResultOperationResult(operationResult);
        }}

        /// <summary>
        /// GET: api/{className}/Where=""null""/OrderBy=""null""/Skip=0/Take=100
        /// </summary>
        /// <param name=""where"">WHERE</param>
        /// <param name=""orderBy"">ORDER BY</param>
        /// <param name=""skip"">SKIP</param>
        /// <param name=""take"">TAKE</param>
        /// <returns>List[{className}DTO]</returns>
        [Route(""{{where}}/{{orderBy}}/{{skip}}/{{take}}"")]
        public IHttpActionResult Get{classNamePlural}([FromUri] string where = null,
            [FromUri] string orderBy = null,
            [FromUri] int? skip = null,
            [FromUri] int? take = null)
        {{
            ZOperationResult operationResult = new ZOperationResult();

            try
            {{
                if (IsSearch(operationResult))
                {{
                    where = string.IsNullOrEmpty(where) || where.ToLower() == ""null"" ? null : where;
                    orderBy = string.IsNullOrEmpty(orderBy) || orderBy.ToLower() == ""null"" ? null : orderBy;

                    IEnumerable<{className}DTO> result = Application.Search(operationResult,
                        where, null, orderBy, skip, take ?? AppDefaults.SyncfusionRecordsBySearch);
                    if (operationResult.Ok)
                    {{
                        return Ok(result);
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                operationResult.ParseException(exception);
            }}

            return ActionResultOperationResult(operationResult);
        }}

        /// <summary>
        /// POST: api/{className}
        /// </summary>
        /// <param name=""{objectName}DTO"">{className}DTO</param>
        /// <returns>object[] Ids</returns>
        [Route("""")]
        public IHttpActionResult Post{className}([FromBody] {className}DTO {objectName}DTO)
        {{
            ZOperationResult operationResult = new ZOperationResult();

            try
            {{
                if (IsCreate(operationResult))
                {{
                    if (Application.Create(operationResult, {objectName}DTO))
                    {{
                        return Ok({objectName}DTO.ToData().GetId());
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                operationResult.ParseException(exception);
            }}

            return ActionResultOperationResult(operationResult);
        }}

        /// <summary>
        /// PUT: api/{className}
        /// </summary>
        /// <param name=""{objectName}DTO"">{className}DTO</param>
        /// <returns>object[] Ids</returns>
        [Route("""")]
        public IHttpActionResult Put{className}([FromBody] {className}DTO {objectName}DTO)
        {{
            ZOperationResult operationResult = new ZOperationResult();

            try
            {{
                if (IsUpdate(operationResult))
                {{
                    object[] ids = {objectName}DTO.ToData().GetId();
                    {className}DTO dto = Application.GetById(operationResult, ids);
                    if (operationResult.Ok)
                    {{
                        if (dto != null)
                        {{
                            if (Application.Update(operationResult, {objectName}DTO))
                            {{
                                return Ok(ids);
                            }}
                        }}
                        else
                        {{
                            for (int i = 0; i < ids.Length; i++)
                            {{
                                ids[i] = null;
                            }}

                            return Ok(ids);
                        }}
                    }}
                }}
            }}
            catch (Exception exception)
            {{
                operationResult.ParseException(exception);
            }}

            return ActionResultOperationResult(operationResult);
        }}

        #endregion Methods REST
    }}
}}");
            }
        }

        #endregion Presentation Controller
    }
}
