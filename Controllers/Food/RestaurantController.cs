using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using GARVIKService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json;
using GARVIKService.Model.Food.Restaurant;


namespace GARVIKService.Controllers.Food
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly List<Restaurant> restaurantList = new List<Restaurant>();
        private readonly IConfiguration configuration;
        public RestaurantController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpGet]
        public List<Restaurant> Get()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKRESTAURANT_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                   /* objCmd.Parameters.AddWithValue("@imagecontext", imagecontext);
                    */

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        Restaurant obj = new Restaurant();
                        GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        Documents[] documentArray = JsonConvert.DeserializeObject<Documents[]>(row["documentdetail"].ToString());


                        obj.Addresses = addressArray;
                        obj.Documents = documentArray;

                        obj.restaurantcode = row["restaurantcode"].ToString();
                        obj.restaurantcode = row["restaurantcode"].ToString();
                        obj.RestaurantName = row["restaurantname"].ToString();
                        obj.Description = row["description"].ToString();
                        obj.LogoUrl = row["logourl"].ToString();
                        obj.CoverImageUrl = row["coverimageurl"].ToString();
                        obj.Category = row["category"].ToString();
                        obj.Cuisine = row["cuisine"].ToString();
                        obj.Phone = row["phone"].ToString();
                        obj.Email = row["email"].ToString();
                        obj.Website = row["website"].ToString();
                        obj.Latitude = Convert.ToDouble(row["latitude"]);
                        obj.Longitude = Convert.ToDouble(row["longitude"]);

                        // Populate operating hours
                        obj.OpeningTime = row["openingtime"].ToString();
                        obj.ClosingTime = row["closingtime"].ToString();

                        // Populate additional information
                        obj.HasDeliveryService = Convert.ToBoolean(row["hasdeliveryservice"]);
                        obj.HasTakeawayService = Convert.ToBoolean(row["hastakeawayservice"]);
                        obj.HasDineInService = Convert.ToBoolean(row["hasdineinservice"]);
                        obj.IsVegetarianFriendly = Convert.ToBoolean(row["isvegetarianfriendly"]);
                        obj.IsVeganFriendly = Convert.ToBoolean(row["isveganfriendly"]);
                        obj.IsHalalCertified = Convert.ToBoolean(row["ishalalcertified"]);

                        // Populate payment options
                        obj.AcceptsCash = Convert.ToBoolean(row["acceptscash"]);
                        obj.AcceptsCard = Convert.ToBoolean(row["acceptscard"]);
                        obj.AcceptsOnlinePayment = Convert.ToBoolean(row["acceptsonlinepayment"]);

                        // Populate ratings and reviews
                        obj.AverageRating = Convert.ToDouble(row["averagerating"]);
                        obj.TotalReviews = Convert.ToInt32(row["totalreviews"]);

                        // Populate other features
                        obj.HasWifi = Convert.ToBoolean(row["haswifi"]);
                        obj.HasParking = Convert.ToBoolean(row["hasparking"]);
                        obj.IsAlcoholServed = Convert.ToBoolean(row["isalcoholserved"]);
                        obj.HasOutdoorSeating = Convert.ToBoolean(row["hasoutdoorseating"]);

                        // Populate operational status
                        obj.IsOpen = Convert.ToBoolean(row["isopen"]);

                        // Populate registration information
                        obj.RegistrationDate = Convert.ToDateTime(row["registrationdate"]);
                        obj.RegistrationNumber = row["registrationnumber"].ToString();
                        obj.RegistrationCertificateUrl = row["registrationcertificateurl"].ToString();
                        obj.CreatedBy = row["createdby"].ToString();
                        //obj.modulecode = row["modulecode"].ToString();


                        // Deserialize and assign address and document details
                        obj.Addresses = addressArray;
                        obj.Documents = documentArray;
                        restaurantList.Add(obj);
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
            return restaurantList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult OnboardRestaurant([FromBody] Restaurant restaurant)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKRESTAURANT_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@restaurantcode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String restaurantcode = "";
                    List<IDictionary<string, string>> dicAddresses = new List<IDictionary<string, string>>();
                    foreach (GarvikAddress objA in restaurant.Addresses)
                    {
                        dicAddresses.Add(GarvikAddress.ConverttoJson(objA));
                    }
                    string json = JsonConvert.SerializeObject(dicAddresses, Formatting.Indented);

                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (Documents objA in restaurant.Documents)
                    {
                        dicDocuments.Add(Documents.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@RestaurantName", restaurant.RestaurantName);
                    objCmd.Parameters.AddWithValue("@Description", restaurant.Description);
                    objCmd.Parameters.AddWithValue("@LogoUrl", restaurant.LogoUrl);
                    objCmd.Parameters.AddWithValue("@CoverImageUrl", restaurant.CoverImageUrl);
                    objCmd.Parameters.AddWithValue("@Category", restaurant.Category);
                    objCmd.Parameters.AddWithValue("@Cuisine", restaurant.Cuisine);
                    objCmd.Parameters.AddWithValue("@Phone", restaurant.Phone);
                    objCmd.Parameters.AddWithValue("@Email", restaurant.Email);
                    objCmd.Parameters.AddWithValue("@Website", restaurant.Website);
                    objCmd.Parameters.AddWithValue("@Latitude", restaurant.Latitude);
                    objCmd.Parameters.AddWithValue("@Longitude", restaurant.Longitude);
                    objCmd.Parameters.AddWithValue("@OpeningTime", restaurant.OpeningTime);
                    objCmd.Parameters.AddWithValue("@ClosingTime", restaurant.ClosingTime);
                    objCmd.Parameters.AddWithValue("@HasDeliveryService", restaurant.HasDeliveryService);
                    objCmd.Parameters.AddWithValue("@HasTakeawayService", restaurant.HasTakeawayService);
                    objCmd.Parameters.AddWithValue("@HasDineInService", restaurant.HasDineInService);
                    objCmd.Parameters.AddWithValue("@IsVegetarianFriendly", restaurant.IsVegetarianFriendly);
                    objCmd.Parameters.AddWithValue("@IsVeganFriendly", restaurant.IsVeganFriendly);
                    objCmd.Parameters.AddWithValue("@AcceptsCard", restaurant.AcceptsCard);
                    
                    objCmd.Parameters.AddWithValue("@IsHalalCertified", restaurant.IsHalalCertified);
                    objCmd.Parameters.AddWithValue("@AcceptsCash", restaurant.AcceptsCash);
                    objCmd.Parameters.AddWithValue("@AcceptsOnlinePayment", restaurant.AcceptsOnlinePayment);
                    objCmd.Parameters.AddWithValue("@AverageRating", restaurant.AverageRating);
                    objCmd.Parameters.AddWithValue("@TotalReviews", restaurant.TotalReviews);

                    objCmd.Parameters.AddWithValue("@HasWifi", restaurant.HasWifi);
                    objCmd.Parameters.AddWithValue("@HasParking", restaurant.HasParking);
                    objCmd.Parameters.AddWithValue("@IsAlcoholServed", restaurant.IsAlcoholServed);
                    objCmd.Parameters.AddWithValue("@HasOutdoorSeating", restaurant.HasOutdoorSeating);
                    objCmd.Parameters.AddWithValue("@IsOpen", restaurant.IsOpen);
                    objCmd.Parameters.AddWithValue("@RegistrationDate", restaurant.RegistrationDate);
                    objCmd.Parameters.AddWithValue("@RegistrationNumber", restaurant.RegistrationNumber);
                    objCmd.Parameters.AddWithValue("@RegistrationCertificateUrl", restaurant.RegistrationCertificateUrl);
                    objCmd.Parameters.AddWithValue("@addressarray", json);
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                    objCmd.Parameters.AddWithValue("@modulecode", restaurant.modulecode);
                    objCmd.Parameters.AddWithValue("@createdby", restaurant.CreatedBy);// Set your module code here
                    objCmd.ExecuteNonQuery();
                    restaurantcode = parm.Value.ToString();
                    restaurant.restaurantcode = restaurantcode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = restaurant;
                    response.message = "Restaurant Onboarded Successfully";
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
        public ActionResult UpdateRestaurant([FromBody] Restaurant restaurant)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKRESTAURANT_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    
                    List<IDictionary<string, string>> dicAddresses = new List<IDictionary<string, string>>();
                    foreach (GarvikAddress objA in restaurant.Addresses)
                    {
                        dicAddresses.Add(GarvikAddress.ConverttoJson(objA));
                    }
                    string json = JsonConvert.SerializeObject(dicAddresses, Formatting.Indented);

                    List<IDictionary<string, string>> dicDocuments = new List<IDictionary<string, string>>();
                    foreach (Documents objA in restaurant.Documents)
                    {
                        dicDocuments.Add(Documents.ConverttoJson(objA));
                    }
                    string jsonDocuments = JsonConvert.SerializeObject(dicDocuments, Formatting.Indented);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@restaurantcode", restaurant.restaurantcode);
                    objCmd.Parameters.AddWithValue("@RestaurantName", restaurant.RestaurantName);
                    objCmd.Parameters.AddWithValue("@Description", restaurant.Description);
                    objCmd.Parameters.AddWithValue("@LogoUrl", restaurant.LogoUrl);
                    objCmd.Parameters.AddWithValue("@CoverImageUrl", restaurant.CoverImageUrl);
                    objCmd.Parameters.AddWithValue("@Category", restaurant.Category);
                    objCmd.Parameters.AddWithValue("@Cuisine", restaurant.Cuisine);
                    objCmd.Parameters.AddWithValue("@Phone", restaurant.Phone);
                    objCmd.Parameters.AddWithValue("@Email", restaurant.Email);
                    objCmd.Parameters.AddWithValue("@Website", restaurant.Website);
                    objCmd.Parameters.AddWithValue("@Latitude", restaurant.Latitude);
                    objCmd.Parameters.AddWithValue("@Longitude", restaurant.Longitude);
                    objCmd.Parameters.AddWithValue("@OpeningTime", restaurant.OpeningTime);
                    objCmd.Parameters.AddWithValue("@ClosingTime", restaurant.ClosingTime);
                    objCmd.Parameters.AddWithValue("@HasDeliveryService", restaurant.HasDeliveryService);
                    objCmd.Parameters.AddWithValue("@HasTakeawayService", restaurant.HasTakeawayService);
                    objCmd.Parameters.AddWithValue("@HasDineInService", restaurant.HasDineInService);
                    objCmd.Parameters.AddWithValue("@IsVegetarianFriendly", restaurant.IsVegetarianFriendly);
                    objCmd.Parameters.AddWithValue("@IsVeganFriendly", restaurant.IsVeganFriendly);
                    objCmd.Parameters.AddWithValue("@AcceptsCard", restaurant.AcceptsCard);

                    objCmd.Parameters.AddWithValue("@IsHalalCertified", restaurant.IsHalalCertified);
                    objCmd.Parameters.AddWithValue("@AcceptsCash", restaurant.AcceptsCash);
                    objCmd.Parameters.AddWithValue("@AcceptsOnlinePayment", restaurant.AcceptsOnlinePayment);
                    objCmd.Parameters.AddWithValue("@AverageRating", restaurant.AverageRating);
                    objCmd.Parameters.AddWithValue("@TotalReviews", restaurant.TotalReviews);

                    objCmd.Parameters.AddWithValue("@HasWifi", restaurant.HasWifi);
                    objCmd.Parameters.AddWithValue("@HasParking", restaurant.HasParking);
                    objCmd.Parameters.AddWithValue("@IsAlcoholServed", restaurant.IsAlcoholServed);
                    objCmd.Parameters.AddWithValue("@HasOutdoorSeating", restaurant.HasOutdoorSeating);
                    objCmd.Parameters.AddWithValue("@IsOpen", restaurant.IsOpen);
                    objCmd.Parameters.AddWithValue("@RegistrationDate", restaurant.RegistrationDate);
                    objCmd.Parameters.AddWithValue("@RegistrationNumber", restaurant.RegistrationNumber);
                    objCmd.Parameters.AddWithValue("@RegistrationCertificateUrl", restaurant.RegistrationCertificateUrl);
                    objCmd.Parameters.AddWithValue("@addressarray", json);
                    objCmd.Parameters.AddWithValue("@documentarray", jsonDocuments);
                    objCmd.Parameters.AddWithValue("@modulecode", restaurant.modulecode);
                    objCmd.Parameters.AddWithValue("@createdby", restaurant.CreatedBy);// Set your module code here
                    objCmd.ExecuteNonQuery();
                    
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = restaurant;
                    response.message = "Restaurant Updated Successfully";
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
        public ActionResult DeleteRestaurant([FromBody] Restaurant obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKRESTAURANT_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@restaurantcode", obj.restaurantcode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Restaurant Deleted Successfully";
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
