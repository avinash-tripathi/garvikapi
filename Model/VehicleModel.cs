using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class VehicleModel
    {
        public string vehiclemodelcode { get; set; }
        public string vehiclemodelname { get; set; }
        public string imageurl { get; set; }
        public string createdby { get; set; }
        public string vehiclecategorycode { get; set; }

        public static IDictionary<string, string> ConverttoJson(VehicleModel obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("vehiclemodelcode", obj.vehiclemodelcode);
            objDict.Add("vehiclemodelname", obj.vehiclemodelname);
            objDict.Add("imageurl", obj.imageurl);
            objDict.Add("vehiclecategorycode", obj.vehiclecategorycode);
            return objDict;
        }
    }
}
