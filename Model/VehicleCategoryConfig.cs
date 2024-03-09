using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class VehicleCategoryConfig
    {
        public string vehiclecategorycode { get; set; }
        public string vehiclecategoryname { get; set; }
        public float baserate { get; set; }
        public float peakhourrate { get; set; }
        public string peakhourtimefrom { get; set; }
        public string peakhourtimeto { get; set; }
        public bool ispeakhour { get; set; } = false;
        public string createdby { get; set; }
        public DateTime createddate { get; set; } = DateTime.Now;

        public float ratepermeter { get; set; }
        public float  rateperminute { get; set; }
        public float waitfeeperminute { get; set; }
        public float taxrate { get; set; }
        public string fleetimage { get; set; }
        public bool status { get; set; } = true;

        public static IDictionary<string, string> ConverttoJson(VehicleCategoryConfig obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("vehiclecategorycode", obj.vehiclecategorycode);
            objDict.Add("vehiclecategoryname", obj.vehiclecategoryname);
            objDict.Add("baserate", obj.baserate.ToString());
            objDict.Add("peakhourrate", obj.peakhourrate.ToString());
            objDict.Add("peakhourtimefrom", obj.peakhourtimefrom);
            objDict.Add("peakhourtimeto", obj.peakhourtimeto);
            objDict.Add("ispeakhour", obj.ispeakhour.ToString());
            objDict.Add("createdby", obj.createdby);
            objDict.Add("ratepermeter", obj.ratepermeter.ToString());
            objDict.Add("rateperminute", obj.rateperminute.ToString());
            objDict.Add("waitfeeperminute", obj.waitfeeperminute.ToString());
            objDict.Add("taxrate", obj.taxrate.ToString());
            objDict.Add("fleetimage", obj.fleetimage.ToString());
            return objDict;
        }
    }
}
