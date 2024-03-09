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
    public class RestaurantMenuController : ControllerBase
    {
        private readonly List<RestaurantMenu> restaurantmenuList = new List<RestaurantMenu>();
        private readonly IConfiguration configuration;
        public RestaurantMenuController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        [HttpGet]
        public List<RestaurantMenu> Get()
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadGARVIKRESTAURANT_Menus";
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
                        RestaurantMenu obj = new RestaurantMenu();
                      /*  GarvikAddress[] addressArray = JsonConvert.DeserializeObject<GarvikAddress[]>(row["addressdetail"].ToString());
                        Documents[] documentArray = JsonConvert.DeserializeObject<Documents[]>(row["documentdetail"].ToString());
*/

  
                        obj.restaurantcode = row["restaurantcode"].ToString();
                        obj.menucode = row["menucode"].ToString();
                        obj.menudescription = row["menudescription"].ToString();
                        obj.menuurl = row["menuurl"].ToString();
                        obj.menuname = row["menuname"].ToString();
                        obj.price = float.Parse(row["menuname"].ToString());

                        restaurantmenuList.Add(obj);
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
            return restaurantmenuList;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult AddRestaurantMenu([FromBody] RestaurantMenu restaurant)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_InsertGARVIKRESTAURANT_Menus";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    SqlParameter parm = new SqlParameter("@menucode", SqlDbType.VarChar, 50);
                    parm.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(parm);
                    String menucode = "";
                    
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@restaurantcode", restaurant.restaurantcode);
                    objCmd.Parameters.AddWithValue("@menuname", restaurant.menuname);
                    objCmd.Parameters.AddWithValue("@price", restaurant.price);
                    objCmd.Parameters.AddWithValue("@menudescription", restaurant.menudescription);
                    objCmd.Parameters.AddWithValue("@menuurl", restaurant.menuurl);
                    objCmd.Parameters.AddWithValue("@createdby", restaurant.createdby);
                    objCmd.ExecuteNonQuery();
                    menucode = parm.Value.ToString();
                    restaurant.menucode = menucode;

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = restaurant;
                    response.message = "Restaurant Menu Created Successfully";
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
        public ActionResult UpdateRestaurantMenu([FromBody] RestaurantMenu restaurant)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_UpdateGARVIKRESTAURANT_Menus";
            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.Parameters.AddWithValue("@restaurantcode", restaurant.restaurantcode);
                    objCmd.Parameters.AddWithValue("@menucode", restaurant.menucode);
                    objCmd.Parameters.AddWithValue("@menuname", restaurant.menuname);
                    objCmd.Parameters.AddWithValue("@price", restaurant.price);
                    objCmd.Parameters.AddWithValue("@menudescription", restaurant.menudescription);
                    objCmd.Parameters.AddWithValue("@menuurl", restaurant.menuurl);
                    objCmd.ExecuteNonQuery();
                    
                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Updated.ToText();
                    response.obj = restaurant;
                    response.message = "Restaurant Menu Updated Successfully";
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
        public ActionResult DeleteRestaurantMenu([FromBody] RestaurantMenu obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_DeleteGARVIKRESTAURANT_Menus";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@menucode", obj.menucode);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    objCmd.ExecuteNonQuery();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Deleted.ToText();
                    response.obj = obj;
                    response.message = "Menu Deleted Successfully";
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
