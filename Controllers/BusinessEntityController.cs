using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GARVIKService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace GARVIKService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessEntityController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<BusinessEntity> BusinessEntityList = new List<BusinessEntity>();
        private readonly List<BusinessEntityDoc> BusinessEntityDocList = new List<BusinessEntityDoc>();
        private readonly List<BusinessTypeDoc> BusinessTypeDocList = new List<BusinessTypeDoc>();

        private readonly List<BusinessType> BusinessTypeList = new List<BusinessType>();

        public BusinessEntityController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }

        [HttpGet]
        public List<BusinessEntity> Get()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadBusinessEntity_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                   
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        BusinessEntityDoc[] docs = JsonConvert.DeserializeObject<BusinessEntityDoc[]>(row["businessentitydocs"].ToString());
                        BusinessType[] btypes = JsonConvert.DeserializeObject<BusinessType[]>(row["businesstypes"].ToString());



                        BusinessEntity obj = new BusinessEntity();
                        obj.businessentitycode = row["businessentitycode"].ToString();
                        obj.businessentityname = row["businessentityname"].ToString();
                        obj.businessentitydescription = row["businessentitydescription"].ToString();
                        obj.businessentitydocs = docs;
                        obj.businesstypes = btypes;
                        BusinessEntityList.Add(obj);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;


                }
                finally
                {
                    objConn.Close();
                }
            }
            return BusinessEntityList;
        }

        // POST api/<BusinessEntitysController>
        [Route("[action]")]
        [HttpPost]
        
        public ActionResult CreateBusinessEntity([FromBody] BusinessEntity obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertBusinessEntity_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@businessentitycode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String businessentitycode = "";

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businessentityname", obj.businessentityname);
                    objCmd.Parameters.AddWithValue("@businessentitydescription", obj.businessentitydescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    businessentitycode = parm.Value.ToString();
                    obj.businessentitycode = businessentitycode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "BusinessEntity Inserted Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (Exception ex)
                {
                    /*return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                    throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        
        [Route("[action]")]
        [HttpPut]
         
        public ActionResult UpdateBusinessEntity([FromBody] BusinessEntity obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_UpdateBusinessEntity_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businessentitycode", obj.businessentitycode);
                    objCmd.Parameters.AddWithValue("@businessentityname", obj.businessentityname);
                    objCmd.Parameters.AddWithValue("@businessentitydescription", obj.businessentitydescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "BusinessEntity Updated Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    /* return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                     throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPut]
         
        public ActionResult DeleteBusinessEntity([FromBody] BusinessEntity obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteBusinessEntity_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@businessentitycode", obj.businessentitycode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "BusinessEntity Deleted Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }

        }


        [Route("[action]")]
        [HttpPost]
        public ActionResult BusinessEntityToBusinessType([FromBody] BusinessEntity obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertbusinessEntityTobusinessType_Map";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    List<IDictionary<string, string>> dicDocs = new List<IDictionary<string, string>>();
                    foreach (BusinessType objA in obj.businesstypes)
                    {
                        dicDocs.Add(BusinessType.ConverttoJson(objA));

                    }
                    string json = JsonConvert.SerializeObject(dicDocs, Formatting.Indented);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businessentitycode", obj.businessentitycode);
                    objCmd.Parameters.AddWithValue("@json", json);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "BusinessTypes Mapped to BusinessEntity Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    /* return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                     throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPost]
        public ActionResult BusinessEntityToDoc([FromBody] BusinessEntity obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertbusinessEntityToDoc_Map";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    List<IDictionary<string, string>> dicDocs = new List<IDictionary<string, string>>();
                    foreach (BusinessEntityDoc objA in obj.businessentitydocs)
                    {
                        dicDocs.Add(BusinessEntityDoc.ConverttoJson(objA));

                    }
                    string json = JsonConvert.SerializeObject(dicDocs, Formatting.Indented);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businessentitycode", obj.businessentitycode);
                    objCmd.Parameters.AddWithValue("@json", json);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Docs Mapped to BusinessEntity Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    /* return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                     throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }


        [Route("[action]")]
        [HttpGet]
        public List<BusinessEntityDoc> GetBusinessEntityDoc()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadBusinessEntityDoc_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        BusinessEntityDoc obj = new BusinessEntityDoc();
                        obj.businessentitydoccode = row["businessentitydoccode"].ToString();
                        obj.businessentitydocname = row["businessentitydocname"].ToString();
                        obj.businessentitydocdescription = row["businessentitydocdescription"].ToString();
                        BusinessEntityDocList.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    objConn.Close();
                }
            }
            return BusinessEntityDocList;
        }

        [Route("[action]")]
        [HttpPost]
         
        public ActionResult CreateBusinessEntityDoc([FromBody] BusinessEntityDoc obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertBusinessEntityDoc_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@businessentitydoccode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String businessentitycode = "";

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businessentitydocname", obj.businessentitydocname);
                    objCmd.Parameters.AddWithValue("@businessentitydocdescription", obj.businessentitydocdescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    businessentitycode = parm.Value.ToString();
                    obj.businessentitydoccode = businessentitycode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "BusinessEntityDoc Inserted Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (Exception ex)
                {
                    /*return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                    throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        [Route("[action]")]
        [HttpPut]
         
        public ActionResult UpdateBusinessEntityDoc([FromBody] BusinessEntityDoc obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_UpdateBusinessEntityDoc_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businessentitydoccode", obj.businessentitydoccode);
                    objCmd.Parameters.AddWithValue("@businessentitydocname", obj.businessentitydocname);
                    objCmd.Parameters.AddWithValue("@businessentitydocdescription", obj.businessentitydocdescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "BusinessEntityDoc Updated Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    /* return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                     throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPut]
         
        public ActionResult DeleteBusinessEntityDoc([FromBody] BusinessEntityDoc obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteBusinessEntityDoc_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@businessentitydoccode", obj.businessentitydoccode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "BusinessEntityDoc Deleted Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }

        }

        //BUSINESSTYPE
        [Route("[action]")]
        [HttpGet]
        public List<BusinessType> GetBusinessType()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadBusinessType_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        BusinessTypeDoc[] docs = JsonConvert.DeserializeObject<BusinessTypeDoc[]>(row["businesstypedocs"].ToString());


                        BusinessType obj = new BusinessType();
                        obj.businesstypecode = row["businesstypecode"].ToString();
                        obj.businesstypename = row["businesstypename"].ToString();
                        obj.businesstypedescription = row["businesstypedescription"].ToString();
                        obj.businesstypedocs = docs;
                        BusinessTypeList.Add(obj);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;


                }
                finally
                {
                    objConn.Close();
                }
            }
            return BusinessTypeList;
        }

        // POST api/<BusinessEntitysController>
        [Route("[action]")]
        [HttpPost]

        public ActionResult CreateBusinessType([FromBody] BusinessType obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertBusinessType_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@businesstypecode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String businesstypecode = "";

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businesstypename", obj.businesstypename);
                    objCmd.Parameters.AddWithValue("@businesstypedescription", obj.businesstypedescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    businesstypecode = parm.Value.ToString();
                    obj.businesstypecode = businesstypecode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "BusinessType Inserted Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (Exception ex)
                {
                    /*return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                    throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        [Route("[action]")]
        [HttpPut]

        public ActionResult UpdateBusinessType([FromBody] BusinessType obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_Update_BusinessType_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businesstypecode", obj.businesstypecode);
                    objCmd.Parameters.AddWithValue("@businesstypename", obj.businesstypename);
                    objCmd.Parameters.AddWithValue("@businesstypedescription", obj.businesstypedescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "BusinessType Updated Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    /* return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                     throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPut]

        public ActionResult DeleteBusinessType([FromBody] BusinessType obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteBusinessType_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@businesstypecode", obj.businesstypecode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "BusinessType Deleted Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }

        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult BusinessTypeToDoc([FromBody] BusinessType obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertbusinesstypeToDoc_Map";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    List<IDictionary<string, string>> dicDocs = new List<IDictionary<string, string>>();
                    foreach (BusinessTypeDoc objA in obj.businesstypedocs)
                    {
                        dicDocs.Add(BusinessTypeDoc.ConverttoJson(objA));

                    }
                    string json = JsonConvert.SerializeObject(dicDocs, Formatting.Indented);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businesstypecode", obj.businesstypecode);
                    objCmd.Parameters.AddWithValue("@json", json);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Docs Mapped to BusinessType Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    /* return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                     throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        //BUSINESSTYPEDOC

        [Route("[action]")]
        [HttpGet]
        public List<BusinessTypeDoc> GetBusinessTypeDoc()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadBusinessTypeDoc_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        BusinessTypeDoc obj = new BusinessTypeDoc();
                        obj.businesstypedoccode = row["businesstypedoccode"].ToString();
                        obj.businesstypedocname = row["businesstypedocname"].ToString();
                        obj.businesstypedocdescription = row["businesstypedocdescription"].ToString();
                        BusinessTypeDocList.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    objConn.Close();
                }
            }
            return BusinessTypeDocList;
        }

        [Route("[action]")]
        [HttpPost]

        public ActionResult CreateBusinessTypeDoc([FromBody] BusinessTypeDoc obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertBusinessTypeDoc_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@businesstypedoccode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String businesstypecode = "";

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businesstypedocname", obj.businesstypedocname);
                    objCmd.Parameters.AddWithValue("@businesstypedocdescription", obj.businesstypedocdescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    businesstypecode = parm.Value.ToString();
                    obj.businesstypedoccode = businesstypecode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "BusinessTypeDoc Inserted Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (Exception ex)
                {
                    /*return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                    throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        [Route("[action]")]
        [HttpPut]

        public ActionResult UpdateBusinessTypeDoc([FromBody] BusinessTypeDoc obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_UpdateBusinessTypeDoc_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@businesstypedoccode", obj.businesstypedoccode);
                    objCmd.Parameters.AddWithValue("@businesstypedocname", obj.businesstypedocname);
                    objCmd.Parameters.AddWithValue("@businesstypedocdescription", obj.businesstypedocdescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "BusinessTypeDoc Updated Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    /* return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                     throw ex;*/
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPut]

        public ActionResult DeleteBusinessTypeDoc([FromBody] BusinessTypeDoc obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteBusinessTypeDoc_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@businesstypedoccode", obj.businesstypedoccode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "BusinessTypeDoc Deleted Successfully";
                    response.code = "200";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return StatusCode(500, response);
                }
                finally
                {
                    objConn.Close();
                }
            }

        }


    }
}
