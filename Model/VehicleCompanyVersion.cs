using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class VehicleCompanyVersion
    {
        public string vehicleversioncode { get; set; }
        public string vehiclecompanycode { get; set; }
        public string modelcode { get; set; }
        public string vehicleversion { get; set; }

        public string createdby { get; set; }

        public static IDictionary<string, string> ConverttoJson(VehicleCompanyVersion obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("vehicleversioncode", obj.vehicleversioncode);
            objDict.Add("vehiclecompanycode", obj.vehiclecompanycode);
            objDict.Add("vehicleversion", obj.vehicleversion);
            objDict.Add("modelcode", obj.modelcode);
            return objDict;
        }
    }
}
