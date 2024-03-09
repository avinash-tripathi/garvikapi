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
using GARVIKService.Model.Taxi;
using GARVIKService.Model.Customer;

namespace GARVIKService.Controllers.Taxi
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxiBookingController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<Booking> BookingList = new List<Booking>();
        private readonly List<GarvikReview> ReviewList = new List<GarvikReview>();

        public TaxiBookingController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }
        public static bool IsUniqueKeyViolation(SqlException ex)
        {
            return ex.Errors.Cast<SqlError>().Any(e => e.Class == 14 && (e.Number == 2601 || e.Number == 2627));
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult CreateBooking([FromBody] Booking obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_Booking";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@bookingcode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    SqlParameter parmAmount = new SqlParameter("@amount", SqlDbType.Float, 20);
                    parmAmount.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    objCmd.Parameters.Add(parmAmount);
                    String bookingcode = "";
                    float amount = 0;

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@customercode", obj.customercode);
                    objCmd.Parameters.AddWithValue("@sourcelocationlongi", obj.sourcelocationlongi);
                    objCmd.Parameters.AddWithValue("@sourcelocationlati", obj.sourcelocationlati);
                    objCmd.Parameters.AddWithValue("@destinationlocationlongi", obj.destinationlocationlongi);
                    objCmd.Parameters.AddWithValue("@destinationlocationlati", obj.destinationlocationlati);
                    objCmd.Parameters.AddWithValue("@vehiclecategorycode", obj.vehiclecategorycode);
                    objCmd.Parameters.AddWithValue("@sourcelocationaddress", obj.sourcelocationaddress);
                    objCmd.Parameters.AddWithValue("@destinationlocationaddress", obj.destinationlocationaddress);
                    objCmd.Parameters.AddWithValue("@eta", obj.eta);

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    bookingcode = parm.Value.ToString();
                    amount = float.Parse(parmAmount.Value.ToString());
                    obj.bookingcode = bookingcode;
                    obj.ridestatus = "pending";
                    obj.amount = amount;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Booking Raised Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (SqlException sqlex)
                {
                    bool isViolated = IsUniqueKeyViolation(sqlex);
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = isViolated ? "Mobile no already exist. Duplicate entry is not allowed." : "Something went wrong. Transaction Failed";
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
        public ActionResult AcceptBooking([FromBody] Booking obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_AcceptGARVIKTAXIBookingRequest";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parmRideStatus = new SqlParameter("@ridestatus", SqlDbType.VarChar, 20);
                    parmRideStatus.Direction = ParameterDirection.Output;
                    SqlParameter parmrandomnumber = new SqlParameter("@randomnumber", SqlDbType.Int, 5);
                    parmRideStatus.Direction = ParameterDirection.Output;
                    parmrandomnumber.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parmRideStatus);
                    objCmd.Parameters.Add(parmrandomnumber);
                    String ridestatus = "";
                    int randomnumber = 0;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", obj.bookingcode);
                    objCmd.Parameters.AddWithValue("@drivercode", obj.assigneddrivercode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    ridestatus = parmRideStatus.Value.ToString();
                    randomnumber = int.Parse(parmrandomnumber.Value.ToString());
                    obj.ridestatus = ridestatus;
                    obj.otpforonboard = randomnumber;
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = ridestatus==""?"": "Booking Accepted Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (SqlException sqlex)
                {
                    bool isViolated = IsUniqueKeyViolation(sqlex);
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = isViolated ? "Duplicate entry is not allowed." : "Something went wrong. Transaction Failed";
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
        public ActionResult CompleteBooking([FromBody] Booking obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_GARVIKTAXIBookingComplete";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parmAmount = new SqlParameter("@amount", SqlDbType.Float, 20);
                    parmAmount.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parmAmount);
                    float amount = 0;
                    
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", obj.bookingcode);
                    objCmd.Parameters.AddWithValue("@drivercode", obj.assigneddrivercode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    amount = float.Parse(parmAmount.Value.ToString());
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    obj.amount = amount;
                    obj.ridestatus = "complete";
                    response.obj = obj;
                    response.message =  "Ride Completed Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (SqlException sqlex)
                {
                    bool isViolated = IsUniqueKeyViolation(sqlex);
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = isViolated ? "Duplicate entry is not allowed." : "Something went wrong. Transaction Failed";
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
        public ActionResult CancelBooking([FromBody] Booking obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_CancelGARVIKTAXIBookingRequest";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parmRideStatus = new SqlParameter("@ridestatus", SqlDbType.VarChar, 20);
                    parmRideStatus.Direction = ParameterDirection.Output;

                    objCmd.Parameters.Add(parmRideStatus);
                    String  ridestatus ="";

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", obj.bookingcode);
                   
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    ridestatus = parmRideStatus.Value.ToString();
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    
                    obj.ridestatus = ridestatus;
                    response.obj = obj;
                    response.message = "Ride Cancelled Successfully";
                    response.code = "200";
                    return Ok(response);

                }
                catch (SqlException sqlex)
                {
                    bool isViolated = IsUniqueKeyViolation(sqlex);
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Error.ToText();
                    response.obj = null;
                    response.message = isViolated ? "Duplicate entry is not allowed." : "Something went wrong. Transaction Failed";
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
        [HttpGet]
        public List<Booking> GetRecentGARVIKTAXIBookingRequest(String requesttime)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_GetRecentGARVIKTAXIBookingRequest";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@requesttime", requesttime);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        GarvikCustomer[] customerinfo = JsonConvert.DeserializeObject<GarvikCustomer[]>(row["customerinfo"].ToString());

                        Booking obj = new Booking();
                        obj.bookingcode = row["bookingcode"].ToString();
                        obj.customercode = row["customercode"].ToString();
                        obj.sourcelocationlati = float.Parse(row["sourcelocationlati"].ToString());
                        obj.sourcelocationlongi = float.Parse(row["sourcelocationlongi"].ToString());
                        obj.destinationlocationlati = float.Parse(row["destinationlocationlati"].ToString());
                        obj.destinationlocationlongi = float.Parse(row["destinationlocationlongi"].ToString());
                        obj.ridestatus = row["ridestatus"].ToString();
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        obj.customerinfo = customerinfo[0];

                        obj.sourcelocationaddress = row["sourcelocationaddress"].ToString();
                        obj.destinationlocationaddress = row["destinationlocationaddress"].ToString();

                        BookingList.Add(obj);
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
            return BookingList;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Booking> GetGARVIKTAXIBookingData(string bookingcode)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_GetGARVIKTAXIBookingData";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", bookingcode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        GarvikCustomer[] customerinfo = JsonConvert.DeserializeObject<GarvikCustomer[]>(row["customerinfo"].ToString());
                        Driver[] driverinfo = JsonConvert.DeserializeObject<Driver[]>(row["driverinfo"].ToString());
                        GarvikReview[] reviews = JsonConvert.DeserializeObject<GarvikReview[]>(row["reviews"].ToString());


                        Booking obj = new Booking();
                        obj.bookingcode = row["bookingcode"].ToString();
                        obj.assigneddrivercode = row["assigneddrivercode"].ToString();
                        obj.customercode = row["customercode"].ToString();
                        obj.sourcelocationlati = float.Parse(row["sourcelocationlati"].ToString());
                        obj.sourcelocationlongi = float.Parse(row["sourcelocationlongi"].ToString());
                        obj.destinationlocationlati = float.Parse(row["destinationlocationlati"].ToString());
                        obj.destinationlocationlongi = float.Parse(row["destinationlocationlongi"].ToString());
                        obj.ridestatus = row["ridestatus"].ToString();
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        obj.customerinfo = customerinfo[0];
                        obj.driverinfo = (driverinfo == null) ? null : driverinfo[0];
                        obj.reviews = (reviews == null) ? null : reviews;
                        obj.startdate = row["startdate"].ToString();
                        obj.completedate = row["completedate"].ToString();
                        BookingList.Add(obj);
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
            return BookingList;
        }

        [Route("[action]")]
        [HttpGet]
        public Booking GetGARVIKTAXIBookingStatus(String bookingcode, String ridestatus)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_Booking";
            DataSet DS = new DataSet();
            Booking obj = new Booking();
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", bookingcode);
                    objCmd.Parameters.AddWithValue("@ridestatus", ridestatus);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        GarvikCustomer[] customerinfo = JsonConvert.DeserializeObject<GarvikCustomer[]>(row["customerinfo"].ToString());
                        Driver[] driverinfo = JsonConvert.DeserializeObject<Driver[]>(row["driverinfo"].ToString());
                        GarvikReview[] reviews = JsonConvert.DeserializeObject<GarvikReview[]>(row["reviews"].ToString());

                        obj.bookingcode = row["bookingcode"].ToString();
                        obj.customercode = row["customercode"].ToString();
                        obj.assigneddrivercode = row["assigneddrivercode"].ToString();
                        obj.assigneddrivercode = row["assigneddrivercode"].ToString();
                        obj.sourcelocationlati = float.Parse(row["sourcelocationlati"].ToString());
                        obj.sourcelocationlongi = float.Parse(row["sourcelocationlongi"].ToString());
                        obj.destinationlocationlati = float.Parse(row["destinationlocationlati"].ToString());
                        obj.destinationlocationlongi = float.Parse(row["destinationlocationlongi"].ToString());
                        obj.ridestatus = row["ridestatus"].ToString();
                        obj.customerinfo = customerinfo[0];
                        obj.driverinfo = (driverinfo ==null)?null: driverinfo[0];
                        obj.reviews = (reviews == null) ? null : reviews;
                        obj.otpforonboard = int.Parse(row["otpforonboard"].ToString());
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        obj.startdate = row["startdate"].ToString();
                        obj.completedate = row["completedate"].ToString();

                        return obj;
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
            return obj;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Booking> GetGARVIKTAXIBookingHistoryByCustomerID(String customercode)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_BookingByCustomerId";
            DataSet DS = new DataSet();
          
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@customercode", customercode);
                 
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        GarvikCustomer[] customerinfo = JsonConvert.DeserializeObject<GarvikCustomer[]>(row["customerinfo"].ToString());
                        Driver[] driverinfo = JsonConvert.DeserializeObject<Driver[]>(row["driverinfo"].ToString());
                        GarvikReview[] reviews = JsonConvert.DeserializeObject<GarvikReview[]>(row["reviews"].ToString());
                        Booking obj = new Booking();
                        obj.bookingcode = row["bookingcode"].ToString();
                        obj.customercode = row["customercode"].ToString();
                        obj.assigneddrivercode = row["assigneddrivercode"].ToString();
                        obj.assigneddrivercode = row["assigneddrivercode"].ToString();
                        obj.sourcelocationlati = float.Parse(row["sourcelocationlati"].ToString());
                        obj.sourcelocationlongi = float.Parse(row["sourcelocationlongi"].ToString());
                        obj.destinationlocationlati = float.Parse(row["destinationlocationlati"].ToString());
                        obj.destinationlocationlongi = float.Parse(row["destinationlocationlongi"].ToString());
                        obj.ridestatus = row["ridestatus"].ToString();
                        obj.customerinfo = customerinfo[0];
                        obj.driverinfo = (driverinfo == null) ? null : driverinfo[0];
                        obj.reviews = (reviews == null) ? null : reviews;
                        obj.otpforonboard = int.Parse(row["otpforonboard"].ToString());
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        obj.startdate = row["startdate"].ToString();
                        obj.completedate = row["completedate"].ToString();

                        BookingList.Add(obj);
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
            return BookingList;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Booking> GetGARVIKTAXIBookingHistoryByDriverID(String drivercode)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_BookingByDriverId";
            DataSet DS = new DataSet();
            
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@drivercode", drivercode);

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        GarvikCustomer[] customerinfo = JsonConvert.DeserializeObject<GarvikCustomer[]>(row["customerinfo"].ToString());
                        Driver[] driverinfo = JsonConvert.DeserializeObject<Driver[]>(row["driverinfo"].ToString());
                        GarvikReview[] reviews = JsonConvert.DeserializeObject<GarvikReview[]>(row["reviews"].ToString());
                        Booking obj = new Booking();
                        obj.bookingcode = row["bookingcode"].ToString();
                        obj.customercode = row["customercode"].ToString();
                        obj.assigneddrivercode = row["assigneddrivercode"].ToString();
                        obj.assigneddrivercode = row["assigneddrivercode"].ToString();
                        obj.sourcelocationlati = float.Parse(row["sourcelocationlati"].ToString());
                        obj.sourcelocationlongi = float.Parse(row["sourcelocationlongi"].ToString());
                        obj.destinationlocationlati = float.Parse(row["destinationlocationlati"].ToString());
                        obj.destinationlocationlongi = float.Parse(row["destinationlocationlongi"].ToString());
                        obj.ridestatus = row["ridestatus"].ToString();
                        obj.customerinfo = customerinfo[0];
                        obj.driverinfo = (driverinfo == null) ? null : driverinfo[0];
                        obj.reviews = (reviews == null) ? null : reviews;
                        obj.otpforonboard = int.Parse(row["otpforonboard"].ToString());
                        obj.vehiclecategorycode = row["vehiclecategorycode"].ToString();
                        obj.startdate = row["startdate"].ToString();
                        obj.completedate = row["completedate"].ToString();

                        BookingList.Add(obj);
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
            return BookingList;
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<string> GetBookingStatusByStageAsync(String bookingcode, String stagename)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_GARVIKTAXI_CheckBookingStatusByStage";
            String returnValue = "";
          
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", bookingcode);
                    objCmd.Parameters.AddWithValue("@stagename", stagename);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    // Use ExecuteScalarAsync and cast the result to string
                    var result = await objCmd.ExecuteScalarAsync();
                    returnValue = result != null ? result.ToString() : null;

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
            return returnValue;


        }

        [Route("[action]")]
        [HttpGet]
        public bool verifyBookingOTP(String bookingcode,int otp)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_VerifyGARVIKTAXIBookingRequestOTP";
           
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", bookingcode);
                    objCmd.Parameters.AddWithValue("@otp", otp);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    object obj = objCmd.ExecuteScalar();
                    return bool.Parse(obj.ToString());
                    
                   
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

        }

        //REVIEW
        [Route("[action]")]
        [HttpGet]
        public List<GarvikReview> GetReviews(String reviewcode)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_Review";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@reviewcode", reviewcode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {

                        GarvikReview obj = new GarvikReview();
                        Booking[] bookingininfo = JsonConvert.DeserializeObject<Booking[]>(row["bookinginfo"].ToString());

                        obj.bookingcode = row["bookingcode"].ToString();
                        obj.reviewcode = row["reviewcode"].ToString();
                        obj.rating = float.Parse(row["rating"].ToString());
                        obj.referencecategory = row["referencecategory"].ToString();
                        obj.referencecode = row["referencecode"].ToString(); 
                        obj.comment = row["comment"].ToString();
                        obj.bookinginfo = bookingininfo[0];

                        ReviewList.Add(obj);
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
            return ReviewList;
        }
        [Route("[action]")]
        [HttpPost]
        
        public ActionResult CreateReview([FromBody] GarvikReview obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_Review";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@reviewcode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String reviewcode = "";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@bookingcode", obj.bookingcode);
                    objCmd.Parameters.AddWithValue("@referencecode", obj.referencecode);
                    objCmd.Parameters.AddWithValue("@rating", obj.rating);
                    objCmd.Parameters.AddWithValue("@referencecategory", obj.referencecategory);
                    objCmd.Parameters.AddWithValue("@comment", obj.comment);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();
                    reviewcode = parm.Value.ToString();
                    obj.reviewcode = reviewcode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Review Created Successfully";
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
