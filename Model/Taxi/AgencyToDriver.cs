using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Taxi
{
    public class AgencyToDriver
    {
        public string agencycode { get; set; }
        public string drivercode { get; set; }
        public string createdby { get; set; }
        public static IDictionary<string, string> ConvertToJson(AgencyToDriver obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("agencycode", obj.agencycode);
            objDict.Add("drivercode", obj.drivercode);
            objDict.Add("createdby", obj.createdby);
            return objDict;
        }

    }
}
