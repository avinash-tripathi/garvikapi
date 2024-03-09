using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class VehicleCompany
    {
        public string vehiclecompanycode { get; set; }
        public string vehiclecompanyname { get; set; }

        public string vehiclecompanytype { get; set; }
        public string createdby { get; set; }

        public static IDictionary<string, string> ConverttoJson(VehicleCompany obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("vehiclecompanycode", obj.vehiclecompanycode);
            objDict.Add("vehiclecompanyname", obj.vehiclecompanyname);
            objDict.Add("vehiclecompanytype", obj.vehiclecompanytype);
            return objDict;
        }

    }
}
