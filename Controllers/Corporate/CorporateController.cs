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
using GARVIKService.Model.Corporate;

namespace GARVIKService.Controllers.Corporate
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorporateController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<GarvikCorporate> CorporateList = new List<GarvikCorporate>();
        public CorporateController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }
        [Route("[action]")]
        [HttpGet]
        public List<GarvikCorporate> GetCorporate(String mobileno)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGarvikCorporate";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@mobileno", mobileno);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        GarvikCorporate obj = new GarvikCorporate();
                        obj.corporatecode = row["corporatecode"].ToString();
                        obj.companyname = row["companyname"].ToString();
                        obj.companydomain = row["companydomain"].ToString();
                        obj.contactnumber = row["contactnumber"].ToString();
                        obj.mobilenumber = row["mobilenumber"].ToString();
                        obj.email = row["email"].ToString();
                        obj.address = row["address"].ToString();
                        obj.city = row["city"].ToString();
                        obj.state = row["state"].ToString();
                        obj.createdby = row["createdby"].ToString();
                        CorporateList.Add(obj);
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
            return CorporateList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult Onboard([FromBody] GarvikCorporate obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertGarvikCorporate";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@corporatecode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String corporatecode = "";
   
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@companyname", obj.companyname);
                    objCmd.Parameters.AddWithValue("@companydomain", obj.companydomain);
                    objCmd.Parameters.AddWithValue("@contactnumber", obj.contactnumber);
                    objCmd.Parameters.AddWithValue("@mobilenumber", obj.mobilenumber);
                    objCmd.Parameters.AddWithValue("@email", obj.email);
                    objCmd.Parameters.AddWithValue("@address", obj.address);
                    objCmd.Parameters.AddWithValue("@city", obj.city);
                    objCmd.Parameters.AddWithValue("@state", obj.state);

                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    corporatecode = parm.Value.ToString();
                    obj.corporatecode = corporatecode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Corporate Inserted Successfully";
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
        public ActionResult UpdateCorporate([FromBody] GarvikCorporate obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGarvikCorporate";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@corporatecode", obj.corporatecode);
                    objCmd.Parameters.AddWithValue("@companyname", obj.companyname);
                    objCmd.Parameters.AddWithValue("@companydomain", obj.companydomain);
                    objCmd.Parameters.AddWithValue("@contactnumber", obj.contactnumber);
                    objCmd.Parameters.AddWithValue("@mobilenumber", obj.mobilenumber);
                    objCmd.Parameters.AddWithValue("@email", obj.email);
                    objCmd.Parameters.AddWithValue("@address", obj.address);
                    objCmd.Parameters.AddWithValue("@city", obj.city);
                    objCmd.Parameters.AddWithValue("@state", obj.state);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Corporate Updated Successfully";
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
        public ActionResult DeleteCorporate([FromBody] GarvikCorporate obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGarvikCorporate";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@corporatecode", obj.corporatecode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Corporate Deleted Successfully";
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
