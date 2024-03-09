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

namespace GARVIKService.Controllers.Taxi
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxiController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static IConfiguration iconfiguration;
        private readonly List<Driver> DriverList = new List<Driver>();
        private readonly List<Agency> AgencyList = new List<Agency>();

        public TaxiController(IConfiguration _configuration)
        {
            configuration = _configuration;
            iconfiguration = _configuration;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Driver> GetDriver()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_Driver_M";
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
                        Driver obj = new Driver();
                        GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        DriverDocuments[] documentArray = JsonConvert.DeserializeObject<DriverDocuments[]>(row["documentdetail"].ToString());
                        Car[] attachedCars = JsonConvert.DeserializeObject<Car[]>(row["attachedcar"].ToString());
                        if (attachedCars != null)
                        {
                            for (int i = 0; i < attachedCars.Length; i++)
                            {
                                Car car = attachedCars[i];
                                car.Documents = JsonConvert.DeserializeObject<CarDocuments[]>(car.ListDocuments.ToString());
                            }
                        }
                        obj.drivercode = row["drivercode"].ToString();
                        obj.FirstName = row["firstname"].ToString();
                        obj.MiddleName = row["middlename"].ToString();
                        obj.LastName = row["lastname"].ToString();
                        obj.Email = row["email"].ToString();
                        obj.ContactNumber = row["contactnumber"].ToString();
                        if (row["dateofbirth"] != DBNull.Value)
                        {
                            obj.DateOfBirth = DateTime.Parse(row["dateofbirth"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.DateOfBirth = DateTime.MinValue; // You can change this to any default value you prefer
                        }
                        obj.AgencyCode = row["agencycode"].ToString();
                        obj.CreateDate = DateTime.Parse(row["createdate"].ToString());
                        obj.CreatedBy = row["createdby"].ToString();
                        if (row["modifydate"] != DBNull.Value)
                        {
                            obj.ModifyDate = DateTime.Parse(row["modifydate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.ModifyDate = DateTime.MinValue; // You can change this to any default value you prefer
                        }

                        obj.ModifiedBy = row["modifiedby"].ToString();
                        obj.drivercategory = row["drivercategory"].ToString();
                        obj.Addresses = addressArray;
                        obj.Documents = documentArray;
                        obj.AttachedCars = attachedCars;
                        obj.approved = bool.Parse(row["approved"].ToString());

                        DriverList.Add(obj);
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
                    return DriverList;


                }
                finally
                {
                    objConn.Close();
                }
            }
            return DriverList;


        }
        [Route("[action]")]
        [HttpGet]
        public List<Driver> GetDriverViaContactNumber(String contactnumber)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_Driver_M";
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
                    objCmd.Parameters.AddWithValue("@ContactNumber", contactnumber);

                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        Driver obj = new Driver();
                        GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        DriverDocuments[] documentArray = JsonConvert.DeserializeObject<DriverDocuments[]>(row["documentdetail"].ToString());
                        Car[] attachedCars = JsonConvert.DeserializeObject<Car[]>(row["attachedcar"].ToString());
                        if (attachedCars != null)
                        {
                            for (int i = 0; i < attachedCars.Length; i++)
                            {
                                Car car = attachedCars[i];
                                car.Documents = JsonConvert.DeserializeObject<CarDocuments[]>(car.ListDocuments.ToString());
                            }
                        }
                        obj.drivercode = row["drivercode"].ToString();
                        obj.FirstName = row["firstname"].ToString();
                        obj.MiddleName = row["middlename"].ToString();
                        obj.LastName = row["lastname"].ToString();
                        obj.Email = row["email"].ToString();
                        obj.ContactNumber = row["contactnumber"].ToString();
                        if (row["dateofbirth"] != DBNull.Value)
                        {
                            obj.DateOfBirth = DateTime.Parse(row["dateofbirth"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.DateOfBirth = DateTime.MinValue; // You can change this to any default value you prefer
                        }
                        obj.AgencyCode = row["agencycode"].ToString();
                        obj.CreateDate = DateTime.Parse(row["createdate"].ToString());
                        obj.CreatedBy = row["createdby"].ToString();
                        if (row["modifydate"] != DBNull.Value)
                        {
                            obj.ModifyDate = DateTime.Parse(row["modifydate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.ModifyDate = DateTime.MinValue; // You can change this to any default value you prefer
                        }

                        obj.ModifiedBy = row["modifiedby"].ToString();
                        obj.drivercategory = row["drivercategory"].ToString();
                        obj.Addresses = addressArray;
                        obj.Documents = documentArray;
                        obj.AttachedCars = attachedCars;
                        DriverList.Add(obj);
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
                    return DriverList;
                }
                finally
                {
                    objConn.Close();
                }
            }
            return DriverList;
        }
        [Route("[action]")]
        [HttpGet]
        public List<Driver> GetDriversReadytoDrive()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_GetDriversReadytoDrive";
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
                        Driver obj = new Driver();
                        GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        DriverDocuments[] documentArray = JsonConvert.DeserializeObject<DriverDocuments[]>(row["documentdetail"].ToString());
                        Car[] attachedCars = JsonConvert.DeserializeObject<Car[]>(row["attachedcar"].ToString());
                        if (attachedCars != null)
                        {
                            for (int i = 0; i < attachedCars.Length; i++)
                            {
                                Car car = attachedCars[i];
                                car.Documents = JsonConvert.DeserializeObject<CarDocuments[]>(car.ListDocuments.ToString());
                            }
                        }
                        obj.drivercode = row["drivercode"].ToString();
                        obj.FirstName = row["firstname"].ToString();
                        obj.MiddleName = row["middlename"].ToString();
                        obj.LastName = row["lastname"].ToString();
                        obj.Email = row["email"].ToString();
                        obj.ContactNumber = row["contactnumber"].ToString();
                        if (row["dateofbirth"] != DBNull.Value)
                        {
                            obj.DateOfBirth = DateTime.Parse(row["dateofbirth"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.DateOfBirth = DateTime.MinValue; // You can change this to any default value you prefer
                        }
                        obj.AgencyCode = row["agencycode"].ToString();
                        obj.CreateDate = DateTime.Parse(row["createdate"].ToString());
                        obj.CreatedBy = row["createdby"].ToString();
                        if (row["modifydate"] != DBNull.Value)
                        {
                            obj.ModifyDate = DateTime.Parse(row["modifydate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.ModifyDate = DateTime.MinValue; // You can change this to any default value you prefer
                        }

                        obj.ModifiedBy = row["modifiedby"].ToString();
                        obj.Addresses = addressArray;
                        obj.Documents = documentArray;
                        obj.AttachedCars = attachedCars;
                        obj.Longitude = row["longitude"].ToString();
                        obj.Latitude = row["latitude"].ToString();
                        DriverList.Add(obj);
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
                    return DriverList;
                }
                finally
                {
                    objConn.Close();
                }
            }
            return DriverList;
        }
        [Route("[action]")]
        [HttpGet]
        public List<Driver> GetDriversRunningStatus(String drivercode)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_GetDriversRunningStatus";
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
                        Driver obj = new Driver();
                        GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        DriverDocuments[] documentArray = JsonConvert.DeserializeObject<DriverDocuments[]>(row["documentdetail"].ToString());
                        Car[] attachedCars = JsonConvert.DeserializeObject<Car[]>(row["attachedcar"].ToString());
                        if (attachedCars != null)
                        {
                            for (int i = 0; i < attachedCars.Length; i++)
                            {
                                Car car = attachedCars[i];
                                car.Documents = JsonConvert.DeserializeObject<CarDocuments[]>(car.ListDocuments.ToString());
                            }
                        }
                        obj.drivercode = row["drivercode"].ToString();
                        obj.FirstName = row["firstname"].ToString();
                        obj.MiddleName = row["middlename"].ToString();
                        obj.LastName = row["lastname"].ToString();
                        obj.Email = row["email"].ToString();
                        obj.ContactNumber = row["contactnumber"].ToString();
                        if (row["dateofbirth"] != DBNull.Value)
                        {
                            obj.DateOfBirth = DateTime.Parse(row["dateofbirth"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.DateOfBirth = DateTime.MinValue; // You can change this to any default value you prefer
                        }
                        obj.AgencyCode = row["agencycode"].ToString();
                        obj.CreateDate = DateTime.Parse(row["createdate"].ToString());
                        obj.CreatedBy = row["createdby"].ToString();
                        if (row["modifydate"] != DBNull.Value)
                        {
                            obj.ModifyDate = DateTime.Parse(row["modifydate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.ModifyDate = DateTime.MinValue; // You can change this to any default value you prefer
                        }

                        obj.ModifiedBy = row["modifiedby"].ToString();
                        obj.Addresses = addressArray;
                        obj.Documents = documentArray;
                        obj.AttachedCars = attachedCars;
                        obj.Longitude = row["longitude"].ToString();
                        obj.Latitude = row["latitude"].ToString();
                        DriverList.Add(obj);
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
                    return DriverList;
                }
                finally
                {
                    objConn.Close();
                }
            }
            return DriverList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult CreateDriver([FromBody] Driver driverModel)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_Driver_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@drivercode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String drivercode = "";
                    List<IDictionary<string, string>> dicAddresses = new List<IDictionary<string, string>>();
                    foreach (GarvikAddress objA in driverModel.Addresses)
                    {
                        dicAddresses.Add(GarvikAddress.ConverttoJson(objA));
                    }
                    string json = JsonConvert.SerializeObject(dicAddresses, Formatting.Indented);

                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (DriverDocuments objA in driverModel.Documents)
                    {
                        dicDocuments.Add(DriverDocuments.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);



                    objCmd.CommandType = CommandType.StoredProcedure;
                       objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@firstname", driverModel.FirstName);
                    objCmd.Parameters.AddWithValue("@middlename", driverModel.MiddleName);
                    objCmd.Parameters.AddWithValue("@lastname", driverModel.LastName);
                    objCmd.Parameters.AddWithValue("@email", driverModel.Email);
                    objCmd.Parameters.AddWithValue("@contactnumber", driverModel.ContactNumber);
                    objCmd.Parameters.AddWithValue("@dateofbirth", driverModel.DateOfBirth);
                    objCmd.Parameters.AddWithValue("@agencycode", driverModel.AgencyCode);
                    objCmd.Parameters.AddWithValue("@createdby", driverModel.CreatedBy);
                    objCmd.Parameters.AddWithValue("@addressarray", json);
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                    objCmd.Parameters.AddWithValue("@modulecode", "MD-223432"); // Set your module code here

                    objCmd.ExecuteNonQuery();
                    drivercode = parm.Value.ToString();
                    driverModel.drivercode = drivercode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = driverModel;
                    response.message = "Driver Inserted Successfully";
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
        [HttpGet]
        public List<Agency> GetAgency()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_Agency_M";
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
                        Agency obj = new Agency();
                        GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        AgencyDocuments[] documentArray = JsonConvert.DeserializeObject<AgencyDocuments[]>(row["documentdetail"].ToString());
                        Car[] attachedCars = JsonConvert.DeserializeObject<Car[]>(row["attachedcar"].ToString());
                        Driver[] attachedDrivers = JsonConvert.DeserializeObject<Driver[]>(row["attacheddriver"].ToString());
                        //JsonConvert.DeserializeObject<CarDocuments[]>(attachedCars[0].ListDocuments.ToString())
                        if (attachedCars != null)
                        {
                            for (int i = 0; i < attachedCars.Length; i++)
                            {
                                Car car = attachedCars[i];
                                car.Documents = JsonConvert.DeserializeObject<CarDocuments[]>(car.ListDocuments.ToString());
                                // Access car properties and perform operations
                            }
                        }
                        
                        obj.agencycode = row["agencycode"].ToString();
                        obj.AgencyName = row["agencyname"].ToString();
                        obj.RegistrationNumber = row["registrationnumber"].ToString();
                        obj.FirstName = row["firstname"].ToString();
                        obj.MiddleName = row["middlename"].ToString();
                        obj.LastName = row["lastname"].ToString();
                        obj.Email = row["email"].ToString();
                        obj.ContactNumber = row["contactnumber"].ToString();
                        obj.Facebookpage = row["facebookpage"].ToString();
                        obj.Linkedinpage = row["linkedinpage"].ToString();
                        obj.Website = row["website"].ToString();
                        obj.AnotherContactNumber = row["anothercontactnumber"].ToString();
                        if (row["registrationdate"] != DBNull.Value)
                        {
                            obj.Registrationdate = DateTime.Parse(row["registrationdate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.Registrationdate = DateTime.MinValue; // You can change this to any default value you prefer
                        }
                       
                        obj.CreateDate = DateTime.Parse(row["createdate"].ToString());
                        obj.CreatedBy = row["createdby"].ToString();
                        if (row["modifydate"] != DBNull.Value)
                        {
                            obj.ModifyDate = DateTime.Parse(row["modifydate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.ModifyDate = DateTime.MinValue; // You can change this to any default value you prefer
                        }

                        obj.ModifiedBy = row["modifiedby"].ToString();
                        obj.agencycategory = row["agencycategory"].ToString();

                        obj.approved = bool.Parse(row["approved"].ToString());
                        obj.Addresses = (addressArray==null)?null:addressArray;
                        obj.Documents = (documentArray==null)?null: documentArray;
                        obj.AttachedCars = (attachedCars==null)?null:attachedCars;
                        obj.AttachedDrivers = (attachedDrivers==null)?null: attachedDrivers;
                        obj.businessentitycode = row["businessentitycode"].ToString();
                        AgencyList.Add(obj);
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
                    return AgencyList;


                }
                finally
                {
                    objConn.Close();
                }
            }
            return AgencyList;


        }
        [Route("[action]")]
        [HttpGet]
        public List<Agency> GetAgencyViaContactNumber(String contactnumber)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKTAXI_Agency_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Parameters.AddWithValue("@ContactNumber", contactnumber);
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        Agency obj = new Agency();
                        GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        AgencyDocuments[] documentArray = JsonConvert.DeserializeObject<AgencyDocuments[]>(row["documentdetail"].ToString());
                        Car[] attachedCars = JsonConvert.DeserializeObject<Car[]>(row["attachedcar"].ToString());
                        Driver[] attachedDrivers = JsonConvert.DeserializeObject<Driver[]>(row["attacheddriver"].ToString());

                        //JsonConvert.DeserializeObject<CarDocuments[]>(attachedCars[0].ListDocuments.ToString())
                        if (attachedCars != null)
                        {
                            for (int i = 0; i < attachedCars.Length; i++)
                            {
                                Car car = attachedCars[i];
                                car.Documents = JsonConvert.DeserializeObject<CarDocuments[]>(car.ListDocuments.ToString());
                                // Access car properties and perform operations
                            }

                        }

                        obj.agencycode = row["agencycode"].ToString();
                        obj.AgencyName = row["agencyname"].ToString();
                        obj.RegistrationNumber = row["registrationnumber"].ToString();
                        obj.FirstName = row["firstname"].ToString();
                        obj.MiddleName = row["middlename"].ToString();
                        obj.LastName = row["lastname"].ToString();
                        obj.Email = row["email"].ToString();
                        obj.ContactNumber = row["contactnumber"].ToString();
                        obj.Facebookpage = row["facebookpage"].ToString();
                        obj.Linkedinpage = row["linkedinpage"].ToString();
                        obj.Website = row["website"].ToString();
                        obj.AnotherContactNumber = row["anothercontactnumber"].ToString();
                        if (row["registrationdate"] != DBNull.Value)
                        {
                            obj.Registrationdate = DateTime.Parse(row["registrationdate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.Registrationdate = DateTime.MinValue; // You can change this to any default value you prefer
                        }

                        obj.CreateDate = DateTime.Parse(row["createdate"].ToString());
                        obj.CreatedBy = row["createdby"].ToString();
                        if (row["modifydate"] != DBNull.Value)
                        {
                            obj.ModifyDate = DateTime.Parse(row["modifydate"].ToString());
                        }
                        else
                        {
                            // Set a default value or handle it as needed when the field is null
                            obj.ModifyDate = DateTime.MinValue; // You can change this to any default value you prefer
                        }

                        obj.ModifiedBy = row["modifiedby"].ToString();
                        obj.agencycategory = row["agencycategory"].ToString();
                        obj.Addresses = addressArray;
                        obj.Documents = documentArray;
                        obj.AttachedCars = attachedCars;
                        obj.AttachedDrivers = attachedDrivers;
                        obj.businessentitycode = row["businessentitycode"].ToString();

                        AgencyList.Add(obj);
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
                    return AgencyList;


                }
                finally
                {
                    objConn.Close();
                }
            }
            return AgencyList;
        }


        [Route("[action]")]
        [HttpPut]
        public ActionResult UpdateDriver([FromBody] Driver driverModel)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_Driver_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    List<IDictionary<string, string>> dicAddresses = new List<IDictionary<string, string>>();
                    foreach (GarvikAddress objA in driverModel.Addresses)
                    {
                        dicAddresses.Add(GarvikAddress.ConverttoJson(objA));
                    }
                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (DriverDocuments objD in driverModel.Documents)
                    {
                        dicDocuments.Add(DriverDocuments.ConverttoJson(objD));
                    }

                    string json = JsonConvert.SerializeObject(dicAddresses, Formatting.Indented);
                    string jsondocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@drivercode", driverModel.drivercode);
                    objCmd.Parameters.AddWithValue("@firstname", driverModel.FirstName);
                    objCmd.Parameters.AddWithValue("@middlename", driverModel.MiddleName);
                    objCmd.Parameters.AddWithValue("@lastname", driverModel.LastName);
                    objCmd.Parameters.AddWithValue("@email", driverModel.Email);
                    objCmd.Parameters.AddWithValue("@contactnumber", driverModel.ContactNumber);
                    objCmd.Parameters.AddWithValue("@dateofbirth", driverModel.DateOfBirth);
                    objCmd.Parameters.AddWithValue("@agencycode", driverModel.AgencyCode);
                    objCmd.Parameters.AddWithValue("@createdby", driverModel.CreatedBy);
                    objCmd.Parameters.AddWithValue("@addressarray", json);
                    objCmd.Parameters.AddWithValue("@documentarray", jsondocuments);
                    objCmd.Parameters.AddWithValue("@modulecode", "MD-223432");
                    objCmd.ExecuteNonQuery();
                   
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = driverModel;
                    response.message = "Driver Updated Successfully";
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
        public ActionResult DeleteDriver([FromBody] Driver obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKTAXI_Driver_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@drivercode", obj.drivercode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;

                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Driver Deleted Successfully";
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
        public ActionResult CreateAgency([FromBody] Agency obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_Agency_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@agencycode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String agencycode = "";
                    List<IDictionary<string, string>> dicAddresses = new List<IDictionary<string, string>>();
                    foreach (GarvikAddress objA in obj.Addresses)
                    {
                        dicAddresses.Add(GarvikAddress.ConverttoJson(objA));
                    }
                    string json = JsonConvert.SerializeObject(dicAddresses, Formatting.Indented);

                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (AgencyDocuments objA in obj.Documents)
                    {
                        dicDocuments.Add(AgencyDocuments.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);




                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@agencyname", obj.AgencyName);
                    objCmd.Parameters.AddWithValue("@registrationnumber", obj.RegistrationNumber);
                    objCmd.Parameters.AddWithValue("@firstname", obj.FirstName);
                    objCmd.Parameters.AddWithValue("@middlename", obj.MiddleName);
                    objCmd.Parameters.AddWithValue("@lastname", obj.LastName);
                    objCmd.Parameters.AddWithValue("@email", obj.Email);
                    objCmd.Parameters.AddWithValue("@website", obj.Website);
                    objCmd.Parameters.AddWithValue("@facebookpage", obj.Facebookpage);
                    objCmd.Parameters.AddWithValue("@linkedinpage", obj.Linkedinpage);
                    objCmd.Parameters.AddWithValue("@contactnumber", obj.ContactNumber);
                    objCmd.Parameters.AddWithValue("@anothercontactnumber", obj.AnotherContactNumber);
                    objCmd.Parameters.AddWithValue("@registrationdate", obj.Registrationdate);
                    objCmd.Parameters.AddWithValue("@createdby", obj.CreatedBy);
                    objCmd.Parameters.AddWithValue("@businessentitycode", obj.businessentitycode);
                    objCmd.Parameters.AddWithValue("@addressarray", json);
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                    objCmd.Parameters.AddWithValue("@modulecode", "MD-223432"); // Set your module code here
                    objCmd.ExecuteNonQuery();
                    agencycode = parm.Value.ToString();
                    obj.agencycode = agencycode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Agency Inserted Successfully";
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
        public ActionResult UpdateAgency([FromBody] Agency obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_Agency_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    List<IDictionary<string, string>> dicAddresses = new List<IDictionary<string, string>>();
                    foreach (GarvikAddress objA in obj.Addresses)
                    {
                        dicAddresses.Add(GarvikAddress.ConverttoJson(objA));
                    }
                    string json = JsonConvert.SerializeObject(dicAddresses, Formatting.Indented);
                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (AgencyDocuments objA in obj.Documents)
                    {
                        dicDocuments.Add(AgencyDocuments.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);



                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@agencycode", obj.agencycode);
                    objCmd.Parameters.AddWithValue("@agencyname", obj.AgencyName);
                    objCmd.Parameters.AddWithValue("@registrationnumber", obj.RegistrationNumber);
                    objCmd.Parameters.AddWithValue("@firstname", obj.FirstName);
                    objCmd.Parameters.AddWithValue("@middlename", obj.MiddleName);
                    objCmd.Parameters.AddWithValue("@lastname", obj.LastName);
                    objCmd.Parameters.AddWithValue("@email", obj.Email);
                    objCmd.Parameters.AddWithValue("@website", obj.Website);
                    objCmd.Parameters.AddWithValue("@facebookpage", obj.Facebookpage);
                    objCmd.Parameters.AddWithValue("@linkedinpage", obj.Linkedinpage);
                    objCmd.Parameters.AddWithValue("@contactnumber", obj.ContactNumber);
                    objCmd.Parameters.AddWithValue("@anothercontactnumber", obj.AnotherContactNumber);
                    objCmd.Parameters.AddWithValue("@registrationdate", obj.Registrationdate);
                    objCmd.Parameters.AddWithValue("@createdby", obj.CreatedBy);
                    objCmd.Parameters.AddWithValue("@addressarray", json);
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                    objCmd.Parameters.AddWithValue("@modulecode", "MD-223432");
                    objCmd.Parameters.AddWithValue("@businessentitycode", obj.businessentitycode);
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Agency Updated Successfully";
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
        public ActionResult UpdateKyc([FromBody] Agency obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_AgencyKYC";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@agencycode", obj.agencycode);
                    objCmd.Parameters.AddWithValue("@approved", obj.approved);
                    objCmd.Parameters.AddWithValue("@kycdoneby", obj.CreatedBy);
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "KYC Updated Successfully";
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
        public ActionResult DeleteAgency([FromBody] Agency obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKTAXI_Agency_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@agencycode", obj.agencycode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;

                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Agency Deleted Successfully";
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
        public ActionResult DriverReadyToRide(String drivercode, Boolean isreadytodrive, String longitude, String latitude)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_DriverDriveMode";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@drivercode", drivercode);
                    objCmd.Parameters.AddWithValue("@isreadytodrive", isreadytodrive);
                    objCmd.Parameters.AddWithValue("@longitude", longitude);
                    objCmd.Parameters.AddWithValue("@latitude", latitude);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    var data = new
                    {
                        drivercode = drivercode,
                        isreadytodrive = isreadytodrive
                    };

                    string jsonString = System.Text.Json.JsonSerializer.Serialize(data); ;
                    response.obj = jsonString;
                    response.message = "Driver Ride Status Updated";
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
        public ActionResult AttachCarToDriver([FromBody] Car obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_Car_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@carcode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String carcode = "";

                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (CarDocuments objA in obj.Documents)
                    {
                        dicDocuments.Add(CarDocuments.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    obj.attachedtotype = "Driver";
                    objCmd.Parameters.AddWithValue("@attachedtocode", obj.attachedtocode);
                    objCmd.Parameters.AddWithValue("@attachedtotype", obj.attachedtotype);
                    objCmd.Parameters.AddWithValue("@make", obj.make);
                    objCmd.Parameters.AddWithValue("@chassisno", obj.chassisno);
                    objCmd.Parameters.AddWithValue("@modelno", obj.modelno);
                    objCmd.Parameters.AddWithValue("@makeyear", obj.makeyear);
                    objCmd.Parameters.AddWithValue("@rcnumber", obj.rcnumber);
                    objCmd.Parameters.AddWithValue("@rcexpiredon", obj.rcexpiredon);
                    objCmd.Parameters.AddWithValue("@permitnumber", obj.permitnumber);
                    objCmd.Parameters.AddWithValue("@permitexpiredon", obj.permitexpiredon);
                    objCmd.Parameters.AddWithValue("@insurancenumber", obj.insurancenumber);
                    objCmd.Parameters.AddWithValue("@insuranceexpiredon", obj.insuranceexpiredon);
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                   
                    objCmd.ExecuteNonQuery();
                    carcode = parm.Value.ToString();
                    obj.carcode = carcode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Car Attached Successfully";
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
        [HttpPost]
        public ActionResult AttachCar([FromBody] Car obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKTAXI_Car_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@carcode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String carcode = "";
                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (CarDocuments objA in obj.Documents)
                    {
                        dicDocuments.Add(CarDocuments.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    obj.attachedtotype = "Agency";
                    objCmd.Parameters.AddWithValue("@attachedtocode", obj.attachedtocode);
                    objCmd.Parameters.AddWithValue("@attachedtotype", obj.attachedtotype);
                    objCmd.Parameters.AddWithValue("@make", obj.make);
                    objCmd.Parameters.AddWithValue("@chassisno", obj.chassisno);
                    objCmd.Parameters.AddWithValue("@modelno", obj.modelno);
                    objCmd.Parameters.AddWithValue("@makeyear", obj.makeyear);
                    objCmd.Parameters.AddWithValue("@rcnumber", obj.rcnumber);
                    objCmd.Parameters.AddWithValue("@rcexpiredon", obj.rcexpiredon);
                    objCmd.Parameters.AddWithValue("@permitnumber", obj.permitnumber);
                    objCmd.Parameters.AddWithValue("@permitexpiredon", obj.permitexpiredon);
                    objCmd.Parameters.AddWithValue("@insurancenumber", obj.insurancenumber);
                    objCmd.Parameters.AddWithValue("@insuranceexpiredon", obj.insuranceexpiredon);
                  
                    objCmd.Parameters.AddWithValue("@createdby", obj.createdby);
                    
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                    objCmd.ExecuteNonQuery();
                    carcode = parm.Value.ToString();
                    obj.carcode = carcode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Car Attached Successfully";
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
        public ActionResult UpdateCar([FromBody] Car obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKTAXI_Car_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (CarDocuments objA in obj.Documents)
                    {
                        dicDocuments.Add(CarDocuments.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@attachedtocode", obj.attachedtocode);
                    objCmd.Parameters.AddWithValue("@carcode", obj.carcode);
                    objCmd.Parameters.AddWithValue("@make", obj.make);
                    objCmd.Parameters.AddWithValue("@chassisno", obj.chassisno);
                    objCmd.Parameters.AddWithValue("@modelno", obj.modelno);
                    objCmd.Parameters.AddWithValue("@makeyear", obj.makeyear);
                    objCmd.Parameters.AddWithValue("@rcnumber", obj.rcnumber);
                    objCmd.Parameters.AddWithValue("@rcexpiredon", obj.rcexpiredon);
                    objCmd.Parameters.AddWithValue("@permitnumber", obj.permitnumber);
                    objCmd.Parameters.AddWithValue("@permitexpiredon", obj.permitexpiredon);
                    objCmd.Parameters.AddWithValue("@insurancenumber", obj.insurancenumber);
                    objCmd.Parameters.AddWithValue("@insuranceexpiredon", obj.insuranceexpiredon);
                    
                    objCmd.Parameters.AddWithValue("@modifyby", obj.modifyby);
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                    objCmd.ExecuteNonQuery();
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = obj;
                    response.message = "Car Data Updated Successfully";
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
        public ActionResult DeleteCar([FromBody] Car obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKTAXI_Car_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@carcode", obj.carcode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Car Deleted Successfully";
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
