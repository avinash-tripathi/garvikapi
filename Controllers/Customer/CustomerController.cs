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

using GARVIKService.Model.Customer;


namespace GARVIKService.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<GarvikCustomer> CustomerList = new List<GarvikCustomer>();
        public CustomerController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }
        public static bool IsUniqueKeyViolation(SqlException ex)
        {
            return ex.Errors.Cast<SqlError>().Any(e => e.Class == 14 && (e.Number == 2601 || e.Number == 2627));
        }
        
        [Route("[action]")]
        [HttpGet]
        public List<GarvikCustomer> GetCustomer(String mobileno)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIK_Customer_M";
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
                        GarvikCustomer obj = new GarvikCustomer();
                        obj.customercode = row["customercode"].ToString();
                        obj.mobileno = row["mobileno"].ToString();
                        obj.email = row["email"].ToString();
                        obj.firstname = row["firstname"].ToString();
                        obj.middlename = row["middlename"].ToString();
                        obj.lastname = row["lastname"].ToString();
                        obj.preferredlanguage = row["preferredlanguage"].ToString();
                        obj.mobilenoverified = bool.Parse(row["mobilenoverified"].ToString());
                        obj.emailverified = bool.Parse(row["emailverified"].ToString());
                        obj.createdby = row["createdby"].ToString();
                        CustomerList.Add(obj);
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
            return CustomerList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult CreateCustomer([FromBody] GarvikCustomer obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_InsertGARVIK_Customer_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@customercode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String customercode = "";

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@mobileno", obj.mobileno);
                    objCmd.Parameters.AddWithValue("@email", obj.email);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    customercode = parm.Value.ToString();
                    obj.customercode = customercode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Customer Inserted Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch(SqlException sqlex)
                {
                    bool isViolated = IsUniqueKeyViolation(sqlex);
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = isViolated? "Mobile no already exist. Duplicate entry is not allowed." : "Something went wrong. Transaction Failed";
                    response.innermessage = sqlex.Message;
                    response.code = "500";
                    return StatusCode(500, response);



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
        public ActionResult UpdateCustomerBaiscInfo([FromBody] GarvikCustomerBasic obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_UpdateGARVIK_Customer_BasicData";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@customercode", obj.customercode);
                    objCmd.Parameters.AddWithValue("@email", obj.email);
                    objCmd.Parameters.AddWithValue("@firstname", obj.firstname);
                    objCmd.Parameters.AddWithValue("@middlename", obj.middlename);
                    objCmd.Parameters.AddWithValue("@lastname", obj.lastname);
                    objCmd.Parameters.AddWithValue("@preferredlanguage", obj.preferredlanguage);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                  
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Customer Basic Info Updated Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (SqlException sqlex)
                {
                    bool isViolated = IsUniqueKeyViolation(sqlex);
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = isViolated ? "Email already exist. Duplicate entry is not allowed." : "Something went wrong. Transaction Failed";
                    response.innermessage = sqlex.Message;
                    response.code = "500";
                    return StatusCode(500, response);

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
        public ActionResult UpdateCustomerSettings([FromBody] GarvikCustomerSettings obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];

            String query = "stp_UpdateGARVIK_Customer_Settings";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@customercode", obj.customercode);
                    objCmd.Parameters.AddWithValue("@homelocation", obj.homelocation);
                    objCmd.Parameters.AddWithValue("@worklocation", obj.worklocation);
                    objCmd.Parameters.AddWithValue("@twostepverification", obj.twostepverification);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Customer Settings Updated Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (SqlException sqlex)
                {
                    bool isViolated = IsUniqueKeyViolation(sqlex);
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = isViolated ? "Email already exist. Duplicate entry is not allowed." : "Something went wrong. Transaction Failed";
                    response.innermessage = sqlex.Message;
                    response.code = "500";
                    return StatusCode(500, response);

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
    }
}
