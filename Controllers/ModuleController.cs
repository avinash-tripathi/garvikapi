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
    public class ModuleController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<Module> ModuleList = new List<Module>();
        private readonly List<RoleAccess> RoleAccessList = new List<RoleAccess>();

        public ModuleController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }

        [HttpGet]
        [Authorize]
        public List<Module> Get()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadModule_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    //objCmd.Parameters.AddWithValue("@distributioncode", distributioncode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        Module obj = new Module();
                        obj.modulecode = row["modulecode"].ToString();
                        obj.modulename = row["modulename"].ToString();
                        obj.moduledescription = row["moduledescription"].ToString();
                        ModuleList.Add(obj);
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
            return ModuleList;
        }
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public List<RoleAccess> GetRoleAccess()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_readRoleAccess_M";
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
                        RoleAccess obj = new RoleAccess();
                        obj.rolecode = row["rolecode"].ToString();
                        obj.rolename = row["rolename"].ToString();
                        obj.modulecode = row["modulecode"].ToString();
                        obj.modulename = row["modulename"].ToString();
                        obj.isreadonly = Boolean.Parse(row["readonly"].ToString());
                        obj.fullaccess = Boolean.Parse(row["fullaccess"].ToString());
                        RoleAccessList.Add(obj);
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
            return RoleAccessList;
        }

        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public ActionResult UpdateRoleAccess([FromBody] RoleAccess obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateRoleAccess_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                  objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@rolecode", obj.rolecode);
                    objCmd.Parameters.AddWithValue("@modulecode", obj.modulecode);
                    objCmd.Parameters.AddWithValue("@isreadonly", obj.isreadonly);
                    objCmd.Parameters.AddWithValue("@fullaccess", obj.fullaccess);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Success.ToText();
                    response.obj = obj;
                    response.message = "Access Configuration Changed.";
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
        [Authorize]
        public ActionResult UserToModuleMap([FromBody] UserToModuleMap obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateuserTomodule_Map";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                List<IDictionary<string, string>> dicModules = new List<IDictionary<string, string>>();
                foreach (Module objA in obj.modules)
                {
                    dicModules.Add(Module.ConverttoJson(objA));
                }
                string json = JsonConvert.SerializeObject(dicModules, Formatting.Indented);
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@usercode", obj.usercode);
                    objCmd.Parameters.AddWithValue("@modulecodearray", json);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;

                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Success.ToText();
                    response.obj = obj;
                    response.message = "Module Mapped Successfully";
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



        // POST api/<ModulesController>
        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public ActionResult CreateModule([FromBody] Module obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertModule_M";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@modulecode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String modulecode = "";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@modulename", obj.modulename);
                    objCmd.Parameters.AddWithValue("@moduledescription", obj.moduledescription);

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    modulecode = parm.Value.ToString();
                    obj.modulecode = modulecode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Module Created Successfully";
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
        [HttpPut]
        [Authorize]
        public ActionResult UpdateModule([FromBody] Module obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateModule_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@modulecode", obj.modulecode);
                    objCmd.Parameters.AddWithValue("@modulename", obj.modulename);
                    objCmd.Parameters.AddWithValue("@moduledescription", obj.moduledescription);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Module Updated Successfully";
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
        [HttpPut]
        [Authorize]
        public ActionResult DeleteModule([FromBody] Module obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteModule_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@modulecode", obj.modulecode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Module Deleted Successfully";
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
