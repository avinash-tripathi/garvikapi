using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class VehicleInfo
    {
        public string vehiclecategorycode { get; set; }
        public string vehiclecategoryname { get; set; }
        public float baserate { get; set; }
        public float peakhourrate { get; set; }
        public float ratepermeter { get; set; }
        public float rateperminute { get; set; }
        public float waitfeeperminute { get; set; }
        public float taxrate { get; set; }
        public string peakhourtimefrom { get; set; }
        public string peakhourtimeto { get; set; }
        public bool ispeakhour { get; set; }
        public string fleetimage { get; set; }
        public string modelcode { get; set; }
        public string modelname { get; set; }
        public string imageurl { get; set; }
        public string vehicleVersion { get; set; }
        public string vehiclecompanycode { get; set; }
        public string vehiclecompanyname { get; set; }
        public static IDictionary<string, string> ConvertToJson(VehicleInfo obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("vehiclecategorycode", obj.vehiclecategorycode);
            objDict.Add("vehiclecategoryname", obj.vehiclecategoryname);
            objDict.Add("baserate", obj.baserate.ToString());
            objDict.Add("peakhourrate", obj.peakhourrate.ToString());

            objDict.Add("ratepermeter", obj.ratepermeter.ToString());
            objDict.Add("rateperminute", obj.rateperminute.ToString());
            objDict.Add("waitfeeperminute", obj.waitfeeperminute.ToString());
            objDict.Add("taxrate", obj.taxrate.ToString());

            objDict.Add("peakhourtimefrom", obj.peakhourtimefrom.ToString());
            objDict.Add("peakhourtimeto", obj.peakhourtimeto.ToString());
            objDict.Add("ispeakhour", obj.ispeakhour.ToString());
            objDict.Add("fleetimage", obj.fleetimage);
            objDict.Add("modelcode", obj.modelcode);
            objDict.Add("modelname", obj.modelname);
            objDict.Add("imageurl", obj.imageurl);
            objDict.Add("vehicleVersion", obj.vehicleVersion);
            objDict.Add("vehiclecompanycode", obj.vehiclecompanycode);
            objDict.Add("vehiclecompanyname", obj.vehiclecompanyname);
            return objDict;
        }
    }

}
