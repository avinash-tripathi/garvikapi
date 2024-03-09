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
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using GARVIKService.Model.Taxi;

namespace GARVIKService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleCategoryConfigController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<VehicleCategoryConfig> VehicleCategoryConfigList = new List<VehicleCategoryConfig>();
        private readonly List<VehicleCompany> VehicleCompanyList = new List<VehicleCompany>();
        private readonly List<VehicleCompanyVersion> VehicleCompanyVersionList = new List<VehicleCompanyVersion>();
        private readonly List<VehicleModel> VehicleModelList = new List<VehicleModel>();
        private readonly List<VehicleInfo> VehicleInfoList = new List<VehicleInfo>();



        public VehicleCategoryConfigController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }

        [HttpGet]
        public List<VehicleCategoryConfig> Get()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_VehicleCategory_Config";
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
                        VehicleCategoryConfig obj = new VehicleCategoryConfig();
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        obj.vehiclecategoryname = row["vehiclecategoryname"].ToString();
                        obj.baserate = float.Parse(row["baserate"].ToString());
                        obj.peakhourrate = float.Parse(row["peakhourrate"].ToString());
                        obj.peakhourtimefrom = row["peakhourtimefrom"].ToString();
                        obj.peakhourtimeto = row["peakhourtimeto"].ToString();
                        obj.ispeakhour = bool.Parse(row["ispeakhour"].ToString());
                        obj.createdby = row["createdby"].ToString();

                        obj.rateperminute = float.Parse(row["rateperminute"].ToString());
                        obj.waitfeeperminute = float.Parse(row["waitfeeperminute"].ToString());
                        obj.taxrate = float.Parse(row["taxrate"].ToString());
                        obj.peakhourrate = float.Parse(row["peakhourrate"].ToString());
                        obj.fleetimage = row["fleetimage"].ToString();

                        VehicleCategoryConfigList.Add(obj);
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
            return VehicleCategoryConfigList;
        }

        [Route("[action]")]
        [HttpGet]
        public List<VehicleInfo> GetVehicleInfo()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_GARVIKTAXI_GetVehicleInfo";
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
                        VehicleInfo obj = new VehicleInfo();
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        obj.vehiclecategoryname = row["vehiclecategoryname"].ToString();
                        obj.baserate = float.Parse(row["baserate"].ToString());
                        obj.peakhourrate = float.Parse(row["peakhourrate"].ToString());
                        obj.peakhourtimefrom = row["peakhourtimefrom"].ToString();
                        obj.peakhourtimeto = row["peakhourtimeto"].ToString();
                        obj.ispeakhour = bool.Parse(row["ispeakhour"].ToString());
                        obj.rateperminute = float.Parse(row["rateperminute"].ToString());
                        obj.waitfeeperminute = float.Parse(row["waitfeeperminute"].ToString());
                        obj.taxrate = float.Parse(row["taxrate"].ToString());
                        obj.peakhourrate = float.Parse(row["peakhourrate"].ToString());
                        obj.fleetimage = row["fleetimage"].ToString();

                        obj.modelcode = row["modelcode"].ToString();
                        obj.modelname = row["modelname"].ToString();
                        obj.imageurl = row["fleetimage"].ToString();
                     
                        obj.vehicleVersion = row["vehicleVersion"].ToString();
                        obj.vehiclecompanycode = row["vehiclecompanycode"].ToString();
                        obj.vehiclecompanyname = row["vehiclecompanyname"].ToString();


                        VehicleInfoList.Add(obj);
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
                return VehicleInfoList;
            }
            
        }


        [Route("[action]")]
        [HttpPost]
        
        public ActionResult CreateVehicleCategoryConfig([FromBody] VehicleCategoryConfig obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_VehicleCategory_Config";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@vehiclecategorycode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String vehiclecategorycode = "";
                    objCmd.CommandType = CommandType.StoredProcedure;
                   
                    objCmd.Parameters.AddWithValue("@vehiclecategoryname", obj.vehiclecategoryname);
                    objCmd.Parameters.AddWithValue("@baserate", obj.baserate);
                    objCmd.Parameters.AddWithValue("@peakhourrate", obj.peakhourrate);
                    objCmd.Parameters.AddWithValue("@peakhourtimefrom", obj.peakhourtimefrom);
                    objCmd.Parameters.AddWithValue("@peakhourtimeto", obj.peakhourtimeto);
                    objCmd.Parameters.AddWithValue("@ispeakhour", obj.ispeakhour);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);

                    objCmd.Parameters.AddWithValue("@ratepermeter", obj.ratepermeter);
                    objCmd.Parameters.AddWithValue("@rateperminute", obj.rateperminute);
                    objCmd.Parameters.AddWithValue("@waitfeeperminute", obj.waitfeeperminute);
                    objCmd.Parameters.AddWithValue("@taxrate", obj.taxrate);
                    objCmd.Parameters.AddWithValue("@fleetimage", obj.fleetimage);

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    vehiclecategorycode = parm.Value.ToString();
                    obj.vehiclecategorycode = vehiclecategorycode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Category Config Created Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        [Route("[action]")]
        [HttpPut]
        
        public ActionResult UpdateVehicleCategoryConfig([FromBody] VehicleCategoryConfig obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_VehicleCategory_Config";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@vehiclecategorycode", obj.vehiclecategorycode);
                    objCmd.Parameters.AddWithValue("@vehiclecategoryname", obj.vehiclecategoryname);
                    objCmd.Parameters.AddWithValue("@baserate", obj.baserate);
                    objCmd.Parameters.AddWithValue("@peakhourrate", obj.peakhourrate);
                    objCmd.Parameters.AddWithValue("@peakhourtimefrom", obj.peakhourtimefrom);
                    objCmd.Parameters.AddWithValue("@peakhourtimeto", obj.peakhourtimeto);
                    objCmd.Parameters.AddWithValue("@ispeakhour", obj.ispeakhour);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);

                    objCmd.Parameters.AddWithValue("@ratepermeter", obj.ratepermeter);
                    objCmd.Parameters.AddWithValue("@rateperminute", obj.rateperminute);
                    objCmd.Parameters.AddWithValue("@waitfeeperminute", obj.waitfeeperminute);
                    objCmd.Parameters.AddWithValue("@taxrate", obj.taxrate);
                    objCmd.Parameters.AddWithValue("@fleetimage", obj.fleetimage);

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Category Config Updated Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
    
        [Route("[action]")]
        [HttpPut]
        public ActionResult DeleteVehicleCategoryConfig([FromBody] VehicleCategoryConfig obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKTAXI_VehicleCategory_Config";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@vehiclecategorycode", obj.vehiclecategorycode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "VehicleCategory Deleted Successfully";
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
        [HttpGet]
        public List<VehicleCompany> GetVehicleCompanies()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_VehicleCompany_M";
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
                        VehicleCompany obj = new VehicleCompany();
                        obj.vehiclecompanycode = row["vehiclecompanycode"].ToString();
                        obj.vehiclecompanyname = row["vehiclecompanyname"].ToString();
                        obj.vehiclecompanytype = row["vehiclecompanytype"].ToString();
                        obj.createdby = row["createdby"].ToString();
                        VehicleCompanyList.Add(obj);
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
            return VehicleCompanyList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult CreateVehicleCompany([FromBody] VehicleCompany obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_VehicleCompany_M";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@vehiclecompanycode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String vehicleccompanycode = "";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@vehiclecompanyname", obj.vehiclecompanyname);
                    objCmd.Parameters.AddWithValue("@vehiclecompanytype", obj.vehiclecompanytype);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    vehicleccompanycode = parm.Value.ToString();
                    obj.vehiclecompanycode = vehicleccompanycode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Company Created Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPut]

        public ActionResult UpdateVehicleCompany([FromBody] VehicleCompany obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_VehicleCompany_M";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {   
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@vehiclecompanycode", obj.vehiclecompanycode);
                    objCmd.Parameters.AddWithValue("@vehiclecompanyname", obj.vehiclecompanyname);
                    objCmd.Parameters.AddWithValue("@vehiclecompanytype", obj.vehiclecompanytype);
                    
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Company Updated Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        [Route("[action]")]
        [HttpPut]
        public ActionResult DeleteVehicleCompany([FromBody] VehicleCompany obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKTAXI_VehicleCompany_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@vehiclecompanycode", obj.vehiclecompanycode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "VehicleCompany Deleted Successfully";
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

        // VERSION 

        [Route("[action]")]
        [HttpGet]
        public List<VehicleCompanyVersion> GetVehicleVersions()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_VehicleCompanyVersion_M";
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
                        VehicleCompanyVersion obj = new VehicleCompanyVersion();
                        obj.vehicleversioncode = row["vehicleversioncode"].ToString();
                        obj.vehiclecompanycode = row["vehiclecompanycode"].ToString();
                        obj.vehicleversion = row["vehicleversion"].ToString();
                        obj.modelcode = row["modelcode"].ToString();
                        obj.createdby = row["createdby"].ToString();
                        VehicleCompanyVersionList.Add(obj);
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
            return VehicleCompanyVersionList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult CreateVehicleVersion([FromBody] VehicleCompanyVersion obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_VehicleCompanyVersion_M";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@vehicleVersioncode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String vehicleversioncode = "";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@vehiclecompanycode", obj.vehiclecompanycode);
                    objCmd.Parameters.AddWithValue("@vehicleversion", obj.vehicleversion);
                    objCmd.Parameters.AddWithValue("@modelcode", obj.modelcode);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    vehicleversioncode = parm.Value.ToString();
                    obj.vehicleversioncode = vehicleversioncode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Company Version Created Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPut]

        public ActionResult UpdateVehicleVersion([FromBody] VehicleCompanyVersion obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_VehicleCompanyVersion_M";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@vehicleversioncode", obj.vehicleversioncode);
                    objCmd.Parameters.AddWithValue("@vehiclecompanycode", obj.vehiclecompanycode);
                    objCmd.Parameters.AddWithValue("@modelcode", obj.modelcode);
                    objCmd.Parameters.AddWithValue("@vehicleversion", obj.vehicleversion);
                    
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Company Version Updated Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        [Route("[action]")]
        [HttpPut]
        public ActionResult DeleteVehicleVersion([FromBody] VehicleCompanyVersion obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKTAXI_VehicleCompanyVersion_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@vehicleversioncode", obj.vehicleversioncode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "VehicleCompany Version Deleted Successfully";
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

        // MODEL

        [Route("[action]")]
        [HttpGet]
        public List<VehicleModel> GetVehicleModels()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_VehicleModel_M";
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
                        VehicleModel obj = new VehicleModel();
                        obj.vehiclemodelcode = row["vehiclemodelcode"].ToString();
                        obj.vehiclemodelname = row["vehiclemodelname"].ToString();
                        obj.imageurl = row["imageurl"].ToString();
                        obj.createdby = row["createdby"].ToString();
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        VehicleModelList.Add(obj);
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
            return VehicleModelList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult CreateVehicleModel([FromBody] VehicleModel obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_VehicleModel_M";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@modelcode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String vehiclemodelcode = "";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@modelname", obj.vehiclemodelname);
                    objCmd.Parameters.AddWithValue("@imageurl", obj.imageurl);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.Parameters.AddWithValue("@vehiclecategorycode", obj.vehiclecategorycode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    vehiclemodelcode = parm.Value.ToString();
                    obj.vehiclemodelcode = vehiclemodelcode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Model Created Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }
        [Route("[action]")]
        [HttpPut]

        public ActionResult UpdateVehicleModel([FromBody] VehicleModel obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_VehicleModel_M";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@modelcode", obj.vehiclemodelcode);
                    objCmd.Parameters.AddWithValue("@modelname", obj.vehiclemodelname);
                    objCmd.Parameters.AddWithValue("@imageurl", obj.imageurl);
                    objCmd.Parameters.AddWithValue("@vehiclecategorycode", obj.vehiclecategorycode);

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Model Updated Successfully";
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
                    return BadRequest(response);
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        [Route("[action]")]
        [HttpPut]
        public ActionResult DeleteVehicleModel([FromBody] VehicleModel obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKTAXI_VehicleModel_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@vehiclemodelcode", obj.vehiclemodelcode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Vehicle Model Deleted Successfully";
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
