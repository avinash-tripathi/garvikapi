using GARVIKService.Model.Customer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Taxi
{
    public class Booking
    {
        public string bookingcode { get; set; }
        public GarvikReview[] reviews { get; set; }
        public string customercode { get; set; }
        public string assigneddrivercode { get; set; }
        [JsonIgnore]
        public Driver driverinfo { get; set; }
        [JsonIgnore]
        public GarvikCustomer customerinfo { get; set; }
        public float sourcelocationlongi { get; set; }
        public float sourcelocationlati { get; set; }
        public float destinationlocationlongi { get; set; }
        public float destinationlocationlati { get; set; }
        public float eta { get; set; }
        public string sourcelocationaddress { get; set; }
        public string destinationlocationaddress { get; set; }
        public string ridestatus { get; set; }
        public string startdate { get; set; }
        public string completedate { get; set; }

        public string vehiclecategorycode { get; set; }

        public int otpforonboard { get; set; }
        public float amount { get; set; }
        private string _customerInfoString; // Private field to store the string representation
        [JsonProperty("customerinfo")] // Map this property to the "customerinfo" field in JSON
        public string CustomerInfoString
        {
            get => _customerInfoString;
            set
            {
                _customerInfoString = value;
                // Deserialize the customerinfo string to a CustomerInfo object
                customerinfo  = JsonConvert.DeserializeObject<GarvikCustomer>(_customerInfoString);
            }
        }
        private string _driverInfoString; // Private field to store the string representation
        [JsonProperty("driverinfo")] // Map this property to the "customerinfo" field in JSON
        public string DriverInfoString
        {
            get => _driverInfoString;
            set
            {
                _driverInfoString = value;
                // Deserialize the customerinfo string to a CustomerInfo object
                driverinfo = JsonConvert.DeserializeObject<Driver>(_driverInfoString);
            }
        }
        public static IDictionary<string, string> ConverttoJson(Booking obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("bookingcode", obj.bookingcode);
            objDict.Add("customercode", obj.customercode);
            objDict.Add("sourcelocationlongi", obj.sourcelocationlongi.ToString());
            objDict.Add("sourcelocationlati", obj.sourcelocationlati.ToString());
            objDict.Add("destinationlocationlongi", obj.destinationlocationlongi.ToString());
            objDict.Add("destinationlocationlati", obj.destinationlocationlati.ToString());
            objDict.Add("ridestatus", obj.ridestatus.ToString());
            return objDict;
        }

    }
}
