using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GARVIKService.Middleware;
using GARVIKService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace GARVIKService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<User> UserList = new List<User>();
        public UsersController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }

        [HttpGet]
        public List<User> Get()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadUser_M";
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
                        User obj = new User();
                        Module[] moduleArray = JsonConvert.DeserializeObject<Module[]>(row["mappedmodule"].ToString());
                        Role[] roleArray = JsonConvert.DeserializeObject<Role[]>(row["mappedrole"].ToString());



                        obj.usercode = row["usercode"].ToString();
                        obj.username = row["username"].ToString();
                        obj.mobileno = row["mobileno"].ToString();
                        obj.email = row["email"].ToString();
                        obj.street1 = row["street1"].ToString();
                        obj.street2 = row["street2"].ToString();
                        obj.city = row["city"].ToString();
                        obj.state = row["state"].ToString();
                        obj.pincode = row["pincode"].ToString();
                        obj.aadhaarno = row["aadhaarno"].ToString();
                        obj.username = row["username"].ToString();
                        obj.mappedmodule = moduleArray;
                        obj.mappedrole = roleArray;
                        UserList.Add(obj);
                    }

                }
                catch (Exception ex)
                {
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = "Something went wrong. Transaction Failed";
                    response.innermessage = ex.Message;
                    response.code = "500";
                    return UserList;


                }
                finally
                {
                    objConn.Close();
                }
            }
            return UserList;


        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult VerifyLoginCredential(User obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_verifyCredential";
            DataSet DS = new DataSet();
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@email", obj.email);
                    objCmd.Parameters.AddWithValue("@password", obj.password);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    //objJSON = JsonConvert.SerializeObject(DS.Tables[0], Formatting.Indented);
                    GarvikResponse response = new GarvikResponse();
                    if (DS.Tables[0].Rows.Count == 0)
                    {
                        response.result = OperationStatus.Error.ToText();
                        response.obj = null;
                        response.message = "Invalid User id or Password.";
                        response.code = "404";
                    }
                    else
                    {
                        // Your existing code to serialize DS.Tables[0] into objJSON
                        Module[] moduleArray = JsonConvert.DeserializeObject<Module[]>(DS.Tables[0].Rows[0]["mappedmodule"].ToString());
                        Role[] roleArray = JsonConvert.DeserializeObject<Role[]>(DS.Tables[0].Rows[0]["mappedrole"].ToString());


                        obj.usercode = DS.Tables[0].Rows[0]["usercode"].ToString();
                        obj.username = DS.Tables[0].Rows[0]["username"].ToString();
                        obj.mobileno = DS.Tables[0].Rows[0]["mobileno"].ToString();
                        obj.email = DS.Tables[0].Rows[0]["email"].ToString();
                        obj.street1 = DS.Tables[0].Rows[0]["street1"].ToString();
                        obj.street2 = DS.Tables[0].Rows[0]["street2"].ToString();
                        obj.city = DS.Tables[0].Rows[0]["city"].ToString();
                        obj.state = DS.Tables[0].Rows[0]["state"].ToString();
                        obj.pincode = DS.Tables[0].Rows[0]["pincode"].ToString();
                        obj.aadhaarno = DS.Tables[0].Rows[0]["aadhaarno"].ToString();
                        obj.usercategory = DS.Tables[0].Rows[0]["usercategory"].ToString();
                        obj.mappedmodule = moduleArray;
                        obj.mappedrole = roleArray;
                        obj.password = "";
                        obj.businessentitycode = DS.Tables[0].Rows[0]["businessentitycode"].ToString();
                        obj.approved = DS.Tables[0].Rows[0]["approved"].ToString()=="1"? true:false;
                     
                        response.result = OperationStatus.Success.ToText();
                        response.obj = obj;
                        response.message = "Authorized User.";
                        response.code = "200";
                        response.token = JwtTokenGenerator.GenerateToken();
                    }

                    //string result = JsonConvert.SerializeObject(response, Formatting.Indented);
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

        // POST api/<UsersController>
        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public ActionResult CreateUser([FromBody] User obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertUser_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@usercode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String usercode = "";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@username", obj.username);
                    objCmd.Parameters.AddWithValue("@mobileno", obj.mobileno);
                    objCmd.Parameters.AddWithValue("@email", obj.email);
                    objCmd.Parameters.AddWithValue("@street1", obj.street1);
                    objCmd.Parameters.AddWithValue("@street2", obj.street2);
                    objCmd.Parameters.AddWithValue("@city", obj.city);
                    objCmd.Parameters.AddWithValue("@state", obj.state);
                    objCmd.Parameters.AddWithValue("@pincode", obj.pincode);
                    objCmd.Parameters.AddWithValue("@aadhaarno", obj.aadhaarno);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    usercode = parm.Value.ToString();
                    obj.usercode = usercode;
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "User Created Successfully";
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
        [Authorize]
        public ActionResult UpdateUser([FromBody] User obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_UpdateUser_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@usercode", obj.usercode);
                    objCmd.Parameters.AddWithValue("@username", obj.username);
                    objCmd.Parameters.AddWithValue("@mobileno", obj.mobileno);
                    objCmd.Parameters.AddWithValue("@email", obj.email);
                    objCmd.Parameters.AddWithValue("@street1", obj.street1);
                    objCmd.Parameters.AddWithValue("@street2", obj.street2);
                    objCmd.Parameters.AddWithValue("@city", obj.city);
                    objCmd.Parameters.AddWithValue("@state", obj.state);
                    objCmd.Parameters.AddWithValue("@pincode", obj.pincode);
                    objCmd.Parameters.AddWithValue("@aadhaarno", obj.aadhaarno);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "User Updated Successfully";
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
        [Authorize]
        [HttpPut]
        public ActionResult DeleteUser([FromBody] User obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteUser_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@usercode", obj.usercode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;

                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "User Deleted Successfully";
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
