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
using Microsoft.AspNetCore.Authorization;
using GARVIKService.Model.Taxi;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GARVIKService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly List<StaticImage> staticImageList = new List<StaticImage>();
        private readonly IConfiguration configuration;
        
        public AttachmentController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        /*[Route("[action]")]
        [HttpPost]
        public ActionResult UploadStaticImages(StaticImage obj)
        {
            try
            {
                //string cloudImageBasePath = "https://garvikblobstorage.blob.core.windows.net/garvikdoc/random/Random/Random_Image.png";
                string cloudImageBasePath = "https://garvikblobstorage.blob.core.windows.net/garvikdoc/";

                byte[] imageBytes = Convert.FromBase64String(obj.base64image);
                string AzureStorageAccountConString = configuration["BlobStorageConnectionString"];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageAccountConString);
                // Create a CloudBlobClient object from the storage account.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Get a reference to the container where you want to create a Blob.
                CloudBlobContainer container = blobClient.GetContainerReference(configuration["AzureStorageContainerName"]);
                string blobName = "STATICIMAGE" + "\\" + obj.imagecontext + "\\" + obj.imagename + ".png";

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

                string _imagepath = cloudImageBasePath + obj.imagecontext + "/" + obj.imagename + ".png";
                obj.imagepath = _imagepath;
                // await RegisterImageAsync(obj);
                return Ok(new { result = "Uploaded" });
            }
            catch (Exception ex)
            {
                return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                throw ex;
            }
        }
*/

        [HttpGet]
        public List<StaticImage> Get(String imagecontext, String imagename)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_ReadstaticImage_M";
            DataSet DS = new DataSet();

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                objConn.Open();
                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@imagecontext", imagecontext);
                    objCmd.Parameters.AddWithValue("@imagename", imagename);

                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    SqlDataAdapter da = new SqlDataAdapter(objCmd);
                    da.Fill(DS);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        StaticImage obj = new StaticImage();
                        obj.imagecontext = row["imagecontext"].ToString();
                        obj.imagename = row["imagename"].ToString();
                        obj.imagepath = row["imagepath"].ToString();
                        obj.width = row["width"].ToString();
                        obj.height = row["height"].ToString();
                        obj.alt = row["alt"].ToString();
                        obj.loading = row["loading"].ToString();
                        obj.extension = row["extension"].ToString();
                        staticImageList.Add(obj);
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
            return staticImageList;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> UploadStaticImageAsync(StaticImage obj)
        {
            try
            {
                //string cloudImageBasePath = "https://garvikblobstorage.blob.core.windows.net/garvikdoc/random/Random/Random_Image.png";
                string cloudImageBasePath = "https://garvikblobstorage.blob.core.windows.net/garvikdoc/";
                string fileextension = ".png";
                if (obj.extension != null || obj.extension != "")
                {
                    fileextension = obj.extension;
                }

                byte[] imageBytes = Convert.FromBase64String(obj.base64image);
                string AzureStorageAccountConString = configuration["BlobStorageConnectionString"];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageAccountConString);
                // Create a CloudBlobClient object from the storage account.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Get a reference to the container where you want to create a Blob.
                CloudBlobContainer container =  blobClient.GetContainerReference(configuration["AzureStorageContainerName"]);
                string blobName = "STATICIMAGE" + "\\" + obj.imagecontext + "\\" + obj.imagename + fileextension;

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                // Set the content type for the blob
                //blockBlob.Properties.ContentType = "image/webp";
                blockBlob.Properties.ContentType = obj.contenttype;
                // Set Content-Disposition to include the file name with the correct extension
                blockBlob.Properties.ContentDisposition = $"attachment; filename={blobName}";

                await blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
                

                string _imagepath = cloudImageBasePath + "STATICIMAGE" + "/" + obj.imagecontext + "/" + obj.imagename + fileextension;
                obj.imagepath = _imagepath;
                 return await RegisterImageAsync(obj);
                //return Ok(new { result = "Uploaded" });
            }
            catch (Exception ex)
            {
                return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                throw ex;
            }
        }


        [Route("[action]")]
        [HttpPut]
        public ActionResult UploadImage(Attachment obj)
        {
                try
                {
                byte[] imageBytes = Convert.FromBase64String(obj.base64image);
                    string AzureStorageAccountConString = configuration["BlobStorageConnectionString"];
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageAccountConString);
                    // Create a CloudBlobClient object from the storage account.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    // Get a reference to the container where you want to create a Blob.
                    CloudBlobContainer container = blobClient.GetContainerReference(configuration["AzureStorageContainerName"]);
                string fileextension = ".png";
                    if (obj.extension!=null || obj.extension!="")
                {
                    fileextension = obj.extension;
                }
                   string blobName =  obj.modulename + "\\" + obj.documentcode + "\\" + obj.documentcode + "_" + obj.documenttype + fileextension;
                

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
             

                blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
                    return Ok(new { result = "Uploaded" });
                }
                catch (Exception ex)
                {
                    return NotFound(new { result = "something went wrong. " + ex.Message.ToString() });
                    throw ex;
                }
                
         
        }
        


        private async Task<ActionResult> RegisterImageAsync(StaticImage obj)
        {
            String connectionString = configuration.GetSection("ConnectionStrings")["Params"];
            String query = "stp_Insertstaticimage_M";

            using (SqlConnection objConn = new SqlConnection(connectionString))
            {
                await objConn.OpenAsync();

                SqlCommand objCmd = new SqlCommand(query, objConn);
                try
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.AddWithValue("@imagecontext", obj.imagecontext);
                    objCmd.Parameters.AddWithValue("@imagename", obj.imagename);
                    objCmd.Parameters.AddWithValue("@imagepath", obj.imagepath);
                    objCmd.Parameters.AddWithValue("@width", obj.width);
                    objCmd.Parameters.AddWithValue("@height", obj.height);
                    objCmd.Parameters.AddWithValue("@alt", obj.alt);
                    objCmd.Parameters.AddWithValue("@loading", obj.loading);
                    objCmd.Parameters.AddWithValue("@extension", obj.extension);
                    objCmd.CommandTimeout = 0;
                    objCmd.Connection = objConn;
                    await objCmd.ExecuteNonQueryAsync();

                    GarvikResponse response = new GarvikResponse();
                    response.result = OperationStatus.Inserted.ToText();
                    response.obj = obj;
                    response.message = "Image Registered Successfully";
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
